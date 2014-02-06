using UnityEngine;
using System.Collections;

public class DebugTest : MonoBehaviour {

	NavMeshAgent navAgent;

	void Start(){
		navAgent = GetComponent<NavMeshAgent>();
		navAgent.SetDestination(GameObject.FindGameObjectWithTag("Crystal").transform.position);
	}

	void Update () {

	}
}
