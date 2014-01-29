using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

	public float health;
	
	public void TakeDamage(float dmg){
		//Debug.Log("Gate took damage");
		health -= dmg;
		
		if(health <= 0){
			Debug.Log("Gate destroyed");
			Destroy(gameObject, 0.2f);
		}
	}

	public bool isDestroyed(){
		return health <= 0 ? true : false;
	}
}
