using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public string Name = "Charac";
    public BasicRoom CurrentRoom;
    public int Number = 0;
    public Vector3 SlotPosition = new Vector3();

    List<Vector3> MotionPath = new List<Vector3>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if (MotionPath.Count>0) {

            float WalkSpeed = 5.0f;

            Vector3 Dir = MotionPath[0] - transform.position;
            float Dist = Dir.magnitude;
            if ( Dist > 0.1f) {
                Dir.Normalize();
                transform.position += WalkSpeed * Dir * Time.deltaTime;
            } else {
                if(MotionPath.Count==1) transform.position = MotionPath[0];
                MotionPath.RemoveAt(0);
            }
        }
	}

    public void AddMotion(List<Vector3> Path) {
        MotionPath.AddRange(Path);
    }

    public void FinishMotion() {
        if (MotionPath.Count > 0) {
            transform.position = MotionPath[MotionPath.Count-1];
            MotionPath.Clear();
        }
    }
}
