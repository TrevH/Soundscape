using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class controller : MonoBehaviour {

	// ***** particle system controlling
	// blue particle
	public ParticleSystem blue_magiccircle_visualisation;
	ParticleSystem.MainModule blue_magiccircle_MainModule;

	public ParticleSystem blue_light_visualisation;
	ParticleSystem.MainModule blue_light_MainModule;

	public ParticleSystem blue_ring_visualisation;
	ParticleSystem.MainModule blue_ring_MainModule;

	// green particle
	public ParticleSystem green_water_visualisation;
	ParticleSystem.MainModule green_water_MainModule;

	public ParticleSystem green_sphere_visualisation;
	ParticleSystem.MainModule green_sphere_MainModule;

	public ParticleSystem green_plane2_visualisation;
	ParticleSystem.MainModule green_plane2_MainModule;

	// purple particle
	public ParticleSystem purple_circle_visualisation;
	ParticleSystem.MainModule purple_circle_MainModule;

	public ParticleSystem purple_particles_visualisation;
	ParticleSystem.MainModule purple_particles_MainModule;

	public ParticleSystem purple_electriclight_visualisation;

	// red particle
	public ParticleSystem red_flash_visualisation;
	ParticleSystem.MainModule red_flash_MainModule;

	public ParticleSystem red_flash2_visualisation;
	ParticleSystem.MainModule red_flash2_MainModule;

	public ParticleSystem red_flash3_visualisation;
	ParticleSystem.MainModule red_flash3_MainModule;

	public ParticleSystem red_light_visualisation;
	ParticleSystem.MainModule red_light_MainModule;

	public ParticleSystem red_smoke_visualisation;
	ParticleSystem.MainModule red_smoke_MainModule;

	// audio management
	private const int SAMPLE_SIZE = 1024;
	public float rmsValue;
	public float dbValue;
	public float pitchValue;

	public float maxVisualScale = 4.0f;
	public float visualModifier = 50.0f;
	public float smoothSpeed = 10.0f;
	public float keepPercentage = 0.5f;

	private AudioSource source;
	private float[] samples;
	private float[] spectrum;
	private float sampleRate;

	private float[] visualScale;
	public int amnVisual = 64;

	private bool firstTime = false;
	private bool nowRunning = false;

	public int blue_magiccircle_visualScaleIndex = 0;
	public int blue_light_visualScaleIndex = 0;
	public int blue_ring_visualScaleIndex = 0;

	public int green_water_visualScaleIndex = 0;
	public int green_sphere_visualScaleIndex = 0;
	public int green_plane2_visualScaleIndex = 0;

	public int purple_circle_visualScaleIndex = 0;
	public int purple_particles_visualScaleIndex = 0;

	public int red_flash_visualScaleIndex = 0;
	public int red_flash2_visualScaleIndex = 0;
	public int red_flash3_visualScaleIndex = 0;
	public int red_light_visualScaleIndex = 0;
	public int red_smoke_visualScaleIndex = 0;

	// sensor readings
	public float sensorReading;
	public float rangeRest;
	public float rangeFullStretch;

	public float maxStartSizeblue_magiccircle = 10.0f;
	public float maxStartSizeblue_light = 10.0f;
	public float maxStartSizeblue_ring = 10.0f;

	public float maxStartSizegreen_water = 2.0f;
	public float maxStartSizegreen_sphere_ = 2.0f;
	public float maxStartSizegreen_plane2_ = 2.0f;

	public float maxStartSizepurple_circle_ = 5.0f;
	public float maxStartSizepurple_particles_ = 5.0f;

	public float maxStartSizered_flash_ = 0.225f;
	public float maxStartSizered_flash2_ = 1.5f;
	public float maxStartSizered_flash3_ = 1.5f;
	public float maxStartSizered_light_ = 4.0f;
	public float maxStartSizered_smoke = 4.0f;

	public float maxStartSpeed = 4.0f;

	private static int numberofarduinos = 4;

	// ***** touch visualisation
	public int numberBubbles = 8;
	public Transform bubble;
	public float bubbleSpeed = 5;

	private Vector3[][] startPositionArrayAll = new Vector3[numberofarduinos][];
	private Vector3[][] targetPositionArrayAll = new Vector3[numberofarduinos][];
	private Vector3[][] finalPositionArrayAll = new Vector3[numberofarduinos][];

	private Vector3[] startPositions = new []
	{
		new Vector3(-5.0f, -20.0f, -8.6f),
		new Vector3(14.5f, -30.0f, -8.5f),
		new Vector3(-4.6f, -20.0f, 10.7f),
		new Vector3(15.0f, -20.0f, 11.0f)
	};

	// panel 0
	Vector3[] finalPositionArrayP0 = new [] 
	{
		new Vector3(-7.8f, -40.0f, -13.2f),
		new Vector3(-8.1f, -40.0f, -9.1f),
		new Vector3(-7.6f, -40.0f, -6.4f),
		new Vector3(-6.1f, -40.0f, -3.0f),
		new Vector3(-3.9f, -40.0f, -2.9f),
		new Vector3(-2.2f, -40.0f, -3.6f),
		new Vector3(-1.9f, -40.0f, -4.6f),
		new Vector3(-1.7f, -40.0f, -8.0f)
	};

	// panel1
	Vector3[] finalPositionArrayP1 = new [] 
	{
		new Vector3(12.1f, -30.0f, -10.0f),
		new Vector3(12.0f, -30.0f, -6.5f),
		new Vector3(11.7f, -30.0f, -4.3f),
		new Vector3(13.7f, -30.0f, -3.7f),
		new Vector3(15.2f, -30.0f, -4.3f),
		new Vector3(16.9f, -30.0f, -7.0f),
		new Vector3(17.2f, -30.0f, -8.6f),
		new Vector3(17.0f, -30.0f, -12.1f)
	};

	// panel2
	Vector3[] finalPositionArrayP2 = new [] 
	{
		new Vector3(-8.8f, -20.0f, 8.6f),
		new Vector3(-8.9f, -20.0f, 11.4f),
		new Vector3(-9.0f, -20.0f, 12.8f),
		new Vector3(-6.5f, -20.0f, 14.6f),
		new Vector3(-4.0f, -20.0f, 15.5f),
		new Vector3(-2.3f, -20.0f, 15.3f),
		new Vector3(-1.9f, -20.0f, 13.6f),
		new Vector3(-1.6f, -20.0f, 10.5f)
	};

	// panel3
	Vector3[] finalPositionArrayP3 = new [] 
	{
		new Vector3(12.4f, -20.0f, 11.0f),
		new Vector3(12.0f, -20.0f, 14.2f),
		new Vector3(11.7f, -20.0f, 15.5f),
		new Vector3(13.8f, -20.0f, 15.4f),
		new Vector3(15.0f, -20.0f, 15.2f),
		new Vector3(17.6f, -20.0f, 13.0f),
		new Vector3(18.0f, -20.0f, 10.5f),
		new Vector3(18.0f, -20.0f, 7.6f)
	};

	//private Transform[] bubbleList;
	public float[] restValues0 = new float[8];
	public float[] pressedValues0 = new float[8];

	public float[] restValues1 = new float[8];
	public float[] pressedValues1 = new float[8];

	public float[] restValues2 = new float[8];
	public float[] pressedValues2 = new float[8];

	public float[] restValues3 = new float[8];
	public float[] pressedValues3 = new float[8];

	private float[][] restValues = new float[numberofarduinos][];
	private float[][] pressedValues = new float[numberofarduinos][];

	// public float maxVisualScale = 4.0f;
	public float visualModifierBubbles = 2.0f;
	// public float smoothSpeed = 10.0f;
	// public float keepPercentage = 0.5f;

	private Transform[][] touchList = new Transform[numberofarduinos][];
	private float[][] visualScaleBubbles = new float[numberofarduinos][];

	// ****** serial reading
	// these come from prefabs added to the scene
	// using UnitySerialPort code
	// http://www.dyadica.co.uk/unity3d-serialport-script/

	public UnitySerialPort portPanel0;
	public UnitySerialPort portPanel1;
	public UnitySerialPort portPanel2;
	public UnitySerialPort portPanel3;

	public static string strIn;

	private int[] readings = new [] {1,1,1,1,1,1,1,1};
	private int[][] allReadings = new int [numberofarduinos][];

	private string readingsToSend;

	// ***** TCP
	// internal Boolean socketReady = false;
	// TcpClient mySocket;
	// NetworkStream theStream;
	// StreamWriter theWriter;
	// StreamReader theReader;
	// String Host = "169.254.51.255";
	// Int32 Port = 8065;

	// ***** UDP communication
	// private static int localPort;
   
    // prefs
    // private string IP;  // define in init
    // public int port;  // define in init

	// private string readingsToSend;
   
    // "connection" things
    // IPEndPoint remoteEndPoint;
    // UdpClient client;

	// ***** oscsimpl

	public OscOut oscout;

	// ****** touch interactions
	private float touchThreshold = 0.7f;
	public int restToPressedDifference = 100;

	// ****** clip_slot controllers
	private int[][] clip_slot_triggers = new int[numberofarduinos][];
	private int[] clip_slot_triggers_start_values = new [] {0,0,0,0,0,0,0,0};

	// ***** manual start - to avoid false starts
	private bool manualStart = false;

	void Start () 
	{
		Debug.Log ("working");
		// ***** setting up visualisations

		// get the main module for each particle system we are controlling
		blue_magiccircle_MainModule = blue_magiccircle_visualisation.main;
		blue_light_MainModule = blue_light_visualisation.main;
		blue_ring_MainModule = blue_ring_visualisation.main;

		green_water_MainModule = green_water_visualisation.main;
		green_sphere_MainModule = green_sphere_visualisation.main;
		green_plane2_MainModule = green_plane2_visualisation.main;

		purple_circle_MainModule = purple_circle_visualisation.main;
		purple_particles_MainModule = purple_particles_visualisation.main;

		red_flash_MainModule = red_flash_visualisation.main;
		red_flash2_MainModule = red_flash2_visualisation.main;
		red_flash3_MainModule = red_flash3_visualisation.main;
		red_light_MainModule = red_light_visualisation.main;
		red_smoke_MainModule = red_smoke_visualisation.main;

		blue_magiccircle_visualisation.Stop();
		blue_light_visualisation.Stop();
		blue_ring_visualisation.Stop();
		green_water_visualisation.Stop();
		green_sphere_visualisation.Stop();
		green_plane2_visualisation.Stop();
		purple_circle_visualisation.Stop();
		purple_particles_visualisation.Stop();
		purple_electriclight_visualisation.Stop();
		red_flash_visualisation.Stop();
		red_flash2_visualisation.Stop();
		red_flash3_visualisation.Stop();
		red_light_visualisation.Stop();
		red_smoke_visualisation.Stop();

		Debug.Log("displays connected: " + Display.displays.Length);

		// ***** allow for multi-display
		if (Display.displays.Length > 1)
			Display.displays[1].Activate();
		if (Display.displays.Length > 2)
			Display.displays[2].Activate();

		// ***** setting up touch bubbles
		// positionArrayAll [0] = positionArrayB0;
		// positionArrayAll [1] = positionArrayB1;
		// positionArrayAll [2] = positionArrayB2;
		// positionArrayAll [3] = positionArrayB3;

		// ***** setting up touch bubbles & start readings array
		for (int k = 0; k < numberofarduinos; k++) 
		{
			// set start readings
			allReadings [k] = new int[8];
			startPositionArrayAll [k] = new Vector3[numberBubbles];
			targetPositionArrayAll [k] = new Vector3[numberBubbles];
			
			// set bubble start position
			for (int n = 0; n<numberBubbles; n++)
			{
				startPositionArrayAll[k][n] = startPositions[k];
				targetPositionArrayAll[k][n] = startPositions[k];
			}

			clip_slot_triggers [k] = clip_slot_triggers_start_values;
		}

		finalPositionArrayAll[0] = finalPositionArrayP0;
		finalPositionArrayAll[1] = finalPositionArrayP1;
		finalPositionArrayAll[2] = finalPositionArrayP2;
		finalPositionArrayAll[3] = finalPositionArrayP3;

		spawnBubbles ();

		// ***** setting rest and pressed values
		restValues [0] = restValues0;
		restValues [1] = restValues1;
		restValues [2] = restValues2;
		restValues [3] = restValues3;

		pressedValues [0] = pressedValues0;
		pressedValues [1] = pressedValues1;
		pressedValues [2] = pressedValues2;
		pressedValues [3] = pressedValues3;

		//UDPinit();
		//setupSocket();
	}

	// start UDP communication
	// public void UDPinit()
    // {
    //     Debug.Log("UDPSend.init()");
       
    //     // define
    //     IP="169.254.51.255";
    //     port=8051;

    //     remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
    //     client = new UdpClient();

    //     Debug.Log("Sending to "+IP+" : "+port);
    //     Debug.Log("Testing: nc -lu "+IP+" : "+port);
   
    // }

	// setup audio visualisations
	void InitialStart()
	{
		Debug.Log ("Initial Start");

		blue_magiccircle_visualisation.Play();
		blue_light_visualisation.Play();
		blue_ring_visualisation.Play();
		green_water_visualisation.Play();
		green_sphere_visualisation.Play();
		green_plane2_visualisation.Play();
		purple_circle_visualisation.Play();
		purple_particles_visualisation.Play();
		purple_electriclight_visualisation.Play();
		red_flash_visualisation.Play();
		red_flash2_visualisation.Play();
		red_flash3_visualisation.Play();
		red_light_visualisation.Play();
		red_smoke_visualisation.Play();

		source = GetComponent<AudioSource>();;
		// to play the built in clip comment out the following 3 lines
		source.clip = Microphone.Start("Built-In Microphone", true, 10, 44100);
		source.loop = true;
		while (!(Microphone.GetPosition(null) > 0)){}
		// end of microphone code
		samples = new float[SAMPLE_SIZE];
		spectrum = new float[SAMPLE_SIZE];
		sampleRate = AudioSettings.outputSampleRate;
		visualScale = new float[amnVisual];
		nowRunning = true;
		source.Play ();
	}
	
	// UPDATE
	void Update () 
	{
		if (Input.GetKey ("up") && !manualStart)
		{
			calibrateReadings();
		} 

		if (Input.GetKeyDown ("space"))
		{
			manualStart = true;
			Debug.Log("manual start!!");
		} 

		if (firstTime)
		{
			Debug.Log("first time");
			firstTime = false;
			oscout.Send("/start", 1);
			InitialStart ();
		}

		if (nowRunning) 
		{
			AnalyseSound();
			updateVisualisation ();
		}

		// ***** reading sensors

		for (int j = 0; j < numberofarduinos; j++) 
		{
			//readings = allReadings [j];
			try{
				switch (j)
				{
					case 0:
						strIn = portPanel0.readingsData;
						break;
					case 1:
						strIn = portPanel1.readingsData;
						break;
					case 2:
						strIn = portPanel2.readingsData;
						break;
					case 3:
						strIn = portPanel3.readingsData;
						break;
					default:
						break;
				}
				//strIn = ReadFromArduino(j);
				//Debug.Log("receieved from arduino" + j + ":" + strIn);
				
				if (strIn != null)
				{
					strIn = strIn.Trim();
					string[] readingsText = strIn.Split(' ');
					if (readingsText.Length == numberBubbles)
					{
						for (int i = 0; i<numberBubbles; i++)
						{
							readingsText[i] = readingsText[i].Trim();
							bool Result;
							int number;
							Result = int.TryParse(readingsText[i], out number);
							if (Result)
							{
								//readings[i] = number;
								allReadings[j][i] = number;
								//Debug.Log("arduino:" + j + " bubble:" + i + " reading:" + number + " allReadings:" + allReadings[j][i]);
							}
							else
							{
								Debug.Log("failed");
							}
						}
						//allReadings [j] = readings;
					}
				}
			}
			catch(TimeoutException) {
				Debug.Log ("timeout exception");
			}
		}

		readingsToSend = "";

		// **** process readings
		for (int o = 0; o < numberofarduinos; o++)
		{
			for (int p = 0; p < numberBubbles; p++) 
			{
				// *** creating string for MAX
				//Debug.Log ("allreadings" + o + ":" + allReadings [o][p]);
				float currentDiff = restValues[o][p] - allReadings[o][p];
				float maxDiff = restValues[o][p] - pressedValues[o][p];
				float percentDiff = currentDiff / maxDiff;
				if (percentDiff < 0)
				{
					percentDiff = 0;
				}
				if (percentDiff > 1)
				{
					percentDiff = 1;
				}
				// int valueForMax = Mathf.RoundToInt(percentDiff * 128);
				// String panelID = "/panel" + o + "_" + p;
				// Debug.Log(panelID + ":" + valueForMax);
				// oscout.Send(panelID, valueForMax);
				// readingsToSend += valueForMax + " ";

				readingsToSend += percentDiff + " ";

				// *** send specific triggers to max
				if (nowRunning)
				{
					if (percentDiff > touchThreshold && clip_slot_triggers[o][p] == 0)
					{
						Debug.Log("press clip_slot_trigger current value pre:" + clip_slot_triggers[o][p] + "percentDiff:" + percentDiff);
						clip_slot_triggers[o][p] = 1;
						Debug.Log("press clip_slot_trigger current value post:" + clip_slot_triggers[o][p] + "percentDiff:" + percentDiff);
						checkTriggers(o, p, percentDiff);
					}

					if (percentDiff < 0.05f && clip_slot_triggers[o][p] == 1)
					{
						Debug.Log("not press the clip_slot_trigger for pre  panel" + o + "_" + p + " is:" + clip_slot_triggers[o][p] + " should be 0");
						clip_slot_triggers[o][p] = 0;
						Debug.Log("not press the clip_slot_trigger for post panel" + o + "_" + p + " is:" + clip_slot_triggers[o][p] + " should be 0");
					}
				}

				// *** check for first touch to start everything
				if (!firstTime && !nowRunning && manualStart)
				{
					if (percentDiff > touchThreshold)
						firstTime = true;
				}
			}
		}
		// Debug.Log(readingsToSend);
		OscMessage readingsMessage = new OscMessage("/touch");
		readingsMessage.Add(readingsToSend);
		oscout.Send(readingsMessage);

		//writeSocket(readingsToSend);
		//sendStringUDP(readingsToSend);

		updateBubbles();
		
	}

	// TOUCH setting up touch bubbles / feedback
	void spawnBubbles()
	{
		for (int k = 0; k < numberofarduinos; k++)
		{
			visualScaleBubbles[k] = new float[numberBubbles];
			touchList[k] = new Transform[numberBubbles];
			for (int i = 0; i < numberBubbles; i++) 
			{
				var go = Instantiate (bubble);

				// *** when just scaling
				go.transform.position = finalPositionArrayAll[k][i];

				// *** when trying to move
				// go.transform.position = startPositionArrayAll[k][i];
				touchList[k] [i] = go.transform;
				//Debug.Log ("created bubble" + i);
			}
		}
	}

	// TOUCH updating bubbles
	public void updateBubbles()
	{
		float step = bubbleSpeed * Time.deltaTime;

		for (int m = 0; m < numberofarduinos; m++) 
		{
			int bubbleIndex = 0;

			while (bubbleIndex < numberBubbles) 
			{
				//Debug.Log (m + ":" + bubbleIndex + ":" + allReadings [m] [bubbleIndex]);
				float bubbleRange = restValues[m] [bubbleIndex] - pressedValues[m] [bubbleIndex];
				float difference = restValues[m] [bubbleIndex] - allReadings[m] [bubbleIndex];
				
				// ** scaling code
				float scaleY = difference / bubbleRange * visualModifierBubbles;
				visualScaleBubbles[m] [bubbleIndex] -= Time.deltaTime * smoothSpeed;
				if (visualScaleBubbles[m] [bubbleIndex] < scaleY) 
				{
					visualScaleBubbles[m] [bubbleIndex] = scaleY;
				}
				if (visualScaleBubbles[m] [bubbleIndex] > maxVisualScale) 
				{
					visualScaleBubbles[m] [bubbleIndex] = maxVisualScale;
				}
				touchList[m] [bubbleIndex].localScale = Vector3.one * visualScaleBubbles[m] [bubbleIndex];

				// ***** attempt at movement code
				// float multiplier = difference / bubbleRange;
				// if (multiplier < 0)
				// 	multiplier = 0;
				// if (multiplier > 1)
				// 	multiplier = 1;
				// float newTargetX = (startPositionArrayAll[m][bubbleIndex].x - finalPositionArrayAll[m][bubbleIndex].x) * multiplier;
				// float newTargetZ = (startPositionArrayAll[m][bubbleIndex].z - finalPositionArrayAll[m][bubbleIndex].z) * multiplier;
				// targetPositionArrayAll[m][bubbleIndex].x = newTargetX;
				// targetPositionArrayAll[m][bubbleIndex].z = newTargetZ;
				// touchList[m][bubbleIndex].position = Vector3.MoveTowards(touchList[m][bubbleIndex].position, targetPositionArrayAll[m][bubbleIndex], step);
				
				bubbleIndex++;
			}
		}

	}

	// AUDIO: updating particle visualisations
	void updateVisualisation()
	{
		// this code is to tie the visualisation to the sensor readings
		// Debug.Log("running updateVis");
		// float multiplier = (rangeRest - sensorReading) / (rangeRest - rangeFullStretch);
		// float newStartSize =  multiplier * maxStartSize;
		// float newStartSpeed = multiplier * maxStartSpeed;
		// cvMainModule.startLifetime = newStartSize;
		// cvMainModule.startSpeed = newStartSpeed;
		int visualIndex = 0;
		int spectrumIndex = 0;
		int averageSize = (int)(SAMPLE_SIZE * keepPercentage / amnVisual);

		while (visualIndex < amnVisual) 
		{
			int j = 0;
			float sum = 0;
			while (j < averageSize) 
			{
				sum += spectrum [spectrumIndex];
				spectrumIndex++;
				j++;
			}
			float scaling = sum / averageSize * visualModifier;
			//Debug.Log(scaling);
			visualScale [visualIndex] = scaling;
			visualIndex++;
		}
		// Panel 0
		float newStartSizeblue_magiccirle =  visualScale[blue_magiccircle_visualScaleIndex] * maxStartSizeblue_magiccircle;
		float newStartSpeedblue_magiccircle = visualScale[blue_magiccircle_visualScaleIndex] * maxStartSpeed;
		blue_magiccircle_MainModule.startSize = newStartSizeblue_magiccirle;
		blue_magiccircle_MainModule.startSpeed = newStartSpeedblue_magiccircle;

		float newStartSizeblue_light =  visualScale[blue_light_visualScaleIndex] * maxStartSizeblue_light;
		float newStartSpeedblue_light = visualScale[blue_light_visualScaleIndex] * maxStartSpeed;
		blue_light_MainModule.startSize = newStartSizeblue_light;
		blue_light_MainModule.startSpeed = newStartSpeedblue_light;

		float newStartSizeblue_ring =  visualScale[blue_ring_visualScaleIndex] * maxStartSizeblue_ring;
		float newStartSpeedblue_ring = visualScale[blue_ring_visualScaleIndex] * maxStartSpeed;
		blue_ring_MainModule.startSize = newStartSizeblue_ring;
		blue_ring_MainModule.startSpeed = newStartSpeedblue_ring;

		// Panel 1
		float newStartSizep1_1 =  visualScale[green_water_visualScaleIndex] * maxStartSizegreen_water;
		float newStartSpeedp1_1 = visualScale[green_water_visualScaleIndex] * maxStartSpeed;
		green_water_MainModule.startSize = newStartSizep1_1;
		green_water_MainModule.startSpeed = newStartSpeedp1_1;

		float newStartSizegreen_sphere_ =  visualScale[green_sphere_visualScaleIndex] * maxStartSizegreen_sphere_;
		float newStartSpeedgreen_sphere_ = visualScale[green_sphere_visualScaleIndex] * maxStartSpeed;
		green_sphere_MainModule.startSize = newStartSizegreen_sphere_;
		green_sphere_MainModule.startSpeed = newStartSpeedgreen_sphere_;

		float newStartSizegreen_plane2_ =  visualScale[green_plane2_visualScaleIndex] * maxStartSizegreen_plane2_;
		float newStartSpeedgreen_plane2_ = visualScale[green_plane2_visualScaleIndex] * maxStartSpeed;
		green_plane2_MainModule.startSize = newStartSizegreen_plane2_;
		green_plane2_MainModule.startSpeed = newStartSpeedgreen_plane2_;
		
		// Panel 2
		float newStartSizepurple_circle_ =  visualScale[purple_circle_visualScaleIndex] * maxStartSizepurple_circle_;
		float newStartSpeedpurple_circle_ = visualScale[purple_circle_visualScaleIndex] * maxStartSpeed;
		purple_circle_MainModule.startSize = newStartSizepurple_circle_;
		purple_circle_MainModule.startSpeed = newStartSpeedpurple_circle_;

		float newStartSizepurple_particles_ =  visualScale[purple_circle_visualScaleIndex] * maxStartSizepurple_particles_;
		float newStartSpeedpurple_particles_ = visualScale[purple_circle_visualScaleIndex] * maxStartSpeed;
		purple_particles_MainModule.startSize = newStartSizepurple_particles_;
		purple_particles_MainModule.startSpeed = newStartSpeedpurple_particles_;

		// Panel 3
		float newStartSizered_flash_ =  visualScale[red_flash_visualScaleIndex] * maxStartSizered_flash_;
		float newStartSpeedred_flash_ = visualScale[red_flash_visualScaleIndex] * maxStartSpeed;
		red_flash_MainModule.startSize = newStartSizered_flash_;
		red_flash_MainModule.startSpeed = newStartSpeedred_flash_;

		float newStartSizered_flash2_ =  visualScale[red_flash2_visualScaleIndex] * maxStartSizered_flash2_;
		float newStartSpeedred_flash2_ = visualScale[purple_circle_visualScaleIndex] * maxStartSpeed;
		red_flash2_MainModule.startSize = newStartSizered_flash2_;
		red_flash2_MainModule.startSpeed = newStartSpeedred_flash2_;

		float newStartSizered_flash3_ =  visualScale[red_flash3_visualScaleIndex] * maxStartSizered_flash3_;
		float newStartSpeedred_flash3_ = visualScale[red_flash3_visualScaleIndex] * maxStartSpeed;
		red_flash3_MainModule.startSize = newStartSizered_flash3_;
		red_flash3_MainModule.startSpeed = newStartSpeedred_flash3_;

		float newStartSizered_light_ =  visualScale[red_light_visualScaleIndex] * maxStartSizered_light_;
		float newStartSpeedred_light_ = visualScale[red_light_visualScaleIndex] * maxStartSpeed;
		red_light_MainModule.startSize = newStartSizered_light_;
		red_light_MainModule.startSpeed = newStartSpeedred_light_;

		float newStartSizered_smoke =  visualScale[red_smoke_visualScaleIndex] * maxStartSizered_smoke;
		float newStartSpeedred_smoke = visualScale[red_smoke_visualScaleIndex] * maxStartSpeed;
		red_smoke_MainModule.startSize = newStartSizered_smoke;
		red_smoke_MainModule.startSpeed = newStartSpeedred_smoke;
	}

	// AUDIO: analyse sound spectrum
	private void AnalyseSound()
	{
		source.GetOutputData (samples, 0);

		int i = 0;
		float sum = 0;
		for (; i < SAMPLE_SIZE; i++) 
		{
			sum += samples [i] * samples [i];
		}
		rmsValue = Mathf.Sqrt (sum / SAMPLE_SIZE);

		dbValue = 20 * Mathf.Log10 (rmsValue / 0.1f);

		// Get sound spectrum

		source.GetSpectrumData (spectrum, 0, FFTWindow.BlackmanHarris);

		// Find pitch

		float maxV = 0;
		var maxN = 0;

		for (i = 0; i < SAMPLE_SIZE; i++) 
		{
			if (!(spectrum[i] > maxV) || !(spectrum[i] > 0.0f))
			{
				continue;
			}
			maxV = spectrum [i];
			maxN = i;
		}

		float freqN = maxN;
		if (maxN > 0 && maxN < SAMPLE_SIZE - 1) 
		{
			var dL = spectrum [maxN - 1] / spectrum [maxN];
			var dR = spectrum [maxN + 1] / spectrum [maxN];
		}
		pitchValue = freqN * (sampleRate / 2) / SAMPLE_SIZE;
	}

	// TCP
	// public void setupSocket() 
	// {
	// 	try {
	// 	mySocket = new TcpClient(Host, Port);
	// 	theStream = mySocket.GetStream();
	// 	theWriter = new StreamWriter(theStream);
	// 	theReader = new StreamReader(theStream);
	// 	socketReady = true;
	// 	}
	// 	catch (Exception e) {
	// 	Debug.Log("Socket error: " + e);
	// 	}
	// }

	// public void writeSocket(string theLine) 
	// {
	// 	if (!socketReady)
	// 		return;
	// 	String foo = theLine + "\r\n";
	// 	theWriter.Write(foo);
	// 	theWriter.Flush();
	// }

	// public String readSocket() 
	// {
	// 	if (!socketReady)
	// 		return "";
	// 	if (theStream.DataAvailable)
	// 		return theReader.ReadLine();
	// 	return "";
	// }

 	// public void closeSocket() 
	// {
	// 	if (!socketReady)
	// 		return;
	// 	theWriter.Close();
	// 	theReader.Close();
	// 	mySocket.Close();
	// 	socketReady = false;
	// }

	void checkTriggers(int panelNumber, int sensorNumber, float currentTouchValue)
	{
			String messageAddress = "/P" + panelNumber + "_" + sensorNumber;
			OscMessage triggerMessage = new OscMessage(messageAddress);
			triggerMessage.Add(clip_slot_triggers[panelNumber][sensorNumber]);
			oscout.Send(triggerMessage);
			Debug.Log("clip slot trigger message sent: " + messageAddress + "-" + clip_slot_triggers[panelNumber][sensorNumber] + " touch val:" + currentTouchValue);
	}

	void calibrateReadings()
	{
		Debug.Log("calibration started");

		int numberOfReadingsCalibration = 10;

		for (int j = 0; j < numberofarduinos; j++) 
		{
			for (int k = 0; k < numberOfReadingsCalibration; k++)
			{
				try{
					switch (j)
					{
						case 0:
							strIn = portPanel0.readingsData;
							break;
						case 1:
							strIn = portPanel1.readingsData;
							break;
						case 2:
							strIn = portPanel2.readingsData;
							break;
						case 3:
							strIn = portPanel3.readingsData;
							break;
						default:
							break;
					}
					//strIn = ReadFromArduino(j);
					//Debug.Log("receieved from arduino" + j + ":" + strIn);
					
					if (strIn != null)
					{
						strIn = strIn.Trim();
						string[] readingsText = strIn.Split(' ');
						if (readingsText.Length == numberBubbles)
						{
							for (int i = 0; i<numberBubbles; i++)
							{
								readingsText[i] = readingsText[i].Trim();
								bool Result;
								int number;
								Result = int.TryParse(readingsText[i], out number);
								if (Result)
								{
									if ( k == 0)
									{
										restValues[j][i] = number;
										pressedValues[j][i] = number - restToPressedDifference;
									}
									else
									{
										if (restValues[j][i] < number)
										{
											restValues[j][i] = number;
											pressedValues[j][i] = number - restToPressedDifference;
										}
									}
									//readings[i] = number;
									//allReadings[j][i] = number;
									//Debug.Log("arduino:" + j + " bubble:" + i + " reading:" + number + " allReadings:" + allReadings[j][i]);
								}
								else
								{
									Debug.Log("failed");
								}
							}
							//allReadings [j] = readings;
						}
					}
				}
				catch(TimeoutException) {
					Debug.Log ("timeout exception");
				}
			} // end calibration passes
		}
		Debug.Log("calibration finished");
	} // end calibrate readings
	
	void OnApplicationQuit()
	{
		//closeSocket();
		//Debug.Log("TCP Socket closed");			
	}
}

// UDP communication adapted from https://forum.unity.com/threads/simple-udp-implementation-send-read-via-mono-c.15900/
