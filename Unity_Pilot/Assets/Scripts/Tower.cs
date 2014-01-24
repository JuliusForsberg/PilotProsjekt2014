using UnityEngine;
using System.Collections;
using System.Collections.Generic; //For using list.

public class Tower : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public int sizeX = 1;
    public int sizeZ = 1;
    public int blueCost;
    public int greenCost;
    public int redCost;

    public List<Vector2> occupiedSquares = new List<Vector2>();

	void Update () {
	
	}
}
