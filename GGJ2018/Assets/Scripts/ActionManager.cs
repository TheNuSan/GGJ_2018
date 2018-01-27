using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionManager : MonoBehaviour {

    public MotionSystem theMotionSystem = MotionSystem.Instance;
    public InterpretError MyInterpretError;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool PerformAction(List<string> word_list) {

        bool res = false;

        // select action by verb    
        switch (word_list[1]) {
            
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
