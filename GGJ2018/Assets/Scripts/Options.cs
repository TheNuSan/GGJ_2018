using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour {

    public int Difficulty = 0;
    public bool EnableTimer = false;
    public float TimerStart = 180.0F;

    Timer TheTimer;
    Interpret TheInterpret;

    // Use this for initialization
    void Start () {
        TheTimer = Timer.Instance;
        TheInterpret = Interpret.Instance;

        TheTimer.init(EnableTimer, TimerStart);
        TheInterpret.Shuffle(Difficulty);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
