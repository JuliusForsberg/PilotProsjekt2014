using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

	public float health;
	
	public void TakeDamage(float dmg){
		//Debug.Log("Gate took damage");
		health -= dmg;
		
		if(health <= 0){
			Debug.Log("Gate destroyed");
			gameObject.collider.enabled = false;

			for(int i=0; i<transform.childCount; i++){
				transform.GetChild(i).gameObject.SetActive(false);
			}
		}
	}

	public bool isDestroyed(){
		return health <= 0 ? true : false;
	}
}
