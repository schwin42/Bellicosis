﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameData {

	public static List<Incident> incidentRecords = new List<Incident>() {
		new Incident() {
			Prompt = "{PC1 fucked up the coolant for the {SE0 and now it's overheating. It will melt through the hull unless we vent the engineering bay. {PC1 is currently in the " +
				"engineering bay. {PC0 stands ready at the door control.",
			controls = new List<Control> () {
				new Control () { internalCharacterId = 0, buttonText = "Vent engineering and lose {PC1.", consequenceId = 0 },
				new Control () { internalCharacterId = 0, buttonText = "Jettison the {SE0.", outcomes =
				new Control () { internalCharacterId = 0, buttonText = "Do nothing and lose both.", outcomes = new List<Consequence> { 
						new Consequence (ConsequenceCommand.Destroy, "SE0"),
						new Consequence (ConsequenceCommand.Destroy, "PC1"),
					}, conclusionText = 			},
			beliefs = new List<Belief> () {
				new Belief () { internalCharacterId = 1, text = "I'm {PC1! I don't want to die! Besides, seconds before the disaster, I discovered the coolant was sabotaged. It wasn't my fault!" },
				new Belief () { internalCharacterId = 2, text = "The {SE0 is integral to our food production. Without it, we'll run out of food in three cycles." },
				new Belief () { internalCharacterId = 3, text = "{PC1 is a goddamn moron and will get us all killed if he's allowed to continue making mistakes." },
				new Belief () { internalCharacterId = 4, text = "{PC1 means well and he would save any one of us if the tables were turned." },
				new Belief () { internalCharacterId = 5, text = "You sabotaged the {SE0 but no one must know. If {PC1 lives, he may be able to discovery your treachery." },
			},
			consequences = new List<Consequence> () {
				new Consequence (0, "{PC1 is blown out into the vacuum of space, but with some work, the {SE0 will be fully operational again.", 
				                 ConsequenceCommand.Destroy, "PC1"),
				new Consequence (1, "The damaged {SE0 is blown out into the vacuum of space, but at least {PC1 was saved.", ConsequenceCommand.Destroy, "SE0"),
				new Consequence (2, "The {SE0 melts through the floor of the engineering bay and the chamber decompresses. {PC1 is killed and the {SE0 is unsalvageable.",
						         ConsequenceCommand.Destroy, "SE0"),
			},
		}
	};
}
