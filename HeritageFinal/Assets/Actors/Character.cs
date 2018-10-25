using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name: Character
    Type: Superclass
    Purpose: A "Character" is any actor in the game that can be interacted with. 
*/

public class Character : Actor {

    // Resources
    public int health;
    public int mana;    // used to cast druidic abilities
    public int psi;     // used to cast psychic abilities

    // Primary stats - increased by armor
    public int stamina;         // Increases health
    public int armor;           // Increases damage mitigation
    public int strength;        // Increases damage output
    public int wisdom;          // Increases mana
    public int intelligence;    // Increases PSI
    public int speed;           // Determines attack order and dodge chance

    // Percentages - increased by weapon
    public float hitChance;         // Chance to land an attack
    public float critChance;        // Chance to land a critical hit
    public float dodgeChance;       // Chance to evade an attack
    


    public float movementSpeed;
    public int animationSpeed;
    public int direction;
    public bool defaultSpeeds;
    public string characterName;
    public int prevDir;
    protected const float DEFAULT_MOVEMENT_SPEED = 0.02f;
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
