using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour {

    public static Options Instance;
    public int LevelNumber = 1;
    public int Difficulty = 0;
    public bool EnableTimer = false;
    public float TimerStart = 180.0F;
    
    public Text StartText;
    Timer TheTimer;
    Interpret TheInterpret;

    public GameObject GlobalPanel;
    public GameObject OK_Button;

    string basic_help = "\n" +
    "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tDICTIONNARY\n" +
    "\t\t\t\t\t\t\tVerbs:\t\t\t\t\t\t\t\t\t\t\t\tDirections and Objects:\n" +
    "\t\t\t\t\t\t\t- move\t\t\t\t\t\t\t\t\t\t\t\t- North\n" +
    "\t\t\t\t\t\t\t- pick\t\t\t\t\t\t\t\t\t\t\t\t- South\n" +
    "\t\t\t\t\t\t\t- use\t\t\t\t\t\t\t\t\t\t\t\t- Est\n" +
    "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t- West\n" +
    "\t\t\t\t\t\t\tCharacters:\n" +
    "\t\t\t\t\t\t\t- Dwarf\t\t\t\t\t\t\t\t\t\t\t- Key\n" +
    "\t\t\t\t\t\t\t- Elf\t\t\t\t\t\t\t\t\t\t\t\t\t- Shovel\n" +
    "\t\t\t\t\t\t\t- Vampire\t\t\t\t\t\t\t\t\t\t- Hammer\n" +
    "\t\t\t\t\t\t\t- Werewolf\n" +
    "\n" +
    "\t\t\t\t\t\t\t\t* Form phrases like [Character][Verb][Direction/Object]\n" +
    "\t\t\t\t\t\t\t\t* The order of the words are not important.";

    string level1 = "\n\n\n\n" +
    "Welcome, Master Wizard! This is dungeon of god Babble Baal. Guide your four adventurers but be careful!\n" +
    "Beware, your dwarf companion hates the elf and the vampire can't stand the werewolf, although she's very nice.\n\n" +
    "Guide them, and avoid the trickery of BABBLE BAAL.\n";

    string level2 = "\n\n\n\n" +
    "\n" +
    "\t\t\t\t\tBabble Baal can modify the meaning of the words!\n" +
    "\n" +
    "His power have made some words swap their meaning. Don't make mistake while you command your team!";

    string level3 = "\n\n\n\n" +
    "\n" +
    "\t\t\t\t\tBabble Baal can make confusion deadly!\n" +
    "\n" +
    "Using his powers, the dungeon god gives you a time limit to complete the level. Be quick and precise!";

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        TheTimer = Timer.Instance;
        TheInterpret = Interpret.Instance;

        TheTimer.init(EnableTimer, TimerStart);
        TheInterpret.Shuffle(Difficulty);

        //GlobalPanelImage.color = Color.black;

        switch (LevelNumber) {

            case 3:
                StartText.text = level3;
                break;
            case 2:
                StartText.text = level2;
                break;
            case 1:
                StartText.text = level1;
                break;

            default:
                // start without panel    
                GlobalPanel.SetActive(false);
                OK_Button.SetActive(false);
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /*public void setBasicHelp() {
        //GlobalPanelImage.color = new Color(128, 128, 118, 31);
        StartText.text = basic_help;
    }*/
}
