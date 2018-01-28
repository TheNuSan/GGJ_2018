using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionManager : MonoBehaviour {

    MotionSystem theMotionSystem;
    public InterpretError MyInterpretError;

    // Use this for initialization
    void Start () {
        theMotionSystem = MotionSystem.Instance;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool PerformAction(List<string> word_list) {

        bool res = false;

        // select action by verb    
        switch (word_list[1]) {

            case "pick":
                //Debug.Log(word_list[0] +" is trying to pick up " + word_list[2]);
                res = theMotionSystem.PickUpObject(word_list[0], word_list[2]);
                //if (!res)

                break;

            case "use":
                Debug.Log(word_list[0] + " is trying to use " + word_list[2]);
                break;

            case "move":
            default:
                res = theMotionSystem.MoveCharacterAlongDirection(word_list[0], word_list[2]);
                if (!res)
                    MyInterpretError.MoveError(word_list[0], word_list[2]);
                break;
        }

        return res;
    }
}
