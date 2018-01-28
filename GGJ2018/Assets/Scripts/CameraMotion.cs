using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour {

    public GameObject Background;

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

        float ZoomSize = Mathf.Max(PartySize.x, PartySize.y)*0.5f + 1.5f;
        transform.position = PartyCenter;
        Cam.orthographicSize = ZoomSize;

        float BackScale = ZoomSize / 5.0f;
        Background.transform.localScale = new Vector3(BackScale, BackScale, BackScale);
    }
}
