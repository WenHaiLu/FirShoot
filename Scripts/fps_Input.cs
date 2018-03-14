using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_Input : MonoBehaviour {

	public class fps_InputAxis
	{
		public KeyCode positive;//正向
		public KeyCode nagative;//负向
	}

	public  Dictionary<string,KeyCode> buttons = new Dictionary<string, KeyCode>();

	public Dictionary<string,fps_InputAxis> axis = new Dictionary<string, fps_InputAxis>();

	public List<string> unityAxis = new List<string> ();

	void Start()
	{
//		SetupDefaults ("buttons");
//		SetupDefaults ("Axis");
//		SetupDefaults ("UnityAxis");
		SetupDefaults();
	}



	private void SetupDefaults(string type = "")
	{
		if (type == "" || type == "buttons") {
			if (buttons.Count == 0) {
				AddButton ("Fire",KeyCode.Mouse0);//开火
				AddButton ("Reload",KeyCode.R);//上弹
				AddButton ("Jump",KeyCode.Space);//跳跃
				AddButton ("Crouch",KeyCode.C);//蹲下
				AddButton ("Sprint",KeyCode.LeftShift);//奔跑
			}
		}

		if (type == "" || type == "Axis") {
			if (axis.Count == 0) {
				AddAxis ("Horizontal",KeyCode.W,KeyCode.S);
				AddAxis ("Vertical",KeyCode.A,KeyCode.D);
			}
		}

		if (type == "" || type == "UnityAxis") {
			if (unityAxis.Count == 0) {
				addUnityAxis ("Mouse X");
				addUnityAxis ("Mouse Y");
				addUnityAxis ("Horizontal");
				addUnityAxis ("Vertical");
			}
		}

	}


	private void AddButton(string n,KeyCode k)
	{
		if (buttons.ContainsKey (n)) {
			buttons [n] = k;
		} else {
		
			buttons.Add (n,k);
		}
	}

	private void AddAxis(string n,KeyCode pk,KeyCode nk)
	{
		if (axis.ContainsKey (n)) {
			axis [n] = new fps_InputAxis (){ positive = pk, nagative = nk };
		
		} else {
			axis.Add (n,new fps_InputAxis(){positive = pk,nagative = nk});
		}
	}



	private void addUnityAxis(string n)
	{
		if (!unityAxis.Contains (n)) {
			unityAxis.Add (n);
		}

	}

	public bool GetButton(string button)
	{
		
		if (buttons.ContainsKey (button)) {
			return Input.GetKey (buttons[button]);
		}
		return false;
	}

	public bool GetButtonDown(string button)
	{
		if (buttons.ContainsKey (button)) {
			return Input.GetKeyDown (buttons[button]);
		}
		return false;
	}


	public float GetAxis(string Axis)
	{
		Debug.Log (Axis);
		if (unityAxis.Contains (Axis)) {
			return Input.GetAxis (Axis);
		} else
			return 0;
	}

	public float GetAxisRaw(string Axis)
	{
		if (axis.ContainsKey (Axis)) {
			float val = 0;
			if (Input.GetKey (axis [Axis].positive))
				return 1;
			if (Input.GetKey (axis [Axis].nagative))
				return -1;
			return val;
		} else if (unityAxis.Contains (Axis)) {
			Debug.Log(Axis);
			return Input.GetAxisRaw (Axis);
		} else {
			return 0;
		}
	}
}
