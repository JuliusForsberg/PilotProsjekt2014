using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

	public float health;
	private bool trollNear = false;
	private int trollCount = 0;
	private int trollIterater = 0;

	private float nextCheck;

	public void TakeDamage(float dmg){
		//Debug.Log("Gate took damage");
		health -= dmg;
		if(!animation.isPlaying)
			animation.Play("DoorSlam");
		
		if(health <= 0){
			//Debug.Log("Gate destroyed");
			Destroy(gameObject, 0.2f);
		}
	}

	public bool isDestroyed(){
		return health <= 0 ? true : false;
	}

	void Update(){
		if(trollNear){
			if(trollNear && (trollCount == 0 || trollIterater == 0)){
				trollNear = false;
			}

			if(Time.time >= nextCheck){
				trollIterater = 0;
				nextCheck = Time.time + 1f;
			}
		}
	}

	void OnTriggerEnter (Collider other){
		if(other.gameObject.tag=="Player" && !trollNear){
			animation.Play("DoorOpen");
		}else if(other.gameObject.tag=="Enemy"){
			if(animation.IsPlaying("DoorOpen") && !animation.IsPlaying("DoorClose") && !trollNear){
				animation.Play ("DoorClose");
			}
			trollCount++;
			trollNear = true;
		}
	}

	void OnTriggerStay(Collider other){
		if(other.gameObject.tag=="Enemy"){
			trollIterater++;
		}
	}
	
	void OnTriggerExit (Collider other){
		Debug.Log("TriggerExit: " + Time.time);
		if(other.gameObject.tag=="Player" && !trollNear){
			animation.Play("DoorClose");
		}else if(other.gameObject.tag=="Enemy"){
			trollCount--;
		}

	}
}
