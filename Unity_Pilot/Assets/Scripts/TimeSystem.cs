using UnityEngine;
using System.Collections;

public class TimeSystem : MonoBehaviour {

	public GUIText clock;
	public Transform hand;
	
	public float secondsPerMinute;
	public int triggerWaveTime;

	public Transform sun;

	public float dawnRotation = 45f;
	public float dawnIntensity = 0.2f;
	public Color dawnColor = new Color(0f, 0f, 0f);
	public float dayRotation = 90f;
	public float dayIntensity = 0.4f;
	public Color dayColor = new Color(0f, 0f, 0f);
	public float duskRotation = 135f;
	public float duskIntensity = 0.2f;
	public Color duskColor = new Color(0f, 0f, 0f);
	public float nightRotation = 270f;
	public float nightIntensity = 0.05f;
	public Color nightColor = new Color(0f, 0f, 0f);

	private float timer;
	private short hour;
	private short minute;

	private short lastMinuteChange;
	private short lastHourChange;

	private bool fade;

	enum timeOfDay{DUSK, DAY, DAWN, NIGHT};
	private timeOfDay sunState;
	
	void Start(){
		timer = 0;
		hour = 0;
		minute = 0;
	
		lastMinuteChange = -1;
		lastHourChange = -1;

		fade = false;
	}

	void Update(){
		timer += Time.deltaTime;

		if(timer > secondsPerMinute){
			timer -= secondsPerMinute;
			minute++;

			if(minute > 59){
				hour++;
				minute = 0;
			}
			if(hour > 23){
				hour = 0;
			}

			if(lastMinuteChange != minute){
				hand.localEulerAngles = new Vector3(0,0,(hour * 15.0f * -1.0f));
				hand.Rotate(0.0f, 0.0f,(minute * 0.25f * -1.0f));

				clock.text = hour.ToString("00") + ":" + minute.ToString("00");

				lastMinuteChange = minute;
			}
			if(lastHourChange != hour){
				if(hour == triggerWaveTime){
					Debug.Log("WAVE");
				}
				if(hour == 0){
					ChangeState(timeOfDay.DAWN);
				}else if(hour == 2){
					ChangeState(timeOfDay.DAY);
				}else if(hour == 17){
					ChangeState(timeOfDay.DUSK);
				}else if(hour == 19){
					ChangeState(timeOfDay.NIGHT);
				}

				Debug.Log(sunState);

				lastHourChange = hour;
			}
		}

		if(fade){
			switch(sunState){
				case timeOfDay.DAWN:
					print ("dawn");
					FadeToDawn();
					break;
				case timeOfDay.DAY:
					print ("day");
					FadeToDay();
					break;
				case timeOfDay.DUSK:
					print ("dusk");
					FadeToDusk();
					break;
				case timeOfDay.NIGHT:
					print ("night");
					FadeToNight();
					break;
			}
		}
	}

	void FadeToDawn(){
		sun.eulerAngles = new Vector3(dawnRotation, 0f, 0f);
		sun.light.intensity = dawnIntensity;
		sun.light.color = dawnColor;

		fade = false;
	}

	void FadeToDay(){
		sun.eulerAngles = new Vector3(dayRotation, 0f, 0f);
		sun.light.intensity = dayIntensity;
		sun.light.color = dayColor;

		fade = false;
	}

	void FadeToDusk(){
		sun.eulerAngles = new Vector3(duskRotation, 0f, 0f);
		sun.light.intensity = duskIntensity;
		sun.light.color = duskColor;

		fade = false;
	}

	void FadeToNight(){
		sun.eulerAngles = new Vector3(nightRotation, 0f, 0f);
		sun.light.intensity = nightIntensity;
		sun.light.color = nightColor;

		fade = false;
	}

	void ChangeState(timeOfDay tod){
		sunState = tod;
		fade = true;
	}
}