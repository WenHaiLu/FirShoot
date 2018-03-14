using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//控制敌人视野
public class fps_EnemySight : MonoBehaviour {

	//敌人视野角度
	public float fieldOfViewAngle = 110f;
	//玩家是否在敌人视野内
	public bool playerInSight;
	public Vector3 playerPosition;//玩家坐标  
	public Vector3 resetPosition = Vector3.zero;//默认位置


	private NavMeshAgent nav;
	private SphereCollider col;//球形触发器
	private Animator anim;
	private GameObject player;
	private fps_PlayreHealth playerHealth;
	private HashIDs hash;
	private fps_PlayerControl playreControl;




	void Start()
	{
		nav = GetComponent<NavMeshAgent>();
		col = GetComponentInChildren<SphereCollider>();
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
		player = GameObject.FindGameObjectWithTag(Tags.player);
		playerHealth = player.GetComponent<fps_PlayreHealth>();
		playreControl = player.GetComponent<fps_PlayerControl>();
		fps_GunScript.playerShootEvent += ListenPlayer;
	
	}


	void Update()
	{
		if (playerHealth.hp > 0)
			anim.SetBool(hash.playerInSightBool,playerInSight);
		else
			anim.SetBool(hash.playerInSightBool,false);
		//Debug.Log(hash.playerInSightBool);
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject == player)
		{
			playerInSight = false;
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);
			if (angle < fieldOfViewAngle * 0.5)
			{
				RaycastHit hit;
				if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
				{
					if (hit.collider.gameObject == player)
					{
						playerInSight = true;
						playerPosition = player.transform.position;

					}
				}
			}
			if (playreControl.State == PlayerState.Walk || playreControl.State == PlayerState.Run)
			{
				ListenPlayer();
			}


		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == player)
		{
			playerInSight = false;
		}

	}


	private void ListenPlayer()
	{
		if (Vector3.Distance(player.transform.position, transform.position) <= col.radius)
		{
			playerPosition = player.transform.position;

		}
	}



	void OnDestroy()
	{
		fps_GunScript.playerShootEvent -= ListenPlayer;

	}
}
