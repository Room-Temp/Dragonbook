﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Name = Dialogue
    Type = Action Script
    Purpose = Facilitate dialog from NPCs and cutscenes.
*/
public class Dialogue : Interaction
{
    // Globals:
    public static bool dialogueRunning; // So two coroutines don't run at the same time
    public static bool dialogueLoaded;

    // Constants:
    const float MAX_DISTANCE = 2f;     // For dialogue initiated by the player 
    const int SCROLL_SPEED = 2; // How many frames pass before text scroll
    const int SCROLL_AMT = 1;   // How many characters per text scroll
    const int CHARACTERS_PER_LINE = 60; // How many characters can fit on a single line of text
    const int LINES_PER_BOX = 4; // How many lines in each text box
    const float TEXT_ADVANCE_BREATHE_RATE = 0.1f;
    const string OPTION_1_DEFAULT = "Yes";
    const string OPTION_2_DEFAULT = "No";

    // Variables:
    public string dialogue; // The dialogue typed in the editor's component
    public bool playerInitiatesDialogue;    // If true, the script listens for the player's button press
    public string option1String;
    public string option2String;
    private string currDialogue;    // String currently shown in the text box
    private int scrollSpeed; // Const scroll speed plus/minus modifiers
    private IEnumerator _scrollText;
    private bool wait;
    private bool lockSpeed;
    private bool instantScroll;
    private bool newText;
    private bool advanceMarker;
    private bool option1;
    private bool option2;
    private bool option1Selected = true;
    private string textCount;
    private int dialogueIndex;

    public void beginDialogue()
    {
        _scrollText = scrollText();
        StartCoroutine(_scrollText);
    }

