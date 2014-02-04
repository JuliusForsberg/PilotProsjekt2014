using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float health = 20f;
	public float damage = 10f;
	public float attackSpeed = 2f;

	private PlayerStats player;
	private Crystal crystal;
	private Gate gate;
	private Porridge porridge;

	public float defaultAggroLevel = 20f;
	public float aggroLevel;

	private float nextAttackTime;

	private NavMeshAgent navAgent;
	private Transform currentTarget;

	private float nextUpdateTime;

	public bool dead = false;
	//Testing
	public bool waveActive;
	//--------

	void Start(){
		navAgent = GetComponent<NavMeshAgent>();

		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
		crystal = GameObject.FindGameObjectWithTag("Crystal").GetComponent<Crystal>();

		if(GameObject.FindGameObjectWithTag("Gate")){
			gate = GameObject.FindGameObjectWithTag("Gate").GetComponent<Gate>();
		}

		aggroLevel = 30f;

		if(gate)
			currentTarget = gate.transform;
		else
			currentTarget = crystal.transform;

		navAgent.SetDestination(currentTarget.position);

		if(waveActive){
			WaveManager waveManager = GameObject.Find("_WaveManager").gameObject.GetComponent<WaveManager>();

			health *= waveManager.waveMultiplier;
		}
	}

	void Update(){
		if(dead)
			return;

		if(currentTarget == player.transform){

			if(Time.time >= nextUpdateTime){
				navAgent.SetDestination(player.transform.position);
				nextUpdateTime = Time.time + 0.2f;
			}

			if(aggroLevel < defaultAggroLevel+1f){
				if(!gate.isDestroyed()){
					Debug.Log("SetTarget: Gate");
					currentTarget = gate.transform;
					navAgent.ResetPath();
					navAgent.SetDestination(gate.transform.position);
				}else{
					Debug.Log("SetTarget: Crystal");
					currentTarget = crystal.transform;
					navAgent.ResetPath();
					navAgent.SetDestination(crystal.transform.position);
				}
			}
		}else if(currentTarget == gate.transform){
			if(gate.isDestroyed()){
				currentTarget = crystal.transform;
				navAgent.ResetPath();
				navAgent.SetDestination(crystal.transform.position);
			}
		}else if(currentTarget == crystal.transform){

		}else if(porridge != null){
			//Debug.Log ("Target Porridge: " + currentTarget);
		}else{
			if(!gate.isDestroyed()){
				Debug.Log("SetTarget: Gate");
				currentTarget = gate.transform;
				navAgent.ResetPath();
				navAgent.SetDestination(gate.transform.position);
			}else{
				Debug.Log("SetTarget: Crystal");
				currentTarget = crystal.transform;
				navAgent.ResetPath();
				navAgent.SetDestination(crystal.transform.position);
			}
		}

		if(currentTarget != player.transform && porridge == null){
			if(aggroLevel > defaultAggroLevel+5f){
				if((player.transform.position - transform.position).sqrMagnitude < 5f*5f){
					Debug.Log("SetTarget: Player");
					currentTarget = player.transform;
					navAgent.SetDestination(player.transform.position);
				}
			}
		}

		//Lerp the aggression level towards default.
		if(Mathf.Abs(aggroLevel - defaultAggroLevel) > 0.1f){
			aggroLevel = Mathf.Lerp(aggroLevel, defaultAggroLevel, Time.deltaTime*0.2f);
		}

		if(porridge == null){
			if(Time.time >= nextAttackTime){
				if((currentTarget.position - transform.position).sqrMagnitude < 3f*3f){
					AttackTarget();
				}
			}
		}
	}

	public void TakeDamage(float dmg){
		//Debug.Log("Enemy took damage");
		health -= dmg;
		aggroLevel += dmg*0.2f;


		if(health <= 0){
			//Debug.Log("Enemy died");
			dead = true;
			StartCoroutine(WaitAndDie(0.2f));
			//Destroy (gameObject);
		}
	}

	public void Petrify(){
		health = -1;
		dead = true;
		StartCoroutine(WaitAndDie(2f));
	}
	
	public bool isDead(){
		return dead;
	}

	public void SetPorridge(Porridge por){
		porridge = por;
		currentTarget = por.transform;
		navAgent.SetDestination(porridge.transform.position);
	}

	private void AttackTarget(){
		Debug.Log("Attack Target");
		nextAttackTime = Time.time + attackSpeed;

		if(currentTarget == player.transform){
			player.TakeDamage(damage);
		}else if(currentTarget == crystal.transform){
			crystal.TakeDamage(damage);
		}else if(currentTarget == gate.transform){
			gate.TakeDamage(damage);
		}

		aggroLevel -= damage*0.2f;
	}


	private IEnumerator WaitAndDie(float seconds){
		navAgent.enabled = false;
		transform.Rotate(Vector3.forward, 90f);

		yield return new WaitForSeconds(seconds);
		Destroy(gameObject);
	}
}
/*
public abstract class State{
	public abstract void OnEnterState(Transform pl);
	public abstract void Execute(Transform tr);
	public abstract void OnExitState();
}

public class StateChase : State{
	
	private Transform player;

	public override void OnEnterState(Transform pl){
		Debug.Log("EnterChase");
		player = pl;
	}

	public override void Execute(Transform tr){
		Debug.Log("Chase");



	}

	public override void OnExitState(){
		Debug.Log("ExitChase");
	}
}

public class StateAttack : State{

	public override void OnEnterState(Transform pl=null){
		Debug.Log("EnterAttack");
	}
	
	public override void Execute(Transform tr=null){
		Debug.Log("Attack");
	}
	
	public override void OnExitState(){
		Debug.Log("ExitAttack");
	}
}
*/