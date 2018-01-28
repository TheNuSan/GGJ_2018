using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Obstacle { Boulder, Wall, Door, Invalid }

public class KeyObject : MonoBehaviour {

    public Obstacle WillOpen = Obstacle.Door;

    Vector3 InitialPosition;

    public int PosX = 0;
    public int PosY = 0;

    public BasicRoom OwnerRoom;
    
    public string KeyName = "Key";

    public Vector3 UseLocation = Vector3.zero;
    float UseTimer = -1.0f;

    // Use this for initialization
    void Awake () {
        InitialPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
		if(UseTimer>0.0f) {
            UseTimer -= Time.deltaTime;

            Vector3 Dir = UseLocation - transform.position;
            float Dist = Dir.magnitude;
            if (Dist > 0.1f) {
                Dir.Normalize();
                transform.position += 5.0f * Dir * Time.deltaTime;
            }

            if(UseTimer<=0.0f) {
                gameObject.SetActive(false);
            }
        }
	}

    public void Use(Vector3 Location) {
        UseTimer = 1.0f;
        UseLocation = Location;
    }

    public void Reset() {
        OwnerRoom = null;
        transform.position = InitialPosition;
        gameObject.SetActive(true);
        UseTimer = -1.0f;
    }
}
