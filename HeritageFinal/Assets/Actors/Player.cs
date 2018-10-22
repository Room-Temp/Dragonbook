using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Player
    Type: Object Script
    Purpose: A player can be either the controllable charactor or followers.
*/

public class Player : Character {

    private const int FOLLOWER_FRAMES = 1000;
    private const int CHARACTER_SPACING = 20;

    //private static int[] playerTrail = new int[FOLLOWER_FRAMES];    
    private static Vector2[] playerTrail = new Vector2[FOLLOWER_FRAMES];
    private static int[] playerDirections = new int[FOLLOWER_FRAMES];
    private static int trailIndex;
    private int thisFollowerCount;
    private bool beginFollow;
    private static bool followerCountChanged;
    public static Vector2 playerPosition;
    public static bool playerHasMoved;
    private int currDir;
    private Vector2 castDirection;
    public static int playerDirection;

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
        trailIndex = 0;
        thisFollowerCount = -1;
        beginFollow = false;
        hasMoved = false;
        Dialogue.dialogueRunning = false;
        playerHasMoved = true;
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
                playerDirection = direction;
                switch (direction)
                {
                    case Direction.UP:
                        castDirection = new Vector2(0, 1);
                        break;
                    case Direction.UP_RIGHT:
                        castDirection = new Vector2(1, 1);
                        break;
                    case Direction.RIGHT:
                        castDirection = new Vector2(1, 0);
                        break;
                    case Direction.DOWN_RIGHT:
                        castDirection = new Vector2(1, -1);
                        break;
                    case Direction.DOWN:
                        castDirection = new Vector2(0, -1);
                        break;
                    case Direction.DOWN_LEFT:
                        castDirection = new Vector2(-1, -1);
                        break;
                    case Direction.LEFT:
                        castDirection = new Vector2(-1, 0);
                        break;
                    case Direction.UP_LEFT:
                        castDirection = new Vector2(-1, 1);
                        break;
                    default:
                        castDirection = new Vector2(0, 0);
                        break;
                }
                RaycastHit2D boxcast = Physics2D.BoxCast(
                    gameObject.GetComponent<BoxCollider2D>().bounds.center,
                        gameObject.GetComponent<BoxCollider2D>().bounds.size,
                        0, castDirection, movementSpeed, ~(Layer.PLAYER | Layer.INTERACTION));

                if (boxcast.collider != null)
                {
                    //Debug.Log("colliding");
                    Player[] players = FindObjectsOfType<Player>();
                    for (int i = 0; i < players.Length; i++)
                    {
                        players[i].gameObject.GetComponent<SpriteRenderer>().sprite =
                            players[i].gameObject.GetComponent<SpriteAnimation>().currentSprites[0];
                    }
                    playerHasMoved = false;
                }
                else
                {
                    playerHasMoved = true;
                }
                playerPosition = gameObject.GetComponent<Rigidbody2D>().position;
                gameObject.GetComponent<Movement>().move(direction, movementSpeed, animationSpeed);
                if (trailIndex - 1 >= 0 && playerHasMoved)
                {
                    // If the position of the character is the same as it was on the last frame,
                    // do not add to the player trail
                    if (playerTrail[trailIndex - 1] != gameObject.GetComponent<Rigidbody2D>().position)
                    {
                        playerTrail[trailIndex] = playerPosition;
                        playerDirections[trailIndex] = prevDir;
                        trailIndex++;
                    }
                    if (trailIndex == FOLLOWER_FRAMES) trailIndex = 0;
                }
                else
                {
                    // If the position of the array is zero
                    if (!hasMoved || playerTrail[FOLLOWER_FRAMES - 1] != gameObject.GetComponent<Rigidbody2D>().position && playerHasMoved)
                    {
                        playerTrail[trailIndex] = gameObject.GetComponent<Rigidbody2D>().position;
                        playerDirections[trailIndex] = prevDir;
                        hasMoved = true;
                        trailIndex++;
                    }       

                }
                
            }
            else if (playerHasMoved)
            {
                thisFollowerCount = trailIndex - ((linePlacement - 1) * CHARACTER_SPACING);
                if (!beginFollow && thisFollowerCount == 0)
                {
                    beginFollow = true;
                }
                if (beginFollow && thisFollowerCount < 0)
                {
                    thisFollowerCount = (FOLLOWER_FRAMES - (((linePlacement - 1) * CHARACTER_SPACING) - trailIndex));
                }
                if (beginFollow && direction != Direction.IDLE)
                    gameObject.GetComponent<Movement>().move
                        (playerDirections[thisFollowerCount], playerTrail[thisFollowerCount], animationSpeed);
                if (beginFollow && direction == Direction.IDLE)
                    gameObject.GetComponent<Movement>().move
                        (Direction.IDLE, playerTrail[thisFollowerCount], animationSpeed);
            }
        }
        base.Update();
    }
}