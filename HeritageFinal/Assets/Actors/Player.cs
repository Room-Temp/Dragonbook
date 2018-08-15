using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Player
    Type: Object Script
    Purpose: A player can be either the controllable charactor or followers.
*/

public class Player : Character {

    private bool up;
    private bool down;
    private bool left;
    private bool right;

    public bool controllable;

    public int linePlacement;   // 1 is the controllable character

    public Object[] interactiveNPCs;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        Dialogue.dialogueRunning = false;
    }
	
    public void stopPlayerControl()
    {
            if (linePlacement == 1)
            {
                controllable = !controllable;
            direction = Direction.IDLE;
            }
    }

	// Update is called once per frame
	protected override void Update () {
        up = Input.GetKey(Controls.up);
        down = Input.GetKey(Controls.down);
        left = Input.GetKey(Controls.left);
        right = Input.GetKey(Controls.right);
		if (controllable && !Interaction.interacting && GameState.getState(GameState.gameState.overworld))
        {    
            // Movement        
            if (up && !down && !left && !right) direction = Direction.UP;
            else if (up && !down && !left && right) direction = Direction.UP_RIGHT;
            else if (!up && !down && !left && right) direction = Direction.RIGHT;
            else if (!up && down && !left && right) direction = Direction.DOWN_RIGHT;
            else if (!up && down && !left && !right) direction = Direction.DOWN;
            else if (!up && down && left && !right) direction = Direction.DOWN_LEFT;
            else if (!up && !down && left && !right) direction = Direction.LEFT;
            else if (up && !down && left && !right) direction = Direction.UP_LEFT;
            else direction = Direction.IDLE;
            gameObject.GetComponent<Movement>().move(direction, movementSpeed, animationSpeed);
        }
        base.Update();
    }
}
