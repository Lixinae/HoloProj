using HoloToolkit.Unity.SpatialMapping;
using UnityEngine;

public class CheckForPlaceByTapOnChildren : MonoBehaviour {


    private GLTFComponentPerso gLTFComponentPerso;

    // Use this for initialization
    void Start() {
        gLTFComponentPerso = GLTFComponentPerso.Instance;
    }

    // Update is called once per frame
    void Update() {
        // Verifie si l'objet est bien chargé, s'il est on verifie si l'enfant a bien un box collider ainsi que le script TapToPlace
        if (gLTFComponentPerso.IsLoaded) {
            if (GetComponentInChildren<BoxCollider>() == null) {
                foreach (Transform child in transform) {
                    child.gameObject.AddComponent<BoxCollider>();
                }
            }
            TapToPlace tapToPlace = GetComponentInChildren<TapToPlace>();
            if (tapToPlace == null) {
                foreach (Transform child in transform) {
                    child.gameObject.AddComponent<TapToPlace>();
                }
            }
            tapToPlace = GetComponentInChildren<TapToPlace>();
            tapToPlace.AllowMeshVisualizationControl = false;
            tapToPlace.DefaultGazeDistance = 1f;
        }


    }
}
