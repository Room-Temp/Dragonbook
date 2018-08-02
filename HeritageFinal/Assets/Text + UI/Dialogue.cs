using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name = Dialogue
    Type = Action Script
    Purpose = Facilitate dialog from NPCs and cutscenes. Includes text parser.

*/
public class Dialogue : MonoBehaviour {

    public static bool dialogueRunning;

    const float MAX_DISTANCE = 2f;     // For dialogue initiated by the player 
    const int SCROLL_SPEED = 3; // How many frames pass before text scroll
    const int SCROLL_AMT = 1;   // How many characters per text scroll
    const int CHARACTERS_PER_LINE = 40; // How many characters can fit on a single line of text
    const int LINES_PER_BOX = 4; // How many lines in each text box
    const float TEXT_ADVANCE_BREATHE_RATE = 0.1f;
    

    public string dialogue; // The dialogue typed in the editor's component
    public bool playerInitiatesDialogue;    // If true, the script listens for the player's button press
    private string currDialogue;    // String currently shown in the text box
    private int scrollSpeed; // Const scroll speed plus/minus modifiers
    private IEnumerator _scrollText;
    private bool wait;
    private bool lockSpeed;
    private bool instantScroll;
    private bool advanceMarker;
    private string textCount;


    private int parseText(int index)
    {
        // [W10][W] - wait ten frames
        // [L] - lock scrolling speed
        // [I] - instant scroll
        textCount += dialogue[index];
        index++;
        switch (dialogue[index])
        {
            case 'W':
                string num = "";
                if (!wait)
                {
                    while (dialogue[index] != ']')
                    {
                        textCount += dialogue[index];
                        num+= dialogue[index];
                        index++;
                    }
                    scrollSpeed = int.Parse(num);
                }
                else
                {
                    scrollSpeed = SCROLL_SPEED;
                    index++;
                }
                wait = !wait;   
                break;
            case 'L':
                lockSpeed = !lockSpeed;
                index++;
                break;
            case 'I':
                instantScroll = !instantScroll;
                index++;
                break;
        }
        return index;   //this should always return the index at ']'. Then the index iterates into the next character.
    }

    private bool checkTextCount()
    {
        string tempCount = "";
        for (int i = 0; i < textCount.Length; i++)
        {
            if (textCount[i] != '\n') tempCount += textCount[i];
        }
        return (dialogue != tempCount);
    }

    private IEnumerator scrollText()
    {
        dialogueRunning = true;
        Interface.dialogueTextBoxImage.enabled = true;
        Interface.dialogueTextBox.enabled = true;
        Interface.dialogueAdvanceSprite.enabled = false;       
        while (checkTextCount())
        {
                //currDialogue = "";
                for (int j = 0; j < LINES_PER_BOX || textCount.Length == 0 && checkTextCount(); j++) // Each line per text box
                {
                    for (int k = 0; (currDialogue.Length % CHARACTERS_PER_LINE > 0 || currDialogue.Length == 0) && checkTextCount(); k++)
                    {
                        if (dialogue[k] == '[')
                        {
                            k = parseText(k);
                        }
                        else
                        {
                            currDialogue += dialogue[k];
                        }
                        textCount += dialogue[k];
                        Interface.dialogueTextBox.text = currDialogue;
                        for (int l = 0; l < scrollSpeed && !instantScroll; l++)
                        {
                            yield return new WaitForEndOfFrame();
                        }
                    }
                    currDialogue += '\n';
                    textCount += '\n';
                }
                Interface.dialogueAdvanceSprite.enabled = true;
                bool fadingIn = true;
                Interface.dialogueAdvanceSprite.color = new
                    Color(Interface.dialogueAdvanceSprite.color.r,
                          Interface.dialogueAdvanceSprite.color.g,
                          Interface.dialogueAdvanceSprite.color.b, 0);
                while (advanceMarker)
                {
                    if (Interface.dialogueAdvanceSprite.color.a <= 0)
                    {
                        fadingIn = true;
                    }
                    else if (Interface.dialogueAdvanceSprite.color.a >= 1)
                    {
                        fadingIn = false;
                    }

                    if (fadingIn)
                    {
                        Interface.dialogueAdvanceSprite.color = new
                            Color(Interface.dialogueAdvanceSprite.color.r,
                                  Interface.dialogueAdvanceSprite.color.g,
                                  Interface.dialogueAdvanceSprite.color.b, 
                                  Interface.dialogueAdvanceSprite.color.a + TEXT_ADVANCE_BREATHE_RATE);
                    }
                    else
                    {
                        Interface.dialogueAdvanceSprite.color = new
                            Color(Interface.dialogueAdvanceSprite.color.r,
                            Interface.dialogueAdvanceSprite.color.g,
                            Interface.dialogueAdvanceSprite.color.b,
                            Interface.dialogueAdvanceSprite.color.a - TEXT_ADVANCE_BREATHE_RATE);
                    }
                    if (Input.GetKey(Controls.buttonA) || Input.GetKey(Controls.buttonB))
                    {
                        advanceMarker = false;
                    }
                    yield return new WaitForEndOfFrame();
                }
            advanceMarker = true;
                Interface.dialogueAdvanceSprite.enabled = false;
        }
        Interface.dialogueTextBoxImage.enabled = false;
        Interface.dialogueTextBox.enabled = false;
        yield return new WaitForEndOfFrame();
        textCount = "";
        dialogueRunning = false;
    }

	// Use this for initialization
	void Start () {
        wait = false;
        lockSpeed = false;
        instantScroll = false;
        advanceMarker = true;
        textCount = "";
        scrollSpeed = SCROLL_SPEED;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(Controls.buttonA) && !dialogueRunning && playerInitiatesDialogue)
        {
            _scrollText = scrollText();
            StartCoroutine(_scrollText);
        }
    }
}
