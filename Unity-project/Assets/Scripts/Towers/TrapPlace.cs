using UnityEngine;
using System.Collections;
using System.Collections.Generic; //For using list.

public class TrapPlace : MonoBehaviour {
	// Use this for initialization
	void Start () {
        mInventory = GameObject.Find("GUI").GetComponent<Inventory>();
		camTopDown = GameObject.Find("CameraTopDown").camera;
		camMain = Camera.main;
		camTopDown.enabled = false;

		gridSquares = new bool[gridSizeX, gridSizeZ]; //Maxuimum grid size. May have to do something smarter later.
	}

    Inventory mInventory;
	Camera camTopDown;
	Camera camMain;
	bool _enabled;
    bool invalid;
    public GameObject gridArea;

    GameObject player;

    public GameObject[] towers;
    Tower selectedTower;
    int sizeX;
    int sizeZ;
    int blueCost;
    int greenCost;
    int redCost;

    public int gridSizeX=8;
    public int gridSizeZ=8;
	public float squareSizeX=1;
	public float squareSizeZ=1;
    GameObject ghostObject; //Ghost of currently selected object/tower.
    Quaternion ghostObjRot; //I need to store the rotation because the object is recreated each time it exits-and-enters the grid.
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
                if (rayHits[i].collider.gameObject == gridArea)
                {

                    float offsetX;
                    float offsetZ;

                    if (highlightSizeX % 2 == 1) //If the size is an odd number.
                    {

                        offsetX = squareSizeX / 2; //Offsets the grid squares so that the corners are in 0,0,0. Seems like a good idea at this time.. 
                    }
                    else
                    {
                        offsetX = 0; //Offset is not needed if the highlightobject is bigger than 1 square.
                    }

                    if (highlightSizeZ % 2 == 1)
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

                    coordX = (int)(((x - gridArea.transform.position.x) / squareSizeX) + gridSizeX/2);
                    coordZ = (int)(((z - gridArea.transform.position.z) / squareSizeZ) + gridSizeZ/2);

                    Debug.Log((coordX) + " " + (coordZ) + " " + x + " " + z);

                    if (highLightObject == null)
                    {
                        highLightObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        highLightObject.transform.localScale = new Vector3(squareSizeX * highlightSizeX, 1, squareSizeZ * highlightSizeZ);
                        highLightObject.renderer.material.shader = Shader.Find("Transparent/Diffuse");
                        highLightObject.renderer.material.color = new Color(1.0f, 1.0f, 1.0f, .5f);

                        if (selectedTower != null)
                        {
                            ghostObject = Instantiate(selectedTower.gameObject) as GameObject;
                            ghostObject.transform.rotation = ghostObjRot;
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

                    if (highlightSizeX > 1 || highlightSizeZ > 1) //Code for stopping at the edge with larger object
                    {
                        int xHalf = Mathf.CeilToInt(highlightSizeX / 2.0f);
                        int zHalf = Mathf.CeilToInt(highlightSizeZ / 2.0f);

                        if (highlightSizeX % 2 == 0)
                        {
                            if (coordX > (gridSizeX - xHalf))
                            {
                                x -= squareSizeX * (coordX - gridSizeX + xHalf);
                            }
                            else if (coordX < xHalf)
                            {
                                x += squareSizeX * (xHalf - coordX);
                            }
                        }
                        else
                        {
                            if (coordX > (gridSizeX - xHalf))
                            {
                                x -= squareSizeX * (coordX - gridSizeX + xHalf);
                            }
                            else if (coordX < (xHalf - 1))
                            {
                                x += squareSizeX * ((xHalf - 1) - coordX);
                            }
                        }

                        if (highlightSizeZ % 2 == 0)
                        {
                            if (coordZ > (gridSizeZ - zHalf))
                            {
                                z -= squareSizeZ * (coordZ - gridSizeZ + zHalf);
                            }
                            else if (coordZ < zHalf)
                            {
                                z += squareSizeZ * (zHalf - coordZ);
                            }
                        }
                        else
                        {
                            if (coordZ > (gridSizeZ - zHalf))
                            {
                                z -= squareSizeZ * (coordZ - gridSizeZ + zHalf);
                            }
                            else if (coordZ < (zHalf - 1))
                            {
                                z += squareSizeZ * ((zHalf - 1) - coordZ);
                            }
                        }

                    }

                    coordX = (int)(((x-gridArea.transform.position.x) / squareSizeX) + gridSizeX/2); //Why is this here again, i don't remember!?
                    coordZ = (int)(((z - gridArea.transform.position.z) / squareSizeZ) + gridSizeZ/2);

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
                        ghostObjRot = ghostObject.transform.rotation;

                        if (selectedTower.sizeX != selectedTower.sizeZ)
                        {
                            flipHighlight(selectedTower);
                        }

                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        ghostObject.transform.Rotate(Vector3.up, -90);
                        ghostObjRot = ghostObject.transform.rotation;

                        if (selectedTower.sizeX != selectedTower.sizeZ)
                        {
                            flipHighlight(selectedTower);
                        }

                    }
                    break;
                }
                else if (i == rayHits.Length - 1)
                {
                    Destroy(highLightObject);

                }
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

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (selectedTower == null)
                {
                    for (int i = 0; i < rayHits.Length; i++)
                    {
                        if (rayHits[i].collider.gameObject.tag == "Tower")
                        {
                            deleteTower(rayHits[i].collider.gameObject);
                            return;
                        }
                    }
                }
                else if (selectedTower != null)
                    placeTower(selectedTower.gameObject);
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
        {
            Destroy(highLightObject);

        }

	}

