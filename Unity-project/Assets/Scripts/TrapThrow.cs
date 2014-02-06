using UnityEngine;
using System.Collections;

public class TrapThrow : MonoBehaviour {

    Vector3 target;

    public GameObject trap;

    bool isActive;

    public float speed=1f;


	void Start () {

        if (trap.GetComponent<SmallTrapThrowable>() == null)
            trap.AddComponent<SmallTrapThrowable>();

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.C))
        {
            Fire();
        }
	
	}

    private void Fire()
    {
        RaycastHit[] rayHits;
        rayHits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 15f);

        for (int i = 0; i < rayHits.Length; i++)
        {
            if (rayHits[i].collider.gameObject.tag == "Ground")
            {
                Quaternion rotation = Quaternion.LookRotation(rayHits[i].normal, Vector3.up);
                GameObject thrown = Instantiate(trap, transform.position, rotation) as GameObject;
                Vector3 target = rayHits[i].point;
                thrown.SendMessage("setEndPoints", target);
                thrown.SendMessage("setSpeed", speed);

                return;
            }
        }
    }
}
