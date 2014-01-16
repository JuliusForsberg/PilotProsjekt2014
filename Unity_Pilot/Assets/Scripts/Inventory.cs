using UnityEngine;
using System.Collections;



public class Inventory : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		inventoryPosX = Screen.width * inventoryPosX;
		inventoryPosY = Screen.height * inventoryPosY;

		emptySlots = AmountSlotsX*AmountSlotsY;

		inventory = new InventorySlot[AmountSlotsX, AmountSlotsY];
		slotSizeX = inventorySizeX/AmountSlotsX;
		slotSizeY = inventorySizeY/AmountSlotsY;

		for(int j=0;j<AmountSlotsY;j++)
		{
			for(int i=0;i<AmountSlotsX;i++)
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

	public void insertObject (GameObject gameObject)
	{
		for(int j=0;j<AmountSlotsY;j++)
		{
			for(int i=0;i<AmountSlotsX;i++)
			{
				if(inventory[i,j].gameObject == null)
				{
					inventory[i,j].gameObject = gameObject;
					inventory[i,j].icon = gameObject.GetComponent<Pickup>().icon;
					emptySlots--;

                    switch (inventory[i, j].gameObject.name)
                    {
                        case "Blue": amountBlues++; break;
                        case "Green": amountGreens++; break;
                        case "Red": amountReds++; break;
                    }

					return;
				}
			}
		}
	}

    public void removeObject(string type, int amount)
    {
        int removed = 0;
        bool stop=false;

        for (int j = 0; j < AmountSlotsY && !stop; j++)
        {
            for (int i = 0; i < AmountSlotsX && !stop; i++)
            {
                if (inventory[i, j].gameObject.name == type)
                {
                    removed++;
                    if (removed >= amount)
                        stop = true;
                }
            }
        }
    }

    public int getAmount(string resource)
    {
        switch (resource)
        {
            case "Blue": return amountBlues;
            case "Green": return amountGreens;
            case "Red": return amountReds;
            default: return 0;
        }
    }

	bool open=false;

    int amountBlues;
    int amountGreens;
    int amountReds;

	public InventorySlot[,] inventory;
	//[HideInInspector]
	public int emptySlots;
	public float inventorySizeX;
	public float inventorySizeY;
	public float inventoryPosX;
	public float inventoryPosY;
	public int AmountSlotsX=4;
	public int AmountSlotsY=4;
	float slotSizeX;
	float slotSizeY;
	public Texture2D backTexture;

	void OnGUI () {

		if(open)
		{ 
			GUI.BeginGroup(new Rect(inventoryPosX-(inventorySizeX/2), inventoryPosY-(inventorySizeY/2), inventorySizeX, inventorySizeY));

			GUI.DrawTexture(new Rect(0,0,inventorySizeX,inventorySizeY), backTexture);

			for(int i=0;i<AmountSlotsX;i++)
			{
				for(int j=0;j<AmountSlotsY;j++)
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
	public Vector2 position;
	public Texture2D icon;
	public GameObject gameObject;
}