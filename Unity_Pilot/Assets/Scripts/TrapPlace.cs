using UnityEngine;
using System.Collections;

public class TrapPlace : MonoBehaviour {
	// Use this for initialization
	void Start () {
		hitObjects = new GameObject[3];
	}
	public GameObject[] hitObjects;
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
				hitObjects[i] = rayHits[i].collider.gameObject;
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

				x = gridSizeX*(Mathf.Round(x/gridSizeX));
				z = gridSizeZ*(Mathf.Round(z/gridSizeZ));

				highLightObject.transform.position = new Vector3(x, rayHits[i].point.y, z);

				break;

			}
		}
		if(rayHits.Length == 0)
		{
			Destroy(highLightObject);
		}
		Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
	}
}
