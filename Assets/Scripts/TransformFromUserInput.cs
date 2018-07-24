using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System;
using UnityEngine;

public class TransformFromUserInput : XboxControllerHandlerBase {

    [SerializeField]
    private float movementSpeedMultiplier = 0.25f;

    [SerializeField]
    private float rotationSpeedMultiplier = 0.25f;

    [SerializeField]
    private float scaleSpeedMultiplier = 0.25f;

    [SerializeField]
    private XboxControllerMappingTypes resetButton = XboxControllerMappingTypes.XboxY;

    private Vector3 initialRotation;
    private Vector3 initialPosition;
    private Vector3 initialScale;
    private Vector3 newPosition;
    private Vector3 newScale;

    public GameObject Camera = null;
    public GameObject UI = null;

    public Boolean UseKeyboard = true;
    public Boolean UseJoystick = false;

    public Boolean devBuild = true; // TODO !!!! Changer à false à la fin du dev , decallage du a l'input de la camera dans unity

    private bool locked = true;

    // Use this for initialization
    void Start() {
        if (Camera == null) {
            Camera = GameObject.Find("HoloLensCamera");
        }
        if (UI == null) {
            UI = GameObject.Find("UI");
        }
        initialPosition = transform.position;
        initialRotation = Vector3.zero;
        initialScale = transform.localScale;
    }

    public override void OnSourceLost(SourceStateEventData eventData) {
        //Debug.LogFormat("Joystick {0} with id: \"{1}\" Disconnected", GamePadName, eventData.SourceId);
        base.OnSourceLost(eventData);
        //debugText.text = "No Controller Connected";
    }

    // Update is called once per frame
    public void Update() {
        if (!UseKeyboard) {
            Debug.Log("Keyboard use not enabled");
            return;
        }
        //if (isLocked) {
        //Debug.Log("Transform is locked");
        // return;
        //}
        newPosition = Vector3.zero;
        newScale = transform.localScale;

        // Position sur "tfgh" 
        if (devBuild) {
            DevBuildKeyMap();
        }
        else {
            EndBuildKeyMap();
        }

    }

    private void EndBuildKeyMap() {
        // Position de l'objet
        if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) { 
            if (Input.GetKey("z")) {
                newPosition.y += 1 * movementSpeedMultiplier;
            }
            if (Input.GetKey("q")) {
                newPosition.x -= 1 * movementSpeedMultiplier;
            }
            if (Input.GetKey("s")) {
                newPosition.y -= 1 * movementSpeedMultiplier;
            }
            if (Input.GetKey("d")) {
                newPosition.x += 1 * movementSpeedMultiplier;
            }
            if (Input.GetKey("c")) {
                newPosition.z += 1 * movementSpeedMultiplier;
            }
            if (Input.GetKey("v")) {
                newPosition.z -= 1 * movementSpeedMultiplier;
            }
            transform.position += newPosition;
        }

        // Orientation
        if (Input.GetKey("a")) {
            transform.RotateAround(transform.position, Vector3.right, -1 * rotationSpeedMultiplier);
        }
        if (Input.GetKey("e")) {
            transform.RotateAround(transform.position, Vector3.right, 1 * rotationSpeedMultiplier);
        }
        if (Input.GetKey("r")) {
            transform.RotateAround(transform.position, Vector3.up, 1 * rotationSpeedMultiplier);
        }
        if (Input.GetKey("f")) {
            transform.RotateAround(transform.position, Vector3.up, -1 * rotationSpeedMultiplier);
        }

        // Scale
        if (Input.GetKey("t")) {
            newScale.x -= 1 * scaleSpeedMultiplier;
            newScale.y -= 1 * scaleSpeedMultiplier;
            newScale.z -= 1 * scaleSpeedMultiplier;
        }
        if (Input.GetKey("g")) {
            newScale.x += 1 * scaleSpeedMultiplier;
            newScale.y += 1 * scaleSpeedMultiplier;
            newScale.z += 1 * scaleSpeedMultiplier;
        }
        transform.localScale = newScale;

