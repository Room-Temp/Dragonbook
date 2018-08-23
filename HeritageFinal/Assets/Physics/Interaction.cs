﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {
    /*
        Name: Interaction
        Type: Action Script
        Purpose: An Interaction happens when the following conditions are met:
            -player is in the overworld
            -player is facing the script's game object
            -player is close enough to the script's game object
            -button A has been pressed
        Interactions include dialogue and vendor menus.

    */
    // Use this for initialization

    const float MAX_DISTANCE = 1f;

    public static bool interacting;
    private float[] interactiveObjectDistances;
    public enum InteractionType {dialogue, vendor};
    public InteractionType[] interactionType;

    protected void startInteraction()
    {
        interacting = true;
    }

    private Player getPlayer()
    {     
        foreach (Player players in Resources.FindObjectsOfTypeAll(typeof(Player)) as Player[])
        {
            if (players.controllable)
            {
                return players;
            }
        }
        return null;  // This should never run
    }

    private bool checkDirection(Player player, GameObject obj)   // Check if the player is facing the object
    {
        int playerDir;
        if (player.gameObject.GetComponent<Movement>().direction == Direction.IDLE)
        {
            playerDir = player.gameObject.GetComponent<Movement>().prevDir;
        }
        else
        {
            playerDir = player.gameObject.GetComponent<Movement>().direction;
        }
        InteractionCollider[] colliders = obj.GetComponentsInChildren<InteractionCollider>();
        /*
        BoxCollider2D left = obj.GetComponent<Character>().setInteractionTriggers(Direction.LEFT);
        BoxCollider2D right = obj.GetComponent<Character>().setInteractionTriggers(Direction.RIGHT);
        BoxCollider2D up = obj.GetComponent<Character>().setInteractionTriggers(Direction.UP);
        BoxCollider2D down = obj.GetComponent<Character>().setInteractionTriggers(Direction.DOWN);
        Bounds playerBounds = player.gameObject.GetComponent<BoxCollider2D>().bounds;
        Vector2 playerPos = playerBounds.center;
        Vector2 objPos = player.gameObject.GetComponent<BoxCollider2D>().bounds.center;
        Bounds objBounds;
        */
        for (int i = 0; i < colliders.Length; i++)
        {
            if (((playerDir == Direction.UP || playerDir == Direction.UP_LEFT || playerDir == Direction.UP_RIGHT) && colliders[i].isTriggered(Direction.DOWN)) ||
                ((playerDir == Direction.LEFT || playerDir == Direction.UP_LEFT || playerDir == Direction.DOWN_LEFT) && colliders[i].isTriggered(Direction.RIGHT)) ||
                ((playerDir == Direction.RIGHT || playerDir == Direction.UP_RIGHT || playerDir == Direction.DOWN_RIGHT) && colliders[i].isTriggered(Direction.LEFT)) ||
                ((playerDir == Direction.DOWN || playerDir == Direction.DOWN_LEFT || playerDir == Direction.DOWN_RIGHT) && colliders[i].isTriggered(Direction.UP)))
            {
                return true;
            }
        }
        return false;
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(Controls.buttonA) && !interacting)   // First, the game checks if button a is pressed
        {
            if (Vector3.Distance(getPlayer().gameObject.GetComponent<Rigidbody2D>().position, gameObject.GetComponent<Rigidbody2D>().position) <= MAX_DISTANCE)     // Then checks if the player is close enough
            {
                if (checkDirection(getPlayer(), gameObject)) // Finally, it checks if the player is facing the right direction
                {
                    GameObject closestObject = gameObject;
                    float closestLength = MAX_DISTANCE;
                    float interactionDistance;
                    foreach (Interaction interactiveObjects in Resources.FindObjectsOfTypeAll(typeof(Interaction)) as Interaction[])
                    {
                        if (checkDirection(getPlayer(), interactiveObjects.gameObject))
                        {
                            interactionDistance = Vector3.Distance(getPlayer().gameObject.GetComponent<Rigidbody2D>().position,
                                interactiveObjects.GetComponent<Rigidbody2D>().position);
                            if (interactionDistance < closestLength)
                            {
                                closestLength = interactionDistance;
                                closestObject = interactiveObjects.gameObject;
                            }
                        }
                    }
                    if (closestObject == gameObject)    // If this is the closest object, begin interaction
                    {
                        GameState.setState(GameState.gameState.paused);
                        interacting = true;
                        for (int i = 0; i < interactionType.Length; i++)
                        {
                            getPlayer().gameObject.GetComponent<Movement>().move(Direction.IDLE, 0, 0);
                            switch (interactionType[i])
                            {
                                case InteractionType.dialogue:
                                    if (gameObject.GetComponent<SpriteAnimation>() != null)
                                    {
                                        int playerDir;                                        
                                        if (getPlayer().gameObject.GetComponent<Movement>().direction == Direction.IDLE)
                                        {
                                            playerDir = getPlayer().gameObject.GetComponent<Movement>().prevDir;
                                        }
                                        else
                                        {
                                            playerDir = getPlayer().gameObject.GetComponent<Movement>().direction;
                                        }
                                        if (playerDir == Direction.UP_RIGHT ||
                                            playerDir == Direction.UP ||
                                            playerDir == Direction.UP_LEFT)
                                        {
                                            gameObject.GetComponent<SpriteAnimation>().startAnimation(gameObject.GetComponent<SpriteAnimation>().downMovementFrames);
                                        }
                                        else if (playerDir == Direction.LEFT)
                                        {
                                            gameObject.GetComponent<SpriteAnimation>().startAnimation(gameObject.GetComponent<SpriteAnimation>().rightMovementFrames);
                                        }
                                        else if (playerDir == Direction.DOWN_LEFT ||
                                                 playerDir == Direction.DOWN ||
                                                 playerDir == Direction.DOWN_RIGHT)
                                        {
                                            gameObject.GetComponent<SpriteAnimation>().startAnimation(gameObject.GetComponent<SpriteAnimation>().upMovementFrames);
                                        }
                                        else if (playerDir == Direction.RIGHT)
                                        {
                                            gameObject.GetComponent<SpriteAnimation>().startAnimation(gameObject.GetComponent<SpriteAnimation>().leftMovementFrames);
                                        }
                                    }
                                    gameObject.GetComponent<Dialogue>().beginDialogue(gameObject.GetComponent<NPC>().characterState);                                    
                                    break;
                                case InteractionType.vendor:
                                    break;
                            }
                        }
                    }
                }
            }
        }
	}
}
