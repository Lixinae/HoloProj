using HoloToolkit.Examples.InteractiveElements;
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
    public GameObject cursorVisual = null;

    bool isMenuShowing = false;
    bool isDebugTextShowing = false;
    bool isCursorVisualShowing = true;
    bool wasIpConfiguratorShowing = false;

    public GameObject startMenu = null;
    public GameObject ipConfigurator = null;
    public GameObject ipButton = null;
    public GameObject startAppButton = null;

    public GameObject calibrationCubes = null;
    private GameObject axisViewer = null;
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

        if (exitConfirmMenu == null) {
            exitConfirmMenu = GameObject.Find("ExitConfirmMenu");
        }

        if (gltf == null) {
            gltf = GameObject.Find("GLTF");
        }
        if (aiguille == null) {
            aiguille = GameObject.Find("Aiguille");
        }
        if (debugText == null) {
            debugText = GameObject.Find("UITextPrefab");
        }

        if (cursorVisual == null) {
            cursorVisual = GameObject.Find("CursorVisual");
        }

        if (startMenu == null) {
            startMenu = GameObject.Find("StartMenu");
        }
        if (ipConfigurator == null) {
            ipConfigurator = GameObject.Find("IpConfigurator");
        }

        if (ipButton == null) {
            ipButton = GameObject.Find("IPButton");
        }
        if (startAppButton == null) {
            startAppButton = GameObject.Find("StartAppButton");
        }
        if (calibrationCubes == null) {
            calibrationCubes = GameObject.Find("CalibrationCubes");
        }

        if (axisViewer == null) {
            axisViewer = GameObject.Find("AxisViewer");
        }
        menuFull.SetActive(isMenuShowing);
        //modelsMenu.SetActive(false);
        mainMenu.SetActive(true);
        exitConfirmMenu.SetActive(false);
        debugText.SetActive(false);
        ipConfigurator.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        // Eventuellement se servir du pad xbox pour afficher / cacher le menu, via le bouton start ou autre
        if (Input.GetKeyDown("escape")) {
            isMenuShowing = !isMenuShowing;
            menuFull.SetActive(isMenuShowing);
            if (isMenuShowing && ipConfigurator.activeInHierarchy) {
                wasIpConfiguratorShowing = true;
                HideIpConfigurator();
            }
            else if (!isMenuShowing && wasIpConfiguratorShowing) {
                ShowIpConfigurator();
            }
        }
        // Affiche le texte de debug
        if (Input.GetKeyDown("p")) {
            isDebugTextShowing = !isDebugTextShowing;
            debugText.SetActive(isDebugTextShowing);
        }

        if (Input.GetKeyDown("n")) {
            isCursorVisualShowing = !isCursorVisualShowing;
            cursorVisual.SetActive(isCursorVisualShowing);
        }
    }

    // Affiche le menu des modeles 3D et cache le menu principal
    public void Show3DModelsMenu() {
        modelsMenu.SetActive(true);
        HideMainMenu();
    }

    // Cache le menu des modeles 3D
    public void Hide3DModelsMenu() {
        modelsMenu.SetActive(false);
    }

    // Affiche le menu principal et cache le menu des modeles 3D
    public void ShowMainMenu() {
        mainMenu.SetActive(true);
        Hide3DModelsMenu();
    }

    // Cache le menu principal
    public void HideMainMenu() {
        mainMenu.SetActive(false);
    }

    // Enlève tous les modèles 3D de la fenetre ( concerne uniquement l'objet GLTF
    public void CleanWindowFromModels() {
        foreach (Transform child in gltf.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    // Permet d'inverser les axes sur le polhemus ( il a parfois, très rarement des soucis de signe et cette fonction permet de palier à ça )
    public void InvertAxesValues() {
        UpdatePosOrient updPO = aiguille.GetComponent<UpdatePosOrient>();
        updPO.invertCoef();
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

    // Affiche l'ip configurator
    public void ShowIpConfigurator() {
        ipButton.SetActive(false);
        ipConfigurator.SetActive(true);
        startAppButton.SetActive(false);
    }

    public void CancelIpConfig() {
        ipButton.SetActive(true);
        ipConfigurator.SetActive(false);
        startAppButton.SetActive(true);
    }

    public void HideIpConfigurator() {
        ipConfigurator.SetActive(false);
    }

    public void HideStartMenu() {
        startMenu.SetActive(false);
    }

    public void StartApp() {
        HideStartMenu();
        // todo remettre une fois calibration fini
        // CalibrationsCubes.setActive(true); // -> Les autres éléments sont affiché après le calibrage
        // Demarrage du plStream après avoir réglé l'ip
        IpConfiguratorTriggers ipConfiguratorTriggers = ipConfigurator.GetComponent<IpConfiguratorTriggers>();
        string host = ipConfiguratorTriggers.IpAdress;
        string port = ipConfiguratorTriggers.Port;
        
        PlStreamCustom.Instance.StartPlStreamCustom(host, port);
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
}
