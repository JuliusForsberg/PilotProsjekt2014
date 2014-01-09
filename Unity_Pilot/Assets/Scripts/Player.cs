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
			Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 3, Color.white, 0);
			if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out forwardHit, 3.0f) && forwardHit.transform.gameObject.tag == "Pickup")
			{
				pickUpObject(forwardHit.transform.gameObject);
			}
		}
	}

	void pickUpObject (GameObject _object)
	{
		if(mInventory.emptySlots > 0)
		{
			mInventory.putObject(_object);
			_object.SetActive(false);
		}
	}
}
