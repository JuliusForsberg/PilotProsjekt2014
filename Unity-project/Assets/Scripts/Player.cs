using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	Inventory mInventory;

	void Start () {

		mInventory = GameObject.Find("GUI").GetComponent<Inventory>();

	}

	RaycastHit[] forwardHits;
    public bool gathering=false;
    public float gatheringTimer;
    public float gatherDelay = 1f;
    public Texture2D gatherBarTex;
    public Texture2D gatherBarTexBack;
    public GameObject currentObject;

	void Update () {

		if(Input.GetKeyDown(KeyCode.E))
	   	{

            forwardHits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 30.0f);

            for (int i = 0; i < forwardHits.Length; i++) //Using RaycastAll.
            {
                print(forwardHits[i].collider.gameObject);
                if (forwardHits[i].transform.gameObject.tag == "Pickup")
                {
                    currentObject = forwardHits[i].collider.gameObject;
                    gathering = true;
                    break;
                }
                if (i == forwardHits.Length - 1 && forwardHits[i].transform.gameObject.tag != "Pickup")
                {
                    print(forwardHits[0].collider.gameObject.name);
                }
            }

            //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out forwardHit, 3.0f) && forwardHit.transform.gameObject.tag == "Pickup")
            //{
            //    pickUpObject(forwardHit.transform.gameObject);
            //}
            //else
            //    print(forwardHit.collider.gameObject.name);
		}

        if (gathering)
        {
            gatheringTimer += Time.deltaTime;

            if (gatheringTimer >= gatherDelay)
            {
                pickUpObject(currentObject);
                stopGathering();
            }

            if (Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.D) ||
                Input.GetKey(KeyCode.Space))
                stopGathering();
        }
	}

    void stopGathering()
    {
        gathering = false;
        gatheringTimer = 0;
    }

	void pickUpObject (GameObject _object)
	{
		if(mInventory.emptySlots > 0)
		{
			mInventory.insertObject(_object);
            _object.SetActive(false);
		}
	}

    void OnGUI()
    {
        if (gathering)
        {
            GUI.BeginGroup(new Rect(Screen.width * 0.5f, Screen.height * 0.8f, 100f, 15f));
            GUI.DrawTexture(new Rect(0, 0f, 100f, 15f), gatherBarTexBack);
            GUI.DrawTexture(new Rect(100f * (gatherDelay * gatheringTimer) - 100, 0f, 100f, 15f), gatherBarTex);
            GUI.EndGroup(); 
        }
    }
}
