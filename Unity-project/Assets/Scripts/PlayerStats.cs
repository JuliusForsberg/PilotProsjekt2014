using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	public float health = 50f;
	public float damage = 2f;
	public float attackSpeed = 1f;

	public void TakeDamage (float dmg) {
		//Debug.Log("Player took damage");
		health -= dmg;
		
		if(health <= 0){
			Debug.Log("GAME OVER: Player Dead");
		}
	}
}
