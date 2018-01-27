using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSystem : MonoBehaviour {

    public InputField TextField;
    public Interpret myInterpret;
    public InterpretError myInterpretError;
    

    // Use this for initialization
    void Start () {
        myInterpret.init();
        myInterpret.Shuffle(0);
        //myInterpret.Shuffle(4);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AcceptInput() {
        Debug.Log(TextField.text);
        string myString = TextField.text;
        string[] words = myString.Split(" ".ToCharArray());
        
        // to few words error
        if (words.Length < Interpret.MinWords)
        {
            myInterpretError.TooFewWords();
            return;
        }

        myInterpret.DoInterpret(words);
    }

    
}
