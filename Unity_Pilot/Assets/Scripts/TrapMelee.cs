using UnityEngine;
using System.Collections;

public class TrapMelee : MonoBehaviour {

	public float cooldownTimer = 1f;
	private float timer;

	public float damage = 20f;
	public int uses = 1;

	private bool cooldown;
	private bool triggered;

	private ArrayList enemyList = new ArrayList();

	void Start(){
		timer = 0;
		cooldown = false;
		triggered = false;
	}

	void Update(){
		if(cooldown){
			timer += Time.deltaTime;

			if(timer > cooldownTimer){
				timer = 0;
				cooldown = false;
			}
		}

		if(triggered && !cooldown){
			uses--;

			if(enemyList.Count > 0){
				for(int i=0; i<enemyList.Count; i++){
					GameObject enemy = (GameObject)enemyList[i];
					enemy.GetComponent<Enemy>().TakeDamage(damage);

					if(enemy.GetComponent<Enemy>().isDead()){
						enemyList.Remove(enemy);
						Debug.Log ("Remove");
					}
				}
			}

			cooldown = true;
			Debug.Log ("TrapTriggered");

			if(enemyList.Count == 0){
				triggered = false;
			}
			if(uses <= 0){
				Destroy(gameObject);
			}
		}
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag=="Enemy"){
			Debug.Log("Enter: " + col.gameObject);
			enemyList.Add(col.gameObject);
			triggered = true;
		}
	}

	void OnTriggerExit(Collider col){
		if(col.gameObject.tag=="Enemy"){
			Debug.Log("Exit: " + col.gameObject);
			enemyList.Remove(col.gameObject);
		}
	}
}
