using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Player
    Type: Object Script
    Purpose: A player can be either the controllable charactor or followers.
*/

public class Player : Character {

    private const int FOLLOWER_FRAMES = 60;

    private static int[] followerDirections;
    private static int followerCount;

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
        followerCount = -1;
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
        if (!Interaction.interacting && GameState.getState(GameState.gameState.overworld))
        {
            if (controllable)
            {
                // Set instructions for followers
                followerCount = (followerCount + 1) % FOLLOWER_FRAMES;
                followerDirections[followerCount] = direction;
                

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
            else
            {
                if (followerCount < 0)
                {
                    gameObject.GetComponent<Movement>().move
                        (followerDirections[followerCount], movementSpeed, animationSpeed);
                }
            }
        }
        base.Update();
    }
}
