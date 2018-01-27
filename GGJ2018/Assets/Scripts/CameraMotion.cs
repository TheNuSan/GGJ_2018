using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour {

    MotionSystem MS;
    Camera Cam;

	// Use this for initialization
	void Start () {
        MS = GameObject.FindObjectOfType<MotionSystem>();
        Cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 PartyCenter = Vector3.zero;
        Vector3 PartySize = Vector3.one;

        MS.GetCamPos(ref PartyCenter, ref PartySize);

        float ZoomSize = Mathf.Max(PartySize.x, PartySize.y) * 0.8f + 1.0f;
        transform.position = PartyCenter;
        Cam.orthographicSize = ZoomSize;

    }
}
