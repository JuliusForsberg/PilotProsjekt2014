using UnityEngine;
using System.Collections;

public class TowerPorridge : MonoBehaviour {
	
	public GameObject projectile;
	public Transform muzzle;
	public Transform tilt;
	public Transform target;

	public float turnSpeed = 2f;
	public float cooldownTime = 1f;
	public float delayAfterShot = 0.25f;
	public float range = 8f;

	public float projectileCurveHeight = 0.5f;
	public float projectileSpeed = 1.0f;
	
	private float nextFireTime = 0f;
	private float nextMoveTime = 0f;

	private GameObject porridge;
	//
	//Curve points.
	private float startPointX;
	private float startPointY;
	private float startPointZ;
	private float controlPointX;
	private float controlPointY;
	private float controlPointZ;
	private float endPointX;
	private float endPointY;
	private float endPointZ;
	
	private float curveX;
	private float curveY;
	private float curveZ;
	private float distanceModifier;
	private float bezierTime = 0f;

	private bool isActive = false;
	private bool nextFired = false;
	
	private float speed = 1.0f;

	void Start(){
		nextFireTime = Time.time + cooldownTime;
		transform.GetComponent<SphereCollider>().radius = range;
	}
	
	void Update(){
		if(porridge == null){
			if(nextFired){
				nextFireTime = Time.time + cooldownTime;
				nextFired = false;
			}

			if(Time.time >= nextMoveTime){
				Vector3 relativePos = target.position - transform.position;
				Quaternion desiredRotation = Quaternion.LookRotation(relativePos);
				Quaternion desiredTiltRotation = desiredRotation;
				
				//Use the rotation for the tilt.
				desiredTiltRotation.eulerAngles = new Vector3(desiredTiltRotation.eulerAngles.x - (15f + ((relativePos.sqrMagnitude/range)*2f)), 0f, 0f);
				tilt.localRotation = Quaternion.Lerp(tilt.localRotation, desiredTiltRotation, turnSpeed*Time.deltaTime);
				
				//Only use the y axis.
				desiredRotation.eulerAngles = new Vector3(0f, desiredRotation.eulerAngles.y, 0f);
				transform.localRotation = Quaternion.Lerp(transform.localRotation, desiredRotation, turnSpeed*Time.deltaTime);
			}
			if(Time.time >= nextFireTime && porridge == null){
				Fire();
			}

		}else{
			if(isActive){
				Projectile ();
			}
		}
	}

	private void Projectile(){
		bezierTime += (speed/distanceModifier)*Time.deltaTime;

		if(bezierTime >= 1){
			bezierTime = 1;
		}
		
		curveX = (((1-bezierTime)*(1-bezierTime)) * startPointX) + (2 * bezierTime * (1 - bezierTime) * controlPointX) + ((bezierTime * bezierTime) * endPointX);
		curveY = (((1-bezierTime)*(1-bezierTime)) * startPointY) + (2 * bezierTime * (1 - bezierTime) * controlPointY) + ((bezierTime * bezierTime) * endPointY);
		curveZ = (((1-bezierTime)*(1-bezierTime)) * startPointZ) + (2 * bezierTime * (1 - bezierTime) * controlPointZ) + ((bezierTime * bezierTime) * endPointZ);

		porridge.transform.position = new Vector3(curveX, curveY, curveZ);

		if(bezierTime >= 1){
			bezierTime = 0;
			isActive = false;
		}
	}
	
	private void Fire(){
		nextMoveTime = Time.time + delayAfterShot;
		
		porridge = Instantiate(projectile, muzzle.position, Quaternion.identity) as GameObject;

		//TEST
		//Destroy (porridge, 4f);
		//----

		startPointX = transform.position.x;
		startPointY = transform.position.y;
		startPointZ = transform.position.z;
		endPointX = target.transform.position.x;
		endPointY  = target.transform.position.y;
		endPointZ  = target.transform.position.z;

		Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
		randomDirection.Normalize();
		Vector3 vector = randomDirection * Random.Range(0f, 3f);

		endPointX = target.transform.position.x + vector.x;
		endPointZ = target.transform.position.z + vector.z;

		/*GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.position = new Vector3(endPointX, endPointY, endPointZ);
		sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);*/


		//Temporarily holds just the distance, so we can use it in controlPointY, then apply the modifier after.
		distanceModifier = Vector3.Distance(target.transform.position, transform.position);
		
		controlPointX = Mathf.Lerp(endPointX, startPointX, 0.5f);
		controlPointY = startPointY + (distanceModifier*projectileCurveHeight);
		controlPointZ = Mathf.Lerp(endPointZ, startPointZ, 0.5f);
		
		distanceModifier *= 0.1f;

		isActive = true;
		nextFired = true;
	}
}
