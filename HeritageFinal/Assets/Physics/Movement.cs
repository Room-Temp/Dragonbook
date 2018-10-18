using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    Name: Movement
    Type: Action
    Purpose: Facilitate movement of characters. 
             The nature of this script changes depending on the type of character that is moving.
             Static NPCs have no animation, player is controlled by input, follower is based on player movement.
*/
public class Movement : MonoBehaviour {

    public int direction;
    public int prevDir;
    private int prevCardDir;
    private int tempCardDir;
    private bool playerMoving;
    public enum CharacterType { Static, Dynamic, Player, Follower };
    public CharacterType characterType;


    void Start () {
        playerMoving = false;
    }

    public void move(int dir, float moveSpeed, int animSpeed)
    {
        switch (characterType)
        {
            case CharacterType.Static:
                staticMovement(dir, moveSpeed);
                break;
            case CharacterType.Dynamic:
                staticMovement(dir, moveSpeed);
                dynamicMovement(dir, animSpeed);
                break;
            case CharacterType.Player:
                if (dir == Direction.IDLE)  //player has released movement key
                {
                    staticMovement(dir, moveSpeed);
                    dynamicMovement(dir, animSpeed);
                }
                else if (dir != Direction.IDLE && !playerMoving) //player begins movement
                {
                    staticMovement(dir, moveSpeed);
                    dynamicMovement(dir, animSpeed);
                    playerMoving = true;
                }
                else if (dir != Direction.IDLE && dir != prevDir) //player changes direction in movement
                {
                    staticMovement(dir, moveSpeed);
                    if (prevCardDir != tempCardDir || prevCardDir == 0) dynamicMovement(dir, animSpeed);
                    playerMoving = true;
                }
                break;
            case CharacterType.Follower:
                break;
        }
    }
    public void move(int dir, Vector2 dest, int animSpeed)
    {
        gameObject.GetComponent<Rigidbody2D>().MovePosition(dest);
        dynamicMovement(dir, animSpeed);
    }
    private void staticMovement(int dir, float moveSpeed)
    {
        // Find new character position based on rigidbody
        Vector2 newPos;
        Vector2 castDirection;
        float x = gameObject.GetComponent<Rigidbody2D>().position.x;
        float y = gameObject.GetComponent<Rigidbody2D>().position.y;
        tempCardDir = prevCardDir;
        switch (dir)
        {
            case Direction.UP:
                newPos = new Vector2(x, y + moveSpeed);
                castDirection = new Vector2(0, 1);
                prevCardDir = Direction.UP;               
                break;
            case Direction.UP_RIGHT:
                newPos = new Vector2(x + moveSpeed, y + moveSpeed);
                castDirection = new Vector2(1, 1);
                break;
            case Direction.RIGHT:
                newPos = new Vector2(x + moveSpeed, y);
                castDirection = new Vector2(1, 0);
                prevCardDir = Direction.RIGHT;
                break;
            case Direction.DOWN_RIGHT:
                newPos = new Vector2(x + moveSpeed, y - moveSpeed);
                castDirection = new Vector2(1, -1);
                break;
            case Direction.DOWN:
                newPos = new Vector2(x, y - moveSpeed);
                castDirection = new Vector2(0, -1);
                prevCardDir = Direction.DOWN;
                break;
            case Direction.DOWN_LEFT:
                newPos = new Vector2(x - moveSpeed, y - moveSpeed);
                castDirection = new Vector2(-1, -1);
                break;
            case Direction.LEFT:
                newPos = new Vector2(x - moveSpeed, y);
                castDirection = new Vector2(-1, 0);
                prevCardDir = Direction.LEFT;
                break;
            case Direction.UP_LEFT:
                newPos = new Vector2(x - moveSpeed, y + moveSpeed);
                castDirection = new Vector2(-1, 1);
                break;
            default:
                newPos = new Vector2(x, y);
                castDirection = new Vector2(0, 0);
                break;
        }

        // Unity's 2D colliders suck - boxcast to check colliders before moving
        RaycastHit2D rh2d = Physics2D.BoxCast(
            gameObject.GetComponent<BoxCollider2D>().bounds.center, 
            gameObject.GetComponent<BoxCollider2D>().bounds.size, 
            0, castDirection, moveSpeed);
        // If the boxcast hit a collider, do not move character
        if (rh2d.collider != null)
        {
            newPos = new Vector2(x, y);
        }
        gameObject.GetComponent<Rigidbody2D>().MovePosition(newPos);

        /*
        Vector2 vel = Vector2.zero;
        tempCardDir = prevCardDir;
        switch (dir)
        {
            case Direction.UP:
                vel = new Vector2(0, moveSpeed);
                prevCardDir = Direction.UP;
                break;
            case Direction.UP_RIGHT:
                vel = new Vector2(moveSpeed, moveSpeed);
                break;
            case Direction.RIGHT:
                vel = new Vector2(moveSpeed, 0);
                prevCardDir = Direction.RIGHT;
                break;
            case Direction.DOWN_RIGHT:
                vel = new Vector2(moveSpeed, -moveSpeed);
                break;
            case Direction.DOWN:
                vel = new Vector2(0, -moveSpeed);
                prevCardDir = Direction.DOWN;
                break;
            case Direction.DOWN_LEFT:
                vel = new Vector2(-moveSpeed, -moveSpeed);
                break;
            case Direction.LEFT:
                vel = new Vector2(-moveSpeed, 0);
                prevCardDir = Direction.LEFT;
                break;
            case Direction.UP_LEFT:
                vel = new Vector2(-moveSpeed, moveSpeed);
                break;
        }        
        if (dir != Direction.IDLE)
        {
            prevDir = dir;
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = vel;
        */
    }
    private void dynamicMovement(int dir, int animSpeed)
    {
            switch (dir)
            {
                case Direction.UP:
                    gameObject.GetComponent<SpriteAnimation>().anim(SpriteAnimation.spriteIndex.upMovement);
                    break;
                case Direction.UP_RIGHT:
                    if (prevCardDir == Direction.RIGHT) gameObject.GetComponent<SpriteAnimation>().anim(SpriteAnimation.spriteIndex.rightMovement);
                    else
                    {
                        prevCardDir = Direction.UP;
                        gameObject.GetComponent<SpriteAnimation>().anim(SpriteAnimation.spriteIndex.upMovement);
                    }
                    break;
                case Direction.RIGHT:
                    gameObject.GetComponent<SpriteAnimation>().anim(SpriteAnimation.spriteIndex.rightMovement);
                    break;
                case Direction.DOWN_RIGHT:
                    if (prevCardDir == Direction.RIGHT) gameObject.GetComponent<SpriteAnimation>().anim(SpriteAnimation.spriteIndex.rightMovement);
                    else
                    {
                        prevCardDir = Direction.DOWN;
                        gameObject.GetComponent<SpriteAnimation>().anim(SpriteAnimation.spriteIndex.downMovement);
                    }
                    break;
                case Direction.DOWN:
                    gameObject.GetComponent<SpriteAnimation>().anim(SpriteAnimation.spriteIndex.downMovement);
                    break;
                case Direction.DOWN_LEFT:
                    if (prevCardDir == Direction.LEFT) gameObject.GetComponent<SpriteAnimation>().anim(SpriteAnimation.spriteIndex.leftMovement);
                    else
                    {
                        prevCardDir = Direction.DOWN;
                        gameObject.GetComponent<SpriteAnimation>().anim(SpriteAnimation.spriteIndex.downMovement);
                    }
                    break;
                case Direction.LEFT:
                    gameObject.GetComponent<SpriteAnimation>().anim(SpriteAnimation.spriteIndex.leftMovement);
                    break;
                case Direction.UP_LEFT:
                    if (prevCardDir == Direction.LEFT) gameObject.GetComponent<SpriteAnimation>().anim(SpriteAnimation.spriteIndex.leftMovement);
                    else
                    {
                        prevCardDir = Direction.UP;
                        gameObject.GetComponent<SpriteAnimation>().anim(SpriteAnimation.spriteIndex.upMovement);
                    }
                    break;
                default:
                    gameObject.GetComponent<SpriteAnimation>().stopAnimation(prevDir, prevCardDir);
                    playerMoving = false;
                    break;
            }      
    }
}
