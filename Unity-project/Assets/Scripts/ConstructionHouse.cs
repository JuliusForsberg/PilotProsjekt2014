using UnityEngine;
using System.Collections;

public class ConstructionHouse : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	Camera topDown;
	// Update is called once per frame
	void Update () {
	
        if(Input.GetKeyDown(KeyCode.B))
            GameObject.Find("CameraTopDown").SendMessage("startConstruction");


	}

	void OnTriggerEnter (Collider collider) {
		if(collider.gameObject.tag == "Player")
		{
            collider.transform.position = transform.position + new Vector3(0, 0, 2);
			GameObject.Find("CameraTopDown").SendMessage("startConstruction");
		}
	}
}
