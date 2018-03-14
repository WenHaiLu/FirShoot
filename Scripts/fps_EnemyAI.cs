using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class fps_EnemyAI : MonoBehaviour {

	public float partolSpeed = 2f;//巡逻速度 
	public float chaseSpeed = 5f;//追击速度
	public float chaseWaitTime = 5f;//丢失玩家后等待的时间
	public float partolWiatTime = 1f;// 巡逻到达一个地点后的等待时间
	public Transform[] partolWayPoints;

	private fps_EnemySight enemySight;
	private NavMeshAgent nav;
	private Transform player;
	private fps_PlayreHealth playerHealth;
	private float chaseTimer;//追击计时器
	private float partolTimer;// 巡逻计时器
	private int wayPointIndex;// 巡逻点的索引


	void Start()
	{
		enemySight = GetComponent<fps_EnemySight>();
		nav = GetComponent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag(Tags.player).transform;
		playerHealth = player.GetComponent<fps_PlayreHealth>();

	}

	void Update()
	{
		if (enemySight.playerInSight && playerHealth.hp > 0f)
			Shooting();
		else if (enemySight.playerPosition != enemySight.resetPosition && playerHealth.hp > 0)//追击
			Chasing();
		else //巡逻
			Partolling();
			
	}


	private void Shooting()
	{
		nav.SetDestination(transform.position);

	}

	private void Chasing()
	{
		Vector3 sightingDeltaPos = enemySight.playerPosition - transform.position;

		if (sightingDeltaPos.sqrMagnitude > 4f)
			nav.destination = enemySight.playerPosition;

		nav.speed = chaseSpeed;

		if (nav.remainingDistance < nav.stoppingDistance)
		{
			chaseTimer += Time.deltaTime;

			if (chaseTimer >= chaseWaitTime)
			{
				enemySight.playerPosition = enemySight.resetPosition;
				chaseTimer = 0f;
			}
		}
		else
			chaseTimer = 0f;
		

		
	}

	private void Partolling()
	{
		nav.speed = partolSpeed;
		if (nav.destination == enemySight.resetPosition || nav.remainingDistance < nav.stoppingDistance)
		{
			partolTimer += Time.deltaTime;
			if (partolTimer >= partolWiatTime)
			{
				if (wayPointIndex == partolWayPoints.Length - 1)
					wayPointIndex = 0;
				else
					wayPointIndex++;

				partolTimer = 0;
			}

		}
		else
			partolTimer = 0;

		nav.destination = partolWayPoints[wayPointIndex].position;
	}


}
