using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCollider : MonoBehaviour {

    /*
        Name: Interaction Collider
        Type: Component Script
        Purpose: Attach this to the interaction trigger game objects
    */
    // Use this for initialization

    public enum TriggerDirection {left, right, up, down};
    public TriggerDirection triggerDirection;
    private int direction;
    private bool triggered;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            if (other.gameObject.GetComponent<Player>().linePlacement == 1) {
                triggered = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            if (other.gameObject.GetComponent<Player>().linePlacement == 1)
            {
                triggered = false;
            }
        }
    }

    public bool isTriggered(int dir)
    {
        return (dir == direction && triggered);
    }

	void Start () {
		switch (triggerDirection)
        {
            case (TriggerDirection.up):
                direction = Direction.UP;
                break;
            case (TriggerDirection.down):
                direction = Direction.DOWN;
                break;
            case (TriggerDirection.left):
                direction = Direction.LEFT;
                break;
            case (TriggerDirection.right):
                direction = Direction.RIGHT;
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
