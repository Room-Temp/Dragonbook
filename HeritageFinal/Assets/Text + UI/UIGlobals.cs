using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGlobals : MonoBehaviour {

    /*
        Name - UIGlobals
        Type - Global Script
        Purpose - contains references for all UI elements
    */

    public static Image dialogueTextBoxImage;
    //public static Image _dialogueTextAdvance;
    public static Text dialogueTextBox;

    public Image _dialogueTextBoxImage;
    public Text _dialogueTextBox;

	// Use this for initialization
	void Start () {
        dialogueTextBoxImage = _dialogueTextBoxImage;
        dialogueTextBox = _dialogueTextBox;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
