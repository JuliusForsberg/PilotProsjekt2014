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
    bool invalid;

    public Tower tower1;
    public Tower tower2;
    public Tower tower3;
    Tower selectedTower;

	public GameObject[] hitObjects;
	public float gridSizeX=1;
	public float gridSizeZ=1;
	GameObject highLightObject;
    int highlightSizeX=1;
    int highlightSizeZ=1;

	bool[,] gridSquares;
	int coordX;
	int coordZ;
	//GameObject ball;

	void Update () {
        if (_enabled)
        {
            Ray ray = new Ray();
            ray = camTopDown.camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] rayHits = Physics.RaycastAll(ray, 100);

            for (int i = 0; i < rayHits.Length; i++)
            {
                if (rayHits[i].collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
                {
                    hitObjects[i] = rayHits[i].collider.gameObject;
                    if (highLightObject == null)
                    {
                        highLightObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        highLightObject.transform.localScale = new Vector3(gridSizeX * highlightSizeX, 1, gridSizeZ * highlightSizeZ);
                        highLightObject.renderer.material.shader = Shader.Find("Transparent/Diffuse");
                        highLightObject.renderer.material.color = new Color(1.0f, 1.0f, 1.0f, .1f);
                    }

                    //				if(ball == null)
                    //				{
                    //					ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    //					ball.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    //					//ball.renderer.material.color = new Vector4(1.0f, 1.0f, 1.0f, 0.1f);
                    //					ball.collider.enabled = false;
                    //				}
                    //				ball.transform.position = new Vector3(rayHits[i].point.x, rayHits[i].point.y, rayHits[i].point.z);

                    float offsetX;
                    float offsetZ;

                    if (highlightSizeX%2 == 1) //If the size is an odd number.
                    {
                    
                        offsetX = gridSizeX / 2; //Offsets the grid squares so that the corners are in 0,0,0. Seems like a good idea at this time.. 
                    }
                    else
                    {
                        offsetX = 0; //Offset is not needed if the highlightobject is bigger than 1 square.
                    }
                    
                    if (highlightSizeZ%2 == 1)
                    {
                    
                        offsetZ = gridSizeZ / 2;
                    }
                    else
                    {
                        offsetZ = 0;
                    }

                    float x = rayHits[i].point.x - offsetX;
                    float z = rayHits[i].point.z - offsetZ;

                    x = (gridSizeX) * (Mathf.Round(x / gridSizeX ));
                    z = (gridSizeZ) * (Mathf.Round(z / gridSizeZ ));


                    coordX = (int)((x / gridSizeX) + 4);
                    coordZ = (int)((z / gridSizeZ) + 4);

                    Debug.Log((coordX) + " " + (coordZ) + (gridSquares[coordX, coordZ]));

                    highLightObject.transform.position = new Vector3(x + offsetX, rayHits[i].point.y, z + offsetZ);

                    if (highlightSizeX > 1 || highlightSizeZ > 1)
                    {
                        int xHalf = Mathf.CeilToInt(highlightSizeX/2);

                        int zHalf = Mathf.CeilToInt(highlightSizeZ/2);

                        bool stop=false;

                        for(int k=0;k<highlightSizeX && !stop;k++) //Checks if any of the squares are occupied.
                        {
                            for(int j=0;j<highlightSizeZ;j++)
                            {
                                if(gridSquares[(coordX-xHalf) + k, (coordZ-zHalf) + j] == true)
                                {
                                    invalid = true;
                                    stop = true;
                                    break;
                                }
                                else
                                    invalid = false;
                            }
                        }
                        stop = false;
                    }
                    else
                    {
                        if (gridSquares[coordX, coordZ] == true)
                        {
                            invalid = true;
                        }
                        else
                            invalid = false;
                    }

                    break;

                }
                else if(i == rayHits.Length-1)
                    Destroy(highLightObject);
            }
            if (highLightObject != null)
            {
                if (invalid)
                {
                    highLightObject.renderer.material.color = new Color(1.0f, 0.0f, 0.0f, .5f);
                }
                else
                    highLightObject.renderer.material.color = new Color(1.0f, 1.0f, 1.0f, .5f);
            }

            if (rayHits.Length == 0)
            {
                Destroy(highLightObject);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if(selectedTower != null)
                    placeTower(selectedTower);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                highlightSizeX = 1;
                highlightSizeZ = 1;
                Destroy(highLightObject);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                highlightSizeX = 2;
                highlightSizeZ = 2;
                Destroy(highLightObject);
            }
        }
        else
            Destroy(highLightObject);

	}

    void setSelectedTower(Tower tower)
    {
        selectedTower = tower;
        highlightSizeX = tower.sizeX;
        highlightSizeZ = tower.sizeZ;
    }

	void placeTower(Tower tower) {

			if(highLightObject != null && !invalid)
			{
				Vector3 pos = highLightObject.transform.position;

				if(gridSquares[coordX, coordZ] == false)
					{
                        Instantiate(tower.gameObject, pos, tower.gameObject.transform.rotation);
						gridSquares[coordX, coordZ] = true;
					}
			}
	}

	void OnGUI() {
		if(_enabled)
		{
			if(GUI.Button(new Rect((Screen.width*0.8f), (Screen.height*0.9f), 100.0f, 50.0f), "Confirm"))
			{
				endConstruction();
			}

            if (GUI.Button(new Rect((Screen.width * 0.8f), (Screen.height * 0.8f), 100.0f, 50.0f), tower1.gameObject.name))
            {
                setSelectedTower(tower1);
            }

            if (GUI.Button(new Rect((Screen.width * 0.8f), (Screen.height * 0.7f), 100.0f, 50.0f), tower2.gameObject.name))
            {
                setSelectedTower(tower2);
            }

            if (GUI.Button(new Rect((Screen.width * 0.8f), (Screen.height * 0.6f), 100.0f, 50.0f), tower3.gameObject.name))
            {
                setSelectedTower(tower3);
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
		_enabled = false;
	}
}

[System.Serializable]
public class Tower
{
    public GameObject gameObject;
    public int sizeX=1;
    public int sizeZ=1;

}