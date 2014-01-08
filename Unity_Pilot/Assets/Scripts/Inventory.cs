using UnityEngine;
using System.Collections;



public class Inventory : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		inventoryPosX = Screen.width * inventoryPosX;
		inventoryPosY = Screen.height * inventoryPosY;

		inventory = new InventorySlot[gridSizeX, gridSizeY];
		slotSizeX = inventorySizeX/gridSizeX;
		slotSizeY = inventorySizeY/gridSizeY;

		for(int j=0;j<gridSizeY;j++)
		{
			for(int i=0;i<gridSizeX;i++)
			{
				inventory[i,j] = new InventorySlot();
				inventory[i,j].position = new Vector2( (slotSizeX/2)+(slotSizeX*i), (slotSizeY/2)+(slotSizeY*j) );
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
		if(Input.GetKey(KeyCode.LeftShift))
		{
			inventory[0,0]._object = new GameObject();
		}

	}

	public GameObject getObject (int gridCoordX, int gridCoordY)
	{
		return inventory[gridCoordX, gridCoordY]._object;
	}

	public void putObject (GameObject _object)
	{
		for(int j=0;j<gridSizeY;j++)
		{
			for(int i=0;i<gridSizeX;i++)
			{
				if(inventory[i,j]._object == null)
				{

					inventory[i,j]._object = _object;
					inventory[i,j].icon = _object.GetComponent<Pickup>().icon;
					print( (inventory[i,j]._object) + " " + i + " " + j);
					return;
				}
			}
		}
	}

	bool open=false;
	public InventorySlot[,] inventory;
	public float inventorySizeX;
	public float inventorySizeY;
	public float inventoryPosX;
	public float inventoryPosY;
	public int gridSizeX;
	public int gridSizeY;
	float slotSizeX;
	float slotSizeY;
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
					if(inventory[i,j].icon != null)
						GUI.DrawTexture(new Rect(inventory[i,j].position.x-(slotSizeX/2), inventory[i,j].position.y-(slotSizeY/2), slotSizeX, slotSizeY), inventory[i,j].icon);
						//GUI.Box(new Rect(inventory[i,j].position.x-(slotSizeX/2), inventory[i,j].position.y-(slotSizeY/2), slotSizeX, slotSizeY), "Empty");
				}
			}

			GUI.EndGroup();
		}
	}

}

public class InventorySlot
{
//	public InventorySlot(Vector2 pos, Texture2D tex, GameObject obj) {
//		this.position = pos;
//		this.icon = tex;
//		this._object = obj;
//	}

	public InventorySlot() {

	}
	public Vector2 position;
	public Texture2D icon;
	public GameObject _object;

}