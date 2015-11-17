using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour, IPointerClickHandler {

	public int choiceIndex;

	private GameController gameController;

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		gameController.ButtonHandler_IncidentInProgress_Control(choiceIndex);
	}

	#endregion

	public void InitializeButton(GameController gameController, int choiceId, string text) {
		this.gameController = gameController;
		this.choiceIndex = choiceId;
		GetComponentInChildren<Text>().text = text;
	}
}
