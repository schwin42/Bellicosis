using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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

	// Use this for initialization
	void Start () {
	
		Transform screenContainer = transform.Find("MainCamera/Canvas");
		stateGoReference = new Dictionary<ScreenState, GameObject> ();
		for (int i = 0; i < Enum.GetValues(typeof(ScreenState)).Length - 1; i++) {
			ScreenState screenState = (ScreenState)i;
			GameObject screenGo = screenContainer.Find (screenContainer.ToString ()).gameObject;
			stateGoReference.Add (screenState, screenGo);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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
//		InitializeState (_currentState);
		stateGoReference [_currentState].SetActive (true);
	}

//	private void InitializeState (ScreenState state) {
//
//	}

	#region button handlers

	public void ButtonHandler_IncidentIntro_Ready () {
		SetScreen(ScreenState.Incident_InProgress);
	}

	public void ButtonHandler_IncidentInProgress_Control (int index) {
		List<Outcome> outcomes = incidentInProgress.controls[index].outcomes;
	}

	#endregion
}
