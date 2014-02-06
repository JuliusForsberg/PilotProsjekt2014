using UnityEngine;
using System.Collections;

public class MouseOrbit : MonoBehaviour{

	public Transform target;
	public float distance = 5.0f;
	public float minDistance = 2f;
	public float maxDistance = 10f;


	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;

	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;

	private float x = 0.0f;
	private float y = 0.0f;
	private float defaultDistance;

	private Transform tr;

	public bool allowZoom = true;

	void Start(){
		tr = transform;

		defaultDistance = distance;
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
	}

	void Update() {
		if(Input.GetAxis("Mouse ScrollWheel") > 0){
			if(defaultDistance > minDistance && allowZoom){
				defaultDistance -= 0.2f;
			}
		}else if(Input.GetAxis("Mouse ScrollWheel") < 0){
			if(defaultDistance < maxDistance && allowZoom){
				defaultDistance += 0.2f;
			}
		}
	}

	void LateUpdate(){
		if (target) {
			RaycastHit hit;
			Vector3 rayVector = tr.position - target.position;
			float desiredDistance = defaultDistance;

			//Player to Camera
			if(Physics.Raycast(target.position, rayVector, out hit, rayVector.magnitude+1f)){
				Debug.DrawRay(target.position, hit.point - target.position, Color.black);
				//distance = Mathf.Lerp (distance, hit.distance, 2f * Time.deltaTime);
				if(desiredDistance > hit.distance){
					desiredDistance = hit.distance;
				}
				if(allowZoom){
					allowZoom = false;
				}
			}

			//Down
			rayVector = (tr.position - tr.up) - target.position;
			if(Physics.Raycast(target.position, rayVector, out hit, rayVector.magnitude)){
				Debug.DrawRay(target.position, hit.point - target.position, Color.green);
				//distance = Mathf.Lerp (distance, hit.distance, 2f * Time.deltaTime);
				if(desiredDistance > hit.distance){
					desiredDistance = hit.distance;
				}
				if(allowZoom){
					allowZoom = false;
				}
			}
			//Left
			rayVector = (tr.position - tr.right) - target.position;
			if(Physics.Raycast(target.position, rayVector, out hit, rayVector.magnitude)){
				Debug.DrawRay(target.position, hit.point - target.position, Color.yellow);
				//distance = Mathf.Lerp (distance, hit.distance, 2f * Time.deltaTime);
				if(desiredDistance > hit.distance){
					desiredDistance = hit.distance;
				}
				if(allowZoom){
					allowZoom = false;
				}
			}
			//Right
			rayVector = (tr.position + tr.right) - target.position;
			if(Physics.Raycast(target.position, rayVector, out hit, rayVector.magnitude)){
				Debug.DrawRay(target.position, hit.point - target.position, Color.red);
				//distance = Mathf.Lerp (distance, hit.distance, 2f * Time.deltaTime);
				if(desiredDistance > hit.distance){
					desiredDistance = hit.distance;
				}
				if(allowZoom){
					allowZoom = false;
				}
			}
			//Up
			rayVector = (tr.position + tr.up) - target.position;
			if(Physics.Raycast(target.position, rayVector, out hit, rayVector.magnitude)){
				Debug.DrawRay(target.position, hit.point - target.position, Color.blue);
				//distance = Mathf.Lerp (distance, hit.distance, 2f * Time.deltaTime);
				if(desiredDistance > hit.distance){
					desiredDistance = hit.distance;
				}
				if(allowZoom){
					allowZoom = false;
				}
			}

			if(desiredDistance != defaultDistance && Mathf.Abs(distance - desiredDistance) > 0.01f){
				distance = Mathf.Lerp (distance, desiredDistance, 2f * Time.deltaTime);
			}

			if(desiredDistance == defaultDistance && Mathf.Abs(distance - defaultDistance) > 0.01f){
				distance = Mathf.Lerp (distance, defaultDistance, 2f * Time.deltaTime);
			}else if(desiredDistance == defaultDistance && !allowZoom){
				allowZoom = true;
			}

			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			
			y = ClampAngle(y, yMinLimit, yMaxLimit);

			Quaternion rotation = Quaternion.Euler(y, x, 0);
			Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

			tr.rotation = rotation;
			tr.position = position;
		}
	}

	static float ClampAngle(float angle, float min, float max) {
		if (angle < -360f)
			angle += 360f; 
		if(angle > 360f)
			angle -= 360f;
		return Mathf.Clamp (angle, min, max);
	}
}