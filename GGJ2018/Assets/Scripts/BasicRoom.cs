using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstacleDoor : System.Object {
    public Obstacle Type = Obstacle.Door;
    public MotionDirection Direction = MotionDirection.Invalid;
    public bool IsActive = true;
}

public class BasicRoom : MonoBehaviour {

    public int PosX;
    public int PosY;

    public enum Type { Regular, Starting, Ending}

    public Type RoomType = Type.Regular;

    public List<Character> Characs;
    public List<KeyObject> Keys;

    public List<ObstacleDoor> Doors;
    public List<GameObject> DoorsObjects;

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
            //if(Characs.Count == MotionSystem.Instance.GetCharacterCount()) { // ALl characters
            if (Characs.Count >= 1) { // First character in the room
                MotionSystem.Instance.SuccesMission();
            }
        }
    }

    public void Reset() {
        Characs.Clear();
        Keys.Clear();
        for(int i=0; i<Doors.Count; ++i) {
            Doors[i].IsActive = true;
        }
    }
    
    public bool HasObstacle(MotionDirection Dir) {
        for (int d = 0; d < Doors.Count; ++d) {
            ObstacleDoor CurDoor = Doors[d];
            if (CurDoor.Direction == Dir && CurDoor.IsActive) {
                return true;
            }
        }
        return false;
    }

     public bool RemoveObstacle(MotionDirection Dir) {
        for (int d = 0; d < Doors.Count; ++d) {
            ObstacleDoor CurDoor = Doors[d];
            if (CurDoor.Direction == Dir) {
                bool WasActive = Doors[d].IsActive;
                Doors[d].IsActive = false;
                return WasActive;
            }
        }
        return false;
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

    void RemoveObstacleAt(int Index) {

        GameObject ObsObj = null;
        if (Index < DoorsObjects.Count) {
            ObsObj = DoorsObjects[Index];
        }

        if (ObsObj) {
            ObsObj.SetActive(false);
        }

        Doors[Index].IsActive = false;
    }

    public bool DisableObstacle(Obstacle ObsType, ref MotionDirection ObstacleDir) {

        for (int d = 0; d < Doors.Count; ++d) {
            ObstacleDoor Door = Doors[d];
            if (Door.Type == ObsType) {

                RemoveObstacleAt(d);
                ObstacleDir = Door.Direction;

                return true;
            }
        }

        return false;
    }

    public bool DisableObstacle(MotionDirection ObstacleDir) {

        for (int d = 0; d < Doors.Count; ++d) {
            ObstacleDoor Door = Doors[d];
            if (Door.Direction == ObstacleDir) {

                RemoveObstacleAt(d);
               
                return true;
            }
        }

        return false;
    }

}
