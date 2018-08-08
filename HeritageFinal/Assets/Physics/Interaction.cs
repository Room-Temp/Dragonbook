using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {


    /*
        Name: Interaction
        Type: Action Script
        Purpose: An Interaction happens when the following conditions are met:
            -player is in the overworld
            -player is facing the script's game object
            -player is close enough to the script's game object
            -button A has been pressed
        Interactions include dialogue and vendor menus.

    */
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(Controls.buttonA))
        {

        }
	}
}
