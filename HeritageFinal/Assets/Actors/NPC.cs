using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Character {

	/*
        Name: NPC
        Type: Object Script
        Purpose: An NPC is a non-controllable character that is not in the player's party or an enemy.
    */
	void Start () {
        switch (direction)  // Direction of an NPC is initially set in the editor. It should always be in a cardinal direction
        {
            case (Direction.UP):
                break;
            case (Direction.DOWN):
                break;
            case (Direction.LEFT):
                break;
            case (Direction.RIGHT):
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
