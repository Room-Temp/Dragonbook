using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour {

    /*
        Name - UIGlobals
        Type - Global Script
        Purpose - contains references for all UI elements
    */

    public static Image dialogueTextBoxImage;
    public static Text dialogueNameBox;
    public static Text dialogueTextBox;
    public static Image dialogueAdvanceSprite;
    public static Text dialogueOption1;
    public static Text dialogueOption2;

    public Image _dialogueTextBoxImage;
    public Text _dialogueNameBox;
    public Text _dialogueTextBox;
    public Image _dialogueAdvanceSprite;
    public Text _dialogueOption1;
    public Text _dialogueOption2;

	// Use this for initialization
	void Start () {
        dialogueTextBoxImage = _dialogueTextBoxImage;
        dialogueTextBox = _dialogueTextBox;
        dialogueNameBox = _dialogueNameBox;
        dialogueAdvanceSprite = _dialogueAdvanceSprite;
        dialogueOption1 = _dialogueOption1;
        dialogueOption2 = _dialogueOption2;
        dialogueTextBoxImage.enabled = false;
        dialogueTextBox.enabled = false;
        dialogueNameBox.enabled = false;
        dialogueAdvanceSprite.enabled = false;
        dialogueOption1.enabled = false;
        dialogueOption2.enabled = false;

        Interaction.interacting = false;


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
