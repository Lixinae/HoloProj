using HoloToolkit.Unity;
using UnityEngine;

public class UpdateDecallageWithAiguille : MonoBehaviour {


    private GameObject Aiguille = null;

    private UpdatePosOrient updatePosOrient;

    private Vector3 previousPosition;
    private Vector3 actualPosition;
    //private GazeManager gaze;
    //private GameObject spatialMapping;

    private bool locked = false;
    private Vector3 prevDecallage = new Vector3(0, 0, 0);
    private Vector3 prevOrientation = new Vector3(0, 0, 0);
    private Vector3 posBase = new Vector3(0, 0, 0);
    private Vector3 orientBase = new Vector3(0, 0, 0);
    // Use this for initialization

    void Start() {
        if (Aiguille == null) {
            Aiguille = GameObject.Find("Aiguille");
            updatePosOrient = Aiguille.GetComponent<UpdatePosOrient>();
        }
        //gaze = GameObject.Find("InputManager").GetComponent<GazeManager>();
        //spatialMapping = GameObject.Find("SpatialMapping");
        if (!locked) {
            if (WorldAnchorManager.Instance != null) {
                locked = true;
                WorldAnchorManager.Instance.AttachAnchor(gameObject); // Permet de charger l'anchor stocker en mémoire
                posBase = transform.position;
                orientBase.x = transform.rotation.x;
                orientBase.y = transform.rotation.y;
                orientBase.z = transform.rotation.z;
            }
        }
    }

    // Update is called once per frame
    void Update() {

        /*previousPosition = transform.position;
        actualPosition = updatePosOrient.totalDecallage * 0.01f; // On divise par 10 pour adapter a la fenetre

        if (actualPosition != previousPosition) {
            Debug.Log("Pos actuel :" + actualPosition);
            Debug.Log("Pos Prev :" + previousPosition);
            Debug.Log("Fuuuuuuuuuuuu");
            // todo 
            WorldAnchorManager.Instance.RemoveAnchor(gameObject);
            transform.position = updatePosOrient.totalDecallage * 0.01f; // On divise par 10 pour adapter a la fenetre
            WorldAnchorManager.Instance.AttachAnchor(gameObject);
        }*/

        // On regarde l'objet et si on appuie sur L , ça verrouille sa position avec un world anchor
        /*if (gaze.HitObject == gameObject) {
            //Debug.Log("Bla bla blablalblalzalreiuhgrj");
            if (Input.GetKeyDown("l")) {
                Debug.Log(gameObject.name + ": Locked");
                LockTransform();
            }
        }*/


        // Pour eviter les déplacement infini
        if (updatePosOrient.totalDecallage != prevDecallage) {
            prevDecallage = updatePosOrient.totalDecallage;
            transform.position = posBase + updatePosOrient.totalDecallage * 0.01f; // On divise par 10 pour adapter a la fenetre
        }

        if (updatePosOrient.orientation != prevOrientation) {
            prevOrientation = updatePosOrient.orientation;
            Quaternion quaternion = Quaternion.Euler(orientBase.x + updatePosOrient.orientation.x, orientBase.y + updatePosOrient.orientation.y, 0);
            transform.rotation = quaternion;
        }

    }

    /// <summary>
    /// Deverouille l'objet et enlève l'anchor pour permettre le déplacement
    /// </summary>
    public void RemoveAnchor() {
        if (WorldAnchorManager.Instance != null) {
            locked = false;
            WorldAnchorManager.Instance.RemoveAnchor(gameObject);
        }
    }

    /// <summary>
    /// Verrouille l'objet et met un anchor pour empecher le déplacement
    /// </summary>
    public void SetAnchor() {
        if (WorldAnchorManager.Instance != null) {
            locked = true;
            WorldAnchorManager.Instance.AttachAnchor(gameObject);
        }
    }

    /// <summary>
    /// Trigger pour verrouiller ou deverouiller l'objet via un bouton
    /// </summary>
    public void LockTransform() {
        if (locked) {
            RemoveAnchor();

        }
        else {
            SetAnchor();
        }
    }
}
