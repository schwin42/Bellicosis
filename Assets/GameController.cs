using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public enum ScreenState {
		Uninitialized = -1,
		Incident_Intro = 0,
		Incident_InProgress = 1,
		Incident_Conclusion = 2,
	}

	public ScreenState _currentState;
	private Dictionary<ScreenState, GameObject> stateGoReference;

	private Incident incidentInProgress = null;

	//UI Elements
	Transform screenContainer;
	Text _console;
	Text _incidentIntro_Prompt;
	Text _incidentInProgress_Body;
	Text _incdientConclusion_Resolution;

	// Use this for initialization
	void Start () {
	
		//Temp
		incidentInProgress = GameData.incidentRecords[0];

		InitializeUi();
	}

	#region FSM

	private void SetScreen (ScreenState targetState) {
		if (_currentState != ScreenState.Uninitialized) {
			//Disable last state and clean up
			stateGoReference [_currentState].SetActive (false);
		} else {
			//Disable all screens before initialization
			foreach (KeyValuePair<ScreenState, GameObject> pair in stateGoReference) {
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

			break;
		case ScreenState.Incident_InProgress:

			break;
		case ScreenState.Incident_Conclusion:

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
			GameObject screenGo = screenContainer.Find (screenContainer.ToString ()).gameObject;
			stateGoReference.Add (screenState, screenGo);
		}

		_console = screenContainer.Find("Console").GetComponent<Text> ();
		_incidentIntro_Prompt = stateGoReference[ScreenState.Incident_Intro].transform.Find("Prompt").GetComponent<Text> ();
		_incidentInProgress_Body = stateGoReference[ScreenState.Incident_InProgress].transform.Find("Body").GetComponent<Text> ();
		_incdientConclusion_Resolution = stateGoReference[ScreenState.Incident_Conclusion].transform.Find("Resolution").GetComponent<Text> ();
	}

	#endregion

	#region button handlers

	public void ButtonHandler_IncidentIntro_Ready () {
		SetScreen(ScreenState.Incident_InProgress);
	}

	public void ButtonHandler_IncidentInProgress_Control (int index) {
		List<Outcome> outcomes = incidentInProgress.controls[index].outcomes; //TODO do something with the outcomes
		SetScreen(ScreenState.Incident_Conclusion);
	}

	#endregion

	#region debug

	public void WriteToConsole(string s) {
		_console.text = s + "\n" + _console.text;
	}

	#endregion
}
