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

	private float offsetX;
	private float offsetY;
	private float offsetZ;

	private float curveX;
	private float curveY;
	private float curveZ;
	private float distanceModifier;
	private float bezierTime;

	private Transform target;
	private float damage;

	private bool isActive;
	private bool reachedEnd;

	void Start(){
		bezierTime = 0f;
		isActive = true;
		reachedEnd = false;
	}

	void Update(){
		if(!isActive)
			return;

		bezierTime += Time.deltaTime/distanceModifier;

		if(bezierTime >= 1){

			if(bezierTime > 2){
				Destroy(gameObject);
				return;
			}

			if(!reachedEnd){
				if(target){
					target.GetComponent<Enemy>().TakeDamage(damage);

					Destroy(gameObject);
					return;
				}else{
					reachedEnd = true;
					gameObject.collider.enabled = true;
				}
			}
		}

		//Can't use target.position directly when calculating the curves, because the projectiles needs an end point even if the target is destroyed.
		if(target){
			endPointX = target.position.x + offsetX;
			endPointY  = target.position.y + offsetY;
			endPointZ  = target.position.z + offsetZ;

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
		if(col.gameObject.tag=="Ground"){
			isActive = false;

			Destroy(gameObject, 2.0f);
		}
	}

	public void Initialize(Transform enemy, float dmg){
		target = enemy;
		damage = dmg;

		offsetX = Random.Range (-0.3f, 0.3f);
		offsetY = Random.Range (-0.4f, 0.4f);
		offsetZ = Random.Range (-0.3f, 0.3f);

		startPointX = transform.position.x;
		startPointY = transform.position.y;
		startPointZ = transform.position.z;
		endPointX = target.position.x + offsetX;
		endPointY  = target.position.y + offsetY;
		endPointZ  = target.position.z + offsetZ;
		
		//Temporarily holds just the distance, so we can use it in controlPointY, then apply the modifier after.
		distanceModifier = Vector3.Distance(target.position, transform.position);
		
		controlPointX = Mathf.Lerp(endPointX, startPointX, 0.5f);
		controlPointY = startPointY + (distanceModifier*0.5f);
		controlPointZ = Mathf.Lerp(endPointZ, startPointZ, 0.5f);
		
		distanceModifier *= 0.1f;
	}
}
