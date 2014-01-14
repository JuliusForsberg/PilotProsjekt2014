using UnityEngine;
using System.Collections;

public class TrapMelee : MonoBehaviour {

	public float cooldownTime = 1f;
	public float damage = 20f;
	public int uses = 1;

	private float nextTriggerTime;

	private ArrayList enemyList = new ArrayList();

	void Start(){
		nextTriggerTime = 0f;
	}

	void Update(){
		if(enemyList.Count > 0){
			if(Time.time >= nextTriggerTime){
				Trigger ();
			}
		}
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag=="Enemy"){
			//Debug.Log("Enter: " + col.gameObject);
			enemyList.Add(col.gameObject);
		}
	}

	void OnTriggerExit(Collider col){
		if(col.gameObject.tag=="Enemy"){
			//Debug.Log("Exit: " + col.gameObject);
			enemyList.Remove(col.gameObject);
		}
	}

	private void Trigger(){
		uses--;
		nextTriggerTime = Time.time + cooldownTime;

		for(int i=0; i<enemyList.Count; i++){
			GameObject enemy = (GameObject)enemyList[i];
			enemy.GetComponent<Enemy>().TakeDamage(damage);

			//If the enemy died by the attack, remove it, "i" is reduced because the next object in the enemyList will have the same index.
			if(enemy.GetComponent<Enemy>().isDead()){
				enemyList.Remove(enemy);
				i--;
			}
		}

		if(uses <= 0){
			Destroy(gameObject);
		}
	}
}
