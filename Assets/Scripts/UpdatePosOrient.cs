﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePosOrient : MonoBehaviour {


    private PlStreamCustom plStream = null;
    [SerializeField]
    private bool decallageByGazeStart = false;

    [SerializeField]
    private bool devBuid = true;

    [SerializeField]
    private int signe = 1;
    [SerializeField]
    private Vector3 decallageClavier = new Vector3(0, 0, 0);

    public Vector3 decallageByCalibragePos = new Vector3(0, 0, 0);
    public Vector3 totalDecallage = new Vector3(0, 0, 0);

    [SerializeField]
    private float reduceMovementCoef = 0.01f; // Plus c'est petit plus le mouvement est lent

    public Vector3 initialPos = new Vector3(0, 0, 0);
    public Vector3 posUnityOuput;
    public float angleX = 0;
    public float angleY = 0;
    public float angleZ = 0;



    private Vector3 decallageByGaze = new Vector3(0, 0, 0);

    private MoveByGaze moveByGaze = null;
    private DeactivateMoveByGaze deactivateMoveByGaze = null;

    private bool decallageByGazeDone = false;
    private bool selectedOnce = false;


    public Text debugText = null;

    //public double rapport = 1;
    // Use this for initialization
    void Start() {
        if (plStream == null) {
            plStream = GetComponent<PlStreamCustom>();
        }

        if (moveByGaze == null) {
            moveByGaze = GetComponent<MoveByGaze>();
        }
        if (deactivateMoveByGaze == null) {
            deactivateMoveByGaze = new DeactivateMoveByGaze();
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (plStream != null) {
            if (plStream.isActive) {
                if (plStream.active[0]) {
                    Vector3 unity_position = new Vector3(0, 0, 0);
                    Vector3 pol_position = plStream.positions[0];// Les valeurs sont en inch !
                    //pol_position = InchToCm(pol_position);

                    // Recupère les bonne positions
                    unity_position.x = pol_position.y;
                    unity_position.y = -pol_position.z;
                    unity_position.z = pol_position.x;

                    initialPos = unity_position;
                    Debug.Log("Pos init :" + initialPos);
                }
            }
        }
        //if(debugText == null) {
        //    debugText = GameObject.FindObjectOfType(Text);
        //}
    }

    // Update is called once per frame
    void Update() {
        // Touches pour le calibrage
        KeyBoardControlsInput();
        // Si l'on desactive l'option du decallage
        if (decallageByGazeStart == false) {
            decallageByGazeDone = true;
        }
        else {
            if (moveByGaze.IsSelected) {
                // garder en mémoire la dernière position de l'objet pour pouvoir appliquer un decallage après coup
                decallageByGaze = transform.position;
                selectedOnce = true;
            }
            else {
                if (selectedOnce) {
                    //Debug.Log("Decallage done");
                    //Debug.Log("Decallage = " + decallageByGaze);
                    decallageByGazeDone = true;
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                }
            }
        }

        if (decallageByGazeDone) {
            if (plStream != null) {
                if (plStream.isActive) {
                    if (plStream.active[0]) {

                        // Le positionnement au regard ne touche pas l'orientation
                        Vector4 pol_rotation = plStream.orientations[0];
                        Vector3 unity_position = new Vector3(0, 0, 0);

                        // doing crude (90 degree) rotations into frame

                        // Dans le doute on desactive les objet pour le move by gaze
                        deactivateMoveByGaze.DeactivateElements(this.gameObject);
                        Vector3 pol_position = plStream.positions[0];// Les valeurs sont en inch !
                        //pol_position = InchToCm(pol_position);

                        // Recupère les bonne positions = renommage des axes
                        unity_position = GetCorrectAxis(pol_position);

                        // Application des correctifs des angles trouvé
                        unity_position = ApplyRotationMatrix(unity_position);



                        unity_position += decallageByCalibragePos * signe;// Surement devoir ajouter un coef multiplicateur

                        // On applique les decallage au clavier et celui du regard
                        unity_position = ApplyDecallage(unity_position);

                        // Il faut enlever la position initial du au capteur;
                        //unity_position += initialPos;

                        Quaternion unity_rotation;
                        unity_rotation.w = pol_rotation[0];
                        unity_rotation.x = -pol_rotation[2];
                        unity_rotation.y = pol_rotation[3];
                        unity_rotation.z = -pol_rotation[1];

                        posUnityOuput = unity_position;
                        transform.position = unity_position * signe * reduceMovementCoef;
                        transform.rotation = unity_rotation;

                    }
                }
            }
        }

        totalDecallage = decallageByCalibragePos + decallageClavier;
        debugText.text =
                string.Format(
                    "{0}\n" +
                    "Rotation sur X: {1:0.000}°\n" +
                    "Rotation sur Y: {2:0.000}°\n" +
                    "Rotation sur Z: {3:0.000}°\n" +
                    "Position :{4}\n" +
                    "Decallage polhemus:{5}\n" +
                    "Decallage clavier:({6})\n" +
                    "Position de base:{7}\n",
                    "Aiguille",
                    angleX, angleY, angleZ,
                    posUnityOuput,
                    decallageByCalibragePos,
                    decallageClavier,
                    initialPos
                    );
    }

    private Vector3 ApplyRotationMatrix(Vector3 posBeforeRotat) {
        if (angleX == 0 && angleY == 0 && angleZ == 0) {
            return posBeforeRotat;
        }

        // TODO -> attention aux angles -> angle X = rotation autour de l'axe Y
        // Angle Y autour de l'axe X
        // Angle Z autour de l'axe Y
        float theta = angleX * Mathf.Deg2Rad; // Rotation autour de Y
        float phi = angleY * Mathf.Deg2Rad; // Rotation autour de X
        float psy = angleZ * Mathf.Deg2Rad; // Rotation autour de Z
        //float psy = 0;// Rotation autour de Z

        float cosTheta = Mathf.Cos(theta);
        float sinTheta = Mathf.Sin(theta);

        float cosPhi = Mathf.Cos(phi);
        float sinPhi = Mathf.Sin(phi);

        float cosPsy = Mathf.Cos(psy);
        float sinPsy = Mathf.Sin(psy);


        // TODO -> mettre a jour avec la correction
        // Z > Y > X
        //        Vector3 unity_position = new Vector3(0, 0, 0) {
        //            x = posBeforeRotat.x * cosTheta * cosPhi + posBeforeRotat.y * (sinPsy * sinTheta * sinPhi - cosPsy * sinPhi) + posBeforeRotat.z * (cosPsy * sinTheta * cosPhi + sinPsy * sinPhi),
        //            y = posBeforeRotat.x * cosTheta * sinPhi + posBeforeRotat.y * sinPhi * sinPsy * sinTheta + posBeforeRotat.z * (cosPsy * sinTheta * sinPhi - sinPsy * cosPhi),
        //            z = -posBeforeRotat.x * sinTheta + posBeforeRotat.y * sinPsy * cosTheta + posBeforeRotat.z * cosPsy * cosTheta
        //        };

        // X > Y > Z
        /*Vector3 unity_position = new Vector3(0, 0, 0) {
            x = posBeforeRotat.x * (cosPhi * cosPsy) - posBeforeRotat.y * (cosPhi * sinPsy) + posBeforeRotat.z * (sinPsy),
            y = posBeforeRotat.x * (sinPhi * cosTheta - cosPsy * sinPhi * sinTheta) + posBeforeRotat.y * (sinPsy * sinPhi * sinTheta + cosPsy * cosTheta) - posBeforeRotat.z * (cosPhi * sinTheta),
            z = posBeforeRotat.x * (cosPsy * cosPhi * cosTheta + sinPsy * sinTheta) + posBeforeRotat.y * (cosPsy * sinTheta - sinPsy * cosPhi * cosTheta) + posBeforeRotat.z * (cosPhi * cosTheta),
        };*/

        // Rotation autour de Y
        Vector3 unity_positionRotationY = new Vector3(0, 0, 0) {
            x = posBeforeRotat.x * cosPhi + posBeforeRotat.z * sinPhi,
            y = posBeforeRotat.y,
            z = posBeforeRotat.x * (-sinPhi) + posBeforeRotat.z * cosPhi
        };

        // Rotation autour de X
        Vector3 unity_positionRotationX = new Vector3(0, 0, 0) {
            x = unity_positionRotationY.x,
            y = unity_positionRotationY.y * cosTheta - unity_positionRotationY.z * sinTheta,
            z = unity_positionRotationY.y * sinTheta + unity_positionRotationY.z * cosTheta,
        };

        // Rotation autour de Z
        Vector3 unity_positionRotationZ = new Vector3(0, 0, 0) {
            x = unity_positionRotationX.x * cosPsy - unity_positionRotationX.y * sinPsy,
            y = unity_positionRotationX.x * sinPsy + unity_positionRotationX.y * cosPsy,
            z = unity_positionRotationX.z,
        };

        //return unity_position;
        return unity_positionRotationZ;

    }

    private Vector3 GetCorrectAxis(Vector3 pol_position) {
        Vector3 unity_position = new Vector3(0, 0, 0) {
            x = pol_position.y,
            y = -pol_position.z,
            z = pol_position.x
        };
        return unity_position;
    }

    private Vector3 ApplyDecallage(Vector3 unity_position) {
        unity_position = DecallageClavier(unity_position);
        unity_position += decallageByGaze * signe * 100;

        return unity_position;
    }

    private void KeyBoardControlsInput() {
        float vitesseTranslation = 0.02f;
        float vitesseRotation = 0.1f;
        if (devBuid) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                // Decallage sur X
                if (Input.GetKey("h")) {
                    decallageClavier.x += vitesseTranslation;
                }
                if (Input.GetKey("f")) {
                    decallageClavier.x -= vitesseTranslation;
                }
                // Decallage sur Y
                if (Input.GetKey("t")) {
                    decallageClavier.y += vitesseTranslation;
                }
                if (Input.GetKey("g")) {
                    decallageClavier.y -= vitesseTranslation;
                }
                // Decallage sur Z
                if (Input.GetKey("x")) {
                    decallageClavier.z += vitesseTranslation;
                }
                if (Input.GetKey("w")) {
                    decallageClavier.z -= vitesseTranslation;
                }

                // Rotation sur l'axe X
                if (Input.GetKey("u")) {
                    angleX += vitesseRotation;
                }
                if (Input.GetKey("j")) {
                    angleX -= vitesseRotation;
                }

                // Rotation sur l'axe Y
                if (Input.GetKey("r")) {
                    angleY += vitesseRotation;
                }
                if (Input.GetKey("y")) {
                    angleY -= vitesseRotation;
                }

            }
        }
        else {
            if (Input.GetKey(KeyCode.LeftShift)) {
                // Decallage sur X
                if (Input.GetKey("d")) {
                    decallageClavier.x += vitesseTranslation;
                }
                if (Input.GetKey("q")) {
                    decallageClavier.x -= vitesseTranslation;
                }
                // Decallage sur Y
                if (Input.GetKey("z")) {
                    decallageClavier.y += vitesseTranslation;
                }
                if (Input.GetKey("s")) {
                    decallageClavier.y -= vitesseTranslation;
                }
                // Decallage sur Z
                if (Input.GetKey("c")) {
                    decallageClavier.z += vitesseTranslation;
                }
                if (Input.GetKey("v")) {
                    decallageClavier.z -= vitesseTranslation;
                }
                // Rotation sur l'axe X
                if (Input.GetKey("r")) {
                    angleX += vitesseRotation;
                }
                if (Input.GetKey("f")) {
                    angleX -= vitesseRotation;
                }

                // Rotation sur l'axe Y
                if (Input.GetKey("a")) {
                    angleY += vitesseRotation;
                }
                if (Input.GetKey("e")) {
                    angleY -= vitesseRotation;
                }
            }
        }
    }

    private static Vector3 InchToCm(Vector3 pol_position) {
        pol_position.x = pol_position.x * 2.54f;
        pol_position.y = pol_position.y * 2.54f;
        pol_position.z = pol_position.z * 2.54f;
        return pol_position;
    }

    private Vector3 DecallageClavier(Vector3 unity_position) {
        unity_position += decallageClavier * signe;
        return unity_position;
    }

    public void invertCoef() {
        signe = -signe;
    }
}