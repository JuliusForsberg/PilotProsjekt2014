using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {

	public GameObject projectile;
	public Transform muzzle;

	public float turnSpeed = 5f;
	public float reloadTime = 1f;
	public float delayAfterShot = 0.25f;
	public float range = 8f;
	public float damage = 20f;

	private float nextFireTime;
	private float nextMoveTime;
	private Quaternion desiredRotation;
	
	private ArrayList enemyList = new ArrayList();
	private GameObject currentEnemy;

	private Quaternion defaultRotation;
	
	void Start(){
		nextFireTime = 0f;
		nextMoveTime = 0f;

		defaultRotation = transform.rotation;
		transform.GetComponent<SphereCollider>().radius = range;
	}
	
	void Update(){
		if(enemyList.Count > 0){
			if(!currentEnemy){
				//Debug.Log("SetNewTarget");
				currentEnemy = (GameObject)enemyList[0];
				nextFireTime = Time.time + reloadTime;

				if(!currentEnemy){
					//Debug.Log ("RemoveTarget");
					enemyList.Remove(currentEnemy);
					currentEnemy = null;
					return;
				}
			} 
			if(currentEnemy){
				if(Time.time >= nextMoveTime){
					Vector3 relativePos = currentEnemy.transform.position - transform.position;
					desiredRotation = Quaternion.LookRotation(relativePos);
					desiredRotation.eulerAngles = new Vector3(0f, desiredRotation.eulerAngles.y, 0f);

					transform.localRotation = Quaternion.Lerp(transform.localRotation, desiredRotation, Time.deltaTime*turnSpeed);
				}
				if(Time.time >= nextFireTime){
					Fire();
				}
			}
		}else if(Quaternion.Angle(transform.localRotation, defaultRotation) > 0.1f){
			transform.localRotation = Quaternion.Lerp(transform.rotation, defaultRotation, Time.deltaTime*turnSpeed);
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

			if(col.gameObject == currentEnemy){
				currentEnemy = null;
			}
		}
	}

	private void Fire(){
		nextFireTime = Time.time + reloadTime;
		nextMoveTime = Time.time + delayAfterShot;

		GameObject projectileObject = Instantiate(projectile, muzzle.position, muzzle.rotation) as GameObject;
		projectileObject.GetComponent<Projectile>().Initialize(currentEnemy.transform, damage);
	}
}
