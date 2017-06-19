using UnityEngine;
using System.Collections;

public class JoystickData{

	public enum Direction{
		
		UP,
		DOWN,
		RIGHT,
		LEFT
	}
	
	public const string DOWN = "joystickDown";
	
	public const string MOVE = "joystickMove";
	
	public const string UP = "joystickUp";

	public const float moveMaxValue = 100f;
}
