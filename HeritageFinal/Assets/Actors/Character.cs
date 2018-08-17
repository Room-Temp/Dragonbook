using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Character
    Type: Superclass
    Purpose: A "Character" is any actor in the game that can be interacted with. 
*/

public class Character : Actor {

    public float movementSpeed;
    public int animationSpeed;
    public int direction;
    public bool defaultSpeeds;
    protected const float DEFAULT_MOVEMENT_SPEED = 0.5f;
    protected const int DEFAULT_ANIMATION_SPEED = 10;

    protected override void Start () {
        if (defaultSpeeds)
        {
            movementSpeed = DEFAULT_MOVEMENT_SPEED;
            animationSpeed = DEFAULT_ANIMATION_SPEED;
        }
        base.Start();
    }
	
	protected override void Update () {
        base.Update();
	}
}
