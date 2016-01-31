﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	enum GameStates{
		Menu=0,
		Intro,
		Playing,
		Paused,
		End
	};

	public float startTimeSeconds;
	public static float timeLeftSeconds;

	GameStates gameState = GameStates.Menu;
	GameObject cat;
	GameObject menu;
	GameObject end;
	Transform endCamTarget;//Cat's Face
	Transform menuPivot;
	Text countDownText;

	// Use this for initialization
	void Start () {
		timeLeftSeconds = startTimeSeconds;
		cat = GameObject.Find("Cat");
		menu = GameObject.Find("Menu");
		menuPivot = menu.transform.FindChild("MenuPivot").transform;
		countDownText = GameObject.Find("HUD/CountDown/Text").GetComponent<Text>();
		endCamTarget = cat.transform.FindChild("Body");

		end = GameObject.Find("End");
		end.SetActive(false);

		ChangeGameState(GameStates.Menu);
	}
	
	// Update is called once per frame
	void Update () {

		if (gameState == GameStates.Menu){
			menuPivot.Rotate(0.0f,-0.5f,0.0f);

			if (Input.GetKeyUp("space")){
				ChangeGameState(GameStates.Intro);
			}

		} else if (gameState == GameStates.Playing){
			if (timeLeftSeconds > 0){
				timeLeftSeconds -= Time.deltaTime;
				countDownText.text = Mathf.FloorToInt(timeLeftSeconds).ToString();
			} else {
				timeLeftSeconds = 0;
				ChangeGameState(GameStates.End);
			}
		}
	}

	void ChangeGameState(GameStates _gameState){
		gameState = _gameState;

		if (gameState == GameStates.Intro){
			cat.SetActive(false);
			menu.SetActive(false);

			//HACK SINCE THERE IS NO INTRO ANIMATION ATM
			ChangeGameState(GameStates.Playing);
		} else if (gameState == GameStates.Playing){
			cat.SetActive(true);
			menu.SetActive(false);
		} else if (gameState == GameStates.Menu){
			cat.SetActive(false);
			menu.SetActive(true);
		} else if (gameState == GameStates.End){
			menu.SetActive(false);
			end.SetActive(true);

			GameObject.Find("CatPOVCam").SetActive(false);
			GameObject.Find("Cat").GetComponent<UnityStandardAssets.Characters.FirstPerson.CatController>().enabled = false;
			GameObject.Find("Cat").GetComponent<SwiperNoSwiping>().enabled = false;

			//Transform _tempFrontCatFace = cat.transform.FindChild("wiskers");
			endCamTarget.gameObject.SetActive(true);
			Transform _tempCatFace = endCamTarget.FindChild("wiskers");
			endCamTarget.transform.eulerAngles = new Vector3(0.0f,endCamTarget.transform.eulerAngles.y,0.0f);
			endCamTarget.GetComponent<Rigidbody>().freezeRotation = true;
			//_tempCatFace.eulerAngles = new Vector3(0.0f,0.0f,0.0f);
			//_tempCatFace.GetComponent<Rigidbody>().fr

			end.transform.position = new Vector3(_tempCatFace.position.x,
												 _tempCatFace.position.y,
												 _tempCatFace.position.z);
			end.transform.eulerAngles = new Vector3(0.0f,
				 									_tempCatFace.eulerAngles.y,
													0.0f);

			//cat.SetActive(false);
		}
	}

	public static void AddTime(float _timeSeconds){
		timeLeftSeconds += _timeSeconds;
	}
}
