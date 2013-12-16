using UnityEngine;
using System.Collections;

public class TrapPlace : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	RaycastHit rayHit;
	public float gridSizeX=1;
	public float gridSizeZ=1;

	void Update () {
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit[] rayHits = Physics.RaycastAll(ray, 10);

		for(int i=0;i<rayHits.Length;i++)
		{
			if(rayHits[i].collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
			{
				float x = rayHits[i].point.x;
				float z = rayHits[i].point.z;

				if( (x - Mathf.Floor(x)) < 0.5)
				{
					print ("From" + x + " To" + (Mathf.Ceil(x)+0.5f));
					x = Mathf.Floor(x)-0.5f;
				}
				else
				{
					print ("From" + x + " To" + (Mathf.Floor(x)-0.5f));
					x = Mathf.Floor(x)+0.5f;
				}

				if( (z - Mathf.Floor(z)) < 0.5)
				{
					z = Mathf.Floor(z)-0.5f;
				}
				else
				{
					z = Mathf.Floor(z)+0.5f;
				}

				GameObject highLightObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
				highLightObject.transform.position = new Vector3(x, rayHits[i].point.y, z);
				highLightObject.transform.localScale = new Vector3(gridSizeX, 1, gridSizeZ);

				Destroy(highLightObject, 1);

				break;
			}
		}
		Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
	}
}
