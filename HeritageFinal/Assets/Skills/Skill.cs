using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {

    /*
        Name: Skill
        Type: Global Script
        Purpose: A skill is anything chosen in battle. Skills can be learned or cast from items.
            Using a food item in battle, for example, counts as a skill
    */

    // Resources
    public int health;
    public int mana;    // used to cast druidic abilities
    public int psi;     // used to cast psychic abilities

    public int damage;
  //public Buff statusEffect;   // if not null, will add a buff or debuff to the spell's effect
    public enum target
    {
        singleFriend,
        singleEnemy,
        singleAll,
        allFriends,
        allEnemies,
        all
    };
    public enum damageType
    {
        physical,
        fire,
        water,
        radiation,
        nature,
        shadow
    };

    // Primary stats
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



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
