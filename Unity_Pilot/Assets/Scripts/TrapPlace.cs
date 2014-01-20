using UnityEngine;
using System.Collections;

public class TrapPlace : MonoBehaviour {
	// Use this for initialization
	void Start () {
        mInventory = GameObject.Find("GUI").GetComponent<Inventory>();
		camTopDown = GameObject.Find("CameraTopDown").camera;
		camMain = Camera.main;
		camTopDown.enabled = false;

		gridSquares = new bool[gridSizeX, gridSizeZ]; //Maxuimum grid size. May have to do something smarter later.
//		for(int i=0;i<20;i++)
//		{
//			for(int j=0;j<20;j++)
//			{
//				gridSquares[i,j] = false;
//			}
//		}
		hitObjects = new GameObject[3];
	}

    Inventory mInventory;
	Camera camTopDown;
	Camera camMain;
	bool _enabled;
    bool invalid;

    public Tower tower1;
    public Tower tower2;
    public Tower tower3;
    Tower selectedTower;

	public GameObject[] hitObjects;
    public int gridSizeX=8;
    public int gridSizeZ=8;
	public float squareSizeX=1;
	public float squareSizeZ=1;
    GameObject ghostObject; //Ghost of currently selected object/tower.
	GameObject highLightObject; //Highlights an area of the grid. E.g. the size the tower will occupy.
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
                        highLightObject.transform.localScale = new Vector3(squareSizeX * highlightSizeX, 1, squareSizeZ * highlightSizeZ);
                        highLightObject.renderer.material.shader = Shader.Find("Transparent/Diffuse");
                        highLightObject.renderer.material.color = new Color(1.0f, 1.0f, 1.0f, .5f);

