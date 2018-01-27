using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public string Name = "Charac";
    public BasicRoom CurrentRoom;
    public int Number = 0;
    public Vector3 SlotPosition = new Vector3();
    public List<Character> WantDead = new List<Character>();

    float FightTime = -1.0f;
    Vector3 FightLocation = new Vector3();
    List<Vector3> MotionPath = new List<Vector3>();
    Vector3 MotionLocation = new Vector3();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 FightOffset = Vector3.zero;

        if (MotionPath.Count>0) {

            float WalkSpeed = 5.0f;

            Vector3 Dir = MotionPath[0] - MotionLocation;
            float Dist = Dir.magnitude;
            if ( Dist > 0.1f) {
                Dir.Normalize();
                MotionLocation += WalkSpeed * Dir * Time.deltaTime;
            } else {
                if(MotionPath.Count==1) MotionLocation = MotionPath[0];
                MotionPath.RemoveAt(0);
            }
        } else {
            if(FightTime>0.0f) {

                float Disp = 1.0f;
                Vector3 Goal = FightLocation + new Vector3(Random.Range(-Disp, Disp), Random.Range(-Disp, Disp), 0.0f);
                Vector3 Dir = Goal - MotionLocation;
                float Dist = Dir.magnitude;
                if (Dist > 0.0f) {
                    FightOffset += Dir.normalized * Random.Range(0.5f, 1.5f) * Time.deltaTime;
                }

                FightTime -= Time.deltaTime;
            }
        }

        transform.position = MotionLocation + FightOffset;
    }

    public void AddMotion(List<Vector3> Path) {
        MotionPath.AddRange(Path);
    }

    public void AddFight(Vector3 Location) {
        FightTime = 2.0f;
        FightLocation = Location;
    }

    public void FinishMotion() {
        if (MotionPath.Count > 0) {
            MotionLocation = MotionPath[MotionPath.Count-1];
            transform.position = MotionLocation;
            MotionPath.Clear();
        }
    }

    public bool IsWantingDead(Character Victim) {
        if (!Victim) return false;
        if (WantDead.Contains(Victim)) return true;
        return false;
    }

    public Vector3 GetMotionLocation() {
        return MotionLocation;
    }
}
