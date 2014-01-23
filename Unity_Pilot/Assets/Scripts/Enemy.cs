using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float health = 20f;
	public float damage = 10f;
	public float attackSpeed = 2f;

	private PlayerStats player;
	private Crystal crystal;
	private AIPath pathTarget;

	public float defaultAggroLevel = 20f;
	public float aggroLevel;

	private float nextAttack;

	private TimeSystem timeSystem;

	//Testing
	public bool waveActive;
	//--------

	void Start(){
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
		crystal = GameObject.FindGameObjectWithTag("Crystal").GetComponent<Crystal>();
		pathTarget = gameObject.GetComponent<AIPath>();

		aggroLevel = 30f;

		if(pathTarget){
			pathTarget.target = crystal.transform;
		}

		if(waveActive){
			WaveManager waveManager = GameObject.Find("_WaveManager").gameObject.GetComponent<WaveManager>();

			health *= waveManager.waveMultiplier;
		}

		timeSystem = GameObject.Find("_TimeSystem").GetComponent<TimeSystem>();
	}

	void Update(){
		if(aggroLevel > defaultAggroLevel+5f && pathTarget.target != player.transform){
			if((player.transform.position - transform.position).sqrMagnitude < 5f*5f){
				Debug.Log("SetTarget: Player");
				pathTarget.target = player.transform;
			}
		}else if(aggroLevel < defaultAggroLevel && pathTarget.target == player.transform){
			Debug.Log("SetTarget: Crystal");
			pathTarget.target = crystal.transform;
		}

		if(Mathf.Abs(aggroLevel - defaultAggroLevel) > 0.1f){
			aggroLevel = Mathf.Lerp(aggroLevel, defaultAggroLevel, Time.deltaTime*0.2f);
		}

		if(Time.time >= nextAttack){
			if((pathTarget.target.position - transform.position).sqrMagnitude < 3f*3f){
				AttackTarget();
			}
		}
	}

	public void TakeDamage(float dmg){
		//Debug.Log("Enemy took damage");
		health -= dmg;
		aggroLevel += dmg*0.2f;


		if(health <= 0){
			//Debug.Log("Enemy died");
			StartCoroutine(WaitAndDie(0.2f));
			//Destroy (gameObject);
		}
	}

	public void Petrify(){
		health = -1;
		StartCoroutine(WaitAndDie(2f));
	}
	
	public bool isDead(){
		return health <= 0 ? true : false;
	}

	private void AttackTarget(){
		//Debug.Log("Attack Target");
		nextAttack = Time.time + attackSpeed;

		if(pathTarget.target == player.transform){
			player.TakeDamage(damage);
		}else if(pathTarget.target == crystal.transform){
			crystal.TakeDamage(damage);
		}

		aggroLevel -= damage*0.2f;
	}


	private IEnumerator WaitAndDie(float seconds){
		timeSystem.RemoveEnemy(this);
		GetComponent<AIPath>().enabled = false;
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