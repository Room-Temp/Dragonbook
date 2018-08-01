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

    public string dialogue; // The dialogue typed in the editor's component
    private string currDialogue;    // String currently shown in the text box
    private int scrollSpeed; // Const scroll speed plus/minus modifiers
    private IEnumerator _scrollText;

    private IEnumerator scrollText()
    {
        UIGlobals.dialogueTextBoxImage.enabled = true;
        string textCount = "";
        while (textCount != dialogue)
        {
            for (int i = 0; i < dialogue.Length; i++)
            {
                currDialogue += dialogue[i];
                textCount += dialogue[i];

                UIGlobals.dialogueTextBox.text = currDialogue;
                for (int j = 0; j < scrollSpeed; j++)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        UIGlobals.dialogueTextBoxImage.enabled = false;
        yield return new WaitForEndOfFrame();
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetKey(Controls.buttonA))
        {
            if (_scrollText != null) StopCoroutine(_scrollText);
            _scrollText = scrollText();
            StartCoroutine(_scrollText);
        }
        */
    }
}
