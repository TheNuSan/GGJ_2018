﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpretError : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TooFewWords() {
        Debug.Log("ERROR : Too few words.");
    }

    public void SyntaxError(string word) {
        Debug.Log("ERROR : Word " + word + " is unknown.");
    }
}