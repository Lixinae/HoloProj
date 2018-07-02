using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

public class UpdateDecallageWithAiguille : MonoBehaviour {


    private GameObject Aiguille = null;

    private UpdatePosOrient updatePosOrient;

    private Vector3 previousPosition;
    private Vector3 actualPosition;
    private GazeManager gaze;
    private GameObject spatialMapping;

    private bool locked = false;
    private Vector3 prevDecallage = new Vector3(0, 0, 0);
    // Use this for initialization
    void Start() {
        if (Aiguille == null) {
            Aiguille = GameObject.Find("Aiguille");
            updatePosOrient = Aiguille.GetComponent<UpdatePosOrient>();
        }
        gaze = GameObject.Find("InputManager").GetComponent<GazeManager>();
        spatialMapping = GameObject.Find("SpatialMapping");
        if (!locked) {
            if (WorldAnchorManager.Instance != null) {
                locked = true;
                WorldAnchorManager.Instance.AttachAnchor(gameObject); // Permet de charger l'anchor stocker en mémoire
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

       
        // Se déplace de manière infinie pour le moment
        if(updatePosOrient.totalDecallage != prevDecallage) {
            prevDecallage = updatePosOrient.totalDecallage;
            transform.position += updatePosOrient.totalDecallage * 0.01f; // On divise par 10 pour adapter a la fenetre
        }
        

        Quaternion quaternion = Quaternion.Euler(updatePosOrient.angleX, updatePosOrient.angleY, 0);
        transform.rotation = quaternion;
    }

    public void RemoveAnchor() {
        if (WorldAnchorManager.Instance != null) {
            locked = false;
            WorldAnchorManager.Instance.RemoveAnchor(gameObject);
        }
    }

    public void SetAnchor() {
        if (WorldAnchorManager.Instance != null) {
            locked = true;
            WorldAnchorManager.Instance.AttachAnchor(gameObject);
        }
    }

    public void LockTransform() {
        if (locked) {
            if (WorldAnchorManager.Instance != null) {
                locked = false;
                WorldAnchorManager.Instance.RemoveAnchor(gameObject);
            }

        }
        else {
            if (WorldAnchorManager.Instance != null) {
                locked = true;
                WorldAnchorManager.Instance.AttachAnchor(gameObject);
            }

        }
        /*if (gameObject.GetComponent<WorldAnchor>() != null) {
            if (!spatialMapping.activeInHierarchy) {
                // On reactive le spatial mapping pour pouvoir remettre les anchor
                spatialMapping.SetActive(true);            
                // On met à jour le world anchor en supprimant le nouveau et en en replaçant un nouveau
                WorldAnchorManager.Instance.RemoveAnchor(gameObject);
                WorldAnchorManager.Instance.AttachAnchor(gameObject);
                spatialMapping.SetActive(false);
            }
            else {
                // On met à jour le world anchor en supprimant le nouveau et en en replaçant un nouveau
                WorldAnchorManager.Instance.RemoveAnchor(gameObject);
                WorldAnchorManager.Instance.AttachAnchor(gameObject);
            }

        }
        else {
            WorldAnchorManager.Instance.AttachAnchor(gameObject);
        }*/
    }
}
