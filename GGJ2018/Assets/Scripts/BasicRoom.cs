using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstacleDoor : System.Object {
    public Obstacle Type = Obstacle.Door;
    public MotionDirection Direction = MotionDirection.Invalid;
}

public class BasicRoom : MonoBehaviour {

    public int PosX;
    public int PosY;

    public enum Type { Regular, Starting, Ending}

    public Type RoomType = Type.Regular;

    public List<Character> Characs;
    public List<KeyObject> Keys;

    public List<ObstacleDoor> Doors;

    //public Dictionary<MotionDirection, Obstacle> Obstacles = new Dictionary<MotionDirection, Obstacle>();

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
        if (Characs.Count == 2) {
            for (int i = 0; i < Characs.Count; ++i) {
                Character FirstChar = Characs[i];
                for (int j = i + 1; j < Characs.Count; ++j) {
                    Character SecondChar = Characs[j];
                    if (FirstChar.IsWantingDead(SecondChar) || SecondChar.IsWantingDead(FirstChar)) {
                        Vector3 FightLocation = (FirstChar.GetMotionLocation() + SecondChar.GetMotionLocation()) / 2.0f;
                        FirstChar.AddFight(FightLocation);
                        SecondChar.AddFight(FightLocation);

                        MotionSystem.Instance.FailedMission();
                    }
                }
            }
        }

        if (RoomType == Type.Ending) {
            if(Characs.Count == MotionSystem.Instance.GetCharacterCount()) {
                MotionSystem.Instance.SuccesMission();
            }
        }
    }

    public void Reset() {
        Characs.Clear();
        Keys.Clear();
    }

    public KeyObject Pickup(string KeyName) {
        for(int i=0; i<Keys.Count; ++i) {
            KeyObject Key = Keys[i];
            if(string.Equals(KeyName, Key.KeyName, StringComparison.CurrentCultureIgnoreCase)) {
                Keys.Remove(Key);
                return Key;
            }
        }
        return null;
    }

}
