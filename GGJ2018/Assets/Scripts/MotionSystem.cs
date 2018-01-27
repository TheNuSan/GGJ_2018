using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionSystem : MonoBehaviour {

    public GameObject RoomPrefab;
    public Vector3 MapOffset = new Vector3(-3.0f, -3.0f, 0.0f);
    public Vector3 RoomOffset = new Vector3(2.1f, 2.1f, 0.0f);

    int LevelSizeX = 4;
    int LevelSizeY = 4;
    
    public List<Character> AllCharacters;

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
        for (int i=0; i<AllCharacters.Count; ++i) {
            Characters.Add(AllCharacters[i]);
        }

        Rooms = new List<List<BasicRoom>>();
        for (int i=0;i<LevelSizeX; ++i) {
            Rooms.Add(new List<BasicRoom>());
            for (int j = 0; j < LevelSizeY; ++j) {

                GameObject RoomObject = GameObject.Instantiate(RoomPrefab);
                BasicRoom NewRoom = RoomObject.GetComponent<BasicRoom>();

                RoomObject.transform.position = new Vector3(RoomOffset.x*(float)i, RoomOffset.y*(float)j, 10.0f);
                RoomObject.transform.position += MapOffset;
                NewRoom.PosX = i;
                NewRoom.PosY = j;

                Rooms[i].Add(NewRoom);
            }
        }

        StartingRoom = Rooms[0][0];
        EndingRoom = Rooms[LevelSizeX - 1][LevelSizeY - 1];

        for (int i = 0; i < Characters.Count; ++i) {
            PlaceCharacter(Characters[i], StartingRoom);
        }

    }

    void PlaceCharacter(Character Char, BasicRoom Room) {

        if(Char.CurrentRoom) {
            Char.CurrentRoom.CharacterExit(Char);
        }

        if(Room) {

            Room.CharacterEnter(Char);

        } else {
            Debug.LogError("Missing room while placing " + Char.name);
        }

        

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
