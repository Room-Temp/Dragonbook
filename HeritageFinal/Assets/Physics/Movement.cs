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
    private int prevDir;
    public enum CharacterType { Static, Dynamic, Player, Follower };
    public CharacterType characterType;


    void Start () {
        
    }

    public void move(int dir, float moveSpeed, int animSpeed, GameObject obj)
    {
        switch (characterType)
        {
            case CharacterType.Static:
                staticMovement(dir, moveSpeed, animSpeed, obj);
                break;
            case CharacterType.Dynamic:
                staticMovement(dir, moveSpeed, animSpeed, obj);
                dynamicMovement(dir, moveSpeed, animSpeed, obj);
                break;
            case CharacterType.Player:
                break;
            case CharacterType.Follower:
                break;
        }
    }

    private void staticMovement(int dir, float moveSpeed, int animSpeed, GameObject obj)
    {
        Vector2 vel = Vector2.zero;
        switch (dir)
        {
            case Direction.UP:
                vel = new Vector2(0, moveSpeed);
                break;
            case Direction.UP_RIGHT:
                vel = new Vector2(moveSpeed, moveSpeed);
                break;
            case Direction.RIGHT:
                vel = new Vector2(moveSpeed, 0);
                break;
            case Direction.DOWN_RIGHT:
                vel = new Vector2(moveSpeed, -moveSpeed);
                break;
            case Direction.DOWN:
                vel = new Vector2(0, -moveSpeed);
                break;
            case Direction.DOWN_LEFT:
                vel = new Vector2(-moveSpeed, -moveSpeed);
                break;
            case Direction.LEFT:
                vel = new Vector2(-moveSpeed, 0);
                break;
            case Direction.UP_LEFT:
                vel = new Vector2(-moveSpeed, moveSpeed);
                break;
        }
        if (dir != Direction.IDLE)
        {
            prevDir = dir;
        }
        obj.GetComponent<Rigidbody2D>().velocity = vel;
    }
    private void dynamicMovement(int dir, float moveSpeed, int animSpeed, GameObject obj)
    {

    }
	// Update is called once per frame
	void Update () {
		
	}
}
