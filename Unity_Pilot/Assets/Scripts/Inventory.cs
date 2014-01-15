using UnityEngine;
using System.Collections;



public class Inventory : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		inventoryPosX = Screen.width * inventoryPosX;
		inventoryPosY = Screen.height * inventoryPosY;

		emptySlots = nrOfslotsX*nrOfslotsY;

		inventory = new InventorySlot[nrOfslotsX, nrOfslotsY];
		slotSizeX = inventorySizeX/nrOfslotsX;
		slotSizeY = inventorySizeY/nrOfslotsY;

		for(int j=0;j<nrOfslotsY;j++)
		{
			for(int i=0;i<nrOfslotsX;i++)
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

	}

	public GameObject getObject (int gridCoordX, int gridCoordY)
	{
		return inventory[gridCoordX, gridCoordY].gameObject;
	}

	public void putObject (GameObject gameObject)
	{
		for(int j=0;j<nrOfslotsY;j++)
		{
			for(int i=0;i<nrOfslotsX;i++)
			{
				if(inventory[i,j].gameObject == null)
				{

					inventory[i,j].gameObject = gameObject;
					inventory[i,j].icon = gameObject.GetComponent<Pickup>().icon;
					emptySlots--;
					return;
				}
			}
		}
	}

	bool open=false;
	public InventorySlot[,] inventory;
	//[HideInInspector]
	public int emptySlots;
	public float inventorySizeX;
	public float inventorySizeY;
	public float inventoryPosX;
	public float inventoryPosY;
	public int nrOfslotsX;
	public int nrOfslotsY;
	float slotSizeX;
	float slotSizeY;
	public Texture2D backTexture;

	void OnGUI () {

		if(open)
		{ 
			GUI.BeginGroup(new Rect(inventoryPosX-(inventorySizeX/2), inventoryPosY-(inventorySizeY/2), inventorySizeX, inventorySizeY));

			GUI.DrawTexture(new Rect(0,0,inventorySizeX,inventorySizeY), backTexture);

			for(int i=0;i<nrOfslotsX;i++)
			{
				for(int j=0;j<nrOfslotsY;j++)
				{
					if(inventory[i,j].icon != null)
						GUI.DrawTexture(new Rect(inventory[i,j].position.x-(slotSizeX/2), inventory[i,j].position.y-(slotSizeY/2), slotSizeX, slotSizeY), inventory[i,j].icon);
				}
			}

			GUI.EndGroup();
		}
	}

}

public class InventorySlot
{

	public InventorySlot() {

	}
	public Vector2 position;
	public Texture2D icon;
	public GameObject gameObject;

}