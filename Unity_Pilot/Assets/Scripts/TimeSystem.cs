using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeSystem : MonoBehaviour {

	public GUIText clock;
	public Transform hand;
	public GameObject waveManager;

	public float secondsPerMinute;
	public int triggerWaveTime;

	public Transform sun;

	public float dawnRotation = 35f;
	public float dawnIntensity = 0.2f;
	public Color dawnColor = new Color(0f, 0f, 0f);
	public float dayRotation = 70f;
	public float dayIntensity = 0.4f;
	public Color dayColor = new Color(0f, 0f, 0f);
	public float duskRotation = 145f;
	public float duskIntensity = 0.2f;
	public Color duskColor = new Color(0f, 0f, 0f);
	public float nightRotation = 185f;
	public float nightIntensity = 0.05f;
	public Color nightColor = new Color(0f, 0f, 0f);

	private float timer;
	private short hour;
	private short minute;
	
	private short lastHourChange;

	private bool fade;
	private float fadeNight;

	private enum TimeOfDay{DUSK, DAY, DAWN, NIGHT};
	private TimeOfDay sunState;

	private List<Enemy> enemyList;
	private float nextUpdatePath;

	private List<Light> lightList;

	void Start(){
		timer = 0;
		hour = 0;
		minute = 0;

		lastHourChange = -1;

		fade = false;

		enemyList = new List<Enemy>();

		lightList = new List<Light>();

		GameObject[] lightArray = GameObject.FindGameObjectsWithTag("NightLight");

		foreach(GameObject light in lightArray){
			lightList.Add(light.GetComponent<Light>());
		}
	}

	void Update(){
		//DEBUG
		if(Input.GetKeyDown(KeyCode.H))
			hour++;
		//----

		timer += Time.deltaTime;

		if(timer > secondsPerMinute){
			timer -= secondsPerMinute;
			minute++;

			if(minute > 59){
				hour++;
				minute = 0;

				if(hour > 23){
					hour = 0;
				}
			}

			hand.localEulerAngles = new Vector3(0,0,(hour * 15.0f * -1.0f));
			hand.Rotate(0.0f, 0.0f,(minute * 0.25f * -1.0f));

			//Temp clock
			clock.text = hour.ToString("00") + ":" + minute.ToString("00");

			if(lastHourChange != hour){
				if(hour == triggerWaveTime){
					Debug.Log("WAVE");
					waveManager.SetActive(true);
				}
				switch(hour){
				case 0:
					sunState = TimeOfDay.DAWN;
					fade = true;
					if(enemyList.Count > 0){
						while(enemyList.Count > 0){
							enemyList[0].Petrify();
						}
					}
					for(int i=0; i<lightList.Count; i++){
						lightList[i].enabled = false;
					}
					break;
				case 2:
					sunState = TimeOfDay.DAY;
					fade = true;
					break;
				case 16:
					sunState = TimeOfDay.DUSK;
					fade = true;
					break;
				case 19:
					sunState = TimeOfDay.NIGHT;
					fade = true;
					fadeNight = Time.time + 2f;
					for(int i=0; i<lightList.Count; i++){
						lightList[i].enabled = true;
					}
					break;
				}
				
				//Debug.Log(sunState);
				
				lastHourChange = hour;
			}
		}

		if(fade){
			switch(sunState){
				case TimeOfDay.DAWN:
					FadeToDawn();
					break;
				case TimeOfDay.DAY:
					FadeToDay();
					break;
				case TimeOfDay.DUSK:
					FadeToDusk();
					break;
				case TimeOfDay.NIGHT:
					FadeToNight();
					break;
			}
		}

		//Update pathfinding if there are enemies.
		if(enemyList.Count > 0){
			if(Time.time >= nextUpdatePath){
				//Debug.Log("Update path");
				AstarPath.active.Scan();
				nextUpdatePath = Time.time + 1f;
			}
		}
	}

	private void FadeToDawn(){
		if(!sun.gameObject.activeSelf){
			sun.gameObject.SetActive(true);
		}
		Quaternion quatDawnRotation = Quaternion.Euler(new Vector3(dawnRotation,0f,0f));

		sun.rotation = Quaternion.RotateTowards(sun.rotation, quatDawnRotation, Time.deltaTime*10f);

		if(sun.light.intensity < dawnIntensity){
			sun.light.intensity += 0.05f * Time.deltaTime;
		}
		sun.GetComponent<LensFlare>().brightness = sun.light.intensity+0.6f;
		sun.light.color = Color.Lerp(sun.light.color, dawnColor, Time.deltaTime*0.5f);

		float temp = RenderSettings.skybox.GetFloat("_Blend");
		temp = Mathf.Lerp(temp, 0f, Time.deltaTime*2f);
		RenderSettings.skybox.SetFloat("_Blend", temp);

		if(sun.rotation == quatDawnRotation && sun.light.intensity > dawnIntensity){
			fade = false;
		}
	}
	
	private void FadeToDay(){
		Quaternion quatDayRotation = Quaternion.Euler(new Vector3(dayRotation,0f,0f));

		sun.rotation = Quaternion.RotateTowards(sun.rotation, quatDayRotation, Time.deltaTime*10f);

		if(sun.light.intensity < dayIntensity){
			sun.light.intensity += 0.05f * Time.deltaTime;
		}
		sun.GetComponent<LensFlare>().brightness = sun.light.intensity+0.6f;
		sun.light.color = Color.Lerp(sun.light.color, dayColor, Time.deltaTime*0.5f);

		if(sun.rotation == quatDayRotation && sun.light.intensity > dayIntensity){
			fade = false;
		}
	}

	private void FadeToDusk(){
		Quaternion quatDuskRotation = Quaternion.Euler(new Vector3(duskRotation,0f,0f));

		sun.rotation = Quaternion.RotateTowards(sun.rotation, quatDuskRotation, Time.deltaTime*10f);

		if(sun.light.intensity > duskIntensity){
			sun.light.intensity -= 0.05f * Time.deltaTime;
		}
		sun.GetComponent<LensFlare>().brightness = sun.light.intensity+0.6f;
		sun.light.color = Color.Lerp(sun.light.color, duskColor, Time.deltaTime*0.5f);

		if(sun.rotation == quatDuskRotation && sun.light.intensity < duskIntensity){
			fade = false;
		}
	}

	private void FadeToNight(){
		Quaternion quatNightRotation = Quaternion.Euler(new Vector3(nightRotation,0f,0f));

		sun.rotation = Quaternion.RotateTowards(sun.rotation, quatNightRotation, Time.deltaTime*10f);

		if(sun.light.intensity > nightIntensity){
			sun.light.intensity -= 0.05f * Time.deltaTime;
		}
		sun.GetComponent<LensFlare>().brightness = sun.light.intensity+0.6f;
		sun.light.color = Color.Lerp(sun.light.color, nightColor, Time.deltaTime*0.5f);

		if(Time.time >= fadeNight){
			float temp = RenderSettings.skybox.GetFloat("_Blend");
			temp = Mathf.Lerp(temp, 1f, Time.deltaTime*2f);
			RenderSettings.skybox.SetFloat("_Blend", temp);
		}

		if(sun.rotation == quatNightRotation && sun.light.intensity <= nightIntensity){
			fade = false;
			sun.rotation = Quaternion.identity;
			sun.gameObject.SetActive(false);
		}
	}

	public void AddEnemy(Enemy enemy){
		enemyList.Add (enemy);
	}

	public void RemoveEnemy(Enemy enemy){
		enemyList.Remove (enemy);
	}
}