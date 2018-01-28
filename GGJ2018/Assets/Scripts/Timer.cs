using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public static Timer Instance;
    bool EnableTimer = false;
    float StartTime = 180.0F;

    public Text CountText;

    MotionSystem theMotionSystem;

    float TimeLimit;
    float count;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        theMotionSystem = MotionSystem.Instance;
        //ResetTimer(); // now made in Motion System
        if (EnableTimer) {
            count = StartTime;
            CountText.text = getCountGUI(); ;
        } else {
            CountText.text = "∞";
        }

    }

    public void init(bool enable, float start)
    {
        EnableTimer = enable;
        StartTime = start;
    }

    public void ResetTimer()
    {
        count = StartTime;
        if (EnableTimer)
        {
            TimeLimit = Time.time + count;
            CountText.color = Color.black;
            CountText.fontSize = 56;
        }
        else
        {
            CountText.fontSize = 68;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (EnableTimer) {
            count = TimeLimit - Time.time;
            if (count <= 0.0F) {
                CountText.color = Color.red;
                CountText.text = "00:00";
                
                theMotionSystem.FailedMission();
            } else {
                CountText.text = getCountGUI();
            }
        }
    }

    string getCountGUI() {
        string res = "";
        int minutes = (int)(count + 1) / 60;
        int seconds = (int)(count + 1) % 60;

        if (minutes < 10)
            res += "0";
        res += minutes.ToString() + ":";
        
        if (seconds < 10)
            res += "0";
        res += seconds.ToString();

        return res;
    }
}
