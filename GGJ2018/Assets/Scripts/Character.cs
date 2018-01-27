using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public string Name = "Charac";
    public BasicRoom CurrentRoom;
    public int Number = 0;
    public Vector3 SlotPosition = new Vector3();
    public List<Character> WantDead = new List<Character>();

    KeyObject InHand;

    SpriteRenderer SR;

    float FightTime;
    float SubFightTime;
    Vector3 FightLocation;
    Vector3 FightOffset;
    List<Vector3> MotionPath;
    Vector3 MotionLocation;

    // Use this for initialization
    void Start () {
        SR = GetComponent<SpriteRenderer>();
    }

    public void Reset() {
        FightTime = -1.0f;
        SubFightTime = 0.1f;
        FightLocation = new Vector3();
        FightOffset = Vector3.zero;
        MotionPath = new List<Vector3>();
        MotionLocation = new Vector3();
        transform.rotation = Quaternion.identity;
        CurrentRoom = null;
        InHand = null;
    }
	
	// Update is called once per frame
	void Update () {

        
        if (MotionPath.Count>0) {

            float WalkSpeed = 5.0f;

            Vector3 Dir = MotionPath[0] - MotionLocation;
            float Dist = Dir.magnitude;
            if ( Dist > 0.1f) {
                Dir.Normalize();
                MotionLocation += WalkSpeed * Dir * Time.deltaTime;
                if(Dir.x>0.1f) {
                    SR.flipX = true;
                }
                if (Dir.x < -0.1f) {
                    SR.flipX = false;
                }
            } else {
                if(MotionPath.Count==1) MotionLocation = MotionPath[0];
                MotionPath.RemoveAt(0);
            }
        } else {
            if(FightTime>0.0f) {

                float Disp = 0.2f;
                Vector3 Dir = FightLocation - transform.position;
                float Dist = Dir.magnitude;
                if (Dist > 0.0f) {
                    FightOffset += Dir.normalized * Time.deltaTime;
                }
                if(SubFightTime>0.0f) {
                    SubFightTime -= Time.deltaTime;
                } else {
                    FightOffset += new Vector3(Random.Range(-Disp, Disp), Random.Range(-Disp, Disp), 0.0f);
                    SubFightTime = Random.Range(0.1f, 0.2f);
                }

                FightTime -= Time.deltaTime;

                if(FightTime<0.0f) {
                    transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                }
            } else {
                FightOffset = Vector3.zero;
            }
        }

        transform.position = MotionLocation + FightOffset;

        if (InHand) {
            Vector3 Dir = transform.position - InHand.transform.position;
            float Dist = Dir.magnitude;
            if (Dist > 0.1f) {
                Dir.Normalize();
                InHand.transform.position += 5.0f * Dir * Time.deltaTime;
            }
        }
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

    public bool CanPickUp() {
        if (InHand) return false;
        return true;
    }

    public bool PickUp(KeyObject NewObj) {
        if (InHand) return false;
        InHand = NewObj;
        return true;
    }
}
