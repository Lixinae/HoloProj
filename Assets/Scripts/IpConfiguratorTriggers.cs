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
    public GameObject Port_Button = null;

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

        if (Port_Button == null) {
            Port_Button = GameObject.Find("Port_Button");
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void SaveIpConfig() {
#if !UNITY_EDITOR
        Task task = new Task(async () => {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("ip.txt");
            string toWrite = string.Concat(Part1_button.GetComponent<LabelTheme>().Default,".",
                Part2_button.GetComponent<LabelTheme>().Default, ".",
                Part3_button.GetComponent<LabelTheme>().Default, ".",
                Part4_button.GetComponent<LabelTheme>().Default,":",
                Port_Button.GetComponent<LabelTheme>().Default);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, toWrite);
        });
        task.Start();
        task.Wait();
#else
        string toWrite = string.Concat(Part1_button.GetComponent<LabelTheme>().Default, ".",
                Part2_button.GetComponent<LabelTheme>().Default, ".",
                Part3_button.GetComponent<LabelTheme>().Default, ".",
                Part4_button.GetComponent<LabelTheme>().Default, ":",
                Port_Button.GetComponent<LabelTheme>().Default);
        File.WriteAllText(Application.streamingAssetsPath + "/ip.txt",toWrite);
#endif
    }

    public void CancelIpConfig() {

    }

    // Modificateurs partie 1
    public void ShowModifiersPart1() {
        throw new NotImplementedException();
    }

    public void HideModifiersPart1() {
        throw new NotImplementedException();
    }

    public void UpCentaine1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[0].ToString(), out x);
        x++;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }

    public void DownCentaine1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[0].ToString(), out x);
        x--;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }
    public void UpDizaine1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[1].ToString(), out x);
        x++;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }

    public void DownDizaine1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[1].ToString(), out x);
        x--;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }
    public void UpUnite1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[2].ToString(), out x);
        x++;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }

    public void DownUnite1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        Debug.Log(text);
        int x = 0;
        int.TryParse(text[2].ToString(), out x);
        x--;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }
    // Modificateurs partie 2
    public void ShowModifiersPart2() {
        throw new NotImplementedException();
    }

    public void HideModifiersPart2() {
        throw new NotImplementedException();
    }

    public void UpCentaine2() {
        LabelTheme theme = Part2_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[0].ToString(), out x);
        x++;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }

    public void DownCentaine2() {
        LabelTheme theme = Part2_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[0].ToString(), out x);
        x--;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }
    public void UpDizaine2() {
        LabelTheme theme = Part2_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[1].ToString(), out x);
        x++;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }

    public void DownDizaine2() {
        LabelTheme theme = Part2_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[1].ToString(), out x);
        x--;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }
    public void UpUnite2() {
        LabelTheme theme = Part2_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[2].ToString(), out x);
        x++;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }

    public void DownUnite2() {
        LabelTheme theme = Part2_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        Debug.Log(text);
        int x = 0;
        int.TryParse(text[2].ToString(), out x);
        x--;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }



    // Modificateurs partie 3
    public void ShowModifiersPart3() {
        throw new NotImplementedException();
    }

    public void HideModifiersPart3() {
        throw new NotImplementedException();
    }

    public void UpCentaine3() {
        LabelTheme theme = Part3_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[0].ToString(), out x);
        x++;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }

    public void DownCentaine3() {
        LabelTheme theme = Part3_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[0].ToString(), out x);
        x--;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }
    public void UpDizaine3() {
        LabelTheme theme = Part3_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[1].ToString(), out x);
        x++;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }

    public void DownDizaine3() {
        LabelTheme theme = Part3_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[1].ToString(), out x);
        x--;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }
    public void UpUnite3() {
        LabelTheme theme = Part3_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[2].ToString(), out x);
        x++;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }

    public void DownUnite3() {
        LabelTheme theme = Part3_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        Debug.Log(text);
        int x = 0;
        int.TryParse(text[2].ToString(), out x);
        x--;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }



    // Modificateurs partie 4
    public void ShowModifiersPart4() {
        throw new NotImplementedException();
    }

    public void HideModifiersPart4() {
        throw new NotImplementedException();
    }
    public void UpCentaine4() {
        LabelTheme theme = Part4_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[0].ToString(), out x);
        x++;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }

    public void DownCentaine4() {
        LabelTheme theme = Part4_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[0].ToString(), out x);
        x--;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }
    public void UpDizaine4() {
        LabelTheme theme = Part4_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[1].ToString(), out x);
        x++;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }

    public void DownDizaine4() {
        LabelTheme theme = Part4_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[1].ToString(), out x);
        x--;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }
    public void UpUnite4() {
        LabelTheme theme = Part4_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = 0;
        int.TryParse(text[2].ToString(), out x);
        x++;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }

    public void DownUnite4() {
        LabelTheme theme = Part4_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        Debug.Log(text);
        int x = 0;
        int.TryParse(text[2].ToString(), out x);
        x--;
        if (x > 9 || x < 0) {
            x = 0;
        }
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }

}