                        if (selectedTower != null)
                        {
                            ghostObject = Instantiate(selectedTower.gameObject) as GameObject;
                            ghostObject.transform.parent = highLightObject.transform;
                            ghostObject.transform.position = new Vector3(0, 0, 0);
                        }
                    }
                    else if (highLightObject.transform.localScale != new Vector3(squareSizeX * highlightSizeX, 1, squareSizeZ * highlightSizeZ))
                        highLightObject.transform.localScale = new Vector3(squareSizeX * highlightSizeX, 1, squareSizeZ * highlightSizeZ);

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
                    
                        offsetX = squareSizeX / 2; //Offsets the grid squares so that the corners are in 0,0,0. Seems like a good idea at this time.. 
                    }
                    else
                    {
                        offsetX = 0; //Offset is not needed if the highlightobject is bigger than 1 square.
                    }
                    
                    if (highlightSizeZ%2 == 1)
                    {
                    
                        offsetZ = squareSizeZ / 2;
                    }
                    else
                    {
                        offsetZ = 0;
                    }

                    float x = rayHits[i].point.x - offsetX;
                    float z = rayHits[i].point.z - offsetZ;

                    x = (squareSizeX) * (Mathf.Round(x / squareSizeX));
                    z = (squareSizeZ) * (Mathf.Round(z / squareSizeZ));

                    coordX = (int)((x / squareSizeX) + 4);
                    coordZ = (int)((z / squareSizeZ) + 4);

                    //Debug.Log((coordX) + " " + (coordZ)+ " " + x + " " + z);

                    if (highlightSizeX > 1 || highlightSizeZ > 1)
                    {
                        int xHalf = Mathf.CeilToInt(5 / 2);
                        int zHalf = Mathf.CeilToInt(highlightSizeZ / 2);
                        //print("CEIL"+highlightSizeX + " " + (xHalf));
                        print(Mathf.CeilToInt(2.5f));
                        if (xHalf%2 == 0)
                        {
                            if (coordX > (gridSizeX - xHalf - 1))
                            {
                                x -= squareSizeX * (coordX - gridSizeX + xHalf);
                            }
                            else
                            {
                                if (coordX < xHalf)
                                {
                                    x += squareSizeX * (xHalf - coordX);
                                }
                            }
                        }
                        else
                        {
                            if (coordX > (gridSizeX - xHalf))
                            {
                                x -= squareSizeX * (coordX - gridSizeX + xHalf-1);
                            }
                            else
                            {
                                if (coordX < xHalf)
                                {
                                    x += squareSizeX * (xHalf - coordX);
                                }
                            }
                        }

                        if (coordZ > (gridSizeZ - 1))
                        {
                            z -= (squareSizeZ);
                        }
                        else if (coordZ < 1)
                        {
                            z += squareSizeZ;
                        }
                        
                    }

                    coordX = (int)((x / squareSizeX) + 4);
                    coordZ = (int)((z / squareSizeZ) + 4);

                    highLightObject.transform.position = new Vector3(x + offsetX, rayHits[i].point.y, z + offsetZ);

                    if (highlightSizeX > 1 || highlightSizeZ > 1)
                    {
                        int xHalf = Mathf.CeilToInt(highlightSizeX / 2);
                        int zHalf = Mathf.CeilToInt(highlightSizeZ / 2);

                        bool stop = false;

                        for (int k = 0; k < highlightSizeX && !stop; k++) //Checks if any of the squares are occupied.
                        {
                            for (int j = 0; j < highlightSizeZ; j++)
                            {
                                if (gridSquares[(coordX - xHalf) + k, (coordZ - zHalf) + j] == true)
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

                    

                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        ghostObject.transform.Rotate(Vector3.up, 90);
                        selectedTower.gameObject.transform.Rotate(Vector3.up, 90);

                        if (selectedTower.sizeX != selectedTower.sizeZ)
                        {
                            int holdX = selectedTower.sizeX;

                            selectedTower.sizeX = selectedTower.sizeZ;
                            selectedTower.sizeZ = holdX;
                        }

                        setSelectedTower(selectedTower);
                        //if (highlightSizeX != highlightSizeZ)
                        //{
                        //    int holdX = highlightSizeX;

                        //    highlightSizeX = highlightSizeZ;
                        //    highlightSizeZ = holdX;
                        //}

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

            if (Input.GetKeyDown(KeyCode.Mouse0))
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
            int amountBlues = mInventory.getAmount("Blue");
            int amountGreens = mInventory.getAmount("Green");
            int amountReds = mInventory.getAmount("Red");


            if (amountBlues >= selectedTower.blueCost &&
                amountGreens >= selectedTower.greenCost &&
                amountReds >= selectedTower.redCost)
            {
                Vector3 pos = highLightObject.transform.position;

                int xHalf = Mathf.CeilToInt(highlightSizeX / 2);
                int zHalf = Mathf.CeilToInt(highlightSizeZ / 2);

                Instantiate(tower.gameObject, pos, ghostObject.transform.rotation);
                gridSquares[coordX, coordZ] = true;

                mInventory.removeObject("Blue", selectedTower.blueCost);
                mInventory.removeObject("Red", selectedTower.greenCost);
                mInventory.removeObject("Green", selectedTower.redCost);

                for (int k = 0; k < highlightSizeX; k++) //Sets squares to occupied;
                {
                    for (int j = 0; j < highlightSizeZ; j++)
                    {
                        gridSquares[(coordX - xHalf) + k, (coordZ - zHalf) + j] = true;
                    }
                }
            }
            else
                Debug.Log("Not Enough Resources");
		}
	}

	void OnGUI() {
		if(_enabled)
		{
            string tower1Info = tower1.gameObject.name + "\nB" + tower1.blueCost.ToString() + " G" + tower1.greenCost.ToString() + " R" + tower1.redCost.ToString();
            string tower2Info = tower2.gameObject.name + "\nB" + tower2.blueCost.ToString() + " G" + tower2.greenCost.ToString() + " R" + tower2.redCost.ToString();
            string tower3Info = tower3.gameObject.name + "\nB" + tower3.blueCost.ToString() + " G" + tower3.greenCost.ToString() + " R" + tower3.redCost.ToString();

			if(GUI.Button(new Rect((Screen.width*0.8f), (Screen.height*0.9f), 100.0f, 50.0f), "Confirm"))
			{
				endConstruction();
			}

            if (GUI.Button(new Rect((Screen.width * 0.8f), (Screen.height * 0.8f), 100.0f, 50.0f), tower1Info))
            {
                setSelectedTower(tower1);
            }

            if (GUI.Button(new Rect((Screen.width * 0.8f), (Screen.height * 0.7f), 100.0f, 50.0f), tower2Info))
            {
                setSelectedTower(tower2);
            }

            if (GUI.Button(new Rect((Screen.width * 0.8f), (Screen.height * 0.6f), 100.0f, 50.0f), tower3Info))
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
    public int blueCost;
    public int greenCost;
    public int redCost;

}