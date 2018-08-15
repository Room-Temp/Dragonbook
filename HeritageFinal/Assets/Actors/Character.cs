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
    public BoxCollider2D upInteractionTrigger;
    public BoxCollider2D downInteractionTrigger;
    public BoxCollider2D leftInteractionTrigger;
    public BoxCollider2D rightInteractionTrigger;
    protected const float DEFAULT_MOVEMENT_SPEED = 0.5f;
    protected const int DEFAULT_ANIMATION_SPEED = 10;

    // Use this for initialization
    protected override void Start () {
        BoxCollider2D[] colliders = gameObject.GetComponentsInChildren<BoxCollider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            switch (colliders[i].name)
            {
                case "Right Interaction Trigger":
                    rightInteractionTrigger = colliders[i];
                    break;
                case "Left Interaction Trigger":
                    leftInteractionTrigger = colliders[i];
                    break;
                case "Up Interaction Trigger":
                    upInteractionTrigger = colliders[i];
                    break;
                case "Down Interaction Trigger":
                    downInteractionTrigger = colliders[i];
                    break;
            }
        }
        if (defaultSpeeds)
        {
            movementSpeed = DEFAULT_MOVEMENT_SPEED;
            animationSpeed = DEFAULT_ANIMATION_SPEED;
        }
        base.Start();
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}
}
