using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateMoveByGaze {

    private MoveByGaze moveByGaze = null;
    private SpatialMappingCollisionDetector spatialMapping = null;
    private PlaceByTap placeByTap = null;
    private Rigidbody rigidbody = null;

    // Permet de limiter l'action à 1 seul fois par script appellant la fonction -> optimisation
    private bool once = true;

    // Desactive tous les composant du moveByGaze
    public void DeactivateElements(GameObject go) {
       

        if (once) {
            moveByGaze = go.GetComponent<MoveByGaze>();
            spatialMapping = go.GetComponent<SpatialMappingCollisionDetector>();
            placeByTap = go.GetComponent<PlaceByTap>();
            rigidbody = go.GetComponent<Rigidbody>();

            moveByGaze.enabled = false;
            spatialMapping.enabled = false;
            placeByTap.enabled = false;
            rigidbody.detectCollisions = false;
            once = false;
        }
    }
}
