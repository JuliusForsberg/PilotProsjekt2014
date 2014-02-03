using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeSystem : MonoBehaviour {

	private delegate void Fade();

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

	private Fade fadeTo;

	private List<GameObject> lightList;

	void Start(){
		timer = 0;
		hour = 0;
		minute = 0;

		lastHourChange = -1;

		fade = false;

		lightList = new List<GameObject>();

		GameObject[] lightArray = GameObject.FindGameObjectsWithTag("NightLight");

		foreach(GameObject light in lightArray){
			lightList.Add(light);
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

			ClockHand.angle = (hour * 15f) + (minute * 0.25f);

			//Temp clock
			clock.text = hour.ToString("00") + ":" + minute.ToString("00");

			if(lastHourChange != hour){
				if(hour == triggerWaveTime){
					Debug.Log("WAVE");
					waveManager.SetActive(true);
				}
				switch(hour){
					case 0:
						fadeTo = FadeToDawn;
						fade = true;
						
						GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
						if(enemies.Length > 0){
							foreach(GameObject en in enemies){
								en.GetComponent<Enemy>().Petrify();
							}
						}

						foreach(GameObject light in lightList){
							light.SetActive(false);
						}
						break;
					case 2:
						fadeTo = FadeToDay;
						fade = true;
						break;
					case 16:
						fadeTo = FadeToDusk;
						fade = true;
						break;
					case 19:
						fadeTo = FadeToNight;
						fade = true;
						fadeNight = Time.time + 3f;

						foreach(GameObject light in lightList){
							light.SetActive(true);
						}
						break;
				}
				lastHourChange = hour;
			}
		}

		if(fade){
			fadeTo();
		}
	}

	private void FadeToDawn(){
		//Debug.Log("FadeDawn");
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
		//Debug.Log("FadeDay");
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
		//Debug.Log("FadeDusk");
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
		//Debug.Log("FadeNight");
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
}