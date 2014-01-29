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

	void Start(){
		defaultDistance = distance;
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
	}

	void LateUpdate(){
		if (target) {
			RaycastHit hit;

			if(Physics.Raycast(target.position, transform.position - target.position, out hit, defaultDistance)){
				distance = hit.distance-0.5f;
			}else{
				distance = defaultDistance;
			}

			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			
			Quaternion rotation = Quaternion.Euler(y, x, 0);
			Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

			transform.rotation = rotation;
			transform.position = position;
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