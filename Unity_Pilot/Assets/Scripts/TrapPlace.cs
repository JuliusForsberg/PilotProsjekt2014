using UnityEngine;
using System.Collections;

public class TrapPlace : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	RaycastHit rayHit;
	public float gridSizeX=1;
	public float gridSizeZ=1;
	GameObject highLightObject;
	GameObject ball;

	void Update () {
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit[] rayHits = Physics.RaycastAll(ray, 10);

		for(int i=0;i<rayHits.Length;i++)
		{
			if(rayHits[i].collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
			{
				if(highLightObject == null)
				{
					highLightObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
					highLightObject.transform.localScale = new Vector3(gridSizeX, 1, gridSizeZ);
				}

				if(ball == null)
				{
					ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					ball.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
					//ball.renderer.material.color = new Vector4(1.0f, 1.0f, 1.0f, 0.1f);
					ball.collider.enabled = false;
				}
				ball.transform.position = new Vector3(rayHits[i].point.x, rayHits[i].point.y, rayHits[i].point.z);

				float x = rayHits[i].point.x;
				float z = rayHits[i].point.z;

				if( (x - Mathf.Floor(x)) < 0.5f)
				{
					print ("From" + x + " To" + (Mathf.Floor(x)));
					x = Mathf.Floor(x);
				}
				else
				{
					print ("From" + x + " To" + (Mathf.Floor(x)-0.5f));
					x = Mathf.Ceil(x);
				}

				if( (z - Mathf.Floor(z)) < 0.5f)
				{
					z = Mathf.Floor(z);
				}
				else
				{
					z = Mathf.Ceil(z);
				}

				highLightObject.transform.position = new Vector3(x, rayHits[i].point.y, z);

				break;

			}
			else
			{
				Destroy (highLightObject);
			}
		}
		Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
	}
}
