using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//动画辅助
public class AnimatorSetup {

	public float speedDampTime = 0.1f;//速度缓冲时间
	public float angularSpeedDampTime = 0.7f;//角速度缓冲时间
	public float angleResposeTime = 1f;//角速度应答时间

	private Animator anim;
	private HashIDs hash;

	public AnimatorSetup(Animator anim,HashIDs hash)
	{
		this.anim = anim;
		this.hash = hash;

	}

	public void Setup(float speed, float angle)
	{
		float angularSpeed = angle / angleResposeTime;//角速度

		anim.SetFloat(hash.SpeedFloat,speed,speedDampTime,Time.deltaTime);
		anim.SetFloat(hash.angularSpeedFloat, angularSpeed, angularSpeedDampTime, Time.deltaTime);


	}


}
