using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float health = 20f;	

	void Start(){
	
	}

	void Update(){
	
	}

	public void TakeDamage(float damage){
		health -= damage;
		Debug.Log("TookDamage");

		if(health <= 0){
			Debug.Log("DIED");
	
			StartCoroutine(WaitAndDie());
			//Destroy (gameObject);
		}
	}

	public bool isDead(){
		if(health <= 0){
			return true;
		}else{
			return false;
		}
	}

	private IEnumerator WaitAndDie(){
		yield return new WaitForSeconds(0.2f);
		Destroy(gameObject);
	}
}
