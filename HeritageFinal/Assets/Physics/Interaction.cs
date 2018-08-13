using System.Collections;
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
    private GameObject[] interactiveObjects;
    private float[] interactiveObjectDistances;
    public enum InteractionType {dialogue, vendor, option};
    public InteractionType[] interactionType;

    protected void startInteraction()
    {
        interacting = true;
    }

    private Player getPlayer()
    {
        Player[] players = GetComponents<Player>();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].controllable)
            {
                return players[i];
            }
        }
        return players[0];  // This should never run
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
        Vector2 playerPos = player.gameObject.GetComponent<Rigidbody2D>().position;
        Vector2 objPos = obj.GetComponent<Rigidbody2D>().position;

        if (playerDir == Direction.UP || playerDir == Direction.UP_LEFT || playerDir == Direction.UP_RIGHT)
        {
            if (playerPos.y < objPos.y)
            {
                return true;
            }
        }
        else if (playerDir == Direction.LEFT || playerDir == Direction.UP_LEFT || playerDir == Direction.DOWN_LEFT)
        {
            if (playerPos.x > objPos.x)
            {
                return true;
            }
        }
        else if (playerDir == Direction.RIGHT || playerDir == Direction.UP_RIGHT || playerDir == Direction.DOWN_RIGHT)
        {
            if (playerPos.x < objPos.x)
            {
                return true;
            }
        }
        else if (playerDir == Direction.DOWN || playerDir == Direction.DOWN_RIGHT || playerDir == Direction.DOWN_LEFT)
        {
            if (playerPos.y > objPos.y)
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
		if (Input.GetKey(Controls.buttonA))   // First, the game checks if button a is pressed
        {
            if (Vector3.Distance(getPlayer().gameObject.GetComponent<Rigidbody2D>().position, gameObject.GetComponent<Rigidbody2D>().position) <= MAX_DISTANCE)     // Then checks if the player is close enough
            {
                if (checkDirection(getPlayer(), gameObject)) // Finally, it checks if the player is facing the right direction
                {
                    GameObject closestObject = gameObject;
                    float closestLength = MAX_DISTANCE;
                    int interactiveObjectIndex = 0;
                    foreach (GameObject objs in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) // Add all objects to array
                    {
                        if (objs.GetComponent<Interaction>() != null)   // Filter to objects with Interaction component
                        {
                            if (checkDirection(getPlayer(), objs))  // Filter further to objects that the player is facing
                            {
                                interactiveObjects[interactiveObjectIndex] = objs;
                                interactiveObjectIndex++;
                            }
                        }
                    }
                    for (int i = 0; i < interactiveObjects.Length; i++) // Find the closest object to the player
                    {
                        interactiveObjectDistances[i] = Vector3.Distance(getPlayer().gameObject.GetComponent<Rigidbody2D>().position,
                            interactiveObjects[i].GetComponent<Rigidbody2D>().position);
                        if (interactiveObjectDistances[i] < closestLength)
                        {
                            closestLength = interactiveObjectDistances[i];
                            closestObject = interactiveObjects[i];
                        }
                    }
                    if (closestObject == gameObject)    // If this is the closest object, begin interaction
                    {
                        // Begin interaction
                        for (int i = 0; i < interactionType.Length; i++)
                        {
                            switch (interactionType[i])
                            {
                                case InteractionType.dialogue:
                                    
                                    break;
                                case InteractionType.option:
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
