using UnityEngine;

public class KeepObjectFacingCamera : MonoBehaviour {

    GameObject camera;
    // Use this for initialization
    void Start() {
        camera = GameObject.Find("HoloLensCamera");
    }

    // Update is called once per frame
    void Update() {
        transform.rotation = camera.transform.rotation;
    }
}
