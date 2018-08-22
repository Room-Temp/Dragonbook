using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name - Controls
    Type - Global Script
    Purpose - hold information for key controls. Can be bound to other keys/controller
*/
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

    private static int up_;
    private static int down_;
    private static int left_;
    private static int right_;
    private static int rightBumper_;
    private static int leftBumper_;
    private static int buttonA_;
    private static int buttonB_;
    private static int pause_;

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
        if (Input.GetKeyDown(up)) up_ = 1;
        else if (Input.GetKeyUp(up)) up_ = -1;
        else up_ = 0;
        if (Input.GetKeyDown(down)) down_ = 1;
        else if (Input.GetKeyUp(down)) down_ = -1;
        else down_ = 0;
        if (Input.GetKeyDown(left)) left_ = 1;
        else if (Input.GetKeyUp(left)) left_ = -1;
        else left_ = 0;
        if (Input.GetKeyDown(right)) right_ = 1;
        else if (Input.GetKeyUp(right)) right_ = -1;
        else right_ = 0;
        if (Input.GetKeyDown(leftBumper)) leftBumper_ = 1;
        else if (Input.GetKeyUp(leftBumper)) leftBumper_ = -1;
        else leftBumper_ = 0;
        if (Input.GetKeyDown(rightBumper)) rightBumper_ = 1;
        else if (Input.GetKeyUp(rightBumper)) rightBumper_ = -1;
        else rightBumper_ = 0;
        if (Input.GetKeyDown(buttonA)) buttonA_ = 1;
        else if (Input.GetKeyUp(buttonA)) buttonA_ = -1;
        else buttonA_ = 0;
        if (Input.GetKeyDown(buttonB)) buttonB_ = 1;
        else if (Input.GetKeyUp(buttonB)) buttonB_ = -1;
        else buttonB_ = 0;
        if (Input.GetKeyDown(pause)) pause_ = 1;
        else if (Input.GetKeyUp(pause)) pause_ = -1;
        else pause_ = 0;
    }
    public static bool getKeyDown(KeyCode key)
    {
        if ((key == up && up_ == 1) ||
            (key == down && down_ == 1) ||
            (key == left && left_ == 1) ||
            (key == right && right_ == 1) ||
            (key == rightBumper && rightBumper_ == 1) ||
            (key == leftBumper && leftBumper_ == 1) ||
            (key == buttonA && buttonA_ == 1) ||
            (key == buttonB && buttonB_ == 1) ||
            (key == pause && pause_ == 1))
            return true;
        return false;
    }

    public static bool getKeyUp(KeyCode key)
    {
        if ((key == up && up_ == -1) ||
            (key == down && down_ == -1) ||
            (key == left && left_ == -1) ||
            (key == right && right_ == -1) ||
            (key == rightBumper && rightBumper_ == -1) ||
            (key == leftBumper && leftBumper_ == -1) ||
            (key == buttonA && buttonA_ == -1) ||
            (key == buttonB && buttonB_ == -1) ||
            (key == pause && pause_ == -1))
            return true;
        return false;
    }
}
