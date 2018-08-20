using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class CalibrageClickAnnex : MonoBehaviour, IInputClickHandler {

    public bool posSet = false;
    private UpdatePosOrientAiguille updatePosOrient = null;
    private AudioSource audio_source = null;
    public Vector3 posCapteur = new Vector3(0, 0, 0);
    // Use this for initialization
    void Start() {
        updatePosOrient = GameObject.Find("Aiguille").GetComponent<UpdatePosOrientAiguille>();
        audio_source = GetComponent<AudioSource>();
    }

    public void OnInputClicked(InputClickedEventData eventData) {
        // Le clicker fonctionne exactement comme le air tap
        if (!posSet) {
            posCapteur = updatePosOrient.posUnityOuput;
            posSet = true;
            audio_source.Play();
            Debug.Log("Pos Set");
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
