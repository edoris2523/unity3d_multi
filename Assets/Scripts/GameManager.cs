﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {
	public Text m_messageText;

	int m_minPlayers = 2;
	int m_maxPlayers = 4;

	[SyncVar]
	public int m_playerCount = 0;

	public Color[] m_playerColors = {Color.red, Color.blue, Color.green, Color.magenta};

	static GameManager instance;

	public static GameManager Instance{
		get{
			if (instance == null) {
				instance = GameObject.FindObjectOfType<GameManager> ();

				if (instance == null) {
					instance = new GameObject ().AddComponent<GameManager> ();
				}
			}
			return instance;
		}

	}

	void Awake(){
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	void Start(){
		StartCoroutine ("GameLoopRoutine");
	}

	IEnumerator GameLoopRoutine(){
		yield return StartCoroutine ("EnterLobby");
		yield return StartCoroutine ("PlayGame");
		yield return StartCoroutine ("EndGame");
	}

	IEnumerator EnterLobby(){
		if (m_messageText != null) {
			m_messageText.gameObject.SetActive (true);
			m_messageText.text = "waiting for players";
		}

		while (m_playerCount < m_minPlayers) {
			DisablePlayers ();
			yield return null;
		}
	}

	IEnumerator PlayGame(){
		EnablePlayers ();
		if (m_messageText != null) {
			m_messageText.gameObject.SetActive (false);
		}
		yield return null;
	}

	IEnumerator EndGame(){
		yield return null;
	}

	void SetPlayerState(bool state){
		PlayerController[] allPlayers = GameObject.FindObjectsOfType<PlayerController> ();
		foreach (PlayerController p in allPlayers) {
			p.enabled = state;
		}
	}

	void EnablePlayers(){
		SetPlayerState (true);
	}

	void DisablePlayers(){
		SetPlayerState (false);
	}

	public void AddPlayer(PlayerSetup pSetup){
		if (m_playerCount < m_maxPlayers) {
			pSetup.m_playerColor = m_playerColors [m_playerCount];
			pSetup.m_playerNum = m_playerCount + 1;
		}
	}
}
