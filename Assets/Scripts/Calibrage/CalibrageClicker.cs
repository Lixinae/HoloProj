using UnityEngine;

public class CalibrageClicker : MonoBehaviour {

    private UpdatePosOrientAiguille updatePosOrient = null;

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

        if (cube1 == null) {
            cube1 = GameObject.Find("Cube_test");
            if (cube1 != null) {
                cube1CalibrageClick = cube1.GetComponent<CalibrageClickAnnex>();
            }
        }
        if (cube2 == null) {
            cube2 = GameObject.Find("Cube_2_test");
            if (cube2 != null) {
                cube2CalibrageClick = cube2.GetComponent<CalibrageClickAnnex>();
            }
        }

        updatePosOrient = GameObject.Find("Aiguille").GetComponent<UpdatePosOrientAiguille>();

        float coef = (1f / (0.1f * 0.25f)); // On adapte avec les scales utilisé dans les éléments
        PosCube1 = cube1.transform.position * coef; // *40 ramène a un scale de 1
        PosCube2 = cube2.transform.position * coef;

        if (sceneItems == null) {
            sceneItems = GameObject.Find("SceneItems");
        }
        if (gltf == null) {
            gltf = GameObject.Find("GLTF");
        }
        if (axisViewer == null) {
            axisViewer = GameObject.Find("AxisViewer");
        }
        HideScene();
        //ObjectToShow.SetActive(false);
    }

    public void LateUpdate() {

        if (!calibrageDone) {
            firstPosSet = cube1CalibrageClick.posSet;
            if (firstPosSet) {
                HideCube1();
            }
            secondPosSet = cube2CalibrageClick.posSet;
            if (secondPosSet) {
                HideCube2();
            }
            if (firstPosSet && secondPosSet) {

                Calibrage();
                calibrageDone = true;
                ShowScene();
                //HideCubeAndShowScene();
            }
        }
    }

    private void HideCube1() {
        cube1.SetActive(false);
    }

    private void HideCube2() {
        cube2.SetActive(false);
    }

    private void HideCubeAndShowScene() {
        //HideCube1();
        //HideCube2();
        ShowScene();
    }



    // TODO -> Trouver une touche pour dire de reset le calibrage
    public void RestartCalibrage() {
        firstPosSet = false;
        secondPosSet = false;
        calibrageDone = false;
        HideScene();
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
    // Calibrage avec le formule determiner sur feuilles
    private void Calibrage() {

        /////////////////////////////////////////
        /// /!\ Gaffe aux arrondi d'unity /!\ ///
        /////////////////////////////////////////

        PosCapteur1 = cube1CalibrageClick.posCapteur;
        PosCapteur2 = cube2CalibrageClick.posCapteur;

        double t1 = PosCapteur1.x;
        double t2 = PosCapteur1.y;
        double t3 = PosCapteur1.z;

        double t4 = PosCapteur2.x;
        double t5 = PosCapteur2.y;
        double t6 = PosCapteur2.z;

        double Xx = PosCube1.x;
        double Xy = PosCube1.y;
        double Xz = PosCube1.z;

        double Yx = PosCube2.x;
        double Yy = PosCube2.y;
        double Yz = PosCube2.z;

        double t1t2t4t5 = t1 + t2 - t4 - t5;

        double t1t3t4t6 = t1 + t3 - t4 - t6;

        double t1t4 = t1 - t4;
        double t3t6 = t3 - t6;
        double t6t3 = t6 - t3;
        double t5t2 = t5 - t2;
        double t2t5 = t2 - t5;

        double YzXz = Yz - Xz;
        double YxXx = Yx - Xx;
        double YyXy = Yy - Xy;

        double t1t4Pow = Mathf.Pow((float)t1t4, 2);
        double t3t6Pow = Mathf.Pow((float)t3t6, 2);

        double zfe = (YzXz + ((t1t4 * YxXx) * (t1t2t4t5 - t3t6Pow) + t1t4 * (t3t6 * YyXy * t1t4 + t3t6Pow * YxXx)) / (t1t4Pow * (t1t2t4t5 - t3t6Pow)) /
            (t1t3t4t6 - (t2t5 * (t1t4Pow * t1t2t4t5 * t5t2 - t3t6Pow * t1t4 * t5t2 + t1t4 * t3t6Pow * t5t2)) / (t1t4Pow * (t1t2t4t5 - t3t6Pow))));
        double yfe = (YyXy * t1t4 + t6t3 * (YxXx + t5t2 * zfe)) / (t1t4 * t1t2t4t5 - t3t6Pow);
        double xfe = (YxXx + t5t2 * zfe + t3t6 * yfe) / t1t4;
        /*double t1t5pow = t1t2t4t5 + (Mathf.Pow((float)(t3 - t6), 2) / (t1 - t4));
        double powDiv = 1 + ((Mathf.Pow((float)(t5 - t2), 2) * t1t2t4t5) / ((t1 - t4) * (t1t5pow * t1t3t4t6)));

        double Azfe = (Yz - Xz) / (t1t3t4t6 * powDiv);
        double zfe = ((((t3 - t6) * (Yy - Xy) + (Yx - Xx) * t1t2t4t5) * (t2 - t5)) / ((t1 - t4) * (t1t5pow) * t1t3t4t6 * powDiv)) + Azfe;


        double xfe = ((t5 - t2) * t1t2t4t5 * zfe + (t3 - t6) * (Yy - Xy) + (Yx - Xx) * t1t2t4t5) / ((t1 - t4) * (t1t5pow));
        double yfe = ((t6 - t3) * xfe - Xy + Yy) / (t1t2t4t5);
		*/

        // X -> 1er pts dans unity
        // Y -> 2e pts dans unity

        double xE = Xx + t1 * xfe + t2 * zfe - t3 * yfe;
        double yE = Xy + t1 * yfe + t2 * yfe + t3 * xfe;
        double zE = Xz + t1 * zfe - t2 * xfe + t3 * zfe;

        Vector3 posEmetteur = new Vector3((float)xE, (float)yE, (float)zE);

        // 3 vecteurs composant le repère du capteur polhemus
        Vector3 FE = new Vector3((float)xfe, (float)yfe, (float)zfe); // orientation sur l'axe X
        // Gaffe UE ET VE inversé pour test
        Vector3 UE = new Vector3((float)-yfe, (float)xfe, (float)zfe); // orientation sur l'axe Y
        Vector3 VE = new Vector3((float)zfe, (float)yfe, (float)-xfe); // orientation sur l'axe Z



        Vector3 axisX = new Vector3(1, 0, 0);
        Vector3 axisY = new Vector3(0, 1, 0);
        Vector3 axisZ = new Vector3(0, 0, 1);

        // TODO -> attention aux angles -> angle X = rotation autour de l'axe Y
        // Angle Y autour de l'axe X
        // Angle Z autour de l'axe Y
        float angleX = Vector3.Angle(axisX, FE);
        float angleY = Vector3.Angle(axisY, UE);
        float angleZ = Vector3.Angle(axisZ, VE);

        Debug.Log("Pos emetteur: " + posEmetteur);

        Debug.Log("FE:" + FE);
        Debug.Log("UE:" + UE);
        Debug.Log("VE:" + VE);

        Debug.Log("angleX:" + angleX);
        Debug.Log("angleY:" + angleY);
        Debug.Log("angleZ:" + angleZ);


        // AngleX -> Rotation autour de l'axe X et donc c'est l'angle Y qu'on determine au dessus
        updatePosOrient.orientation.x = angleX;
        // Angle Y -> Rotation autour de l'axe Y et donc c'est l'angle X determiné au dessus ( ou l'angle Z, les 2 étant normalement égaux)
        updatePosOrient.orientation.y = angleY;
        updatePosOrient.orientation.z = angleZ; // utile uniquement pour le debug, la rotation sur Z n'est pas appliqué lors de la transformation
        updatePosOrient.decallageByCalibragePos = posEmetteur;

        axisViewer.SetActive(true);

        // On doit enlever l'anchor pour déplacer l'objet et on la remet après
        UpdateDecallageWithAiguille updateDecallage = axisViewer.GetComponent<UpdateDecallageWithAiguille>();
        updateDecallage.RemoveAnchor();

        axisViewer.transform.position = posEmetteur * 0.01f;
        axisViewer.transform.rotation = Quaternion.Euler(angleX, angleY, angleZ);

        updateDecallage.SetAnchor();


    }


}
