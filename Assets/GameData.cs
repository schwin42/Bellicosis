using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData : MonoBehaviour {

	public List<Incident> incidentRecords = new List<Incident>();

	// Use this for initialization
	void Start () {
	
		incidentRecords.Add(new Incident() {
			prompt = "{PC1} fucked up the coolant for the {SE0} and now it's overeating. It will melt through the hull unless we vent the engineering bay. {PC1} is currently in the engineering bay.",
			controls = new List<Control> () {
				new Control () { relativeCharacterId = "PC0", buttonText = "Vent engineering and lose {PC1}.", outcomes = new List<Outcome> { new Outcome(OutcomeCommand.Destroy, "PC1") }, 
					conclusionText = "{PC1} is blown out into the vacuum of space, but with some work, the {SE0} will be fully operational again." },
				new Control () { relativeCharacterId = "PC0", buttonText = "Jettison the {SE0}.", outcomes = new List<Outcome> { 
						new Outcome (OutcomeCommand.Destroy, "SE0") }, conclusionText = "The damaged {SE0} is blown out into the vacuum of space, but at least {PC1} was saved." },
				new Control () { relativeCharacterId = "PC0", buttonText = "Do nothing and lose both.", outcomes = new List<Outcome> { 
						new Outcome (OutcomeCommand.Destroy, "SE0"),
						new Outcome (OutcomeCommand.Destroy, "PC1"),
					}, conclusionText = "The {SE0} melts through the floor of the engineering bay and the chamber decompresses. {PC1} is killed and the {SE0} is unsalvageable." }
			},
			beliefs = new List<Belief> () {
				new Belief () { relativeCharacterId = "PC1", text = "I'm {PC1}! I don't want to die! Besides, seconds before the disaster, I discovered the coolant was sabotaged. It wasn't my fault!" },
				new Belief () { relativeCharacterId = "PC2", text = "The {SE0} is integral to our food production. Without it, we'll run out of food in three cycles." },
				new Belief () { relativeCharacterId = "PC3", text = "{PC1} is a goddamn moron and will get us all killed if he's allowed to continue making mistakes." },
				new Belief () { relativeCharacterId = "PC4", text = "{PC1} means well and he would save any one of us if the tables were turned." },
				new Belief () { relativeCharacterId = "PC5", text = "You sabotaged the {SE0} but no one must know. If {PC1} lives, he may be able to discovery your treachery." },
			}
		});
	}
}
