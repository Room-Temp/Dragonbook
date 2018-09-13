using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Player
    Type: Object Script
    Purpose: A player can be either the controllable charactor or followers.
*/

public class Player : Character {

    private const int FOLLOWER_FRAMES = 200;
    private const int CHARACTER_SPACING = 7;

    private static int[] followerDirections = new int[FOLLOWER_FRAMES];    
    private static int followerCount;
    private int[] thisFollowerDirection = new int[FOLLOWER_FRAMES];
    private int thisFollowerCount;
    private bool beginFollow;
    private static bool followerCountChanged;

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
        followerCount = 0;
        thisFollowerCount = -1;
        beginFollow = false;
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

                followerCountChanged = false;
                if (direction != Direction.IDLE)
                {
                    if (followerCount == FOLLOWER_FRAMES)
                    {
                        followerCount = 0;
                    }
                    followerDirections[followerCount] = direction;
                    followerCount++;
                    followerCountChanged = true;
                }
            }
            else
            {
                // Follower Movement
                if (followerCountChanged)
                {
                    thisFollowerCount = followerCount - (CHARACTER_SPACING * (linePlacement - 1));
                    if (thisFollowerCount >= 0 && !beginFollow)
                    {
                        beginFollow = true;
                    }
                    if (thisFollowerCount < 0 && beginFollow)
                    {
                        gameObject.GetComponent<Movement>().move(followerDirections[FOLLOWER_FRAMES - ((CHARACTER_SPACING * (linePlacement - 1)) - followerCount)], movementSpeed, animationSpeed);
                    }
                    else if (thisFollowerCount >= 0)
                    {
                        gameObject.GetComponent<Movement>().move(followerDirections[thisFollowerCount], movementSpeed, animationSpeed);
                    }
                }
                else
                {
                    gameObject.GetComponent<Movement>().move(Direction.IDLE, movementSpeed, animationSpeed);
                }
                
            }
        }
        base.Update();
    }
}
