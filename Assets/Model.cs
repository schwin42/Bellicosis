using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Model
{
}

public class Incident
{
	public string prompt;
	public List<Belief> beliefs;
	public List<Control> controls;
}

public class Belief
{
	public string relativeCharacterId;
	public string text;
}

public class Control
{
	public string relativeCharacterId;
	public string buttonText;
	public List<Outcome> outcomes;
	public string conclusionText;
}

public class Outcome {
	public OutcomeCommand command = OutcomeCommand.Uninitialized;
	public string target;

	public Outcome (OutcomeCommand command, string target) {
		this.command = command;
		this.target = target;
	}
}

public class Character {
	public string name;
}

public enum OutcomeCommand {
	Uninitialized,
	Destroy,
}