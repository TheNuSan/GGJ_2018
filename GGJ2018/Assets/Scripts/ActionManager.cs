using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionManager : MonoBehaviour {

    public MotionSystem theMotionSystem = MotionSystem.Instance;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PerformAction(List<string> word_list) {
        
        // select action by verb    
        switch (word_list[1]) {

            case "move":
            default:
                theMotionSystem.MoveCharacterAlongDirection(word_list[0], word_list[2]);
                break;
        }

    }
}
