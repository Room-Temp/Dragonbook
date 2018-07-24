using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {

    public static KeyCode up;
    public static KeyCode down;
    public static KeyCode left;
    public static KeyCode right;
    public static KeyCode rightBumper;
    public static KeyCode leftBumper;
    public static KeyCode buttonA;
    public static KeyCode buttonB;
    public static KeyCode pause;

    public KeyCode _up;
    public KeyCode _down;
    public KeyCode _left;
    public KeyCode _right;
    public KeyCode _rightBumper;
    public KeyCode _leftBumper;
    public KeyCode _buttonA;
    public KeyCode _buttonB;
    public KeyCode _pause;

    // Use this for initialization
    void Start () {
        up = _up;
        down = _down;
        left = _left;
        right = _right;
        rightBumper = _rightBumper;
        leftBumper = _leftBumper;
        buttonA = _buttonA;
        buttonB = _buttonB;
        pause = _pause;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
