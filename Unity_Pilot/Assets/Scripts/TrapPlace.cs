using UnityEngine;
using System.Collections;

public class TrapPlace : MonoBehaviour {
	// Use this for initialization
	void Start () {
		camTopDown = GameObject.Find("CameraTopDown").camera;
		camMain = Camera.main;
		camTopDown.enabled = false;

		gridSquares = new bool[20, 20]; //Maxuimum grid size. May have to do something smarter later.
//		for(int i=0;i<20;i++)
//		{
//			for(int j=0;j<20;j++)
//			{
//				gridSquares[i,j] = false;
//			}
//		}
		
		hitObjects = new GameObject[3];
	}

	Camera camTopDown;
	Camera camMain;
	bool _enabled;

	public GameObject[] hitObjects;
	public float gridSizeX=1;
	public float gridSizeZ=1;
	GameObject highLightObject;

	bool[,] gridSquares;
	int coordX;
	int coordZ;
	//GameObject ball;

	void Update () {
	if(_enabled)
	{
		Ray ray = new Ray();
		ray = camTopDown.camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] rayHits = Physics.RaycastAll(ray, 100);

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

//				if(ball == null)
//				{
//					ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//					ball.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
//					//ball.renderer.material.color = new Vector4(1.0f, 1.0f, 1.0f, 0.1f);
//					ball.collider.enabled = false;
//				}
//				ball.transform.position = new Vector3(rayHits[i].point.x, rayHits[i].point.y, rayHits[i].point.z);

				float offsetX = gridSizeX/2; //Offsets the grid squares so that the corners are in 0,0,0. Seems like a good idea at this time..
				float offsetZ = gridSizeZ/2;

				float x = rayHits[i].point.x-offsetX;
				float z = rayHits[i].point.z-offsetZ;

				x = gridSizeX*(Mathf.Round(x/gridSizeX));
				z = gridSizeZ*(Mathf.Round(z/gridSizeZ));


				//print (gridSquares.Length);
				
				coordX = (int)( (x/gridSizeX)+4);
				coordZ = (int)( (z/gridSizeZ)+4);
				
					Debug.Log((coordZ)+" "+(coordX)+(gridSquares[coordX, coordZ]));

				highLightObject.transform.position = new Vector3(x+offsetX, rayHits[i].point.y, z+offsetZ);
					if(gridSquares[coordX, coordZ] == true)
					{
						highLightObject.renderer.material.color = Color.red;
					}


				break;

			}
		}
		if(rayHits.Length == 0)
		{
			Destroy(highLightObject);
		}

		if(Input.GetKeyDown(KeyCode.R))
		{
			placeObject(GameObject.CreatePrimitive(PrimitiveType.Cube));
		}
	}
	}

	void placeObject(GameObject _object) {

			if(highLightObject != null)
			{
				Vector3 pos = highLightObject.transform.position;

				if(gridSquares[coordX, coordZ] == false)
					{
						_object.SetActive(true);
						_object.transform.position = pos;
						gridSquares[coordX, coordZ] = true;
					}
			}
			else
				Destroy(_object);
	}

	void OnGUI() {
		if(_enabled)
		{
			if(GUI.Button(new Rect((Screen.width*0.8f), (Screen.height*0.8f), 100.0f, 50.0f), "Confirm"))
			{
				endConstruction();
			}
		}
	}

	void startConstruction () {
		camMain.enabled = false;
		camTopDown.enabled = true;
		_enabled = true;
	}

	void endConstruction () {
		camMain.enabled = true;
		camTopDown.enabled = false;
		enabled = false;
	}
}
