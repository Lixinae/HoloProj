using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.Receivers;
using System;
using System.Collections;
using UnityEngine;

public class CalibrageClicker : MonoBehaviour {

    private UpdatePosOrient updatePosOrient = null;

    private GameObject cube1 = null;
    private GameObject cube2 = null;

    private bool firstPosSet = false;
    private bool secondPosSet = false;

    private Vector3 PosCapteur1 = new Vector3(0, 0, 0);
    private Vector3 PosCapteur2 = new Vector3(0, 0, 0);

    private Vector3 PosCube1 = new Vector3(0, 0, 0);
    private Vector3 PosCube2 = new Vector3(0, 0, 0);
    private CalibrageClickAnnex cube1CalibrageClick;
    private CalibrageClickAnnex cube2CalibrageClick;

    private bool calibrageDone = false;

    private GameObject sceneItems = null;
    private GameObject gltf = null;
    private GameObject axisViewer = null;


    private void Awake() {

        cube1 = GameObject.Find("Cube_test");
        cube1CalibrageClick = cube1.GetComponent<CalibrageClickAnnex>();
        cube2 = GameObject.Find("Cube_2_test");
        cube2CalibrageClick = cube2.GetComponent<CalibrageClickAnnex>();
        updatePosOrient = GameObject.Find("Aiguille").GetComponent<UpdatePosOrient>();
        float coef = (1f / (0.1f * 0.25f));
        PosCube1 = cube1.transform.position * coef; // *40 ramène a un scale de 1
        PosCube2 = cube2.transform.position * coef;

        if (sceneItems == null) {
            sceneItems = GameObject.Find("SceneItems");
        }
        if (gltf == null) {
            gltf = GameObject.Find("GLTF");
        }

        axisViewer = GameObject.Find("AxisViewer");


        HideScene();
        //ObjectToShow.SetActive(false);
    }

    public void LateUpdate() {

        if (!calibrageDone) {
            firstPosSet = cube1CalibrageClick.posSet;
            secondPosSet = cube2CalibrageClick.posSet;
            if (firstPosSet && secondPosSet) {

                Calibrage();
                calibrageDone = true;
                // TODO  -> Changer les calculs, besoin de 3D


                //Vector3 angles = MathCalcul.GetAngle(firstPos, secondPos);

                // todo Faire les calcul

                // Nommage pour coller aux calculs sur la feuille
                // zU1 = YuA sur la feuille
                /*float zUA = firstPosCube.z; 
                float yUA = firstPosCube.y; 
                float xUA = firstPosCube.x;

                float zTA = firstPosCapteur.z;
                float yTA = firstPosCapteur.y;
                float xTA = firstPosCapteur.x;

                float zUB = secondPosCube.z;
                float yUB = secondPosCube.y;
                float xUB = secondPosCube.x;

                float zTB = secondPosCapteur.z;
                float yTB = secondPosCapteur.y;
                float xTB = secondPosCapteur.x;

                Debug.Log("zUA = " +zUA);
                Debug.Log("xUA = " +xUA);

                Debug.Log("zTA = " +zTA);
                Debug.Log("xTA = " +xTA);

                Debug.Log("zUB = " +zUB);
                Debug.Log("xUB = " +xUB);

                Debug.Log("zTB = " +zTB);
                Debug.Log("xTB = " +xTB);

                float offSetXt = (((zUB - zUA) * xTB - xUB * zTB) * xTA - xUA * zTA * xTB) / (zTA * xTB - zTB * xTA);
                float offSetZt = (((offSetXt - xUB) * zTB) / xTB) + zUB;

                Vector3 resultatCalculPosAvecAngle = new Vector3(offSetXt, 0, offSetZt);
                Debug.Log("XU1:" + xUA);
                Debug.Log("XT1:" + xTA);
                Debug.Log("OffsetX :" + offSetXt);
                Debug.Log("OffsetZ :" + offSetZt);
                float valTmp = ((xUA - offSetXt) / xTA);
                Debug.Log("Val tmp :" + valTmp);
                // On passe de degré à radians si necessaire
                if (valTmp > 1 || valTmp < -1) {
                    valTmp *= Mathf.Deg2Rad;
                }
                //float valTmpRad = ((xUA - offSetXt) / xTA) * Mathf.Deg2Rad;
                // Debug.Log("Val tmp Rad :" + valTmpRad);

                // Modification sont faites ici car ça évite de les recalculer à chaque tour de boucle du update -> optimisation de calcul
                updatePosOrient.angle = Mathf.Acos(valTmp);
                updatePosOrient.rapport = valTmp;
                //updatePosOrient.reduceMovementCoef = 0.01f / Mathf.Cos(updatePosOrient.angle); Pas forcément la meilleur idée
                // updatePosOrient.angle -= (Mathf.PI / 2f); -> Theorie foireuse , marche pas
                //Debug.Log("Angle :" + updatePosOrient.angle * Mathf.Rad2Deg);




                updatePosOrient.decallageByCalibragePos = resultatCalculPosAvecAngle;
                Debug.Log("Calibrage :" + resultatCalculPosAvecAngle);
                calibrageDone = true;

                HideCubeAndShowScene();
                */
            }
        }
    }

