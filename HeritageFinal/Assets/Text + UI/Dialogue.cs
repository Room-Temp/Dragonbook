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

    const int SCROLL_SPEED = 1; // How many frames pass before text scroll
    const int SCROLL_AMT = 1;   // How many characters per text scroll
    const int CHARACTERS_PER_LINE = 40; // How many characters can fit on a single line of text
    const int LINES_PER_BOX = 4; // How many lines in each text box
    const float TEXT_ADVANCE_BREATHE_RATE = 0.1f;

    public string dialogue; // The dialogue typed in the editor's component
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
                        num += dialogue[index];
                        index++;
                    }
                    scrollSpeed = int.Parse(num);
                }
                else
                {
                    scrollSpeed = SCROLL_SPEED;
                    textCount += dialogue[index];
                    index++;
                }
                wait = !wait;   
                break;
            case 'L':
                lockSpeed = !lockSpeed;
                textCount += dialogue[index];
                index++;
                break;
            case 'I':
                instantScroll = !instantScroll;
                textCount += dialogue[index];
                index++;
                break;
        }
        return index;   //this should always return the index at ']'. Then the index iterates into the next character.
    }

    private IEnumerator scrollText()
    {
        Interface.dialogueTextBoxImage.enabled = true;
        Interface.dialogueAdvanceSprite.enabled = false;
        
        while (textCount != dialogue)
        {
            for (int i = 0; i < dialogue.Length; i++)   // Each text box
            {
                currDialogue = "";
                for (int j = 0; j < LINES_PER_BOX; j++) // Each line per text box
                {
                    for (int k = 0; currDialogue.Length + 1 % CHARACTERS_PER_LINE > 0; k++)
                    {
                        if (dialogue[k] == ']')
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
                }
                Interface.dialogueAdvanceSprite.enabled = true;
                bool fadingIn = true;
                Interface.dialogueAdvanceSprite.color = new
                    Color(Interface.dialogueAdvanceSprite.color.r,
                          Interface.dialogueAdvanceSprite.color.g,
                          Interface.dialogueAdvanceSprite.color.b, 0);
                while (advanceMarker)
                {
                    if (Interface.dialogueAdvanceSprite.color.a >= 0)
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
                        advanceMarker = true;
                    }
                    yield return new WaitForEndOfFrame();
                }
                Interface.dialogueAdvanceSprite.enabled = false;
            }
        }
        Interface.dialogueTextBoxImage.enabled = false;
        yield return new WaitForEndOfFrame();
    }

	// Use this for initialization
	void Start () {
        wait = false;
        lockSpeed = false;
        instantScroll = false;
        advanceMarker = true;
        textCount = "";
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(Controls.buttonA) && Interface.dialogueTextBoxImage.enabled)
        {
            if (_scrollText != null) StopCoroutine(_scrollText);
            _scrollText = scrollText();
            StartCoroutine(_scrollText);
        }
    }
}
