using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Obstacle { Boulder, Wall, Door }

public class KeyObject : MonoBehaviour {

    public Obstacle WillOpen = Obstacle.Door;

    Vector3 InitialPosition;

    public int PosX = 0;
    public int PosY = 0;

    public BasicRoom OwnerRoom;
    
    public string KeyName = "Key";

    // Use this for initialization
    void Awake () {
        InitialPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Reset() {
        OwnerRoom = null;
        transform.position = InitialPosition;
    }
}
