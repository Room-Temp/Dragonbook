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
    private const int CHARACTER_SPACING = 15;

    //private static int[] playerTrail = new int[FOLLOWER_FRAMES];    
    private static Vector2[] playerTrail = new Vector2[FOLLOWER_FRAMES];
    private static int[] playerDirections = new int[FOLLOWER_FRAMES];
    private static int trailIndex;
    private static int followerCount;
    private int[] followerTrail = new int[FOLLOWER_FRAMES];
    private int thisFollowerCount;
    private bool beginFollow;
    private static bool followerCountChanged;
    private Vector2 currPos;
    private int prevDir;
    private int currDir;

    private bool up;
    private bool down;
    private bool left;
    private bool right;


    public bool controllable;
    public static bool hasMoved;

    public int linePlacement;   // 1 is the controllable character

    public Object[] interactiveNPCs;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        followerCount = 0;
        trailIndex = 0;
        thisFollowerCount = -1;
        beginFollow = false;
        hasMoved = false;
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
            if (controllable && prevDir != Direction.IDLE)
            {
                currPos = gameObject.GetComponentInParent<Player>().gameObject.GetComponent<BoxCollider2D>().offset;
                prevDir = direction;
            }
            if (up && !down && !left && !right) direction = Direction.UP;
            else if (up && !down && !left && right) direction = Direction.UP_RIGHT;
            else if (!up && !down && !left && right) direction = Direction.RIGHT;
            else if (!up && down && !left && right) direction = Direction.DOWN_RIGHT;
            else if (!up && down && !left && !right) direction = Direction.DOWN;
            else if (!up && down && left && !right) direction = Direction.DOWN_LEFT;
            else if (!up && !down && left && !right) direction = Direction.LEFT;
            else if (up && !down && left && !right) direction = Direction.UP_LEFT;
            else direction = Direction.IDLE;
            
            if (controllable)
            {
                if (trailIndex - 1 >= 0)
                {
                    // If the position of the character is the same as it was on the last frame,
                    // do not add to the player trail
                    if (playerTrail[trailIndex - 1] != gameObject.GetComponent<Rigidbody2D>().position)
                    {
                        playerTrail[trailIndex] = gameObject.GetComponent<Rigidbody2D>().position;
                        playerDirections[trailIndex] = prevDir;
                        trailIndex++;
                    }
                    if (trailIndex == FOLLOWER_FRAMES) trailIndex = 0;
                }
                else
                {
                    // If the position of the array is zero
                    if (!hasMoved || playerTrail[FOLLOWER_FRAMES - 1] != gameObject.GetComponent<Rigidbody2D>().position)
                    {
                        playerTrail[trailIndex] = gameObject.GetComponent<Rigidbody2D>().position;
                        hasMoved = true;
                        trailIndex++;
                    }
                }
            }
            else
            {
                thisFollowerCount = (linePlacement - 1) * CHARACTER_SPACING;
                if (!beginFollow && thisFollowerCount == trailIndex)
                {
                    beginFollow = true;
                }
                if (thisFollowerCount <= trailIndex)
                {
                    // The follower will follow the player trail at ((linePlacement - 1) * CHARACTER_SPACING)
                    gameObject.GetComponent<Movement>().move(playerDirections[thisFollowerCount], movementSpeed, animationSpeed);                    
                }
                else if (beginFollow)
                {
                     thisFollowerCount = (FOLLOWER_FRAMES - (thisFollowerCount - trailIndex));
                }

            }

            /*
            if (controllable && direction != Direction.IDLE)
            {
                currDir = direction;    //last non-idle direction
                if (currDir != prevDir)
                {
                    playerTrail[trailIndex].setNode(direction, currPos);
                    trailIndex++;

                    if (trailIndex == FOLLOWER_FRAMES)
                    {
                        trailIndex = 0;
                    }
                }
            }

            if (controllable)
            {
                gameObject.GetComponent<Movement>().move(direction, movementSpeed, animationSpeed);
            }
            else
            {
                
            }

            
            if (controllable)
            {
                gameObject.GetComponent<Movement>().move(direction, movementSpeed, animationSpeed);               
            }
            else if (hasMoved)
            {
                playerTrail[followerCount] = direction;
                if (followerCountChanged)
               {
                    thisFollowerCount = followerCount - (CHARACTER_SPACING * (linePlacement - 1));
                    if (thisFollowerCount == 0 && !beginFollow)
                    {
                        beginFollow = true;
                    }
                    if (thisFollowerCount < 0 && beginFollow)
                    {
                        //gameObject.GetComponent<Movement>().move(playerTrail[(FOLLOWER_FRAMES - ((CHARACTER_SPACING * (linePlacement - 1)) - followerCount) - 1)], movementSpeed, animationSpeed);
                        gameObject.GetComponent<Movement>().move(playerTrail[FOLLOWER_FRAMES + thisFollowerCount], movementSpeed, animationSpeed);
                    }
                    else if (thisFollowerCount >= 0)
                    {
                        gameObject.GetComponent<Movement>().move(playerTrail[thisFollowerCount], movementSpeed, animationSpeed);
                    }
                }
                else
                {
                    gameObject.GetComponent<Movement>().move(Direction.IDLE, movementSpeed, animationSpeed);
                }
            }
            
            if (followerCount == FOLLOWER_FRAMES)
            {
                followerCount = 0;
            }
            followerCountChanged = true;
        */

            /*
               if (direction != Direction.IDLE)
               {
                   followerCount++;
                   if (followerCount == FOLLOWER_FRAMES)
                   {
                       followerCount = 0;
                   }
                   followerCountChanged = true;
               }
               */
        }
        /*
        if (!Interaction.interacting && GameState.getState(GameState.gameState.overworld))
        {
            if (controllable)
            {
                // Movement
                gameObject.GetComponent<Movement>().move(direction, movementSpeed, animationSpeed);

                followerCountChanged = false;
                if (direction != Direction.IDLE)
                {
                    if (followerCount == FOLLOWER_FRAMES)
                    {
                        followerCount = 0;
                    }
                    playerTrail[followerCount] = direction;
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
                        gameObject.GetComponent<Movement>().move(playerTrail[FOLLOWER_FRAMES - ((CHARACTER_SPACING * (linePlacement - 1)) - followerCount)], movementSpeed, animationSpeed);
                    }
                    else if (thisFollowerCount >= 0)
                    {
                        gameObject.GetComponent<Movement>().move(playerTrail[thisFollowerCount], movementSpeed, animationSpeed);
                    }
                }
                else
                {
                    gameObject.GetComponent<Movement>().move(Direction.IDLE, movementSpeed, animationSpeed);
                }
                
            }

        }
        */
        base.Update();
    }
}
