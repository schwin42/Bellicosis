using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class Model
{

}

[System.Serializable]
public class Encounter
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
	public List<Control> abstractControls;
	public List<EncounterOutcome> outcomes;

	public RealizedEncounter Realize (List<Character> charactersById, List<ShipEquipment> shipEquipmentById)
	{
		return new RealizedEncounter (this, charactersById, shipEquipmentById);
	}
}

[System.Serializable]
public class RealizedEncounter : Encounter
{
	public override string Prompt {
		get {
			return RealizeText (abstractPrompt);
		}
	}

	public List<Character> charactersByInternalId;
	public List<ShipEquipment> shipEquipmentById;

	public RealizedEncounter (Encounter abstractEncounter, List<Character> charactersById, List<ShipEquipment> shipEquipmentById)
	{
		this.abstractPrompt = abstractEncounter.abstractPrompt;
		this.beliefs = abstractEncounter.beliefs;
		this.abstractControls = abstractEncounter.abstractControls;
		this.outcomes = abstractEncounter.outcomes;

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
			.Where(belief => charactersByInternalId[belief.internalCharacterId].ThingId == characterThingId)
			.Select(belief => belief.text).ToList();

//		Debug.Log("info strings count: " + infoStrings.Count());
		for(int i = 0; i < infoStrings.Count; i++) {
			infoStrings[i] = RealizeText(infoStrings[i]);
		}
		return string.Join("\n", infoStrings.ToArray());
	}

//	public List<Control> GetPlayerControlsText (int characterThingId) {
//		List<string> abstractButtonStrings = this.abstractControls
//			.Where(control => charactersByInternalId[control.internalCharacterId].thingId == characterThingId)
//			.Select(control => control.buttonText).ToList();
//		List<string> instantiatedButtonStrings = new List<string> ();
//		foreach(string s in abstractButtonStrings) {
//			instantiatedButtonStrings.Add(InstantiateText(s));
//		}
//		return instantiatedButtonStrings;
//	}

	public string RealizeText (string input)
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

	public int GetThingIdFromInternalString (string internalStringId) {
		Debug.Log("internal string id: " + internalStringId);
		if(internalStringId.StartsWith("PC")) {
			int i = (int)char.GetNumericValue(internalStringId[2]);
			return charactersByInternalId[i].ThingId;
		} else if(internalStringId.StartsWith("SE")) {
			int i = (int)char.GetNumericValue(internalStringId[2]);
			int output = shipEquipmentById[i].ThingId;
			Debug.Log ("GTIFIS output: " + output);
			return output;
		} else {
			Debug.LogError("GetThingFromInternalString failed");
			return -1;
		}
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
	public List<int> consequenceIds;
}

[System.Serializable]
public class EncounterOutcome
{
	public int internalId;
	public string abstractText;
	public List<ConsequenceCommand> commands;  //Ordered with target ids
	public List<string> targetIds; //Ordered with commands

	public EncounterOutcome (int internalId, string abstractText, List<ConsequenceCommand> commands, List<string> targetIds)
	{
		Debug.Log("Creating eo");
		this.internalId = internalId;
		this.abstractText = abstractText;
		if(commands.Count != targetIds.Count) {
			Debug.LogError("Invalid Operation: command count " + commands.Count + " does not equal target count " + targetIds.Count);
		}
		this.commands = commands;
		this.targetIds = targetIds;
	}
}

public class Character : IThing
{
	private int _thingId = -1;
	public int ThingId {
		get {
			return _thingId;
		}
		set {
			_thingId = value;
		}
	}
	public object Thing { get; set; }
	public string name;

	public Character() {

	}
}

[System.Serializable]
public class Ship {
	public List<int> activeThings = new List<int> ();
	public List<int> deadThings = new List<int> ();
}

public class ShipEquipment : IThing
{
	private int _thingId = -1;
	public int ThingId {
		get {
			return _thingId;
		}
		set {
			_thingId = value;
		}
	}
	public object Thing { get; set; }
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

public interface IThing {
	int ThingId { get; set; }
	object Thing { get; set; }
}