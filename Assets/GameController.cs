using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Linq;

public class GameController : MonoBehaviour {

	public enum ScreenState {
		Uninitialized = -1,
		Incident_Intro = 0,
		Incident_InProgress = 1,
		Incident_Conclusion = 2,
	}

	public int myCharacterThingId = 0;
	List<Player> joinedPlayers = new List<Player> {
		new Player () { playerCharacter = new Character () { name = "Aaron" } },
		new Player () { playerCharacter = new Character () { name = "Bobby" } },
		new Player () { playerCharacter = new Character () { name = "Catherine" } },
		new Player () { playerCharacter = new Character () { name = "Dwayne" } },
		new Player () { playerCharacter = new Character () { name = "Evan" } },
		new Player () { playerCharacter = new Character () { name = "Frank" } },
		new Player () { playerCharacter = new Character () { name = "George" } },
	};

	private ScreenState _currentState = ScreenState.Uninitialized;
	private Dictionary<ScreenState, GameObject> stateGoReference;

	private InstantiatedIncident currentIncident = null;

	//Game Instance
	public int nextThingId = 0;

	//UI Elements
	Transform screenContainer;
	Text _console;
	Text _incidentIntro_Prompt;
	Text _incidentInProgress_Body;
	Transform _incidentInProgress_ButtonGrid;
	GameObject _incidentInProgress_ButtonTemplate;
	Text _incidentConclusion_Resolution;

	// Use this for initialization
	void Start () {
	
		//Temp
		currentIncident = GameData.incidentRecords[0].Instantiate(joinedPlayers.Select(p => p.playerCharacter).ToList(), new List<ShipEquipment> { new ShipEquipment { name = "samoflange generator"} });
		foreach(Player player in joinedPlayers) {
			player.playerCharacter.Register(nextThingId);
			nextThingId++;
		}

		InitializeUi();

		SetScreen(ScreenState.Incident_Intro);
	}

	#region FSM

	private void SetScreen (ScreenState targetState) {
		if (_currentState != ScreenState.Uninitialized) {
			//Disable last state and clean up
			stateGoReference [_currentState].SetActive (false);
		} else {
			Debug.Log("Unintialized");
			//Disable all screens before initialization
			foreach (KeyValuePair<ScreenState, GameObject> pair in stateGoReference) {
				Debug.Log("Disabling screen: " + pair.Key);
				pair.Value.SetActive (false);
			}
		}
		_currentState = targetState;
		InitializeState (_currentState);
		stateGoReference [_currentState].SetActive (true);
	}

	private void InitializeState (ScreenState state) {
		switch(state) {
		case ScreenState.Incident_Intro:
			_incidentIntro_Prompt.text = currentIncident.Prompt;
			break;
		case ScreenState.Incident_InProgress:
			_incidentInProgress_Body.text = currentIncident.GetPlayerInfoText(myCharacterThingId);
			foreach(string s in currentIncident.GetPlayerControlsText(myCharacterThingId)) {
				GameObject buttonGo = Instantiate<GameObject>(_incidentInProgress_ButtonTemplate) as GameObject;
				buttonGo.SetActive(true);
				buttonGo.GetComponentInChildren<Text>().text = s;
				buttonGo.transform.parent = _incidentInProgress_ButtonGrid;
				buttonGo.transform.localScale = Vector3.one;
			}
//			_incidentInProgress_ButtonGrid.GetComponent<VerticalLayoutGroup>().enabled = true;
//			_incidentInProgress_Body.text = currentIncident.beliefs[0].text;
			break;
		case ScreenState.Incident_Conclusion:
			_incidentConclusion_Resolution.text = currentIncident.controls[0].conclusionText;
			break;

		}
	}

	#endregion

	#region ui

	private void InitializeUi() {
		screenContainer = transform.Find("MainCamera/Canvas");
		stateGoReference = new Dictionary<ScreenState, GameObject> ();
		for (int i = 0; i < Enum.GetValues(typeof(ScreenState)).Length - 1; i++) {
			ScreenState screenState = (ScreenState)i;
//			print("screen state: " + screenState.ToString());
			print("screen container: " + screenContainer.name);
			GameObject screenGo = screenContainer.Find (screenState.ToString ()).gameObject;
			stateGoReference.Add (screenState, screenGo);
		}

		_console = screenContainer.Find("Console").GetComponent<Text> ();
		_incidentIntro_Prompt = stateGoReference[ScreenState.Incident_Intro].transform.Find("Prompt").GetComponent<Text> ();
		_incidentInProgress_Body = stateGoReference[ScreenState.Incident_InProgress].transform.Find("Body").GetComponent<Text> ();
		_incidentInProgress_ButtonGrid = stateGoReference[ScreenState.Incident_InProgress].transform.Find("ButtonGrid").transform;
		for(int i = 0; i < _incidentInProgress_ButtonGrid.childCount; i++) {
			Transform buttonTransform = _incidentInProgress_ButtonGrid.GetChild(i);
			if(i == 0) {
				_incidentInProgress_ButtonTemplate = buttonTransform.gameObject;
				Debug.Log ("Acquired button template: " + _incidentInProgress_ButtonTemplate);
			}
			buttonTransform.gameObject.SetActive(false);
		}
		_incidentConclusion_Resolution = stateGoReference[ScreenState.Incident_Conclusion].transform.Find("Resolution").GetComponent<Text> ();
	}

	#endregion

	#region button handlers

	public void ButtonHandler_IncidentIntro_Ready () {
		SetScreen(ScreenState.Incident_InProgress);
	}

	public void ButtonHandler_IncidentInProgress_Control (int index) {
		List<Outcome> outcomes = currentIncident.controls[index].outcomes; //TODO do something with the outcomes
		SetScreen(ScreenState.Incident_Conclusion);
	}

	#endregion

	#region debug

	public void WriteToConsole(string s) {
		print (s);
		_console.text = s + "\n" + _console.text;
	}

	#endregion
}
