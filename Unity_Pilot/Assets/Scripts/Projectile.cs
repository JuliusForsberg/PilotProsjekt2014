using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
		
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
	private float bezierTime;

	private Transform target;
	private float damage;

	void Start(){
		bezierTime = 0f;
	}

	void Update(){
		bezierTime += Time.deltaTime/distanceModifier;

		if(bezierTime > 2){
			Destroy(gameObject);
			return;
		}
		//Can't use target.position directly when calculating the curves, because the projectiles needs an end point even if the target is destroyed.
		if(target){
			endPointX = target.position.x;
			endPointY  = target.position.y;
			endPointZ  = target.position.z;

			controlPointX = Mathf.Lerp(endPointX, startPointX, 0.5f);
			controlPointZ = Mathf.Lerp(endPointZ, startPointZ, 0.5f);
		}

		curveX = (((1-bezierTime)*(1-bezierTime)) * startPointX) + (2 * bezierTime * (1 - bezierTime) * controlPointX) + ((bezierTime * bezierTime) * endPointX);
		curveY = (((1-bezierTime)*(1-bezierTime)) * startPointY) + (2 * bezierTime * (1 - bezierTime) * controlPointY) + ((bezierTime * bezierTime) * endPointY);
		curveZ = (((1-bezierTime)*(1-bezierTime)) * startPointZ) + (2 * bezierTime * (1 - bezierTime) * controlPointZ) + ((bezierTime * bezierTime) * endPointZ);

		Vector3 newPosition = new Vector3(curveX, curveY, curveZ);
		transform.LookAt(newPosition);
		float distance = Vector3.Distance(newPosition, transform.position);

		transform.Translate(Vector3.forward*distance);
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.transform == target){
			target.GetComponent<Enemy>().TakeDamage(damage);
			Destroy(gameObject);
		}else if(col.gameObject.tag!="Tower"){
			//Destroy(gameObject);
		}
	}

	public void Initialize(Transform enemy, float dmg){
		target = enemy;
		damage = dmg;
		
		startPointX = transform.position.x;
		startPointY = transform.position.y;
		startPointZ = transform.position.z;
		endPointX = target.position.x;
		endPointY  = target.position.y;
		endPointZ  = target.position.z;
		
		//Temporarily holds just the distance, so we can use it in controlPointY, then apply the modifier after.
		distanceModifier = Vector3.Distance(target.position, transform.position);
		
		controlPointX = Mathf.Lerp(endPointX, startPointX, 0.5f);
		controlPointY = startPointY + (distanceModifier*0.5f);
		controlPointZ = Mathf.Lerp(endPointZ, startPointZ, 0.5f);
		
		distanceModifier *= 0.1f;
	}
}
