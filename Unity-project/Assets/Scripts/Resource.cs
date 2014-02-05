using UnityEngine;
using System.Collections;

public enum resourceEnum { Rock, Wood };

public class Resource : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		gameObject.tag = "Resource";

        if (pickupObject.GetComponent<Pickup>() == null)
            pickupObject.AddComponent<Pickup>();

	}

	public Texture2D icon;

    public resourceEnum resource;

    public int amountOfDrops=1;
    public GameObject pickupObject;
	// Update is called once per frame
	void Update () {
	
	}

    void Destroy()
    {
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
            dropPoint = transform.position + new Vector3(Random.Range(-2.0f, 2.0f), 0, Random.Range(-2.0f, 2.0f));
            pickup.SendMessage("setEndPoints", dropPoint);
        }
    }
}
