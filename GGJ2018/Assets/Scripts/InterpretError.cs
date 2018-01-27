using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterpretError : MonoBehaviour {

    public Text MessageText;

	// Use this for initialization
	void Start () {
        MessageText.text = "Welcome to the dungeon";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Success(string character)
    {
        //Debug.Log("OK.");
        switch (character.ToLower()) {
            case "dwarf":
                MessageText.text = character.ToUpper() + ": Aye, sir.";
                break;

            case "elf":
                MessageText.text = character.ToUpper() + ": Yes, master wizard.";
                break;

            case "vampire":
                MessageText.text = character.ToUpper() + ": As you wish, wizard.";
                break;
            
            case "werewolf":
            default:
                MessageText.text = character.ToUpper() + ": Yes, my master.";
                break;
        }

    }

    public void TooFewWords() {
        //Debug.Log("ERROR : Too few words.");
        MessageText.text = "Please, speak more words, master wizard.";
    }

    public void SyntaxError(string word) {
        //Debug.Log("ERROR : Word " + word + " is unknown.");
        //MessageText.text = "Sorry, we don't know what \"" + word + "\" means";
        MessageText.text = "What, does \"" + word + "\" mean?";
    }

    public void MeaningError(List<string> word_list)
    {
        string sentence = "";
        
        foreach (string s in word_list)
        {
            sentence += " " + s;
        }

        //switch (Random.Range(0, 4))

        //Debug.Log("ERROR : The sentence \"" + sentence + "\" has no meaning.");
        //MessageText.text = "Sorry, we don't understand, what did you mean?";
        MessageText.text = "This doesn't make any sense. Are you alright?";
    }

    public void MoveError(string character, string direction)
    {
        switch (character.ToLower())
        {
            case "dwarf":
                MessageText.text = character.ToUpper() + ": Nope, can't move to the " + direction + ".";
                break;

            case "elf":
                MessageText.text = character.ToUpper() + ": Sorry, I can't move to the " + direction + ".";
                break;

            case "vampire":
                MessageText.text = character.ToUpper() + ": I'm afraid it is not possible to go to the " + direction + ".";
                break;

            case "werewolf":
            default:
                MessageText.text = character.ToUpper() + ": the path is blocked on the " + direction + ".";
                break;
        }
    }
}
