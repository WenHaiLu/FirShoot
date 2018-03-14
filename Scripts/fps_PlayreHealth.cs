using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_PlayreHealth : MonoBehaviour {

	public bool isDead;
	public float resetAfterDeathTime = 5f;//玩家死后等待多久场景重置
	public AudioClip deathClip;//死亡音效
	public AudioClip damageClip;//受伤后的音效
	public float maxHp = 100f;
	public float hp = 100f;//当前生命值
	public float recoverSpeed = 1f;// 生命值恢复速度

	private float timer = 0;
	private FadeInOut fader;


	void Start()
	{
		hp = maxHp;
		fader = GameObject.FindGameObjectWithTag(Tags.fader).GetComponent<FadeInOut>();
		BleedBehavior.BloodAmount = 0;
	}


	void Update()
	{
		if (!isDead)
		{
			hp += recoverSpeed * Time.deltaTime;

			if (hp > maxHp)
				hp = maxHp;
		}

		if (hp < 0)
		{
			if (!isDead)
				PlayerDead();
			else
				LevelReset();
		}
	}

	public void TakeDamage(float damage)
	{
		if (isDead)
			return;
		AudioSource.PlayClipAtPoint(damageClip,transform.position);
		BleedBehavior.BloodAmount += Mathf.Clamp01(damage / hp);
		hp -= damage;
		Debug.Log(hp);
	}

	public void DisableInput()
	{
		transform.Find("FP_Camera/Weapon_Camera").gameObject.SetActive(false);
		GetComponent<AudioSource>().enabled = false;
		GetComponent<fps_PlayerControl>().enabled = false;
		GetComponent<fps_FPInpout>().enabled = false;
		if (GameObject.Find("Canvas") != null)
			GameObject.Find("Canvas").SetActive(false);
		GameObject.Find("FP_Camera").GetComponent<fps_FPCamera>().enabled = false;

	}

	// 死亡方法
	public void PlayerDead()
	{
		isDead = true;
		DisableInput();
		AudioSource.PlayClipAtPoint(deathClip,transform.position);

	}

	//重置方法
	public void LevelReset()
	{
		timer += Time.deltaTime;

		if (timer >= resetAfterDeathTime)
			fader.endScene();

	}


}
