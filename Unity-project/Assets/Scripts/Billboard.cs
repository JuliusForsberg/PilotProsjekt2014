using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}

    GameObject player;
	// Update is called once per frame
	void Update () {

        transform.LookAt(player.transform);

	}
}
