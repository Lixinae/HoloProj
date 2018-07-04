using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Examples.InteractiveElements;
using UnityEngine.Events;
// todo Creer les éléments et les mettre sous l'item MenuItems pour pouvoir les afficher

public class CreateElementsFromFileList : MonoBehaviour {

    private int previousCount = 0;
    private GameObject modelsMenu = null;
    private GameObject gltf = null;

    private float timer = 60f;
    // Use this for initialization
    void Start() {
        var files = new ShowFileInFolder();
        var fileList = files.GetFileList();

        if (modelsMenu == null) {
            modelsMenu = GameObject.Find("ModelsMenu");
        }
        if (gltf == null) {
            gltf = GameObject.Find("GLTF");
        }
        SetupToggles(fileList);
        modelsMenu.SetActive(false);
        StartCoroutine("DoCheck");
    }

    private void SetupToggles(List<string> fileList) {
        previousCount = fileList.Count;
        var xInit = 0.25f;
        var yInit = 0.075f;
        var x = -xInit;
        var y = 0f;
        foreach (var f in fileList) {
            if (x > xInit) {
                y += yInit;
                x = -xInit;
            }
            CreateToggle(x, y, f);
            x += xInit;
        }
    }

    // creer un toggle en x,y avec le texte f, avec pour parent
    private void CreateToggle(float x, float y, string f) {
        var path = "Prefabs/Button";

        GameObject obj = Resources.Load(path) as GameObject;
        GameObject toggle = Instantiate(obj) as GameObject;
        //toggle.
        Interactive interactive = toggle.GetComponent<Interactive>();
        UnityEvent unityEvent = new UnityEvent();

        // charger le modèle 3D voulu et decharger les autres
        unityEvent.AddListener(() => {
            CreateEventAndDeleteOthers(f);
        });

        interactive.OnDownEvent = unityEvent;


        LabelTheme theme = toggle.GetComponent<LabelTheme>();
        theme.Default = FormatedFileName(f);
        theme.Selected = "Placeholder";
        toggle.transform.parent = modelsMenu.transform; //this.transform;
        toggle.transform.localPosition = new Vector3(x, y, 0f);
        toggle.transform.localScale = new Vector3(1, 1, 1);
        toggle.transform.localRotation = Quaternion.Euler(0, 0, 0);

    }

    private string FormatedFileName(string f) {
        string[] tokens = f.Split('/');
        string output = "";
        foreach (var tok in tokens) {
            output = tok;
        }
        // D:/ UnityProjects / HololensProject / Assets / 3DModels / skull_downloadable / scene.gltf
        string inter = f.Substring(0, f.LastIndexOf("/"));
        output = inter.Substring(inter.LastIndexOf("/") + 1);
        //Debug.Log();
        return output.Replace(".gltf", "");
    }

    void CreateEventAndDeleteOthers(string filename) {
        DebugHelper.Instance.AddDebugText("filename :" +filename, 3);
        DeleteElementsFromGameObject(gltf);
        CreateElementAndAttachToGameObject(gltf, filename);
    }

    // Charge l'objet dans l'application et l'attache au parent fourni
    private void CreateElementAndAttachToGameObject(GameObject parent, string filename) {
        GLTFComponentPerso gLTFComponentPerso = GLTFComponentPerso.Instance;
        StartCoroutine(gLTFComponentPerso.CreateComponentFromFile(filename, parent));
    }

    private void DeleteElementsFromGameObject(GameObject go) {
        foreach (Transform child in go.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    // Toute les 60s verifie s'il y a des nouveau fichiers qui ont été ajouté
    // Nettement mieux que le fixedUpdate -> évite que le menu rame
    IEnumerator DoCheck() {
        for (; ; ) {
            if (modelsMenu.activeInHierarchy) {
                var files = new ShowFileInFolder();
                var fileList = files.GetFileList();
                if (fileList.Count != previousCount) {
                    SetupToggles(fileList);
                }

            }
            yield return new WaitForSeconds(timer);
        }
    }

    public void FixedUpdate() {
        //var files = new ShowFileInFolder();
        //var fileList = files.GetFileList();
        //if (fileList.Count != previousCount) {
        //    SetupToggles(fileList);
        //}

    }
}
