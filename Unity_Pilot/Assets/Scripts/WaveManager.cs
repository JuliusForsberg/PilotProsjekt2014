using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {

	public string[] waves;
	public GameObject[] enemyType;
	public float spawnInterval = 1f;
	public int waveNumber = -1;

	public int spawnerFinished;

	private Transform[] spawns;
	private float[] nextSpawnTime;

	private int[,] spawnNumber;

	private int[] iteraterNumber;
	private int[] iterator;

	private List<int>[,] spawnArrayList;

	//TESTING
	private float nextActiveTime;
	private bool waveActive;
	//------

	void Start(){
		spawnerFinished = 0;

		spawns = new Transform[transform.childCount];
		spawns[0] = transform.FindChild("SpawnPointSouth");
		spawns[1] = transform.FindChild("SpawnPointWest");
		spawns[2] = transform.FindChild("SpawnPointEast");

		spawnNumber = new int[transform.childCount,waves.Length];

		spawnArrayList = new List<int>[3,2];
		for(int e=0; e<3; e++){
			List<int> spawnEnemyNumber = new List<int>();
			List<int> spawnEnemyType = new List<int>();
			spawnArrayList[e,0] = spawnEnemyNumber;
			spawnArrayList[e,1] = spawnEnemyType;
		}

		for(int j=0; j<waves.Length; j++){
			string[] splitWave;
			splitWave = waves[j].Split(new char[] {'/'});

			for(int k=0; k<splitWave.Length; k++){
				if(splitWave[k] != "0"){
					if(splitWave[k].Contains(",")){
						string[] splitSpawn;
						splitSpawn = splitWave[k].Split(new char[] {','});

						for(int l=0; l<splitSpawn.Length; l++){
							if(splitSpawn[l].Contains("-")){
								string[] splitType;
								splitType = splitSpawn[l].Split(new char[] {'-'});

								spawnArrayList[k,0].Add(int.Parse(splitType[0]));
								spawnArrayList[k,1].Add(int.Parse(splitType[1]));

								spawnNumber[k,j] = (int)spawnNumber[k,j] + (int)spawnArrayList[k,0][spawnArrayList[k,0].Count-1];
								//Debug.Log(splitType[0]);
								//Debug.Log(splitType[1]);
							}else{
								Debug.Log("Specify type of enemy: 2");
							}
						}
					}else{
						if(splitWave[k].Contains("-")){
							string[] splitType;
							splitType = splitWave[k].Split(new char[] {'-'});

							spawnArrayList[k,0].Add(int.Parse(splitType[0]));
							spawnArrayList[k,1].Add(int.Parse(splitType[1]));

							spawnNumber[k,j] = (int)spawnNumber[k,j] + (int)spawnArrayList[k,0][spawnArrayList[k,0].Count-1];
							//Debug.Log(splitType[0]);
							//Debug.Log(splitType[1]);
						}else{
							Debug.Log("Specify type of enemy: 1");
						}
					}
				}else{
					Debug.Log("Do nothing");
				}
			}
		}
		iteraterNumber = new int[transform.childCount];
		iterator = new int[transform.childCount];

		for(int sp=0; sp<iteraterNumber.Length; sp++){
			for(int list=0; list<spawnArrayList[sp, 0].Count; list++){
				iteraterNumber[sp] += spawnArrayList[sp,0][list];
			}
		}

		nextSpawnTime = new float[transform.childCount];
	}

	void OnEnable(){
		waveActive = true;
		waveNumber++;
	}

	void Update(){
		if(waveActive){
			if(waveNumber < waves.Length){
				for(int i=0; i<spawns.Length; i++){
					if(spawnNumber[i,waveNumber] > 0){
						if(spawnArrayList[i,0][iterator[i]] > 0){
							if(Time.time >= nextSpawnTime[i]){
								SpawnEnemy(i, spawnArrayList[i,1][iterator[i]]);
								spawnArrayList[i,0][iterator[i]]--;
							}
						}else if(iterator[i] < iteraterNumber[i]){
							iterator[i]++;
							//Debug.Log("It++: " + iterator[i] + "Spawn: " + i);
						}
					}else if(spawnNumber[i,waveNumber] != -1){
						spawnerFinished++;
						spawnNumber[i,waveNumber] = -1;
					}
				}
			}else{
				Debug.Log("Spawn waves with multiplier");
			}
		}

		if(spawnerFinished == 3){
			spawnerFinished = 0;
			waveActive = false;
			//gameObject.SetActive(false);

			nextActiveTime = Time.time + 3f;
		}

		//TESTING
		if(!waveActive && Time.time >= nextActiveTime){
			OnEnable();
		}
		//------
	}

	private void SpawnEnemy(int num, int spawnType){
		spawnNumber[num,waveNumber]--;
		nextSpawnTime[num] = Time.time + spawnInterval;

		Instantiate(enemyType[spawnType].gameObject, spawns[num].position, spawns[num].rotation);
	}
}