using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MotionDirection { North, East, South, West, Invalid };

public class MotionSystem : MonoBehaviour {

    public GameObject RoomPrefab;
    public static MotionSystem Instance;
    Vector3 MapOffset = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 RoomOffset = new Vector3(2.0f, 2.0f, 0.0f);
    Vector3 CharacHeight = new Vector3(0.0f, 0.0f, -1.0f);

    public int LevelSizeX = 4;
    public int LevelSizeY = 4;
    
    List<Character> Characters;
    List<List<BasicRoom>> Rooms;

    BasicRoom StartingRoom;
    BasicRoom EndingRoom;

    bool NeedAfterMotionCheck = false;
    float MotionTimer = -1.0f;

    bool IsFailedMission;
    bool IsSuccesMission;

    GameObject LevelMaster;
    LevelSystem LevelSys;

    void Awake() {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        LevelSys = GetComponent<LevelSystem>();

        InitLevel();
    }

    void AutoFillLevel() {
        if (LevelMaster) {
            GameObject.Destroy(LevelMaster);
        }
        LevelMaster = new GameObject("LevelMaster");
        LevelMaster.transform.position = Vector3.zero;

        Rooms = new List<List<BasicRoom>>();
        for (int i = 0; i < LevelSizeX; ++i) {
            Rooms.Add(new List<BasicRoom>());
            for (int j = 0; j < LevelSizeY; ++j) {

                GameObject RoomObject = GameObject.Instantiate(RoomPrefab, LevelMaster.transform);
                BasicRoom NewRoom = RoomObject.GetComponent<BasicRoom>();

                RoomObject.transform.position = new Vector3(RoomOffset.x * (float)i, RoomOffset.y * (float)j, 10.0f);
                RoomObject.transform.position += MapOffset;

                RoomObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                NewRoom.PosX = i;
                NewRoom.PosY = j;

                Rooms[i].Add(NewRoom);
            }
        }
    }

    void LoadLevelFromScene() {

        Rooms = new List<List<BasicRoom>>();
        for (int i = 0; i < LevelSizeX; ++i) {
            Rooms.Add(new List<BasicRoom>());
            for (int j = 0; j < LevelSizeY; ++j) {
                Rooms[i].Add(null);
            }
        }

        GameObject[] RoomList = GameObject.FindGameObjectsWithTag("Room");
        for (int i = 0; i < RoomList.Length; ++i) {
            BasicRoom Room = RoomList[i].GetComponent<BasicRoom>();
            if (Room) {
                Room.Reset();

                Room.PosX = (int)Mathf.Floor(Room.transform.position.x / 2.0f + 0.5f);
                Room.PosY = (int)Mathf.Floor(Room.transform.position.y / 2.0f + 0.5f);

                if (!ValidGridPos(Room.PosX, Room.PosY)) {
                    GameObject.Destroy(Room.gameObject);
                    Debug.LogError("Room outside " + Room.name);
                    continue;
                }

                if (Rooms[Room.PosX][Room.PosY] != null) {
                    GameObject.Destroy(Room.gameObject);
                    Debug.LogError("Room already there " + Room.name);
                    continue;
                }

                Room.transform.position = new Vector3(RoomOffset.x * (float)Room.PosX, RoomOffset.y * (float)Room.PosY, 10.0f);
                Room.transform.position += MapOffset;
                Room.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                if (Room.RoomType == BasicRoom.Type.Starting) {
                    StartingRoom = Room;
                }
                if (Room.RoomType == BasicRoom.Type.Ending) {
                    EndingRoom = Room;
                }

                Rooms[Room.PosX][Room.PosY] = Room;
            }
        }

        GameObject[] KeyList = GameObject.FindGameObjectsWithTag("KeyObject");
        for (int i = 0; i < KeyList.Length; ++i) {
            KeyObject Key = KeyList[i].GetComponent<KeyObject>();
            if (Key) {
                Key.Reset();

                Key.PosX = (int)Mathf.Floor(Key.transform.position.x / 2.0f + 0.5f);
                Key.PosY = (int)Mathf.Floor(Key.transform.position.y / 2.0f + 0.5f);

                if (!ValidGridPos(Key.PosX, Key.PosY)) {
                    GameObject.Destroy(Key.gameObject);
                    Debug.LogError("Key outside " + Key.name);
                    continue;
                }

                BasicRoom OwnerRoom = Rooms[Key.PosX][Key.PosY];
                if (!OwnerRoom) {
                    GameObject.Destroy(Key.gameObject);
                    Debug.LogError("Key no room " + Key.name);
                    continue;
                }

                Key.OwnerRoom = OwnerRoom;
                OwnerRoom.Keys.Add(Key);
            }
        }
    }

