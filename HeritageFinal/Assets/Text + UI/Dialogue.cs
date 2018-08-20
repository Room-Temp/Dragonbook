using System;
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
    private bool option1Selected;
    private string textCount;
    private int dialogueIndex;

    public void beginDialogue() 
    {
        _scrollText = scrollText();
        StartCoroutine(_scrollText);
    }       // Trigger the dialogue from another script
    private int parseText(int index)
    {
        // [Wn][W] - wait n frames (n = positive integer)
        // [L] - lock scrolling speed
        // [I] - instant scroll
        // [N] - new text box
        // [O] - new option text box
        // [V] - new vendor text box
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
                    currDialogue = "";
                    option1 = false;                    
                    if (option1Selected)
                    {
                        newText = true;
                        index++;
                        while (!(dialogue[index] == 'O' && dialogue[index + 1] == ']'))
                        {
                            textCount += dialogue[index];
                            index++;
                        }
                        textCount += dialogue[index];
                    }
                    else
                    {
                        option2 = true;
                    }
                }
                else if (!option1 && option2)
                {
                    if (!option1Selected)
                    {
                        newText = true;
                    }
                    option2 = false;
                }
                index++;
                break;
            case 'V':
                break;
        }
        textCount += dialogue[index];
        return index;   //this should always return the index at ']'.
    }   // Reads and interprets commands
    private int readWord(int index)
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
    }    // Returns the length of the word beginning at index + 1
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
        Interface.dialogueNameBox.text = gameObject.GetComponent<Character>().name;
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
                        if (dialogueIndex >= dialogue.Length)
                        {
                            goto newBox;
                        }
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
            bool buttonPressed = true;
            if (option1 && !option2)
            {
                // Display
                Interface.dialogueOption1.enabled = true;
                Interface.dialogueOption2.enabled = true;
                Interface.dialogueOption1.text = "";
                Interface.dialogueOption2.text = "";
                Interface.dialogueOption1.color = new Color(1, 1, 0, 1);    // yellow text indicates what is selected
                Interface.dialogueOption2.color = new Color(1, 1, 1, 1);
                bool optionAnswered = false;
                option1Selected = true;
                lockSpeed = true;
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
                lockSpeed = false;
                while (!optionAnswered)
                {
                    Interface.dialogueOption1.color = new Color(1, 1, 0, 1);    // yellow text indicates what is selected
                    Interface.dialogueOption2.color = new Color(1, 1, 1, 1);
                    while (option1Selected)
                    {
                        if (!Input.GetKey(Controls.buttonA))
                        {
                            buttonPressed = false;
                        }
                        yield return new WaitForEndOfFrame();
                        if (Input.GetKey(Controls.right))
                        {
                            option1Selected = false;
                        }
                        else if (Input.GetKey(Controls.buttonA) && !buttonPressed)
                        {
                            optionAnswered = true;
                            break;
                            // TODO: branching scripts using an option
                        }                       
                    }
                    if (optionAnswered) break;
                    Interface.dialogueOption1.color = new Color(1, 1, 1, 1);
                    Interface.dialogueOption2.color = new Color(1, 1, 0, 1);
                    while (!option1Selected)
                    {
                        if (!Input.GetKey(Controls.buttonA))
                        {
                            buttonPressed = false;
                        }
                        yield return new WaitForEndOfFrame();
                        if (Input.GetKey(Controls.left))
                        {
                            option1Selected = true;
                        }
                        else if (Input.GetKey(Controls.buttonA) && !buttonPressed)
                        {
                            optionAnswered = true;
                            break;
                        }
                    }
                }
                if (!option1Selected)
                {
                    while (!(dialogue[dialogueIndex] == '[' && dialogue[dialogueIndex + 1] == 'O'))
                    {
                        textCount += dialogue[dialogueIndex];
                        dialogueIndex++;
                    }
                }
                Interface.dialogueOption1.enabled = false;
                Interface.dialogueOption2.enabled = false;
            }
            else
            {
                bool fadingIn = true;
                
                Interface.dialogueAdvanceSprite.enabled = true;
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
                    if (!(Input.GetKey(Controls.buttonA) || Input.GetKey(Controls.buttonB)))
                    {
                        buttonPressed = false;
                    }
                    yield return new WaitForEndOfFrame();
                    if ((Input.GetKey(Controls.buttonA) || Input.GetKey(Controls.buttonB)) && !buttonPressed)
                    {
                        advanceMarker = false;
                    }
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
        option1Selected = true;
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
        if (dialogueRunning)
        {
            if (Input.GetKey(Controls.buttonA) && !lockSpeed)
            {
                scrollSpeed = SCROLL_SPEED - 1;
            }
            else
            {
                scrollSpeed = SCROLL_SPEED;
            }
        }
    }
}
