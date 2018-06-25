using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;

public class PlaceByTap : MonoBehaviour, IInputClickHandler {
    protected MoveByGaze GazeMover;
    protected GazeManager gaze;
    protected MoveByGaze[] allMoveByGaze;

    protected void Start() {
        //GazeMover = GetComponent<MoveByGaze>();
        //gaze = GameObject.Find("InputManager").GetComponent<GazeManager>();

        //allMoveByGaze = GameObject.Find("MovableByGaze").GetComponentsInChildren<MoveByGaze>();

        // Fonctionne tant qu'on ajoute pas d'autres objets déplaçable avec le regard dans la scene, ils seront pas pris en compte sinon
        //allMoveByGaze = FindObjectsOfType<MoveByGaze>();
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }

    public void OnInputClicked(InputClickedEventData eventData) {

        //var tapToPlace = gameObject.AddComponent<TapToPlace>();
        //tapToPlace.SavedAnchorFriendlyName = (++this.count).ToString();


        /*if (gaze.HitObject == gameObject) {
            if (!GazeMover.IsSelected) {
                // On ne veut qu'un seul objet selectionné à la fois
                foreach (var move in allMoveByGaze) {
                    move.DeSelect();
                }
                GazeMover.RemoveWorldAnchor();
                GazeMover.Select();
                //Debug.Log(gameObject);
            }
            else {

                /*
                 * when moving objects that have world anchors we recommend:
                 * 
                    Removing the world anchor from that component.
                    Move it to where you want it
                    Add the world anchor back once you have placed it.

                https://github.com/Microsoft/MixedRealityToolkit-Unity/issues/937
                /
                
                // On deselectionne l'objet
                GazeMover.DeSelect();
                GazeMover.SetupWorldAnchor();

            }
        }*/
    }

    // Thx to https://mtaulty.com/2016/12/20/hitchhiking-the-holotoolkity-unity-leg-5-baby-steps-with-world-anchors-and-persisting-holograms/

}