    // Functions:
    private int parseText(int index)
    {
        // [Wn][W] - wait n frames (n = positive integer)
        // [L] - lock scrolling speed
        // [I] - instant scroll
        // [N] - new text box
        // [O] - new option text box
        textCount += dialogue[index];
        index++;
        textCount += dialogue[index];
        switch (dialogue[index])
        {
            case 'W':
                string num = "";
                if (!wait)
                {
                    index++;
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
            case 'N':
                newText = true;
                index++;
                break;
            case 'O':
                if (!option1 && !option2)   // Option text display
                {
                    option1 = true;
                    newText = true;
                }
                else if (option1 && !option2) // Option 1 text
                {
                    if (!option1Selected)
                    {
                        instantScroll = true;
                    }
                    else
                    {
                        Interface.dialogueTextBox.text = "";
                        instantScroll = false;
                    }
                    option1 = false;
                    option2 = true;
                }
                else if (!option1 && option2) // Option 2 text
                {
                    if (!option1Selected)
                    {
                        Interface.dialogueTextBox.text = "";
                        instantScroll = false;
                    }
                    else
                    {
                        instantScroll = true;
                    }
                    option1 = true;
                }
                else if (option1 && option2)    // End Option text
                {
                    option1 = false;
                    option2 = false;
                }
                index++;
                break;
        }
        textCount += dialogue[index];
        return index;   //this should always return the index after ']'.
    }
    private int readWord(int index) // Returns the length of the word beginning at index + 1
    {
        int count = 0;
        try
        {
            for (int i = index + 1; dialogue[i] != ' ' && dialogue[i] != '['; i++)
            {
                count++;
            }
        }
        catch
        {
            count = 0;
        }
        return count;
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
    private int getTextCount(int textBoxNo)
    {
        int length = 0;
        for (int i = 0; i < currDialogue.Length; i++)
        {
            if (currDialogue[i] != '\n') length++;
        }
        return length;
    }
    private IEnumerator scrollText()
    {
        dialogueRunning = true;
        Interface.dialogueTextBoxImage.enabled = true;
        Interface.dialogueTextBox.enabled = true;
        Interface.dialogueNameBox.enabled = true;
        Interface.dialogueAdvanceSprite.enabled = false;
        Interface.dialogueOption1.enabled = false;
        Interface.dialogueOption2.enabled = false;
        for (int i = 0; textCount != dialogue; i++)
        {
            currDialogue = "";
            newText = false;
            for (int j = -1; j < LINES_PER_BOX && textCount != dialogue && !newText; j++) // Each line per text box
            {               
                while ((getTextCount(i + 1) < CHARACTERS_PER_LINE * (j + 1) || j == -1) && textCount != dialogue && !newText)
                {
                    
                    if (j == -1) j = 0;
                    if (dialogue[dialogueIndex] == '[') // Beginning of command
                    {
                        dialogueIndex = parseText(dialogueIndex);
                        if (newText)
                        {
                            dialogueIndex++;
                            goto newBox;
                        }
                        goto skipCommand;
                    }
                    else if (dialogue[dialogueIndex] == ' ')
                    {
                        
                        textCount += ' ';
                        if (getTextCount(i + 1) % CHARACTERS_PER_LINE == 0) {  } //First character, print nothing
                        else if (getTextCount(i + 1) + readWord(dialogueIndex) >= CHARACTERS_PER_LINE * (j + 1)) //following word is greater than allowed characters
                        {
                            while (getTextCount(i + 1) % CHARACTERS_PER_LINE < CHARACTERS_PER_LINE - 1) //fill remainder of the line with spaces
                            {
                                currDialogue += ' ';
                            }
                            // last character, print space then go to next line
                            currDialogue += ' ';
                        }
                        else // Any other space is printed normally
                        {
                            currDialogue += ' ';
                        }


                    }
                    else
                    {
                        currDialogue += dialogue[dialogueIndex];
                        textCount += dialogue[dialogueIndex];
                    }
                    Interface.dialogueTextBox.text = currDialogue;
                    for (int l = 0; l < scrollSpeed && !instantScroll; l++)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    skipCommand:
                    dialogueIndex++;
                }
                currDialogue += '\n';
            }
        newBox:
            if (option1 && !option2)
            {
                // Display
                Interface.dialogueOption1.color = new Color(1, 1, 0, 1);    // yellow text indicates what is selected
                Interface.dialogueOption2.color = new Color(1, 1, 1, 1);
                bool optionAnswered = false;
                for (int j = 0; j < OPTION_1_DEFAULT.Length; j++)
                {
                    Interface.dialogueOption1.text += OPTION_1_DEFAULT[j];
                    for (int l = 0; l < scrollSpeed; l++)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
                for (int l = 0; l < scrollSpeed + 10; l++)
                {
                    yield return new WaitForEndOfFrame();
                }
                for (int j = 0; j < OPTION_2_DEFAULT.Length; j++)
                {
                    Interface.dialogueOption2.text += OPTION_2_DEFAULT[j];
                    for (int l = 0; l < scrollSpeed; l++)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
                while (!optionAnswered)
                {
                    Interface.dialogueOption1.color = new Color(1, 1, 0, 1);    // yellow text indicates what is selected
                    Interface.dialogueOption2.color = new Color(1, 1, 1, 1);
                    while (option1Selected)
                    {
                        if (Input.GetKeyDown(Controls.right))
                        {
                            option1Selected = false;
                        }
                        else if (Input.GetKeyDown(Controls.buttonA))
                        {
                            optionAnswered = true;
                            // TODO: branching scripts using an option
                        }
                        yield return new WaitForEndOfFrame();
                    }
                    if (optionAnswered) break;
                    Interface.dialogueOption1.color = new Color(1, 1, 1, 1);
                    Interface.dialogueOption2.color = new Color(1, 1, 0, 1);
                    while (!option1Selected)
                    {
                        if (Input.GetKeyDown(Controls.left))
                        {
                            option1Selected = true;
                        }
                        else if (Input.GetKeyDown(Controls.buttonA))
                        {
                            optionAnswered = true;
                        }
                        yield return new WaitForEndOfFrame();
                    }
                }

            }
            else
            {
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
        }
        Interface.dialogueTextBoxImage.enabled = false;
        Interface.dialogueTextBox.enabled = false;
        Interface.dialogueNameBox.enabled = false;
        Interface.dialogueOption1.enabled = false;
        Interface.dialogueOption2.enabled = false;
        textCount = "";
        dialogueRunning = false;
        dialogueIndex = 0;
        interacting = false;
        gameObject.GetComponent<SpriteAnimation>().stopAnimation();
        gameObject.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteAnimation>().currentSprites[0];
        GameState.setState(GameState.gameState.overworld);
        yield return new WaitForEndOfFrame();
    }
    void Start()
    {
        wait = false;
        lockSpeed = false;
        instantScroll = false;
        advanceMarker = true;
        currDialogue = "";
        textCount = "";
        scrollSpeed = SCROLL_SPEED;
        dialogueIndex = 0;
    }
    void Update()
    {
    }
}
