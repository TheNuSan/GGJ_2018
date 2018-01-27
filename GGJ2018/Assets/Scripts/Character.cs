using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public string Name = "Charac";
    public BasicRoom CurrentRoom;

    List<Vector3> MotionPath = new List<Vector3>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if (MotionPath.Count>0) {

            Vector3 Dir = MotionPath[0] - transform.position;
            float Dist = Dir.magnitude;
            if ( Dist > 0.1f) {
                Dir.Normalize();
                transform.position += Dir * Time.deltaTime;
            } else {
                MotionPath.RemoveAt(0);
            }

        }
	}

    public void AddMotion(List<Vector3> Path) {
        MotionPath.AddRange(Path);
    }
}