        // Reset
        if (Input.GetKey("y")) {
            ResetObjet();
        }
    }

    private void DevBuildKeyMap() {
        if (!(Input.GetKey(KeyCode.LeftShift ) || Input.GetKey(KeyCode.RightShift))) {
            // Position de l'objet
            if (Input.GetKey("t")) {
                newPosition.y += 1 * movementSpeedMultiplier;
            }
            if (Input.GetKey("f")) {
                newPosition.x -= 1 * movementSpeedMultiplier;
            }
            if (Input.GetKey("g")) {
                newPosition.y -= 1 * movementSpeedMultiplier;
            }
            if (Input.GetKey("h")) {
                newPosition.x += 1 * movementSpeedMultiplier;
            }
            if (Input.GetKey("w")) {
                newPosition.z += 1 * movementSpeedMultiplier;
            }
            if (Input.GetKey("x")) {
                newPosition.z -= 1 * movementSpeedMultiplier;
            }
            transform.position += newPosition;

            // Orientation sur ry uj

            // Orientation
            if (Input.GetKey("r")) {
                transform.RotateAround(transform.position, Vector3.up, 1 * rotationSpeedMultiplier);
            }
            if (Input.GetKey("y")) {
                transform.RotateAround(transform.position, Vector3.up, -1 * rotationSpeedMultiplier);
            }
            if (Input.GetKey("u")) {
                transform.RotateAround(transform.position, Vector3.right, -1 * rotationSpeedMultiplier);
            }
            if (Input.GetKey("j")) {
                transform.RotateAround(transform.position, Vector3.right, 1 * rotationSpeedMultiplier);
            }


            // Scale
            if (Input.GetKey("i")) {
                newScale.x -= 1 * scaleSpeedMultiplier;
                newScale.y -= 1 * scaleSpeedMultiplier;
                newScale.z -= 1 * scaleSpeedMultiplier;
            }
            if (Input.GetKey("k")) {
                newScale.x += 1 * scaleSpeedMultiplier;
                newScale.y += 1 * scaleSpeedMultiplier;
                newScale.z += 1 * scaleSpeedMultiplier;
            }
            transform.localScale = newScale;

            // Reset
            if (Input.GetKey("o")) {
                ResetObjet();
            }
        }
    }

    private void ResetObjet() {
        transform.position = initialPosition;
        transform.rotation = Quaternion.Euler(initialRotation);
        transform.localScale = initialScale;
    }


    // todo -> eventuellement rajouter dans un autre fichier le controle de l'axisviewer si on appuie sur une touche
    public override void OnXboxInputUpdate(XboxControllerEventData eventData) {
        if (!UseJoystick) {
            Debug.Log("Joystick use not enabled");
            return;
        }

        if (string.IsNullOrEmpty(GamePadName)) {
            Debug.LogFormat("Joystick {0} with id: \"{1}\" Connected", eventData.GamePadName, eventData.SourceId);
        }

        base.OnXboxInputUpdate(eventData);
        //if (isLocked) {
        //Debug.Log("Transform is locked");
        // return;
        //}
        newPosition = Vector3.zero;
        newScale = transform.localScale;

        // position
        newPosition.x += eventData.XboxLeftStickHorizontalAxis * movementSpeedMultiplier;
        newPosition.y += eventData.XboxLeftStickVerticalAxis * movementSpeedMultiplier;
        newPosition.z += eventData.XboxSharedTriggerAxis * movementSpeedMultiplier;
        transform.position += newPosition;

        // Axe X
        transform.RotateAround(transform.position, Vector3.right, eventData.XboxRightStickVerticalAxis * rotationSpeedMultiplier);

        // Axe Y
        transform.RotateAround(transform.position, Vector3.up, eventData.XboxRightStickHorizontalAxis * rotationSpeedMultiplier);


        // scale
        if (eventData.XboxLeftBumper_Down) {
            newScale.x -= 1 * scaleSpeedMultiplier;
            newScale.y -= 1 * scaleSpeedMultiplier;
            newScale.z -= 1 * scaleSpeedMultiplier;
        }
        if (eventData.XboxRightBumper_Down) {
            newScale.x += 1 * scaleSpeedMultiplier;
            newScale.y += 1 * scaleSpeedMultiplier;
            newScale.z += 1 * scaleSpeedMultiplier;
        }

        transform.localScale = newScale;

        if (XboxControllerMapping.GetButton_Up(resetButton, eventData)) {
            ResetObjet();
        }
    }

    public void LockTransform() {
        if (locked) {
            if (WorldAnchorManager.Instance != null) {
                foreach (Transform child in transform) { // 1 seul enfant à tout moment
                    locked = false;
                    WorldAnchorManager.Instance.RemoveAnchor(child.gameObject);
                }
            }

        }
        else {
            if (WorldAnchorManager.Instance != null) {
                foreach (Transform child in transform) { // 1 seul enfant à tout moment
                    locked = true;
                    WorldAnchorManager.Instance.AttachAnchor(child.gameObject);
                }
            }
        }

    }
}
