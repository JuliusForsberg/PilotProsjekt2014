using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Porridge : MonoBehaviour {

	public float mass = 20f;
	public float depletionSpeed = 1f;

	public int numberAtTheTime = 3;

	private List<Enemy> enemyList = new List<Enemy>();

	void Update(){
		if(enemyList.Count > 0){
			for(int i=0; i<enemyList.Count; i++){
				if(enemyList[i].isDead()){
					enemyList.Remove(enemyList[i]);
					i--;
				}else{
					mass -= depletionSpeed * Time.deltaTime;
				}
			}
			if(mass <= 0f){
				Destroy (gameObject);
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag=="Enemy"){
			if(enemyList.Count < numberAtTheTime){
				other.GetComponent<Enemy>().SetPorridge(this);
				enemyList.Add(other.GetComponent<Enemy>());
			}
		}
	}
}
