using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class fps_PlayerParameter : MonoBehaviour {

	[HideInInspector]
	public Vector2 inputSmoothLook;
	[HideInInspector]
	public Vector2 inputMoveVector;
	[HideInInspector]
	public bool inputCrouch;//是否是蹲下
	[HideInInspector]
	public bool inputJump;//是否跳跃
	[HideInInspector] 
	public bool inputSprint;//是否冲刺
	[HideInInspector]
	public bool inputFire;//是否开火
	[HideInInspector]
	public bool inputReload;//是否重新装填子弹


}
