using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float health = 20f;
	public float damage = 10f;
	
	void Start(){
		if(gameObject.GetComponent<AIPath>()){
			gameObject.GetComponent<AIPath>().target = GameObject.FindGameObjectWithTag("Crystal").transform;
		}
		WaveManager waveManager = GameObject.Find("_WaveManager").gameObject.GetComponent<WaveManager>();
		health *= waveManager.waveMultiplier;
	}

	public void TakeDamage(float damage){
		health -= damage;
		//Debug.Log("TookDamage");

		if(health <= 0){
			//Debug.Log("DIED");
	
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
