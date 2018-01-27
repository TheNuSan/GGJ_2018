using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRoom : MonoBehaviour {

    public int PosX;
    public int PosY;

    public List<Character> Characs;

    public void CharacterEnter(Character Char) {
        if(Characs.Contains(Char)) {
            Debug.LogError("Character allready in room " + name);
        }
        Characs.Add(Char);
    }

    public void CharacterExit(Character Char) {
        if (!Characs.Contains(Char)) {
            Debug.LogError("Character was not in room " + name);
        }
        Characs.Remove(Char);
    }

    public Vector3 GetCenterPosition() {
        return transform.position;
    }

    public void CheckRoom() {
        if (Characs.Count != 2) return;
        for(int i=0; i<Characs.Count; ++i) {
            Character FirstChar = Characs[i];
            for (int j = i+1; j < Characs.Count; ++j) {
                Character SecondChar = Characs[j];
                if( FirstChar.IsWantingDead(SecondChar) || SecondChar.IsWantingDead(FirstChar)) {
                    Vector3 FightLocation = (FirstChar.GetMotionLocation() + SecondChar.GetMotionLocation()) / 2.0f;
                    FirstChar.AddFight(FightLocation);
                    SecondChar.AddFight(FightLocation);

                    MotionSystem.Instance.FailedMission();
                }
            }
        }
    }
}
