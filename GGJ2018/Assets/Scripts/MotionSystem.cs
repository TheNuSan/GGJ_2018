using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MotionDirection { North, East, South, West, Invalid };

public class MotionSystem : MonoBehaviour {

    public GameObject RoomPrefab;
    public static MotionSystem Instance;
    Vector3 MapOffset = new Vector3(-3.0f, -3.0f, 0.0f);
    Vector3 RoomOffset = new Vector3(2.0f, 2.0f, 0.0f);
    Vector3 CharacHeight = new Vector3(0.0f, 0.0f, -1.0f);

    int LevelSizeX = 4;
    int LevelSizeY = 4;
    
    List<Character> Characters;
    List<List<BasicRoom>> Rooms;

    public BasicRoom StartingRoom;
    public BasicRoom EndingRoom;

    bool NeedAfterMotionCheck = false;
    float MotionTimer = -1.0f;

    bool IsFailedMission;

    GameObject LevelMaster;

    void Awake() {
        Instance = this;
    }

    // Use this for initialization
    void Start () {

        InitLevel();

    }

    void InitLevel() {

        LevelSizeX = 4;
        LevelSizeY = 4;

        IsFailedMission = false;

        Characters = new List<Character>();
        GameObject[] CharacList = GameObject.FindGameObjectsWithTag("Character");
        for(int i=0; i<CharacList.Length; ++i) {
            Character Char = CharacList[i].GetComponent<Character>();
            if (Char) {
                Char.Reset();
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

        if(LevelMaster) {
            GameObject.Destroy(LevelMaster);
        }
        LevelMaster = new GameObject("LevelMaster");
        LevelMaster.transform.position = Vector3.zero;

        Rooms = new List<List<BasicRoom>>();
        for (int i=0;i<LevelSizeX; ++i) {
            Rooms.Add(new List<BasicRoom>());
            for (int j = 0; j < LevelSizeY; ++j) {

                GameObject RoomObject = GameObject.Instantiate(RoomPrefab, LevelMaster.transform);
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
            PlaceCharacter(Char, StartingRoom, true);
            Char.FinishMotion();
        }

        StartCoroutine(FakeParty());
        
    }

    private IEnumerator FakeParty() {

        yield return new WaitForSeconds(1f);
        MoveCharacterAlongDirection("Elf", "East");

        yield return new WaitForSeconds(1f);
        MoveCharacterAlongDirection("Dwarf", "North");

        /*

        yield return new WaitForSeconds(1f);
        MoveCharacterAlongDirection("Dwarf", "North");

        yield return new WaitForSeconds(1f);
        MoveCharacterAlongDirection("Dwarf", "North");

        yield return new WaitForSeconds(1f);
        MoveCharacterAlongDirection("Elf", "East");

        yield return new WaitForSeconds(1f);
        MoveCharacterAlongDirection("Elf", "North");

        yield return new WaitForSeconds(1f);
        MoveCharacterAlongDirection("Elf", "North");

        yield return new WaitForSeconds(1f);
        MoveCharacterAlongDirection("Werewolf", "North");

        yield return new WaitForSeconds(1f);
        MoveCharacterAlongDirection("Vampire", "North");

        yield return new WaitForSeconds(1f);
        MoveCharacterAlongDirection("Werewolf", "North");

        yield return new WaitForSeconds(1f);
        MoveCharacterAlongDirection("Vampire", "North");

    */

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

    void PlaceCharacter(Character Char, BasicRoom Room, bool FirstPlacing = false) {

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
        if (!FirstPlacing) {
            FinishTurn();
            StartMotionTimer();
        }
        Char.AddMotion(Path);
    }

    private void StartMotionTimer() {
        NeedAfterMotionCheck = true;
        MotionTimer = 0.7f;
    }

    public void FinishTurn() {

        if(NeedAfterMotionCheck) {

            for (int i = 0; i < LevelSizeX; ++i) {
                for (int j = 0; j < LevelSizeY; ++j) {
                    BasicRoom Room = Rooms[i][j];
                    Room.CheckRoom();
                }
            }

        }

        NeedAfterMotionCheck = false;
    }    

    public void GetCamPos(ref Vector3 RefPosition, ref Vector3 RefSize) {
        
        Bounds Boun = new Bounds();
        for (int i = 0; i < Characters.Count; ++i) {
            Character Char = Characters[i];
            Boun.Encapsulate(Char.GetMotionLocation());
        }

        RefPosition = Boun.center;
        RefSize = Boun.size;
    }

    // Update is called once per frame
    void Update () {
		if(NeedAfterMotionCheck) {
            if (MotionTimer > 0.0f) {
                MotionTimer -= Time.deltaTime;
            } else {
                FinishTurn();
                NeedAfterMotionCheck = false;
            }
        }
	}

    private IEnumerator RestartTimer() {
        yield return new WaitForSeconds(5f);
        InitLevel();
    }

    public void FailedMission() {
        IsFailedMission = true;

        StartCoroutine(RestartTimer());
    }

}
