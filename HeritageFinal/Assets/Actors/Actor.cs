using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    /* 
        Script - Actor
        Type - Superscript
        Purpose - An actor is defined as anything that is NOT a background/terrain tile
                Actors can be houses, trees, the player, etc.
    */
	// Use this for initialization
	protected virtual void Start () {
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(gameObject.GetComponent<Transform>().position.y * 1000);

    }
}
