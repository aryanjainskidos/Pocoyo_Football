using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class TalkListen : MonoBehaviour {

	public static TalkListen instance;

	[Range(-3f,3f)]
	public float pitch = 1.4f;
	public bool firstTimeEnabled = true;
	[Header("Sensibility")]
	public float startSensitivyOffset = 35f;
	public float stopSensitivyOffset = 20f;
	[Header("Signals")]
	public UnityEvent StartCalibrating;
	public UnityEvent StopCalibrating;
	public UnityEvent StartListening;
	public UnityEvent StopListening;
	public UnityEvent StartSpeaking;
	public UnityEvent StopSpeaking;

	private float startSensitivy = 0;
	private float stopSensitivy = 0;
	private AudioSource audioSource;

	//Talk Listen States
	private bool isCalibrated;
	[ReadOnly]
	public bool allowListen;
    private bool disabled;

	private float[] startValues;
	private float[] stopValues;
	private float[] calibrateValues;

	private int startIndex;
	private int stopIndex;
	private int calibrateIndex;

	private const int startSizeValues = 3;
	private const int stopSizeValues = 30;
	private const int calibrateSizeValues = 20;

	private float startListeningTime;
	private float stopListeningTime;

	private int delay;
	private IEnumerator coroutine;

	private void Awake()
	{
		instance = this;
	}

	void Start () 
	{
#if UNITY_EDITOR
		SPermanetVariables.isMicrophonePermitted = true;
		QualitySettings.vSyncCount = 0; // 1 == 60 frames / 2 == 30 frames
		Application.targetFrameRate = 25;
#endif

		Debug.Log("TALKING LISTEN START");

		isCalibrated = false;
		allowListen = false;
		disabled = false;

		audioSource = GetComponent<AudioSource>();
		audioSource.pitch = pitch;

		startValues = new float[startSizeValues];
		stopValues = new float[stopSizeValues];
		calibrateValues = new float[calibrateSizeValues];

		startIndex = stopIndex = 0;

		StartCoroutine(Calibrate());
		StartCoroutine(TalkingLoop());
	}

	void OnDisable()
    {
		StopAllCoroutines();
    }

	IEnumerator Calibrate()
    {
		yield return null;

		if (Microphone.devices.Length > 0 && SPermanetVariables.isMicrophonePermitted)
        {
			//Debug.Log("Start Calibration");
			isCalibrated = false;

			StartCalibrating.Invoke();

			audioSource.clip = Microphone.Start(null, true, 60, 11025);
			bool isCalibrating = true;

			while(isCalibrating)
            {
				float temp = 0;
				calibrateIndex++;

				calibrateValues[calibrateIndex] = GetMicrophoneVolume();

				for (int i = 0; i < calibrateSizeValues; i++)
				{
					temp += calibrateValues[i];
				}

				if (calibrateIndex == (calibrateSizeValues - 1))
				{
					float calibrateValue = (temp / calibrateValues.Length);

					startSensitivy = calibrateValue + startSensitivyOffset;
					stopSensitivy = calibrateValue + stopSensitivyOffset;

					isCalibrated = true;
					
					Microphone.End(null);

					isCalibrating = false;

					StopCalibrating.Invoke();
				}
				yield return null;
			}
		}
	}

	IEnumerator TalkingLoop()
    {
		while (true)
		{
			yield return new WaitUntil(() => { return isCalibrated == true; }); //Wait until calibrated

			if (!firstTimeEnabled)
			{
				disabled = false;
				allowListen = false;
				Microphone.End(null);
			}
			
			firstTimeEnabled = false;

			yield return new WaitUntil(() => { return allowListen; }); //Wait until system allow listen

			audioSource.clip = Microphone.Start(null, true, 60, 11025); //Start Microphone

			if (audioSource.clip == null) //fail to start record
            {
				yield return null; //wait next frame
				continue; //restart process
            }

			yield return new WaitUntil(() => { return (detectStartActivity() == 1 || disabled); }); //Wait until detect activity

			if(disabled)
            {
				continue;
            }

			StartListening.Invoke();
						
			delay = Microphone.GetPosition(null);			
			startListeningTime = Time.time;

			yield return new WaitUntil(() => { return (detectStopActivity() == 0 || disabled); }); //Wait until detect no activity

			if (disabled)
			{
				continue;
			}

			stopListeningTime = Time.time - startListeningTime;
			StopListening.Invoke();
		}
	}

	public void EnableTalking()
    {
		disabled = false;
		allowListen = true;
    }

	public void DisableTalking()
	{
        disabled = true;
		allowListen = false;
		CancelInvoke ();
	}

	public void Speak()
	{
		StartSpeaking.Invoke();

		audioSource.time = delay / 11025;
		audioSource.Play();
		Invoke("StopSpeak", stopListeningTime);

		disabled = true;
		allowListen = false;
	}

	public void StopSpeak()
    {
		StopSpeaking.Invoke();
	}

    #region MICROPHONE_STATE_CHECK
    private int detectStartActivity()
	{
		if (startSensitivy > 0f)
		{
			float temp = 0f;
			if (startIndex == (startSizeValues - 1))
			{
				startIndex = 0;
			}
			else
			{
				startIndex++;
			}

			startValues[startIndex] = GetMicrophoneVolume();

			for (int i = 0; i < startSizeValues; i++)
			{
				temp += startValues[i];
			}

			if (startIndex == (startSizeValues - 1))
			{
				if ((temp / startSizeValues) > startSensitivy)
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
		}
		return -1;
	}

	private int detectStopActivity()
	{
		if (stopSensitivy > 0)
		{
			float temp = 0;
			if (stopIndex == (stopSizeValues - 1))
			{
				stopIndex = 0;
			}
			else
			{
				stopIndex++;
			}

			stopValues[stopIndex] = GetMicrophoneVolume();

			for (int i = 0; i < stopSizeValues; i++)
			{
				temp += stopValues[i];
			}

			if (stopIndex == (stopSizeValues - 1))
			{
				if ((temp / stopSizeValues) > stopSensitivy)
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
		}
		return -1;
	}

	private float GetMicrophoneVolume()
	{
		int samples = 128;
		
		if (Microphone.IsRecording(null) && Microphone.GetPosition(null) > samples)
		{
			float levelMax = 0;
			float[] waveData = new float[samples];
			int micPosition = Microphone.GetPosition(null) - (samples + 1);

			audioSource.clip.GetData(waveData, micPosition);

			for (int i = 0; i < samples; i++)
			{
				float wavePeak = waveData[i] * waveData[i];
				if (levelMax < wavePeak)
				{
					levelMax = wavePeak;
				}
			}

			return (Mathf.Sqrt(levelMax) * 100f);
		}
		else
		{
			return 0;
		}
	}
	#endregion
}