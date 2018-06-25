using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity;

// Thx to LocalJoost https://github.com/LocalJoost/HoloDragByGaze
// https://dzone.com/articles/dragging-and-placing-holograms

public class MoveByGaze : MonoBehaviour {
    /*public float MaxDistance = 1f;
    //public bool IsActive = true;

    public float DistanceTrigger = 0f;
    public BaseRayStabilizer Stabilizer = null;
    public SpatialMappingCollisionDetector CollisonDetector;

    // PC
    public GameObject cameraToFollow;
    public bool IsSelected = false;
    public bool IsPlaced = false; // utile lors du placement de l'aiguille


    public Vector3 _lastMoveToLocation;

    private float _startTime;
    private float _delay = 0.5f;
    private bool _isJustEnabled;
    
    private bool _isBusy;

    private Rigidbody _rigidbody;

    private SpatialMappingManager MappingManager {
        get { return SpatialMappingManager.Instance; }
    }

    void OnEnable() {
        _isJustEnabled = true;
    }

    void Start() {
        if(cameraToFollow == null) {
            cameraToFollow = GameObject.Find("HoloLensCamera");
        }
        
        _startTime = Time.time+ _delay;
        _isJustEnabled = true;
        if (CollisonDetector == null) {
            CollisonDetector = gameObject.AddComponent<SpatialMappingCollisionDetector>();
        }
    }
    
    public void Select() {
        IsSelected = true;
    }

    public void DeSelect() {
        IsSelected = false;
    }

    public void SetupWorldAnchor() {
        RemoveRigidBody();
        WorldAnchorManager.Instance.AttachAnchor(gameObject,gameObject.name);
    }

    public void RemoveWorldAnchor() {
        WorldAnchorManager.Instance.RemoveAnchor(gameObject);
        SetupRigidBody();
        //Debug.Log("World anchor removed");
    }

    private void SetupRigidBody() {
        if(gameObject.GetComponent<Rigidbody>() == null) {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            Debug.Log(_rigidbody);
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
        }
    }

    private void RemoveRigidBody() {
        Destroy(gameObject.GetComponent<Rigidbody>());
    }

    // Todo , attacher le world anchor une fois le mouvement fait
    void Update() {
        if (_isBusy || !IsSelected ||  _startTime > Time.time) {
            //Debug.Log("Object : "+ gameObject +"Busy :" + _isBusy + " | Selected : " + IsSelected + " | _startTime > Time.time : " + (_startTime > Time.time));
            return;
        }
            
        _isBusy = true;
        var newPos = GetPostionInLookingDirection();
        if ((newPos - _lastMoveToLocation).magnitude > DistanceTrigger || _isJustEnabled) {
            _isJustEnabled = false;
            var maxDelta = CollisonDetector.GetMaxDelta(newPos - transform.position);
            if (maxDelta != Vector3.zero) {
                newPos = transform.position + maxDelta;
                iTween.MoveTo(gameObject,
                    iTween.Hash("position", newPos, "time", 2.0f * maxDelta.magnitude,
                        "easetype", iTween.EaseType.easeInOutSine, "islocal", false,
                        "oncomplete", "MovingDone", "oncompletetarget", gameObject));
                _lastMoveToLocation = newPos;

            }
            else {
                MovingDone();
            }
        }
        else {
            MovingDone();
        }
        // Suivi de l'orientation de la camera quand on deplace l'objet
        Quaternion rota = gameObject.transform.rotation;
        rota.y = cameraToFollow.transform.rotation.y;

        gameObject.transform.rotation = rota;
    }

    private void MovingDone() {
        _isBusy = false;
    }


    private Vector3 GetPostionInLookingDirection() {
        RaycastHit hitInfo;

        var headReady = Stabilizer != null ? Stabilizer.StableRay : new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (MappingManager != null &&
            Physics.Raycast(headReady, out hitInfo, MaxDistance, MappingManager.LayerMask)) {
            return hitInfo.point;
        }

        return CalculatePositionDeadAhead(MaxDistance);
    }

    private Vector3 CalculatePositionDeadAhead(float distance) {
        return Stabilizer != null
            ? Stabilizer.StableRay.origin + Stabilizer.StableRay.direction.normalized * distance
            : Camera.main.transform.position + Camera.main.transform.forward.normalized * distance;
    }*/
}