    void InitLevel() {

        IsFailedMission = false;
        IsSuccesMission = false;

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
        
        LoadLevelFromScene();
        
        if(!StartingRoom) {
            Debug.LogError("No starting room found");
        }
        if (!EndingRoom) {
            Debug.LogError("No ending room found");
        }
        for (int i = 0; i < Characters.Count; ++i) {
            Character Char = Characters[i];
            PlaceCharacter(Char, StartingRoom, true);
            Char.FinishMotion();
        }

        //LevelSys.GotToNextLevel();

        StartCoroutine(FakeParty());

    }

    private IEnumerator FakeParty() {

        Debug.Log("Start Party");

        yield return new WaitForSeconds(1f);
        MoveCharacterAlongDirection("ELF", "eAsT");

        yield return new WaitForSeconds(1f);
        PickUpObject("ELF", "key");

        yield return new WaitForSeconds(1f);
        MoveCharacterAlongDirection("Dwarf", "North");

        yield return new WaitForSeconds(1f);
        MoveCharacterAlongDirection("ELF", "north");

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
            if(string.Equals(Char.name, CharName, StringComparison.CurrentCultureIgnoreCase)) {
                return Char;
            }
        }
        return null;
    }

    public MotionDirection GetDirection(string DirName) {
        foreach (MotionDirection Dir in Enum.GetValues(typeof(MotionDirection))) {
            string EnumName = Enum.GetName(typeof(MotionDirection), Dir);
            if (string.Equals(EnumName, DirName, StringComparison.CurrentCultureIgnoreCase)) {
                return Dir;
            }
        }
        return MotionDirection.Invalid;
    }

    public bool MoveCharacterAlongDirection(string CharName, string DirName) {
        //Debug.Log("Try move " + CharName + " to " + DirName);
        Character Char = GetCharacter(CharName);
        MotionDirection Dir = GetDirection(DirName);
        if(!Char || Dir == MotionDirection.Invalid) {
            return false;
        }
        return MoveCharacterAlongDirection(Char, Dir);
    }

    public bool ValidGridPos(int i, int j) {
        if (i < 0 || i >= LevelSizeX || j < 0 || j >= LevelSizeY) return false;
        return true;
    }

    public bool MoveCharacterAlongDirection(Character Char, MotionDirection Dir) {

        if(!CanRecieveCommand()) {
            return false;
        }

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

        if(!ValidGridPos(NewCaseX, NewCaseY)) {
            return false;
        }

        BasicRoom OtherRoom = Rooms[NewCaseX][NewCaseY];
        if(!OtherRoom) {
            return false;
        }

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

    public bool PickUpObject(string CharName, string KeyName) {

        Character Char = GetCharacter(CharName);
        if (!Char || !Char.CurrentRoom) return false;
        if (!Char.CanPickUp()) return false;
        BasicRoom CurrentRoom = Char.CurrentRoom;
        KeyObject Key = CurrentRoom.Pickup(KeyName);
        if (!Key) return false;

        Char.PickUp(Key);
        return true;
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
                    if (Room) {
                        Room.CheckRoom();
                    }
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
        if (!IsFailedMission) {
            IsFailedMission = true;
            StartCoroutine(RestartTimer());
        }
    }

    private IEnumerator NextLevelTimer() {
        yield return new WaitForSeconds(1f);
        //InitLevel();
        LevelSys.GotToNextLevel();
    }

    public void SuccesMission() {
        if(!IsSuccesMission) {
            IsSuccesMission = true;
            StartCoroutine(NextLevelTimer());
        }
    }

    public bool CanRecieveCommand() {
        return !IsFailedMission && !IsSuccesMission;
    }

    public int GetCharacterCount() { return Characters.Count; }
}
