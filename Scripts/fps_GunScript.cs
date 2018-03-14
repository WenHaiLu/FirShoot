using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void PlayerShoot();

public class fps_GunScript : MonoBehaviour
{

	public static event PlayerShoot playerShootEvent;
	public float fireRate = 0.1f;//射击间隔
	public float damage = 40;//武器伤害
	public float reloadTime = 1.5f;//重装填时间
	public float flashRate = 0.02f;//闪光效果持续时间
	public AudioClip fireAudio;//开火音效
	public AudioClip reloadAudio;//装填音效
	public AudioClip damagaeAudio;//受伤音效
	public AudioClip dryFireAudio;//没有子弹无法开火音效
	public GameObject explosion;//爆炸特效
	public int bulletCount = 30;//每个弹夹子弹数量
	public int chargeBulletCount = 60;//可装填子弹数量 
	public Text bulletText;


	private string reloadAnim = "Reload";//装填动画
	private string fireAnim = "Single_Shot";//射击动画
	private string walkAnim = "Walk";
	private string runAnim = "Run";
	private string jumpAnim = "Jump";
	private string idleAnim = "Idle";

	private Animation anim;
	private float nextFireTime = 0f;
	private MeshRenderer flash;
	private int currentBllet;//当前子弹数量
	private int currentChargeBullet;//当前可状填的子弹数量
	private fps_PlayerParameter paremeter;
	private fps_PlayerControl playerControl;


	void Start()
	{
		paremeter = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<fps_PlayerParameter>();
		playerControl = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<fps_PlayerControl>();
		anim = GetComponent<Animation>();
		flash = this.transform.Find("muzzle_flash").GetComponent<MeshRenderer>();
		flash.enabled = false;
		currentBllet = bulletCount;
		currentChargeBullet = chargeBulletCount;
		bulletText.text = currentBllet + "/" + currentChargeBullet;

	}


	void Update()
	{

		if (paremeter.inputReload && currentBllet < bulletCount)
			Reload();
		if (paremeter.inputFire && !anim.IsPlaying(reloadAnim))
			Fire();
		else if (!anim.IsPlaying(reloadAnim))
			StateAnim(playerControl.State);
		
			
		
	}

	private void ReloadAnim()
	{
		anim.Stop(reloadAnim);
		anim[reloadAnim].speed = (anim[reloadAnim].length / reloadTime);
		anim.Rewind(reloadAnim);
		anim.Play(reloadAnim);
	}

	private IEnumerator ReloadFinish()
	{
		yield return new WaitForSeconds(reloadTime);
		if (currentChargeBullet >= bulletCount - currentBllet)
		{
			currentChargeBullet -= (bulletCount - currentBllet);
			currentBllet = bulletCount;
		}
		else
		{
			currentBllet += currentChargeBullet;
			currentChargeBullet = 0;
		}
		bulletText.text = currentBllet + "/" + currentChargeBullet;
	}

	private void Reload()
	{
		if (!anim.IsPlaying(reloadAnim))
		{
			if (currentChargeBullet > 0)
				StartCoroutine(ReloadFinish());
			else
			{
				anim.Rewind(fireAnim);
				anim.Play(fireAnim);
				AudioSource.PlayClipAtPoint(dryFireAudio,transform.position);
				return;
			}
			AudioSource.PlayClipAtPoint(reloadAudio,transform.position);
			ReloadAnim();
		}

	}


	private IEnumerator Flash()
	{
		flash.enabled = true;
		yield return new WaitForSeconds(flashRate);
		flash.enabled = false;
	}


	private void Fire()
	{
		if (Time.time > nextFireTime)
		{
			if (currentBllet <= 0)
			{
				Reload();
				nextFireTime += Time.deltaTime + fireRate;
				//nextFireTime = 0;
				return;
			}
			currentBllet--;
			bulletText.text = currentBllet + "/" + currentChargeBullet;
			DamageEnemy();
			if (playerShootEvent != null)
				playerShootEvent();
			AudioSource.PlayClipAtPoint(fireAudio,transform.position);
			nextFireTime = Time.deltaTime + fireRate;
			//nextFireTime = 0;
			anim.Rewind(fireAnim);
			anim.Play(fireAnim);
			StartCoroutine(Flash());
		}
	}


	private void DamageEnemy()
	{

		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2,0));
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			if (hit.transform.tag == Tags.enemy && hit.collider is CapsuleCollider)
			{
				AudioSource.PlayClipAtPoint(damagaeAudio,hit.transform.position);
				GameObject go = Instantiate(explosion,hit.point,Quaternion.identity);
				Destroy(go, 3);
				hit.transform.GetComponent<fps_EnemyHealth>().TakeDamage(damage);

			}
				
		}


	}


	private void PlayreStateAnim(string animName)
	{
		if (!anim.IsPlaying(animName))
		{ 
			anim.Rewind(animName);
			anim.Play(animName);
		}
	
		
	}



	private void StateAnim(PlayerState state)
	{
		switch (state)
		{ 
			case PlayerState.Idle:
				PlayreStateAnim(idleAnim);
				break;
			case PlayerState.Walk:
				PlayreStateAnim(walkAnim);
				break;
			case PlayerState.Crouch:
				PlayreStateAnim(walkAnim);
				break;
			case PlayerState.Run:
				PlayreStateAnim(runAnim);
				break;
		
		}
	}


}
