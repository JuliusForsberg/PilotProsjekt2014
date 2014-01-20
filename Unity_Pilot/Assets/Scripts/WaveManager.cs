using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {

	public List<Wave> waves;

	public float waveMultiplier;
	public float defaultWaveLength;

	public GameObject[] enemyType;
	public Vector2 spawnInterval;
	public Vector2 positionOffsetXZ;
	public int waveNumber = 0;

	private int[] spawnerFinished;

	private Transform[] spawns;
	private float[] nextSpawnTime;
	private float waveEndTime;

	public float waveLength;
	//TESTING
	private float nextActiveTime;
	private bool waveActive;
	//------

	void Start(){
		foreach(Wave wave in waves){
			wave.Initialize();
		}

		spawnerFinished = new int[]{0,0,0};

		spawns = new Transform[transform.childCount];
		spawns[0] = transform.FindChild("SpawnPointSouth");
		spawns[1] = transform.FindChild("SpawnPointWest");
		spawns[2] = transform.FindChild("SpawnPointEast");
		
		nextSpawnTime = new float[transform.childCount];
	}

	void OnEnable(){
		waveActive = true;
		waveNumber++;

		waveMultiplier = ((Mathf.Pow (waveNumber, 2)) * 0.005f)+1;

		waveLength = defaultWaveLength * waveMultiplier;
		waveEndTime = Time.time + waveLength;
	}

	void Update(){
		if(waveActive){
			if(waves.Count > 0){
				for(int i=0; i<spawns.Length; i++){
					if(Time.time >= nextSpawnTime[i]){
						CheckSpawn(i);
					}
				}
				if(spawnerFinished[0] == 1){
					if(spawnerFinished[1] == 1){
						if(spawnerFinished[2] == 1){
							spawnerFinished[0] = 0;
							spawnerFinished[1] = 0;
							spawnerFinished[2] = 0;

							waves.RemoveAt(0);
							
							//TESTING
							waveActive = false;
							//gameObject.SetActive(false);
							//------
							
							nextActiveTime = Time.time + 3f;
						}
					}
				}
			}else if(Time.time <= waveEndTime){
				for(int i=0; i<spawns.Length; i++){
					if(Time.time >= nextSpawnTime[i]){
						int type = Random.Range(0, enemyType.Length);
						SpawnEnemy(i, type);
					}
				}
			}else{
				//TESTING
				waveActive = false;
				//gameObject.SetActive(false);
				//------
				
				nextActiveTime = Time.time + 5f;
			}
		}



		//TESTING
		if(!waveActive && Time.time >= nextActiveTime){
			OnEnable();
		}
		//------
	}

	private void CheckSpawn(int currentSpawn){
		if(waves[0].GetSpawn(currentSpawn).SpawnNumber > 0){
			int enemyType = waves[0].GetSpawn(currentSpawn).EnemyType;
			
			//Spawn as normal. Else, go into sleep mode. Set type to -2 to prevent multiple Coroutines.
			if(enemyType >= 0){
				SpawnEnemy(currentSpawn, enemyType);
				waves[0].GetSpawn(currentSpawn).SpawnNumber = 1;
			}else if(enemyType == -1){
				waves[0].GetSpawn(currentSpawn).EnemyType = -2;
				StartCoroutine(SleepForSeconds((float)waves[0].GetSpawn(currentSpawn).GetEnemyCount(), currentSpawn));
			}
		}else if(spawnerFinished[currentSpawn] != 1){
			spawnerFinished[currentSpawn] = 1;
		}
	}

	private void SpawnEnemy(int num, int spawnType=0){
		nextSpawnTime[num] = Time.time + Random.Range(spawnInterval.x, spawnInterval.y);

		Vector3 spawnPoint = new Vector3(spawns[num].position.x, spawns[num].position.y, spawns[num].position.z);
		spawnPoint.x = Random.Range(spawnPoint.x - positionOffsetXZ.x, spawnPoint.x + positionOffsetXZ.x);
		spawnPoint.z = Random.Range(spawnPoint.z - positionOffsetXZ.y, spawnPoint.z + positionOffsetXZ.y);
		Instantiate(enemyType[spawnType].gameObject, spawnPoint, spawns[num].rotation);
	}

	private IEnumerator SleepForSeconds(float seconds, int spawnNumber){
		//Debug.Log("Start: " + Time.time);
		yield return new WaitForSeconds(seconds);
		//Debug.Log("End: " + Time.time);
		waves[0].GetSpawn(spawnNumber).RemoveInternalWave();
	}
}

[System.Serializable]
public class Wave{
	public Spawn southSpawn;
	public Spawn westSpawn;
	public Spawn eastSpawn;

	public void Initialize(){
		southSpawn.Initialize();
		westSpawn.Initialize();
		eastSpawn.Initialize();
	}

	public Spawn GetSpawn(int spawnNumber){
		switch(spawnNumber){
		case 0:
			return southSpawn;
		case 1:
			return westSpawn;
		case 2:
			return eastSpawn;
		default:
			return southSpawn;
		}
	}
}

[System.Serializable]
public class Spawn{
	public List<string> internalWaves;
	
	private List<int> enemyCount;
	private List<int> enemyType;

	public void Initialize(){
		enemyCount = new List<int>();
		enemyType = new List<int>();

		for(int i=0; i<internalWaves.Count; i++){
			if(internalWaves[i].Contains("#")){
				//Remove the "#".
				internalWaves[i] = internalWaves[i].Remove(0,1);

				//Add the wait amount to enemyCount, set the type to -1, to check later.
				enemyCount.Add(int.Parse(internalWaves[i]));
				enemyType.Add(-1);
			}else{
				//Split the two numbers, add them to their list.
				string[] split = internalWaves[i].Split(new char[]{'-'});

				enemyCount.Add(int.Parse(split[0]));
				enemyType.Add(int.Parse(split[1]));
			}
		}
	}
	
	public int EnemyType{
		get{
			return enemyType[0];
		}
		set{
			enemyType[0] = value;
		}
	}

	public int GetEnemyCount(){
		return enemyCount[0];
	}

	public void RemoveInternalWave(){
		enemyCount.RemoveAt(0);
		enemyType.RemoveAt(0);
		internalWaves.RemoveAt(0);
	}

	public int SpawnNumber{
		get{
			return internalWaves.Count;
		}
		set{
			enemyCount[0] -= value;
			//Debug.Log("Set: " + enemyCount[0]);

			if(enemyCount[0] <= 0){
				RemoveInternalWave();
			}
		}
	}
}