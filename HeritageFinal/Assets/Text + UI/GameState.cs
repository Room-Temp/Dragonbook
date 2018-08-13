using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    /*
     Name - Game State
     Type - Global Script
     Purpose - A global variable that contains different states from the game.
        This indicates whether the game is paused, in combat, in overworld, or in cutscene

    */

    static public bool paused;
    static public bool combat;
    static public bool overworld;
    static public bool cutscene;
    // "paused" in the case means vendor menus as well
    // "cutscene" in this case means anything that stops player control but is NOT in combat or paused


    public enum gameState {paused, combat, overworld, cutscene};

    static public bool getState(gameState state)
    {
        switch (state)
        {
            case gameState.paused:
                return paused;
            case gameState.combat:
                return combat;
            case gameState.overworld:
                return overworld;
            case gameState.cutscene:
                return cutscene;
        }
        return false;
    }

    static public void setState(gameState state)
    {
        paused = false;
        combat = false;
        overworld = false;
        cutscene = false;
        switch (state)
        {
            case gameState.paused:
                paused = true;
                break;
            case gameState.combat:
                combat = true;
                break;
            case gameState.overworld:
                overworld = true;
                break;
            case gameState.cutscene:
                cutscene = true;
                break;
        }
    }

	void Start () {
        setState(gameState.overworld);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
