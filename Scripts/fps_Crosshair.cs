using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//十字准星
public class fps_Crosshair : MonoBehaviour {

	public float length;//准星长度 
	public float width;//准星宽度
	public float distance;
	public Texture2D crosshairTexture;

	private GUIStyle lineStyle;
	private Texture tex;

	void Start()
	{
		lineStyle = new GUIStyle();
		lineStyle.normal.background = crosshairTexture;

	}

	void OnGUI()
	{
		GUI.Box(new Rect((Screen.width-distance)/2-length,(Screen.height-width)/2,length,width),tex,lineStyle);
		GUI.Box(new Rect((Screen.width+distance)/2,(Screen.height-width)/2,length,width),tex,lineStyle);
		GUI.Box(new Rect((Screen.width-width)/2,(Screen.height-distance)/2-length,width,length),tex,lineStyle);
		GUI.Box(new Rect((Screen.width-width)/2,(Screen.height+distance)/2,width,length),tex,lineStyle);
	}

}
