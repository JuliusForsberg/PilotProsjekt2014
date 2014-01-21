using UnityEngine;
using System.Collections;

public class Crystal : MonoBehaviour {

	public float health;

	public void TakeDamage(float dmg){
		//Debug.Log("Crystal took damage");
		health -= dmg;
				
		if(health <= 0){
			Debug.Log("GAME OVER: Crystal destroyed");
		}
	}
}
