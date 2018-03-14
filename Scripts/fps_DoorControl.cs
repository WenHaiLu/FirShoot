using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_DoorControl : MonoBehaviour {

	public int doorId;//门ID
	public Vector3 from;//门起始地
	public Vector3 to;//门终点
	public float fadeSpeed = 5.0f;
	public bool requireKey = false;//当前门打开是否需要Key
	public AudioClip doorSwitchClip;//门允许被打开的音效
	public AudioClip accessDeniedClip;//门不被允许打开的音效


	private Transform door;
	private GameObject player;
	private AudioSource audioSource;
	private fps_PlayerInventory playerInventory;
	private int count;//代表人物数量

	public int Count
	{ 
		get {
			return count;
		}
		set {
			if (count == 0 && value == 1 || count == 1 && value == 0)
			{
				audioSource.clip = doorSwitchClip;
				audioSource.Play();
			}
			count = value;
		}
	}

	void Start()
	{
		if (transform.childCount > 0)
			door = transform.GetChild(0);
		player = GameObject.FindGameObjectWithTag(Tags.player);
		playerInventory = player.GetComponent<fps_PlayerInventory>();
		audioSource = this.GetComponent<AudioSource>();
		door.localPosition = from;
	}


	void Update()
	{
		if (Count > 0)
		{
			door.localPosition = Vector3.Lerp(door.localPosition, to, fadeSpeed * Time.deltaTime);
		}
		else
		{ 
			door.localPosition = Vector3.Lerp(door.localPosition, from, fadeSpeed* Time.deltaTime);
		}	
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == player)
		{
			if (requireKey)
			{
				if (playerInventory.HasKey(doorId))
					Count++;
				else
				{
					audioSource.clip = accessDeniedClip;
					audioSource.Play();
				}
			}
			else
			{
				Count++;
			}
		}
		else if(other.gameObject.tag == Tags.enemy && other is CapsuleCollider)
		{
			Count++;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == player || other.gameObject.tag == Tags.enemy && other is CapsuleCollider)
		{
			Count = Mathf.Max(0,Count-1);
		}
	}

}
