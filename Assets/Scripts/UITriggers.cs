using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class UITriggers : MonoBehaviour {

    public GameObject menuFull = null;
    public GameObject modelsMenu = null; 
    public GameObject mainMenu = null;
    public GameObject exitConfirmMenu = null;
    public GameObject gltf = null;
    public GameObject aiguille = null;
    public GameObject debugText = null;

    bool isMenuShowing = false;
    bool isDebugTextShowing = false;
    // Use this for initialization
    void Awake() {
        if (menuFull == null) {
            menuFull = GameObject.Find("MenuItems");
        }
        if (modelsMenu == null) {
            modelsMenu = GameObject.Find("ModelsMenu");
        }
        if (mainMenu == null) {
            mainMenu = GameObject.Find("MainMenu");
        }
        if(exitConfirmMenu == null) {
            exitConfirmMenu = GameObject.Find("ExitConfirmMenu");
        }

        if(gltf == null) {
            gltf = GameObject.Find("GLTF");
        }
        if(aiguille == null) {
            aiguille = GameObject.Find("Aiguille");
        }
        if(debugText == null) {
            debugText = GameObject.Find("UITextPrefab");
        }

        menuFull.SetActive(isMenuShowing);
        //modelsMenu.SetActive(false);
        mainMenu.SetActive(true);
        exitConfirmMenu.SetActive(false);
        debugText.SetActive(false);
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("escape")) { // Eventuellement se servir du pad xbox pour afficher / cacher le menu, via le bouton start ou autre
            isMenuShowing = !isMenuShowing;
            menuFull.SetActive(isMenuShowing);
            
        }
        if (Input.GetKeyDown("p")) {
            isDebugTextShowing = !isDebugTextShowing;
            debugText.SetActive(isDebugTextShowing);
        }
    }

    public void Show3DModelsMenu() {
        modelsMenu.SetActive(true);
        HideMainMenu();
    }

    public void Hide3DModelsMenu() {
        modelsMenu.SetActive(false);
    }

    public void ShowMainMenu() {
        mainMenu.SetActive(true);
        Hide3DModelsMenu();
    }

    public void HideMainMenu() {
        mainMenu.SetActive(false);
    }

    public void CleanWindowFromModels() {
        
        foreach (Transform child in gltf.transform) {
            GameObject.Destroy(child.gameObject);
        }
        //GCSettings.GCLargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce; ;
        //GC.Collect();
        //GC.WaitForPendingFinalizers();
        //GC.Collect();
        //GC.WaitForPendingFinalizers();
    }

    public void InvertAxesValues() {
        UpdatePosOrient updPO = aiguille.GetComponent<UpdatePosOrient>();
        updPO.invertCoef();
        Debug.Log("Coef inversé !");
    }

    public void ExitProgram() {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void NoExit() {
        exitConfirmMenu.SetActive(false);
    }

    public void DisplayExitConfirm() {
        exitConfirmMenu.SetActive(true);
    }
}
