using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_EnemyShoot : MonoBehaviour {


	public float maxmumDamage = 120;//最大伤害
	public float minmumDamage = 45;//最小伤害
	public AudioClip shotClip;
	public float flashIntensity = 3f;//闪光强度
	public float fadeSpeed = 10f;

	private Animator anim;
	private HashIDs hash;
	private LineRenderer laserShotLine;
	private Light LaserShotLight;
	private SphereCollider col;
	private Transform player;
	private fps_PlayreHealth playerHealth;
	private bool shooting;
	private float ScaleDamage;//伤害取值范围


	void Start()
	{
		anim = this.GetComponent<Animator>();
		laserShotLine = GetComponentInChildren<LineRenderer>();
		LaserShotLight = laserShotLine.gameObject.GetComponent<Light>();

		col = GetComponentInChildren<SphereCollider>();
		player = GameObject.FindGameObjectWithTag(Tags.player).transform;
		playerHealth = player.GetComponent<fps_PlayreHealth>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();

		laserShotLine.enabled = false;
		LaserShotLight.intensity = 0;


		ScaleDamage = maxmumDamage - minmumDamage;
	}


	void Update()
	{
		float shot = anim.GetFloat(hash.shotFloat);
		Debug.Log("Shot:");
		Debug.Log(shot);
		if (shot > 0.05f && !shooting)
		{
			Shoot();
		
		}
		if (shot < 0.05f)
		{
			shooting = false;
			laserShotLine.enabled = false;
		}

		LaserShotLight.intensity = Mathf.Lerp(LaserShotLight.intensity, 0f, fadeSpeed * Time.deltaTime);
	}


	void OnAnimatorIK(int layerIndex)
	{
		
		float aimWeight = anim.GetFloat(hash.aimWeightFloat);

		anim.SetIKPosition(AvatarIKGoal.RightHand, player.position + Vector3.up * 1.5f);
		anim.SetIKPositionWeight(AvatarIKGoal.RightHand, aimWeight);

	}

	private void Shoot()
	{
		shooting = true;
		//计算伤害百分比，距离越近伤害越高
		float fractionalDistance = ((col.radius - Vector3.Distance(transform.position,player.position))/col.radius);
		float damage = ScaleDamage * fractionalDistance + minmumDamage;// 当前伤害值

		playerHealth.TakeDamage(damage);
		ShotEffects();
			
	}


	private void ShotEffects()
	{
		laserShotLine.SetPosition(0,laserShotLine.transform.position);
		laserShotLine.SetPosition(1, player.position + Vector3.up * 1.5f);
		laserShotLine.enabled = true;

		LaserShotLight.intensity = flashIntensity;
		AudioSource.PlayClipAtPoint(shotClip, LaserShotLight.transform.position);


	}

}
