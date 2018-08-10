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

    public Image _dialogueTextBoxImage;
    public Text _dialogueNameBox;
    public Text _dialogueTextBox;
    public Image _dialogueAdvanceSprite;

	// Use this for initialization
	void Start () {
        dialogueTextBoxImage = _dialogueTextBoxImage;
        dialogueTextBox = _dialogueTextBox;
        dialogueNameBox = _dialogueNameBox;
        dialogueAdvanceSprite = _dialogueAdvanceSprite;
        Interaction.interacting = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
