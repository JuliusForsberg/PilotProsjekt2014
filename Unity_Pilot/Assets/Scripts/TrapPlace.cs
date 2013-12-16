using UnityEngine;
using System.Collections;

public class TrapPlace : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	RaycastHit rayHit;

	void Update () {
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		if(Physics.Raycast(ray, out rayHit, 10) && rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Environment") )
		{
			Object highLightObject = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), rayHit.point, Quaternion.identity);
			Destroy(highLightObject, 1);
		}
		Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
	}
}
