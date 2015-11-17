using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class Model
{

}

public class Incident
{
	public virtual string Prompt {
		get {
			return abstractPrompt;
		}
		set {
			abstractPrompt = value;
		}
	}

	public string abstractPrompt;
	public List<Belief> beliefs;
	public List<Control> controls;
	public List<Consequence> consequences;

	public InstantiatedIncident Instantiate (List<Character> charactersById, List<ShipEquipment> shipEquipmentById)
	{
		return new InstantiatedIncident (this, charactersById, shipEquipmentById);
	}
}

public class InstantiatedIncident : Incident
{
	public override string Prompt {
		get {
			return InstantiateText (abstractPrompt);
		}
	}

	public List<Character> charactersByInternalId;
	public List<ShipEquipment> shipEquipmentById;

	public InstantiatedIncident (Incident abstractIncident, List<Character> charactersById, List<ShipEquipment> shipEquipmentById)
	{
		this.abstractPrompt = abstractIncident.abstractPrompt;
		this.beliefs = abstractIncident.beliefs;
		this.controls = abstractIncident.controls;

		this.charactersByInternalId = charactersById;
		this.shipEquipmentById = shipEquipmentById;
	}

	public string GetPlayerInfoText (int characterThingId) {
//		Debug.Log("characters by id count: " + charactersByInternalId.Count);
//		foreach(Character character in charactersByInternalId) {
//			Debug.Log("character thing id: " + character.thingId);
//		}
//		Debug.Log("belief count: " + beliefs.Count);
		List<string> infoStrings = this.beliefs
			.Where(belief => charactersByInternalId[belief.internalCharacterId].thingId == characterThingId)
			.Select(belief => belief.text).ToList();

//		Debug.Log("info strings count: " + infoStrings.Count());
		for(int i = 0; i < infoStrings.Count; i++) {
			infoStrings[i] = InstantiateText(infoStrings[i]);
		}
		return string.Join("\n", infoStrings.ToArray());
	}

	public List<string> GetPlayerControlsText (int characterThingId) {
		List<string> abstractButtonStrings = this.controls
			.Where(control => charactersByInternalId[control.internalCharacterId].thingId == characterThingId)
			.Select(control => control.buttonText).ToList();
		List<string> instantiatedButtonStrings = new List<string> ();
		foreach(string s in abstractButtonStrings) {
			instantiatedButtonStrings.Add(InstantiateText(s));
		}
		return instantiatedButtonStrings;
	}

	private string InstantiateText (string input)
	{
		string output = input.Replace ("{", "<");
		//			Debug.Log("1st version: " + output);
		int i = 0;
		while ((i = output.IndexOf("<PC")) != -1) {
			output = output.Insert (i + 4, "}");
			//				Debug.Log("1.1 version: " + output);
			output = output.Remove (i, 3);
			//				Debug.Log("1.2 version: " + output);
			output = output.Insert (i, "{");
			//				Debug.Log("1.3 version: " + output);
		}
		//			Debug.Log("character count: " + charactersById.Count);
		//			Debug.Log("2nd version: " + output);
		output = string.Format (output, charactersByInternalId.Select (c => c.name).ToArray ());
		while ((i = output.IndexOf("<SE")) != -1) {
			output = output.Insert (i + 4, "}");
			output = output.Remove (i, 3);
			output = output.Insert (i, "{");
		}
		//			Debug.Log("3rd version: " + output);
		output = string.Format (output, shipEquipmentById.Select (se => se.name).ToArray ());
		return output;

	}
}

public class Belief
{
	public int internalCharacterId;
	public string text;
}

public class Control
{
	public int internalCharacterId;
	public string buttonText;
	public int consequenceId;
}

public class Consequence
{
	public int internalId;
	public string abstractText;
	public ConsequenceCommand command = ConsequenceCommand.Uninitialized;
	public string target;

	public Consequence (int internalId, string abstractText, ConsequenceCommand command, string target)
	{
		this.internalId = internalId;
		this.abstractText = abstractText;
		this.command = command;
		this.target = target;
	}
}

public class Character
{
	public int thingId = -1;
	public string name;

	public Character() {

	}
	public void Register(int id) {
		thingId = id;
	}
}

public class ShipEquipment
{
	public string name;
}

[System.Serializable]
public class Player
{
	public Character playerCharacter;

}

public enum ConsequenceCommand
{
	Uninitialized,
	Destroy,
}