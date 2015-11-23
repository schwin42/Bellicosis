using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ButtonHandler : MonoBehaviour, IPointerClickHandler {

	public List<int> choiceIds;

	private GameController gameController;

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		gameController.ButtonHandler_IncidentInProgress_Control(choiceIds);
	}

	#endregion

	public void InitializeButton(GameController gameController, List<int> choiceIds, string text) {
		this.gameController = gameController;
		this.choiceIds = choiceIds;
		GetComponentInChildren<Text>().text = text;
	}
}
