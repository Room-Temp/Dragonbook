﻿using System.Collections;
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
    private Vector2 newPos;
    private Vector2 castDirection;
    private int prevCardDir;
    public enum CharacterType { Static, Dynamic, Player, Follower };
    public CharacterType characterType;


    void Start () {
    }

    public void move(int dir, float moveSpeed, int animSpeed)
    {
        
        direction = dir;
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
                // Movement functionality depends on the state of the player
                dir = Player.playerDirection;
                if (!Player.playerHasMoved)
                {
                    // State 1:
                    // If the player is colliding with an object
                    // set the direction to idle and stop animation
                    staticMovement(Direction.IDLE, moveSpeed);
                    dynamicMovement(Direction.IDLE, animSpeed);
                    break;
                }

                if (dir != Direction.IDLE && prevDir == Direction.IDLE)
                {
                    // State 2:
                    // The previous direction was idle, while the new direction
                    // has been pressed.
                    // This indicates beginning a movement
                    staticMovement(dir, moveSpeed);
                    dynamicMovement(dir, animSpeed);
                    break;
                }
                if (dir != Direction.IDLE && prevDir != Direction.IDLE && dir != prevDir)
                {
                    // State 3:
                    // The player has changed directions while still moving
                    staticMovement(dir, moveSpeed);
                    dynamicMovement(dir, animSpeed);
                    break;
                }
                if (dir != Direction.IDLE && prevDir == dir)
                {
                    // State 4:
                    // The player has not changed directions while still moving
                    // In this case dynamicMovement() does not need to be called
                    staticMovement(dir, moveSpeed);
                    break;
                }
                if (dir == Direction.IDLE && prevDir != Direction.IDLE)
                {
                    // State 5:
                    // The player is idle, but the previous direction
                    // is not. This indicates the player stopping
                    staticMovement(dir, moveSpeed);
                    dynamicMovement(dir, animSpeed);
                    break;
                }
                break;
                
            case CharacterType.Follower:
                break;
        }
        prevDir = dir;
    }
    public void move(int dir, Vector2 dest, int animSpeed)
    {
        if (prevDir != dir)
        {
            dynamicMovement(dir, animSpeed);
        }
        prevDir = dir;
        gameObject.GetComponent<Rigidbody2D>().MovePosition(dest);
    }
    private void staticMovement(int dir, float moveSpeed)
    {
        // Find new character position based on rigidbody
               
        float x = gameObject.GetComponent<Rigidbody2D>().position.x;
        float y = gameObject.GetComponent<Rigidbody2D>().position.y;
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
            0, castDirection, moveSpeed, ~(Layer.PLAYER | Layer.INTERACTION));
        // If the boxcast hit a collider, do not move character
        if (rh2d.collider != null)
        {
            newPos = new Vector2(x, y);
        }
        gameObject.GetComponent<Rigidbody2D>().MovePosition(newPos);
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
                    break;
            }      
    }
}