    private void HideCubeAndShowScene() {
        cube1.SetActive(false);
        cube2.SetActive(false);
        ShowScene();
    }

    private void HideScene() {
        //sceneItems.SetActive(false);
        axisViewer.SetActive(false);
        gltf.SetActive(false);
    }

    private void ShowScene() {
        //sceneItems.SetActive(true);
        axisViewer.SetActive(true);
        gltf.SetActive(true);
    }

    // TODO -> Trouver une touche pour dire de reset le calibrage
    public void RestartCalibrage() {
        firstPosSet = false;
        secondPosSet = false;
        calibrageDone = false;
        HideScene();
    }

    // Calibrage avec le formule determiner sur feuilles
    private void Calibrage() {

        // TODO 
        /////////////////////////////////////////
        /// /!\ Gaffe aux arrondi d'unity /!\ ///
        /////////////////////////////////////////

        PosCapteur1 = cube1CalibrageClick.posCapteur;
        PosCapteur2 = cube2CalibrageClick.posCapteur;

        float t1 = PosCapteur1.x;
        float t2 = PosCapteur1.z;
        float t3 = PosCapteur1.y;

        float t4 = PosCapteur2.x;
        float t5 = PosCapteur2.z;
        float t6 = PosCapteur2.y;

        float Xx = PosCube1.x;
        float Xy = PosCube1.y;
        float Xz = PosCube1.z;

        float Yx = PosCube2.x;
        float Yy = PosCube2.y;
        float Yz = PosCube2.z;

        float t1t2t4t5 = t1 + t2 - t4 - t5;
        float t1t5pow = t1t2t4t5 + (Mathf.Pow(t3 - t6, 2) / (t1 - t4));
        float t1t3t4t6 = t1 + t3 - t4 - t6;

        float powDiv = 1 + ((Mathf.Pow(t5 - t2, 2) * t1t2t4t5) / (t1t5pow * t1t3t4t6));

        float Azfe = (Yz - Xz) / (t1t3t4t6 * powDiv);
        float zfe = ((((t3 - t6) * (Yy - Xy) + (Yx - Xx) * t1t2t4t5) * (t2 - t5)) / ((t1 - t4) * (t1t5pow) * t1t3t4t6 * powDiv)) + Azfe;


        float xfe = ((t5 - t2) * t1t2t4t5 * zfe + (t3 - t6) * (Yy - Xy) + (Yx - Xx) * t1t2t4t5) / ((t1 - t4) * (t1t5pow));
        float yfe = ((t6 - t3) * xfe - Xy + Yy) / (t1t2t4t5);


        // X -> 1er pts dans unity
        // Y -> 2e pts dans unity

        float xE = Xx + t1 * xfe + t2 * zfe - t3 * yfe;
        float yE = Xy + t1 * yfe + t2 * yfe + t3 * xfe;
        float zE = Xz + t1 * zfe - t2 * xfe + t3 * zfe;

        Vector3 posEmetteur = new Vector3(xE, yE, zE);

        // 3 vecteurs composant le repère du capteur polhemus
        Vector3 FE = new Vector3(xfe, yfe, zfe); // orientation sur l'axe X
        Vector3 UE = new Vector3(zfe, yfe, -xfe); // orientation sur l'axe Z
        Vector3 VE = new Vector3(-yfe, xfe, zfe); // orientation sur l'axe Y

        Vector3 axisX = new Vector3(1, 0, 0);
        Vector3 axisY = new Vector3(0, 1, 0);
        Vector3 axisZ = new Vector3(0, 0, 1);

        // TODO -> attention aux angles -> angle X = rotation autour de l'axe Y
        // Angle Y autour de l'axe X
        // Angle Z autour de l'axe Y
        float angleX = Vector3.Angle(axisX, FE);
        float angleY = Vector3.Angle(axisY, VE);
        float angleZ = Vector3.Angle(axisZ, UE);

        // AngleX -> Rotation autour de l'axe X et donc c'est l'angle Y qu'on determine au dessus
        updatePosOrient.angleX = angleY;
        // Angle Y -> Rotation autour de l'axe Y et donc c'est l'angle X determiné au dessus ( ou l'angle Z, les 2 étant normalement égaux)
        updatePosOrient.angleY = angleX;
        updatePosOrient.decallageByCalibragePos = posEmetteur;

        axisViewer.SetActive(true);

        // On doit enlever l'anchor pour déplacer l'objet et on la remet après
        UpdateDecallageWithAiguille updateDecallage = axisViewer.GetComponent<UpdateDecallageWithAiguille>();
        updateDecallage.RemoveAnchor();

        axisViewer.transform.position = posEmetteur;
        axisViewer.transform.rotation = new Quaternion(0, angleY, angleX, angleZ);

        updateDecallage.SetAnchor();
    }
}
