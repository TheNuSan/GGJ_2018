using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MotionDirection { North, East, South, West, Invalid };

public class MotionSystem : MonoBehaviour {

    public GameObject RoomPrefab;
    Vector3 MapOffset = new Vector3(-3.0f, -3.0f, 0.0f);
    Vector3 RoomOffset = new Vector3(2.0f, 2.0f, 0.0f);
    Vector3 CharacHeight = new Vector3(0.0f, 0.0f, -1.0f);

    int LevelSizeX = 4;
    int LevelSizeY = 4;
    
    List<Character> Characters;
    List<List<BasicRoom>> Rooms;

    public BasicRoom StartingRoom;
    public BasicRoom EndingRoom;
    
    // Use this for initialization
    void Start () {

        InitLevel();

    }

    void InitLevel() {

        LevelSizeX = 4;
        LevelSizeY = 4;

        Characters = new List<Character>();
        GameObject[] CharacList = GameObject.FindGameObjectsWithTag("Character");
        for(int i=0; i<CharacList.Length; ++i) {
            Character Char = CharacList[i].GetComponent<Character>();
            if (Char) {
                Char.Number = i;
                Characters.Add(Char);
            }
        }

        // update slot position
        for(int i=0; i<Characters.Count; ++i) {
            Character Char = Characters[i];
            float Dist = 0.5f;
            float Alpha = Char.Number * Mathf.PI * 2.0f / (float)Characters.Count;
            Char.SlotPosition = new Vector3(Mathf.Cos(Alpha) * Dist, Mathf.Sin(Alpha) * Dist, 0.0f);
        }

        Rooms = new List<List<BasicRoom>>();
        for (int i=0;i<LevelSizeX; ++i) {
            Rooms.Add(new List<BasicRoom>());
            for (int j = 0; j < LevelSizeY; ++j) {

                GameObject RoomObject = GameObject.Instantiate(RoomPrefab);
                BasicRoom NewRoom = RoomObject.GetComponent<BasicRoom>();

                RoomObject.transform.position = new Vector3(RoomOffset.x*(float)i, RoomOffset.y*(float)j, 10.0f);
                RoomObject.transform.position += MapOffset;
                RoomObject.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                NewRoom.PosX = i;
                NewRoom.PosY = j;

                Rooms[i].Add(NewRoom);
            }
        }

        StartingRoom = Rooms[0][0];
        EndingRoom = Rooms[LevelSizeX - 1][LevelSizeY - 1];

        for (int i = 0; i < Characters.Count; ++i) {
            Character Char = Characters[i];
            PlaceCharacter(Char, StartingRoom);
            Char.FinishMotion();
        }

        StartCoroutine(FakeParty());
        
    }

    private IEnumerator FakeParty() {

        yield return new WaitForSeconds(1f);

        MoveCharacterAlongDirection("Elf", "East");

        yield return new WaitForSeconds(1f);

        MoveCharacterAlongDirection("Dwarf", "North");
    }

    public Character GetCharacter(string CharName) {
        for (int i = 0; i < Characters.Count; ++i) {
            Character Char = Characters[i];
            if(Char.name == CharName) {
                return Char;
            }
        }
        return null;
    }

    public MotionDirection GetDirection(string DirName) {
        foreach (MotionDirection Dir in Enum.GetValues(typeof(MotionDirection))) {
            if (Enum.GetName(typeof(MotionDirection), Dir) == DirName) {
                return Dir;
            }
        }
        return MotionDirection.Invalid;
    }

    public bool MoveCharacterAlongDirection(string CharName, string DirName) {
        Character Char = GetCharacter(CharName);
        MotionDirection Dir = GetDirection(DirName);
        if(!Char || Dir == MotionDirection.Invalid) {
            return false;
        }
        return MoveCharacterAlongDirection(Char, Dir);
    }

    public bool MoveCharacterAlongDirection(Character Char, MotionDirection Dir) {

        int OffsetX = 0;
        int OffsetY = 0;
        switch(Dir) {
            case MotionDirection.North: OffsetY = 1; break;
            case MotionDirection.South: OffsetY = -1; break;
            case MotionDirection.East: OffsetX = 1; break;
            case MotionDirection.West: OffsetX = -1; break;
            case MotionDirection.Invalid: return false; break;
        }

        BasicRoom Room = Char.CurrentRoom;
        int NewCaseX = Room.PosX + OffsetX;
        int NewCaseY = Room.PosY + OffsetY;

        if(NewCaseX<0 || NewCaseX>=LevelSizeX || NewCaseY<0 || NewCaseY>=LevelSizeY) {
            return false;
        }

        BasicRoom OtherRoom = Rooms[NewCaseX][NewCaseY];
        PlaceCharacter(Char, OtherRoom);

        return true;
    }

    void PlaceCharacter(Character Char, BasicRoom Room) {

        BasicRoom OldRoom = Char.CurrentRoom;
        if(Char.CurrentRoom) {
            Char.CurrentRoom.CharacterExit(Char);
        }

        if(Room) {
            Char.CurrentRoom = Room;
            Room.CharacterEnter(Char);

        } else {
            Char.CurrentRoom = null;
            Debug.LogError("Missing room while placing " + Char.name);
        }

        //Char.transform.position = Room.transform.position + Char.SlotPosition + CharacHeight;
        List<Vector3> Path = new List<Vector3>();
        if(OldRoom) Path.Add(OldRoom.GetCenterPosition() + CharacHeight);
        if (Char.CurrentRoom) {
            Path.Add(Room.GetCenterPosition() + CharacHeight);
            Path.Add(Char.CurrentRoom.GetCenterPosition() + CharacHeight);
            Path.Add(Char.CurrentRoom.GetCenterPosition() + Char.SlotPosition + CharacHeight);
        }
        Char.FinishMotion();
        Char.AddMotion(Path);
    }

    public void FinishTurn() {

        for (int i = 0; i < LevelSizeX; ++i) {
            for (int j = 0; j < LevelSizeY; ++j) {
                BasicRoom Room = Rooms[i][j];
                Room.CheckRoom();
            }
        }

    }    
	
	// Update is called once per frame
	void Update () {
		
	}
}
