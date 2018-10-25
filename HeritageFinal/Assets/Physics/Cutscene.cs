using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour {

    /*
        Name: Cutscene
        Type: Action Script
        Purpose: A cutscene is a sequence of events that disables player control other than dialogue
            Cutscenes will use a node class to string together multiple events



    Types of cutscene events
     -Movement
     -Animation
     -Item Get
     -Item Remove
     -Stat Change
     -Skill Get
     -Enter Battle
     -Teleport
     -Load/Unload Actors
    */

    public const int MOVEMENT = 0;
    public const int ANIMATION = 1;
    public const int ITEM_GET = 2;
    public const int ITEM_REMOVE = 3;
    public const int STAT_CHANGE = 4;
    public const int SKILL_GET = 5;
    public const int ENTER_BATTLE = 6;
    public const int TELEPORT = 7;
    public const int LOAD = 8;
    public const int UNLOAD = 9;

    /*
        Each type has a string value assigned to it:
        Movement - Direction.ToString()
        Animation - Sprite Index ID

    */


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
