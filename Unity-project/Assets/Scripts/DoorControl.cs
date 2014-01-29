using UnityEngine;
using System.Collections;

public class DoorControl : MonoBehaviour {

	
	// Update is called once per frame
	void OnTriggerEnter (Collider other){
		if(other.gameObject.tag=="Player"){
		animation.Play("DoorOpen");
		}
	}

	void OnTriggerExit (Collider other){
		if(other.gameObject.tag=="Player"){
		animation.Play("DoorClose");
		}
	}
	
}