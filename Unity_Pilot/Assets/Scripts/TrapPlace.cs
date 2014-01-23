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

		gridSquares = new Tower[gridSizeX, gridSizeZ]; //Maxuimum grid size. May have to do something smarter later.
	}

    Inventory mInventory;
	Camera camTopDown;
	Camera camMain;
	bool _enabled;
    bool invalid;

    GameObject player;

    Tower delete = new Tower(null, 0, 0, 0, 0, 0);
    public Tower tower1;
    public Tower tower2;
    public Tower tower3;
    Tower selectedTower;

    public int gridSizeX=8;
    public int gridSizeZ=8;
	public float squareSizeX=1;
	public float squareSizeZ=1;
    GameObject ghostObject; //Ghost of currently selected object/tower.
    Quaternion ghostObjRot;
	GameObject highLightObject; //Highlights an area of the grid. E.g. the size the tower will occupy.
    int highlightSizeX=1;
    int highlightSizeZ=1;

	Tower[,] gridSquares;
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
                    if (highLightObject == null)
                    {
                        highLightObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        highLightObject.transform.localScale = new Vector3(squareSizeX * highlightSizeX, 1, squareSizeZ * highlightSizeZ);
                        highLightObject.renderer.material.shader = Shader.Find("Transparent/Diffuse");
                        highLightObject.renderer.material.color = new Color(1.0f, 1.0f, 1.0f, .5f);

                        if (selectedTower.gameObject != null)
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

                    coordX = (int)((x / squareSizeX) + 4);
                    coordZ = (int)((z / squareSizeZ) + 4);

                    //Debug.Log((coordX) + " " + (coordZ)+ " " + x + " " + z);

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
                                if (gridSquares[(coordX - xHalf) + k, (coordZ - zHalf) + j] != null)
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
                        if (gridSquares[coordX, coordZ] != null)
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
                            int holdX = selectedTower.sizeX;

                            selectedTower.sizeX = selectedTower.sizeZ;
                            selectedTower.sizeZ = holdX;
                        }

                        setSelectedTower(selectedTower);

                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        ghostObject.transform.Rotate(Vector3.up, -90);
                        ghostObjRot = ghostObject.transform.rotation;

                        if (selectedTower.sizeX != selectedTower.sizeZ)
                        {
                            int holdX = selectedTower.sizeX;

                            selectedTower.sizeX = selectedTower.sizeZ;
                            selectedTower.sizeZ = holdX;
                        }

                        setSelectedTower(selectedTower);

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
                if (selectedTower == delete)
                {
                    for (int i = 0; i < rayHits.Length; i++)
                    {
                        //if (rayHits[i].collider.gameObject.GetComponent<Tower>() =! null)
                        //{
                        if (rayHits[i].collider.gameObject.tag == "Tower")
                        {
                            deleteTower(rayHits[i].collider.gameObject;
                        }
                        //}

                    }
                }
                else if (selectedTower != null)
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
        {
            Destroy(highLightObject);

        }

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

                Tower thisTower = towerCopy(tower);
                thisTower.gameObject = Instantiate(tower.gameObject, pos, ghostObject.transform.rotation) as GameObject;
                thisTower.gameObject.tag = "Tower";

                mInventory.removeObject("Blue", selectedTower.blueCost);
                mInventory.removeObject("Red", selectedTower.greenCost);
                mInventory.removeObject("Green", selectedTower.redCost);

                for (int k = 0; k < highlightSizeX; k++) //Sets squares to occupied;
                {
                    for (int j = 0; j < highlightSizeZ; j++)
                    {
                        gridSquares[(coordX - xHalf) + k, (coordZ - zHalf) + j] = thisTower;
                        thisTower.occupiedSquares.Add(new Vector2((coordX - xHalf) + k, (coordZ - zHalf) + j));
                    }
                }
            }
            else
                Debug.Log("Not Enough Resources");
		}
	}

    void deleteTower(Tower tower) {

        for (int i = 0; i < tower.occupiedSquares.Count; i++)
        {
            gridSquares[(int)tower.occupiedSquares[i].x, (int)tower.occupiedSquares[i].y] = null;
        }

        Destroy(tower.gameObject);
        tower = null;

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

            if (GUI.Button(new Rect((Screen.width * 0.8f), (Screen.height * 0.5f), 100.0f, 50.0f), "Remove"))
            {
                setSelectedTower(delete);
            }
		}
	}

	void startConstruction () {
		camMain.enabled = false;
		camTopDown.enabled = true;
		_enabled = true;

        if (player == null)
            player = GameObject.FindWithTag("Player");

        player.SetActive(false);
	}

	void endConstruction () {
		camMain.enabled = true;
		camTopDown.enabled = false;
		_enabled = false;
        player.transform.position = new Vector3(-12.0f, 1.0f, -3.0f);
        player.SetActive(true);
	}

    Tower towerCopy(Tower tower)
    {
        Tower newTower = new Tower();

        newTower.gameObject = tower.gameObject;
        newTower.sizeX = tower.sizeX;
        newTower.sizeZ = tower.sizeZ;
        newTower.blueCost = tower.blueCost;
        newTower.greenCost = tower.greenCost;
        newTower.redCost = tower.redCost;

        return newTower;
    }
}

[System.Serializable]
public class Tower
{
    public Tower()
    {
    }

    public Tower(GameObject _gameObject, int _sizeX, int _sizeZ, int _blueCost, int _greenCost, int _redCost)
    {
        gameObject = _gameObject;
        sizeX = _sizeX;
        sizeZ = _sizeZ;
        blueCost = _blueCost;
        greenCost = _greenCost;
        redCost = _redCost;
    }

    public GameObject gameObject;
    public int sizeX=1;
    public int sizeZ=1;
    public int blueCost;
    public int greenCost;
    public int redCost;

    public List<Vector2> occupiedSquares = new List<Vector2>();
}