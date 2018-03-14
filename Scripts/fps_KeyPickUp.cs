using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_KeyPickUp : MonoBehaviour {

	public AudioClip keyGrab;
	public int keyId;

	private GameObject player;
	private fps_PlayerInventory playreInventory;


	void Start()
	{
		player = GameObject.FindWithTag(Tags.player);
		playreInventory = player.GetComponent<fps_PlayerInventory>();

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == player)
		{
			AudioSource.PlayClipAtPoint(keyGrab, transform.position);
			playreInventory.AddKey(keyId);
			Destroy(gameObject);
		}
	}

}
