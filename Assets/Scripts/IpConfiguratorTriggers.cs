using HoloToolkit.Examples.InteractiveElements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class IpConfiguratorTriggers : MonoBehaviour {

    public GameObject Part1_button = null;
    public GameObject Part2_button = null;
    public GameObject Part3_button = null;
    public GameObject Part4_button = null;
    public GameObject Port_button = null;

    private GameObject TopButtons_part1 = null;
    private GameObject TopButtons_part2 = null;
    private GameObject TopButtons_part3 = null;
    private GameObject TopButtons_part4 = null;
    private GameObject TopButtons_port = null;

    private GameObject BottomButtons_part1 = null;
    private GameObject BottomButtons_part2 = null;
    private GameObject BottomButtons_part3 = null;
    private GameObject BottomButtons_part4 = null;
    private GameObject BottomButtons_port = null;

    // Use this for initialization
    void Start() {
        if (Part1_button == null) {
            Part1_button = GameObject.Find("Part1_button");
        }

        if (Part2_button == null) {
            Part2_button = GameObject.Find("Part4_button");
        }

        if (Part3_button == null) {
            Part3_button = GameObject.Find("Part4_button");
        }

        if (Part4_button == null) {
            Part4_button = GameObject.Find("Part4_button");
        }

        if (Port_button == null) {
            Port_button = GameObject.Find("Port_Button");
        }

        if (TopButtons_part1 == null) {
            TopButtons_part1 = GameObject.Find("TopButtons_part1");
        }
        if (TopButtons_part2 == null) {
            TopButtons_part2 = GameObject.Find("TopButtons_part2");
        }
        if (TopButtons_part3 == null) {
            TopButtons_part3 = GameObject.Find("TopButtons_part3");
        }
        if (TopButtons_part4 == null) {
            TopButtons_part4 = GameObject.Find("TopButtons_part4");
        }
        if (TopButtons_port == null) {
            TopButtons_port = GameObject.Find("TopButtons_port");
        }

        if (BottomButtons_part1 == null) {
            BottomButtons_part1 = GameObject.Find("BottomButtons_part1");
        }
        if (BottomButtons_part2 == null) {
            BottomButtons_part2 = GameObject.Find("BottomButtons_part2");
        }
        if (BottomButtons_part3 == null) {
            BottomButtons_part3 = GameObject.Find("BottomButtons_part3");
        }
        if (BottomButtons_part4 == null) {
            BottomButtons_part4 = GameObject.Find("BottomButtons_part4");
        }
        if (BottomButtons_port == null) {
            BottomButtons_port = GameObject.Find("BottomButtons_port");
        }
    }

    // Sauvegarde la configuration IP dans un fichier
    public void SaveIpConfig() {
#if !UNITY_EDITOR
        Task task = new Task(async () => {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("ip.txt");
            string toWrite = string.Concat(Part1_button.GetComponent<LabelTheme>().Default, ".",
                Part2_button.GetComponent<LabelTheme>().Default, ".",
                Part3_button.GetComponent<LabelTheme>().Default, ".",
                Part4_button.GetComponent<LabelTheme>().Default, ":",
                Port_button.GetComponent<LabelTheme>().Default);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, toWrite);
        });
        task.Start();
        task.Wait();
#else
        string toWrite = string.Concat(Part1_button.GetComponent<LabelTheme>().Default, ".",
                Part2_button.GetComponent<LabelTheme>().Default, ".",
                Part3_button.GetComponent<LabelTheme>().Default, ".",
                Part4_button.GetComponent<LabelTheme>().Default, ":",
                Port_button.GetComponent<LabelTheme>().Default);
        File.WriteAllText(Application.streamingAssetsPath + "/ip.txt",toWrite);
#endif
    }

    public void CancelIpConfig() {

    }

    /// <summary>
    /// Si on clique sur les boutons avec l'adresse IP on peut afficher ou cacher les bouttons de réglage
    /// </summary>

    public void ChangeStateButtonsPart1() {
        bool active = TopButtons_part1.activeInHierarchy;
        TopButtons_part1.SetActive(!active);
        BottomButtons_part1.SetActive(!active);
    }

    public void ChangeStateButtonsPart2() {
        bool active = TopButtons_part2.activeInHierarchy;
        TopButtons_part2.SetActive(!active);
        BottomButtons_part2.SetActive(!active);
    }

    public void ChangeStateButtonsPart3() {
        bool active = TopButtons_part3.activeInHierarchy;
        TopButtons_part3.SetActive(!active);
        BottomButtons_part3.SetActive(!active);
    }

    public void ChangeStateButtonsPart4() {
        bool active = TopButtons_part4.activeInHierarchy;
        TopButtons_part4.SetActive(!active);
        BottomButtons_part4.SetActive(!active);
    }

    public void ChangeStateButtonsPort() {
        bool active = TopButtons_port.activeInHierarchy;
        TopButtons_port.SetActive(!active);
        BottomButtons_port.SetActive(!active);
    }

    // Parse le caractère et retire 1 à la valeur numérique
    private int ParseTextMoins(char text) {
        int x = 0;
        int.TryParse(text.ToString(), out x);
        x++;
        if (x > 9 || x < 0) {
            x = 0;
        }
        return x;
    }

    // Parse le caractère et ajoute 1 à la valeur numérique
    private int ParseTextPlus(char text) {
        int x = 0;
        int.TryParse(text.ToString(), out x);
        x++;
        if (x > 9 || x < 0) {
            x = 0;
        }
        return x;
    }

    /// <summary>
    /// Modificateurs partie 1
    /// </summary>
    /// 
    public void UpCentaine1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[0]);
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }

    public void DownCentaine1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextMoins(text[0]);
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }

    public void UpDizaine1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[1]);
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }

    public void DownDizaine1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextMoins(text[1]);
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }

    public void UpUnite1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[2]);
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }

    public void DownUnite1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        Debug.Log(text);
        int x = ParseTextMoins(text[2]);
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }

    /// <summary>
    /// Modificateurs partie 2
    /// </summary>
    /// 
    public void UpCentaine2() {
        LabelTheme theme = Part2_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[0]);
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }

    public void DownCentaine2() {
        LabelTheme theme = Part2_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextMoins(text[0]);
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }

    public void UpDizaine2() {
        LabelTheme theme = Part2_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[1]);
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }

    public void DownDizaine2() {
        LabelTheme theme = Part2_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextMoins(text[1]);
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }

    public void UpUnite2() {
        LabelTheme theme = Part2_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[2]);
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }

    public void DownUnite2() {
        LabelTheme theme = Part2_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        Debug.Log(text);
        int x = ParseTextMoins(text[2]);
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }

    /// <summary>
    /// Modificateurs partie 3
    /// </summary>
    /// 

    public void UpCentaine3() {
        LabelTheme theme = Part3_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[0]);
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }

    public void DownCentaine3() {
        LabelTheme theme = Part3_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextMoins(text[0]);
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }

    public void UpDizaine3() {
        LabelTheme theme = Part3_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[1]);
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }

    public void DownDizaine3() {
        LabelTheme theme = Part3_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextMoins(text[1]);
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }

    public void UpUnite3() {
        LabelTheme theme = Part3_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[2]);
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }

    public void DownUnite3() {
        LabelTheme theme = Part3_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        Debug.Log(text);
        int x = ParseTextMoins(text[2]);
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }

    /// <summary>
    /// Modificateurs partie 4
    /// </summary>
    /// 

    public void UpCentaine4() {
        LabelTheme theme = Part4_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[0]);
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }

    public void DownCentaine4() {
        LabelTheme theme = Part4_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextMoins(text[0]);
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }

    public void UpDizaine4() {
        LabelTheme theme = Part4_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[1]);
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }

    public void DownDizaine4() {
        LabelTheme theme = Part4_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextMoins(text[1]);
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }

    public void UpUnite4() {
        LabelTheme theme = Part4_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[2]);
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }

    public void DownUnite4() {
        LabelTheme theme = Part4_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        Debug.Log(text);
        int x = ParseTextMoins(text[2]);
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }

    // Port triggers

    public void UpMilliersPort() {
        LabelTheme theme = Port_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[0]);
        theme.Default = string.Concat(x.ToString(), text[1], text[2], text[3]);
    }

    public void DownMilliersPort() {
        LabelTheme theme = Port_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextMoins(text[0]);
        theme.Default = string.Concat(x.ToString(), text[1], text[2], text[3]);
    }

    public void UpCentainePort() {
        LabelTheme theme = Port_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[1]);
        theme.Default = string.Concat(text[0], x.ToString(), text[2], text[3]);
    }

    public void DownCentainePort() {
        LabelTheme theme = Port_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextMoins(text[1]);
        theme.Default = string.Concat(text[0], x.ToString(), text[2], text[3]);
    }

    public void UpDizainePort() {
        LabelTheme theme = Port_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[2]);
        theme.Default = string.Concat(text[0], text[1], x.ToString(), text[3]);
    }

    public void DownDizainePort() {
        LabelTheme theme = Port_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextMoins(text[2]);
        theme.Default = string.Concat(text[0], text[1], x.ToString(), text[3]);
    }

    public void UpUnitePort() {
        LabelTheme theme = Port_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[3]);
        theme.Default = string.Concat(text[0], text[1], text[2], x.ToString());
    }

    public void DownUnitePort() {
        LabelTheme theme = Port_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        Debug.Log(text);
        int x = ParseTextMoins(text[3]);
        theme.Default = string.Concat(text[0], text[1], text[2], x.ToString());
    }

}
