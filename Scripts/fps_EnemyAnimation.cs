using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class fps_EnemyAnimation : MonoBehaviour {

	public float deadZone = 5f;

	private Transform player;
	private fps_EnemySight enemySight;
	private NavMeshAgent nav;
	private Animator anim;
	private HashIDs hash;
	private AnimatorSetup animSetup;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag(Tags.player).transform;
		enemySight = GetComponent<fps_EnemySight>();
		nav = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		hash = GameObject.Find(Tags.gameController).GetComponent<HashIDs>();
		animSetup = new AnimatorSetup(anim,hash);

		nav.updateRotation = false;//禁用寻路组件
		anim.SetLayerWeight(1, 1f);
		anim.SetLayerWeight(2, 1f);

		deadZone *= Mathf.Deg2Rad;//角度转化为弧度


	}


	void Update()
	{
		NavAnimSetUp();
	}

	//控制敌人移动
	void OnAnimatorMove()
	{
		nav.velocity = anim.deltaPosition / Time.deltaTime;
		transform.rotation = anim.rootRotation;
	}

	void NavAnimSetUp()
	{
		float speed;
		float angle;

		if (enemySight.playerInSight)
		{
			speed = 0;

			angle = FindAngle(transform.forward, player.position - transform.position, transform.up);

		}
		else
		{
			speed = Vector3.Project(nav.desiredVelocity, transform.forward).magnitude;
			angle = FindAngle(transform.forward,nav.desiredVelocity,transform.up);

			if (Mathf.Abs(angle) < deadZone)
			{
				transform.LookAt(transform.position + nav.desiredVelocity);
				angle = 0;
			}

		}
		animSetup.Setup(speed,angle);

	}

	private float FindAngle(Vector3 fromVector, Vector3 toVector, Vector3 upVector)
	{
		if (toVector == Vector3.zero)
			return 0;
		float angle = Vector3.Angle(fromVector,toVector);
		Vector3 normal = Vector3.Cross(fromVector,toVector);
		angle *= Mathf.Sign(Vector3.Dot(normal,upVector));
		angle *= Mathf.Deg2Rad;
		return angle;
	}


}
