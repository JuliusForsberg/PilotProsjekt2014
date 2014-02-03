using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {

	public GameObject projectile;
	public Transform muzzle;
	public Transform tilt;

	public float turnSpeed = 2f;
	public float reloadTime = 1f;
	public float delayAfterShot = 0.25f;
	public float range = 8f;
	public float damage = 20f;
	public float projectileCurveHeight = 0.5f;
	public float projectileSpeed = 1.0f;

	private float nextFireTime = 0f;
	private float nextMoveTime = 0f;
	
	private ArrayList enemyList = new ArrayList();
	private Enemy currentEnemy;

	private Quaternion defaultRotation;
	private Quaternion defaultTiltRotation;
	
	void Start(){
		defaultRotation = transform.localRotation;
		defaultTiltRotation = tilt.localRotation;
		transform.GetComponent<SphereCollider>().radius = range;
	}
	
	void Update(){
		if(enemyList.Count > 0){
			if(!currentEnemy){
				//Debug.Log("SetNewTarget");
				currentEnemy = (Enemy)enemyList[0];
				nextFireTime = Time.time + reloadTime;

				if(currentEnemy.isDead()){
					//Debug.Log ("RemoveTarget");
					enemyList.Remove(currentEnemy);
					currentEnemy = null;
					return;
				}
			} 
			if(currentEnemy){
				if(Time.time >= nextMoveTime){
					Vector3 relativePos = currentEnemy.transform.position - transform.position;
					Quaternion desiredRotation = Quaternion.LookRotation(relativePos);
					Quaternion desiredTiltRotation = desiredRotation;

					//Use the rotation for the tilt.
					desiredTiltRotation.eulerAngles = new Vector3(desiredTiltRotation.eulerAngles.x - (15f + ((relativePos.sqrMagnitude/range)*2f)), 0f, 0f);
					tilt.localRotation = Quaternion.Lerp(tilt.localRotation, desiredTiltRotation, turnSpeed*Time.deltaTime);

					//Only use the y axis.
					desiredRotation.eulerAngles = new Vector3(0f, desiredRotation.eulerAngles.y, 0f);
					transform.localRotation = Quaternion.Lerp(transform.localRotation, desiredRotation, turnSpeed*Time.deltaTime);
				}
				if(Time.time >= nextFireTime){
					Fire();
				}
			}
		}else if(Quaternion.Angle(transform.localRotation, defaultRotation) > 0.1f){
			transform.localRotation = Quaternion.Lerp(transform.localRotation, defaultRotation, turnSpeed*Time.deltaTime);
		}else if(Quaternion.Angle(tilt.localRotation, defaultTiltRotation) > 0.1f){
			tilt.localRotation = Quaternion.Lerp(tilt.localRotation, defaultTiltRotation, turnSpeed*Time.deltaTime);
		}
	}
	
	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag=="Enemy"){
			//Debug.Log("Enter: " + col.gameObject);
			enemyList.Add(col.gameObject.GetComponent<Enemy>());
		}
	}
	
	void OnTriggerExit(Collider col){
		if(col.gameObject.tag=="Enemy"){
			//Debug.Log("Exit: " + col.gameObject);
			enemyList.Remove(col.gameObject.GetComponent<Enemy>());

			if(col.gameObject.GetComponent<Enemy>() == currentEnemy){
				currentEnemy = null;
			}
		}
	}

	private void Fire(){
		nextFireTime = Time.time + reloadTime;
		nextMoveTime = Time.time + delayAfterShot;

		GameObject projectileObject = Instantiate(projectile, muzzle.position, muzzle.rotation) as GameObject;
		projectileObject.GetComponent<Projectile>().Initialize(currentEnemy, damage, projectileSpeed, projectileCurveHeight);
	}
}
