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

    string basic_help = "" +
    "Welcome, Master Wizard! This is dungeon of god Babble Baal. Guide your four adventurers but be careful! Some of them don't get along with each other very well... " +
    "Guide them, and avoid the trickery of BABBLE BAAL.\n" +
    "\n" +
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

    string level2 = "" +
    "\n" +
    "\t\t\t\t\t\t\t\tBabble Baal can modify the sense of the words!\n" +
    "\n" +
    "His power have made some words swap their meaning. Don't make mistake while you command!";

    string level3 = "" +
    "\n" +
    "\t\t\t\t\t\t\t\tBabble Baal can make confusion deadly!\n" +
    "\n" +
    "Using his powets, the dungeon god gives you a time limit to complete the level. Be quick and precise!";

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

        switch (LevelNumber) {

            case 3:
                StartText.text = level3;
                break;
            case 2:
                StartText.text = level2;
                break;
            case 1:
                setBasicHelp();
                break;

            default:
                setBasicHelp();
                // start without panel
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setBasicHelp() {
        StartText.text = basic_help;
    }
}
