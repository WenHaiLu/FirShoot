﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_ExitTrigger : MonoBehaviour {

	public float timeToInactivePlayer = 2.0f;//出口触发时间
	public float timeToRestart = 5.0f;// 重置时间


	private GameObject player;
	private bool playerInExit;
	private float timer;
	private FadeInOut fader;

	void Start()
	{
		fader = GameObject.FindGameObjectWithTag(Tags.fader).GetComponent<FadeInOut>();
		player = GameObject.FindGameObjectWithTag(Tags.player);

	}


	void Update()
	{
		if (playerInExit)
		{
			InExitActivation();
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == player)
		{
			playerInExit = true;

		}
	}


	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == player)
		{
			playerInExit = false;
			timer = 0;
		}
	}


	private void InExitActivation()
	{
		timer += Time.deltaTime;
		if (timer >= timeToInactivePlayer)
		{
			player.GetComponent<fps_PlayreHealth>().DisableInput();

		}

		if (timer >= timeToRestart)
		{
			fader.endScene();
		}
	
	}

}