    void flipHighlight(Tower tower)
    {

        if (highlightSizeX == tower.sizeX && highlightSizeZ == tower.sizeZ)
        {
            highlightSizeX = tower.sizeZ;
            highlightSizeZ = tower.sizeX;
        }
        else
        {
            highlightSizeX = tower.sizeX;
            highlightSizeZ = tower.sizeZ;
        }
    }

    void setSelectedTower(GameObject tower)
    {
        selectedTower = tower.GetComponent<Tower>();

        ghostObjRot = tower.transform.rotation;

        highlightSizeX = selectedTower.sizeX;
        highlightSizeZ = selectedTower.sizeZ;

        Destroy(highLightObject);
    }

    void placeTower(GameObject tower)
    {

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

                //Tower thisTower = towerCopy(tower);
                GameObject thisTower = Instantiate(tower, pos, ghostObject.transform.rotation) as GameObject;
                //thisTower.gameObject.tag = "Tower";

                mInventory.removeObject("Blue", selectedTower.blueCost);
                mInventory.removeObject("Red", selectedTower.greenCost);
                mInventory.removeObject("Green", selectedTower.redCost);

                List<Vector2> mOccupiedSquares = new List<Vector2>();

                for (int k = 0; k < highlightSizeX; k++) //Sets squares to occupied;
                {
                    for (int j = 0; j < highlightSizeZ; j++)
                    {
                        gridSquares[(coordX - xHalf) + k, (coordZ - zHalf) + j] = true;
                        mOccupiedSquares.Add(new Vector2((coordX - xHalf) + k, (coordZ - zHalf) + j));
                        
                    }
                }

                thisTower.GetComponent<Tower>().setOccupied(mOccupiedSquares);
            }
            else
                Debug.Log("Not Enough Resources");
		}
	}

    void deleteTower(GameObject tower) {

        List<Vector2> mOccupiedSquares = tower.GetComponent<Tower>().getOccupied();

        for (int i = 0; i < mOccupiedSquares.Count; i++)
        {
            gridSquares[(int)mOccupiedSquares[i].x, (int)mOccupiedSquares[i].y] = false;
        }

        Destroy(tower);

    }

	void OnGUI() {
		if(_enabled)
		{
                Tower tower1 = towers[0].GetComponent<Tower>(); //Needs to be optimized.
                //Tower tower2 = towers[1].GetComponent<Tower>();
                //Tower tower3 = towers[2].GetComponent<Tower>();

                string tower1Info = tower1.gameObject.name + "\nB" + tower1.blueCost.ToString() + " G" + tower1.greenCost.ToString() + " R" + tower1.redCost.ToString();
                //string tower2Info = tower2.gameObject.name + "\nB" + tower2.blueCost.ToString() + " G" + tower2.greenCost.ToString() + " R" + tower2.redCost.ToString();
                //string tower3Info = tower3.gameObject.name + "\nB" + tower3.blueCost.ToString() + " G" + tower3.greenCost.ToString() + " R" + tower3.redCost.ToString();

			if(GUI.Button(new Rect((Screen.width*0.8f), (Screen.height*0.9f), 100.0f, 50.0f), "Confirm"))
			{
				endConstruction();
			}

            if (GUI.Button(new Rect((Screen.width * 0.8f), (Screen.height * 0.8f), 100.0f, 50.0f), tower1Info))
            {
                setSelectedTower(towers[0]);
            }

            //if (GUI.Button(new Rect((Screen.width * 0.8f), (Screen.height * 0.7f), 100.0f, 50.0f), tower2Info))
            //{
            //    setSelectedTower(towers[1]);
            //}

            //if (GUI.Button(new Rect((Screen.width * 0.8f), (Screen.height * 0.6f), 100.0f, 50.0f), tower3Info))
            //{
            //    setSelectedTower(towers[2]);
            //}

            if (GUI.Button(new Rect((Screen.width * 0.8f), (Screen.height * 0.5f), 100.0f, 50.0f), "Remove"))
            {
                selectedTower = null;
                highlightSizeX = 1;
                highlightSizeZ = 1;
            }
		}
	}

	void startConstruction () {
		camMain.enabled = false;
		camTopDown.enabled = true;
		_enabled = true;

        if (player == null)
            player = GameObject.FindWithTag("Player");

        //player.GetComponent<CharacterController>().canControl = false;
	}

	void endConstruction () {
		camMain.enabled = true;
		camTopDown.enabled = false;
		_enabled = false;
        player.SetActive(true);
	}

    //Tower towerCopy(Tower tower)
    //{
    //    Tower newTower = new Tower();

    //    newTower.gameObject = tower.gameObject;
    //    newTower.sizeX = tower.sizeX;
    //    newTower.sizeZ = tower.sizeZ;
    //    newTower.blueCost = tower.blueCost;
    //    newTower.greenCost = tower.greenCost;
    //    newTower.redCost = tower.redCost;

    //    return newTower;
    //}
}

//[System.Serializable]
//public class Tower
//{
//    public Tower()
//    {
//    }

//    public Tower(GameObject _gameObject, int _sizeX, int _sizeZ, int _blueCost, int _greenCost, int _redCost)
//    {
//        gameObject = _gameObject;
//        sizeX = _sizeX;
//        sizeZ = _sizeZ;
//        blueCost = _blueCost;
//        greenCost = _greenCost;
//        redCost = _redCost;
//    }

//    public GameObject gameObject;
//    public int sizeX=1;
//    public int sizeZ=1;
//    public int blueCost;
//    public int greenCost;
//    public int redCost;

//    public List<Vector2> occupiedSquares = new List<Vector2>();
//}