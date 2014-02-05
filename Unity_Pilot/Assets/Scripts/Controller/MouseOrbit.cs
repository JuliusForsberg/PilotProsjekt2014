using UnityEngine;
using System.Collections;

public class MouseOrbit : MonoBehaviour{

	public Transform target;
	public float distance = 5.0f;

	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;

	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;

	private float x = 0.0f;
	private float y = 0.0f;
	private float defaultDistance;

	private Transform tr;

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
			if(defaultDistance > 2f){
				defaultDistance -= 0.2f;
			}
		}else if(Input.GetAxis("Mouse ScrollWheel") < 0){
			if(defaultDistance < 10f){
				defaultDistance += 0.2f;
			}
		}
	}

	void LateUpdate(){
		if (target) {
			RaycastHit hit;
			Vector3 rayVector = tr.position - target.position;

			//Player to Camera
			/*if(Physics.Raycast(target.position, rayVector, out hit, rayVector.magnitude)){
				distance = Mathf.Lerp (distance, hit.distance, 2f * Time.deltaTime);
			}*/

			//Down
			rayVector = (tr.position - tr.up) - target.position;
			if(Physics.Raycast(target.position, rayVector, out hit, rayVector.magnitude)){
				Debug.DrawRay(target.position, hit.point - target.position, Color.green);
				if(hit.distance < distance)
					distance = Mathf.Lerp (distance, hit.distance, 2f * Time.deltaTime);
			}
			//Left
			rayVector = (tr.position - tr.right) - target.position;
			if(Physics.Raycast(target.position, rayVector, out hit, rayVector.magnitude)){
				Debug.DrawRay(target.position, hit.point - target.position, Color.yellow);
				if(hit.distance < distance)
					distance = Mathf.Lerp (distance, hit.distance, 2f * Time.deltaTime);
			}
			//Right
			rayVector = (tr.position + tr.right) - target.position;
			if(Physics.Raycast(target.position, rayVector, out hit, rayVector.magnitude)){
				Debug.DrawRay(target.position, hit.point - target.position, Color.red);
				if(hit.distance < distance)
					distance = Mathf.Lerp (distance, hit.distance, 2f * Time.deltaTime);
			}
			//Up
			rayVector = (tr.position + tr.up) - target.position;
			if(Physics.Raycast(target.position, rayVector, out hit, rayVector.magnitude)){
				Debug.DrawRay(target.position, hit.point - target.position, Color.blue);
				if(hit.distance < distance)
					distance = Mathf.Lerp (distance, hit.distance, 2f * Time.deltaTime);
			}

			if(hit.distance == 0 && Mathf.Abs(distance - defaultDistance) > 0.01f){
				distance = Mathf.Lerp (distance, defaultDistance, 2f * Time.deltaTime);
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