using UnityEngine;
using System.Collections;



public class Inventory : MonoBehaviour {

    bool open = false;

    public int amountRocks;
    public int amountMetal;

    public InventorySlot[,] inventory;
    [HideInInspector]
    public int emptySlots;
    public float inventorySizeX;
    public float inventorySizeY;
    public float inventoryPosX;
    public float inventoryPosY;
    public int AmountSlotsX = 4;
    public int AmountSlotsY = 4;
    float slotSizeX;
    float slotSizeY;
    public Texture2D backTexture;

	void Start () {

        //amountRocks = 9999;
        //amountMetal = 9999;

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
        Resource _pickup = gameObject.GetComponent<Resource>();
		for(int j=0;j<AmountSlotsY;j++)
		{
			for(int i=0;i<AmountSlotsX;i++)
			{

                if (inventory[i, j].gameObject == null)
                {
                    inventory[i, j].gameObject = gameObject;
                    inventory[i, j].amount = 1;
                    inventory[i, j].icon = gameObject.GetComponent<Resource>().icon;
                    emptySlots--;

                    print(i + " " + j);

                    switch (_pickup.resource)
                    {
                        case resourceEnum.Rock: amountRocks++; break;
                        case resourceEnum.Wood: amountMetal++; break;
                    }

                    return;
                }
                else if (inventory[i, j].gameObject.GetComponent<Resource>().resource == gameObject.GetComponent<Resource>().resource && inventory[i, j].amount <= 20)
                {
                    inventory[i, j].amount++;

                    switch (_pickup.resource)
                    {
                        case resourceEnum.Rock: amountRocks++; break;
                        case resourceEnum.Wood: amountMetal++; break;
                    }

                    return;
                } 
			}
		}
	}

    public void removeObject(resourceEnum type, int amount)
    {
        if (amount == 0)
            return;

        int removed = 0;
        bool stop = false;

        for (int j = 0; j < AmountSlotsY && !stop; j++)
        {
            for (int i = 0; i < AmountSlotsX && !stop; i++)
            {
                if (inventory[i, j].gameObject != null)
                {
                    if (inventory[i, j].gameObject.GetComponent<Resource>().resource == type)
                    {

                        if (inventory[i, j].amount <= 1)
                        {
                            inventory[i, j].gameObject = null;
                            inventory[i, j].icon = null;
                        }
                        else
                            inventory[i, j].amount -= amount;
                        removed++;

                        switch (type)
                        {
                            case resourceEnum.Rock: amountRocks--; break;
                            case resourceEnum.Wood: amountMetal--; break;
                        }

                        if (removed >= amount)
                            stop = true;
                    } 
                }
            }
        }
    }

    public int getAmount(resourceEnum resource)
    {
        switch (resource)
        {
            case resourceEnum.Rock: return amountRocks;
            case resourceEnum.Wood: return amountMetal;

            default: return 0;
        }
    }

    public GUIStyle stackLabelStyle;

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
                    {
                        Rect square = new Rect(inventory[i,j].position.x-(slotSizeX/2), inventory[i,j].position.y-(slotSizeY/2), slotSizeX, slotSizeY);

                        
						GUI.DrawTexture(square, inventory[i,j].icon);

                        //if (inventory[i, j].amount > 1)
                        //{
                            GUI.BeginGroup(square);
                            GUI.Label(new Rect(slotSizeX * 0.7f, slotSizeY * 0.7f, 20f, 20f), inventory[i, j].amount.ToString(), stackLabelStyle);
                            GUI.EndGroup();
                        //}
                    }
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
    public int amount;
}