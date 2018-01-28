using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterpretError : MonoBehaviour {

    public static InterpretError Instance;
    public Text MessageText;

    void Awake()
    {
        Instance = this;
    }

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
        int r = Random.Range(0, 8);

        switch (r)
        {
            case 0:
                MessageText.text = "DWARF : Well you have to tell us more, right?";
                break;

            case 1:
                MessageText.text = "ELF : Please, speak more words, master wizard.";
                break;

            case 2:
                MessageText.text = "VAMPIRE : Speak up, wizard, we are listening.";
                break;

            case 3:
            default:
                MessageText.text = "WEREWOLF : What is your command, masterrr?";
                break;
        }
    }

    public void SyntaxError(string word) {
        int r = Random.Range(0, 4);

        switch (r)
        {
            case 0:
                MessageText.text = "DWARF : Why did you say \"" + word + "\"?";
                break;

            case 1:
                MessageText.text = "ELF : I don't understand the meaning of \"" + word + "\" in this context.";
                break;

            case 2:
                MessageText.text = "VAMPIRE : Is the word \"" + word + "\" part of a riddle?";
                break;

            case 3:
            default:
                MessageText.text = "WEREWOLF : I don't underrrstand \"" + word + "\" , masterrr.";
                break;
        }
    }

    public void MeaningError(List<string> word_list)
    {
        string sentence = "";
        int r = Random.Range(0, 8);

        foreach (string s in word_list)
        {
            sentence += " " + s;
        }

        switch (r) {
            case 0:
                MessageText.text = "DWARF : What's with this nonsense? Are you alright?";
                break;
            case 1:
                MessageText.text = "DWARF : This does not mean anything. Get a grip!";
                break;

            case 2:
                MessageText.text = "ELF : We haven't understood. What did you mean?";
                break;
            case 3:
                MessageText.text = "ELF : Could you rephrase, master wizard?";
                break;

            case 4:
                MessageText.text = "VAMPIRE : It seems you have messed up your words, wizard.";
                break;

            case 5:
                MessageText.text = "VAMPIRE : And what would be the meaning of this?";
                break;

            case 6:
                MessageText.text = "WEREWOLF : I don't underrrstand, masterrr.";
                break;

            case 7:
            default:
                MessageText.text = "WEREWOLF : Sorrrrry, I don't underrrstand!";
                break;
        }
    }

    public void InconsistentDirection(string character, string thing) {
        switch (character.ToLower())
        {
            case "dwarf":
                MessageText.text = character.ToUpper() + ": What? \"" + thing + "\" is not even a direction!";
                break;

            case "elf":
                MessageText.text = character.ToUpper() + ": I can't go to direction \"" + thing + "\".";
                break;

            case "vampire":
                MessageText.text = character.ToUpper() + ": I believe \"" + thing + "\" is not the way.";
                break;

            case "werewolf":
            default:
                MessageText.text = character.ToUpper() + ": Growl... Which way is \"" + thing + "\"?";
                break;
        }
    }

    public void InconsistentAction(string character, string thing)
    {
        switch (character.ToLower())
        {
            case "dwarf":
                MessageText.text = character.ToUpper() + ": What? \"" + thing + "\" is not an object I use!";
                break;

            case "elf":
                MessageText.text = character.ToUpper() + ": I'm afraid it is not possible to grab \"" + thing + "\".";
                break;

            case "vampire":
                MessageText.text = character.ToUpper() + ": It is not like I had \"" + thing + "\" within grasp.";
                break;

            case "werewolf":
            default:
                MessageText.text = character.ToUpper() + ": Growl. I can't do anything with \"" + thing + "\".";
                break;
        }
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
                MessageText.text = character.ToUpper() + ": Growl. The path is blocked on the " + direction + ".";
                break;
        }
    }

    public void NoObject(string character, string thing)
    {
        switch (character.ToLower())
        {
            case "dwarf":
                MessageText.text = character.ToUpper() + ": There is no " + thing + " here.";
                break;

            case "elf":
                MessageText.text = character.ToUpper() + ": Sorry, I can't find the " + thing + ".";
                break;

            case "vampire":
                MessageText.text = character.ToUpper() + ": I would gladly have picked the " + thing + " up if it was there.";
                break;

            case "werewolf":
            default:
                MessageText.text = character.ToUpper() + ": Growl! Which " + thing + "? Where?";
                break;
        }
    }

    public void OtherObject(string character, string thing, string myThing)
    {
        switch (character.ToLower())
        {
            case "dwarf":
                MessageText.text = character.ToUpper() + ": I can't carry both the" + myThing + " AND the " + thing + "!";
                break;

            case "elf":
                MessageText.text = character.ToUpper() + ": I already have the " + myThing + ", maybe I could pick up the " + thing + " later.";
                break;

            case "vampire":
                MessageText.text = character.ToUpper() + ": Let somone else carry the " + thing + ", I'll manage the " + myThing + ".";
                break;

            case "werewolf":
            default:
                MessageText.text = character.ToUpper() + ": Growl! I have a " + myThing + ". I can't take the" + thing + " now.";
                break;
        }
    }


    public void NoPosess(string character, string thing)
    {
        switch (character.ToLower())
        {
            case "dwarf":
                MessageText.text = character.ToUpper() + ": I don't have a " + thing + "!";
                break;

            case "elf":
                MessageText.text = character.ToUpper() + ": The " + thing + " is not in my posession.";
                break;

            case "vampire":
                MessageText.text = character.ToUpper() + ": I whish I had the " + thing + ". It is not the case.";
                break;

            case "werewolf":
            default:
                MessageText.text = character.ToUpper() + ": Growl! Which " + thing + "? I don't have it!";
                break;
        }
    }


    public void NoUse(string character, string thing)
    {
        switch (character.ToLower())
        {
            case "dwarf":
                MessageText.text = character.ToUpper() + ": I can't use the " + thing + " here!";
                break;

            case "elf":
                MessageText.text = character.ToUpper() + ": It is not possible for me to use the " + thing + " here.";
                break;

            case "vampire":
                MessageText.text = character.ToUpper() + ": I'm afraid the " + thing + " is of no use in there.";
                break;

            case "werewolf":
            default:
                MessageText.text = character.ToUpper() + ": What would I do with a " + thing + " here?";
                break;
        }
    }

}
