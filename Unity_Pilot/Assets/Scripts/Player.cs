using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	Inventory mInventory;

	void Start () {
	
		mInventory = GameObject.Find("GUI").GetComponent<Inventory>();

	}

	RaycastHit forwardHit;

	void Update () {

		if(Input.GetKeyDown(KeyCode.E))
	   	{

            //forwardHits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 30.0f);

            //for (int i = 0; i < forwardHits.Length - 1; i++) //Using RaycastAll.
            //{
            //    print(forwardHits[i].collider.gameObject);
            //    if (forwardHits[i].transform.gameObject.tag == "Pickup")
            //    {
            //        pickUpObject(forwardHits[i].transform.gameObject);
            //        break;
            //    }
            //    if (i == forwardHits.Length - 1 && forwardHits[i].transform.gameObject.tag != "Pickup")
            //    {
            //        print("WHAT");
            //        print(forwardHits[0].collider.gameObject.name);
            //    }
            //}

            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 3, Color.white, 0);
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out forwardHit, 3.0f) && forwardHit.transform.gameObject.tag == "Pickup")
            {
                pickUpObject(forwardHit.transform.gameObject);
            }
            else
                print(forwardHit.collider.gameObject.name);
		}
	}

	void pickUpObject (GameObject _object)
	{
		if(mInventory.emptySlots > 0)
		{
			mInventory.insertObject(_object);
			_object.SetActive(false);
		}
	}
}
