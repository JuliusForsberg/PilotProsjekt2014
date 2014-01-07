using UnityEngine;
using System.Collections;



public class Inventory : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		inventoryPosX = Screen.width * inventoryPosX;
		inventoryPosY = Screen.height * inventoryPosY;

		inventory = new InventorySlots[gridSizeX, gridSizeY];
		float gridStepX = inventorySizeX/gridSizeX; //Distance between each point in the grid.
		float gridStepY = inventorySizeY/gridSizeY;
		print (inventory[1,1]._object);
		inventory[1,1].icon = backTexture;
		print (inventory[1,1].icon);
		for(int i=0;i<gridSizeX;i++)
		{
			for(int j=0;j<gridSizeY;j++)
			{
				inventory[i,j].position = new Vector2( (gridStepX/2)+(gridStepX*i), (gridStepY/2)+(gridStepY*j) );
			}
		}


	}
	void Update () {
		if(Input.GetKeyDown(KeyCode.F))
		{
			if(open)
				open = false;
			else
				open = true;
		}

	}

	bool open=false;
	public InventorySlots[,] inventory;
	public float inventorySizeX;
	public float inventorySizeY;
	public float inventoryPosX;
	public float inventoryPosY;
	public int gridSizeX;
	public int gridSizeY;
	public Texture2D backTexture;

	void OnGUI () {

		if(open)
		{ 
			GUI.BeginGroup(new Rect(inventoryPosX-(inventorySizeX/2), inventoryPosY-(inventorySizeY/2), inventorySizeX, inventorySizeY));

			GUI.DrawTexture(new Rect(0,0,inventorySizeX,inventorySizeY), backTexture);

			for(int i=0;i<gridSizeX;i++)
			{
				for(int j=0;j<gridSizeY;j++)
				{
					GUI.Box(new Rect(inventory[i,j].position.x-5, inventory[i,j].position.y-5, 10, 10), "X");
				}
			}

			GUI.EndGroup();
		}
	}

}

public class InventorySlots
{
	public Vector2 position;
	public Texture2D icon;
	public GameObject _object;

}