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
}
