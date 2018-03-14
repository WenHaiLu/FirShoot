using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
	None,
	Idle,
	Walk,
	Crouch,
	Run
}

public class fps_PlayerControl : MonoBehaviour {
	private PlayerState state = PlayerState.None;

	public PlayerState State
	{ 
		get {
			if (running)
				state = PlayerState.Run;
			else if (walking)
				state = PlayerState.Walk;
			else if (crouching)
				state = PlayerState.Crouch;
			else
				state = PlayerState.Idle;
			return state;
		}
	}


	//跑状态下
	public float sprintSpeed = 10.0f;
	public float sprintJumpSpeed = 8.0f;

	//正常状态下
	public float normalSpeed = 6.0f;
	public float normalJumpSpeed = 7.0f;

	//蹲伏状态下
	public float crouchSpeed = 2.0f;
	public float crouchJumpSpeed = 5.0f;

	//蹲下高度
	public float crouchDeltaHeight = 0.5f;

	//重力
	public float gravity = 20.0f;

	//相机移动速度
	public float CameraMoveSpeed = 8.0f;
	//跳跃音效
	public AudioClip jumpAudio;

	private float speed;
	private float jumpSpeed;
	private Transform mainCamera;
	private float standarCamHeight;// 站立状态下相机高度
	private float crouchCamHeight;//蹲下状态下相机高度
	private bool grounded = false;
	private bool walking = false;
	private bool crouching = false;
	private bool stopCrouching = false;
	private bool running = false;
	private Vector3 normalControllerCenter = Vector3.zero;
	private float normalControllerHeight = 0.0f;
	private float timer = 0.0f;
	private CharacterController controller;
	private AudioSource audioSource;
	private fps_PlayerParameter parameter;
	private Vector3 moveDirection = Vector3.zero;

	//初始化
	void Start()
	{
		crouching = false;
		walking = false;
		running = false;
		speed = normalSpeed;
		jumpSpeed = normalJumpSpeed;
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
		standarCamHeight = mainCamera.localPosition.y;
		crouchCamHeight = standarCamHeight - crouchDeltaHeight;
		audioSource = GetComponent<AudioSource>();
		controller = GetComponent<CharacterController>();
		parameter = GetComponent<fps_PlayerParameter>();
		normalControllerCenter = controller.center;
		normalControllerHeight = controller.height;
	}

	private void FixedUpdate()
	{
		UpdateMove();
		AudioManagemnet();
	}

	//控制玩家移动
	private void UpdateMove()
	{
		if (grounded)
		{
			//设置玩家方向
			moveDirection = new Vector3(parameter.inputMoveVector.x, 0, parameter.inputMoveVector.y);
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			//跳跃
			if (parameter.inputJump)
			{
				moveDirection.y = jumpSpeed;
				AudioSource.PlayClipAtPoint(jumpAudio, transform.position);
				CurrentSpeed();

			}

		}

		moveDirection.y -= gravity * Time.deltaTime;
		CollisionFlags flags = controller.Move(moveDirection * Time.deltaTime);
		grounded = (flags & CollisionFlags.CollidedBelow) != 0;


		if (Mathf.Abs(moveDirection.x) > 0 && grounded || Mathf.Abs(moveDirection.z) > 0 && grounded)
		{
			if (parameter.inputSprint)
			{
				walking = false;
				running = true;
				crouching = false;
			}
			else if (parameter.inputCrouch)
			{
				crouching = true;
				walking = false;
				running = false;
			}
			else
			{
				walking = true;
				crouching = false;
				running = false;
			}


		}
		else 
		{
			if (walking)
				walking = false;
			if (running)
				running = false;
			if (parameter.inputCrouch)
				crouching = true;
			else
				crouching = false;
		}

		if (crouching)
		{
			
			controller.height = normalControllerHeight - crouchDeltaHeight;
			controller.center = normalControllerCenter - new Vector3(0, crouchDeltaHeight / 3, 0);
		}
		else
		{
			controller.height = normalControllerHeight;
			controller.center = normalControllerCenter;
		}
		UpdateCrouch();
		CurrentSpeed();
		//Debug.Log(crouching);


	}



	//根据当前玩家的状态进行speed赋值
	private void CurrentSpeed()
	{
		switch (State)
		{
			case PlayerState.Idle:
				speed = normalSpeed;
				jumpSpeed = normalJumpSpeed;
				break;
			case PlayerState.Walk:
				speed = normalSpeed;
				jumpSpeed = normalJumpSpeed;
				break;
			case PlayerState.Crouch:
				speed = crouchSpeed;
				jumpSpeed = crouchJumpSpeed;
				break;
			case PlayerState.Run:
				speed = sprintSpeed;
				jumpSpeed = sprintJumpSpeed;
				break;

		}


	
	}

	// 玩家音效管理
	private void AudioManagemnet()
	{
		if (State == PlayerState.Walk)
		{
			audioSource.pitch = 1.0f;
			if (!audioSource.isPlaying)
				audioSource.Play();
		}
		else if (State == PlayerState.Run)
		{
			audioSource.pitch = 1.3f;
			if (!audioSource.isPlaying)
				audioSource.Play();
		}
		else audioSource.Stop();
	}

	//蹲伏状态下的相机高度渐变封装
	private void UpdateCrouch()
	{
		if (crouching)
		{
			if (mainCamera.localPosition.y > crouchCamHeight)
			{
				if (mainCamera.localPosition.y - (crouchDeltaHeight * Time.deltaTime * CameraMoveSpeed) < crouchCamHeight)
					mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, crouchCamHeight, mainCamera.localPosition.z);
				else
					mainCamera.localPosition -= new Vector3(0, crouchDeltaHeight * Time.deltaTime * CameraMoveSpeed, 0);

			}
			else
				mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, crouchCamHeight, mainCamera.localPosition.z);

		}
		else
		{
			if (mainCamera.localPosition.y < standarCamHeight)
			{
				//Debug.Log("松开C");
				if (mainCamera.localPosition.y + (crouchDeltaHeight * Time.deltaTime * CameraMoveSpeed) > standarCamHeight)
					mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, standarCamHeight, mainCamera.localPosition.z);
				else
					mainCamera.localPosition += new Vector3(0, crouchDeltaHeight * Time.deltaTime * CameraMoveSpeed, 0);
				

			}
			else
				mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, standarCamHeight, mainCamera.localPosition.z);

		}
	}
}
