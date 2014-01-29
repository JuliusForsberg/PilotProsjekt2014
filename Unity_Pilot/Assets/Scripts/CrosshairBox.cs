using UnityEngine;
using System.Collections;

public class MyGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		GUI.Box(new Rect((Screen.width/2)-0.5f, (Screen.height/2)-0.5f, 1, 1), "x");
	}
}
