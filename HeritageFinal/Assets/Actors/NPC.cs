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
        if (defaultSpeeds)
        {
            movementSpeed = DEFAULT_MOVEMENT_SPEED;
            animationSpeed = DEFAULT_ANIMATION_SPEED;
        }
        switch (direction)  // Direction of an NPC is initially set in the editor. It should always be in a cardinal direction
        {
            case (Direction.UP):
                gameObject.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteAnimation>().upMovementFrames[0];
                break;
            case (Direction.DOWN):
                gameObject.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteAnimation>().downMovementFrames[0];
                break;
            case (Direction.LEFT):
                gameObject.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteAnimation>().leftMovementFrames[0];
                break;
            case (Direction.RIGHT):
                gameObject.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteAnimation>().rightMovementFrames[0];
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
