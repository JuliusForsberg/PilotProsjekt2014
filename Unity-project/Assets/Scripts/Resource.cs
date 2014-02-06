using UnityEngine;
using System.Collections;

public enum resourceEnum { Rock, Wood };

public class Resource : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		gameObject.tag = "Resource";

        Pickup pickupComp = pickupObject.GetComponent<Pickup>();

        if (pickupComp == null)
            pickupObject.AddComponent<Pickup>();

	}

    public resourceEnum resource;
    public Texture2D icon;

    public int amountOfDrops=1;
    public GameObject pickupObject;
	// Update is called once per frame
	void Update () {
	
	}

    void Destroy()
    {
        gameObject.SetActive(false); //Disable so that the rays i cast don't hit the resource itself.
        breakApart();
        Destroy(gameObject);
    }

    void breakApart()
    {
        GameObject pickup;
        Vector3 dropPoint;
        for (int i = 0; i < amountOfDrops; i++)
        {
            pickup = Instantiate(pickupObject, transform.position, pickupObject.transform.rotation) as GameObject;
            Pickup pickupComp = pickup.GetComponent<Pickup>();
            pickupComp.resource = resource;
            pickupComp.icon = icon;

            RaycastHit rayHit;
            Vector3 pos = transform.position + new Vector3(Random.Range(-2.0f, 2.0f), +2, Random.Range(-2.0f, 2.0f));
            Physics.Raycast(pos, Vector3.down, out rayHit, 5f);
            dropPoint = rayHit.point;
            pickup.SendMessage("setEndPoints", dropPoint);
        }
    }
}
