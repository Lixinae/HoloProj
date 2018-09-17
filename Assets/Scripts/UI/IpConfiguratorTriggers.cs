using HoloToolkit.Examples.InteractiveElements;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
#if !UNITY_EDITOR
using Windows.Storage;
#endif


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

    public GameObject IpConfigurator = null;

    public string IpAdress = null;
    public string Port = null;

    private bool Loaded = false;
    // Use this for initialization
    void Awake() {
        if (IpConfigurator == null) {
            IpConfigurator = GameObject.Find("IpConfigurator");
        }

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

        LoadIpConfig();
    }

    public bool IsLoaded() {
        return Loaded;
    }

    private void LoadIpConfig() {
        bool error = false;
        string info = null;

        try {
#if !UNITY_EDITOR
            info = ReadHostFromFileAsync().Result;
#else
            info = File.ReadAllText(Application.streamingAssetsPath + "/ip.txt");
#endif
        }
        catch (Exception e) {
            Debug.Log(e);
            error = true;
        }
        string host;
        string port;
        // Si on a pas pu lire le fichier , on met des valeurs par defaut
        if (error) {
            host = "192.168.137.1";
            port = "5124";
        }
        else {
            host = info.Split(':')[0];
            port = info.Split(':')[1];
        }
        //while (info == null) ;
        IpAdress = host;
        Port = port;
        WriteLabelThemesOnButtons(host, port);
        Loaded = true;
    }

    /// <summary>
    /// Ajout des zero au debut de la chaine pour la manipulation avec les bouttons
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    private string FillMissingZeroIP(string entry) {

        string output;
        if (entry.Length == 2) {
            output = string.Concat('0', entry);
        }
        if (entry.Length == 1) {
            output = string.Concat('0', '0', entry);
        }
        else {
            output = entry;
        }
        return output;
    }

    private string FillMissingZeroPort(string entry) {

        string output;
        if (entry.Length == 3) {
            output = string.Concat('0', entry);
        }
        if (entry.Length == 2) {
            output = string.Concat('0', '0', entry);
        }
        if (entry.Length == 1) {
            output = string.Concat('0', '0', '0', entry);
        }
        else {
            output = entry;
        }
        return output;
    }
    /// <summary>
    /// Ecrit l'hote et le port sur les boutons correspondant
    /// </summary>
    /// <param name="host"> Hôte à écrire sur les boutons</param>
    /// <param name="port"> Port à écrire sur le bouton </param>
    private void WriteLabelThemesOnButtons(string host, string port) {
        string[] hostParts = host.Split('.');
        LabelTheme part1Theme = Part1_button.GetComponent<LabelTheme>();
        part1Theme.Default = FillMissingZeroIP(hostParts[0]);

        LabelTheme part2Theme = Part2_button.GetComponent<LabelTheme>();
        part2Theme.Default = FillMissingZeroIP(hostParts[1]);

        LabelTheme part3Theme = Part3_button.GetComponent<LabelTheme>();
        part3Theme.Default = FillMissingZeroIP(hostParts[2]);

        LabelTheme part4Theme = Part4_button.GetComponent<LabelTheme>();
        part4Theme.Default = FillMissingZeroIP(hostParts[3]);

        LabelTheme portTheme = Port_button.GetComponent<LabelTheme>();
        portTheme.Default = FillMissingZeroPort(port);
    }

#if !UNITY_EDITOR
    /// <summary>
    /// Lis l'adresse IP dans un fichier en utilisant une methode async
    /// </summary>
    /// <returns></returns>
    private async Task<string> ReadHostFromFileAsync() {
        StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        StorageFile sampleFile = await storageFolder.GetFileAsync("ip.txt");
        return await FileIO.ReadTextAsync(sampleFile);
    }

#endif

    public string GetIpAdress() {
        string part1 = Part1_button.GetComponent<LabelTheme>().Default;
        string part2 = Part2_button.GetComponent<LabelTheme>().Default;
        string part3 = Part3_button.GetComponent<LabelTheme>().Default;
        string part4 = Part4_button.GetComponent<LabelTheme>().Default;
        string hostTrimed = string.Concat(TrimValFromLeftZero(part1), ".",
                    TrimValFromLeftZero(part2), ".",
                    TrimValFromLeftZero(part3), ".",
                    TrimValFromLeftZero(part4));
        return hostTrimed;
    }

    public string GetPort() {
        return TrimValFromLeftZero(Port);
    }

    /// <summary>
    /// Supprime les zero a gauche dans la chaine, en en laissant au moins 1 si jamais il n'y a que des 0
    /// </summary>
    private string TrimValFromLeftZero(string entry) {
        string output;
        int x = 0;
        var count = 0;
        // Lis de gauche à droite
        foreach (var c in entry) {
            int.TryParse(c.ToString(), out x);
            if (x != 0) {
                break;
            }

            if (x == 0) {
                count++;
            }
        }
        output = entry.Substring(count);
        // Si == 0 -> Que des 0 dans la chaine et donc il faut en garder au moins 1
        if (output.Length == 0) {
            output = "0";
        }

        return output;
    }

    /// <summary>
    /// Sauvegarde la configuration IP dans un fichier
    /// </summary>
    public void SaveIpConfig() {
#if !UNITY_EDITOR
        Task task = new Task(async () => {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.CreateFileAsync("ip.txt", CreationCollisionOption.ReplaceExisting);


            string part1 = Part1_button.GetComponent<LabelTheme>().Default;
            string part2 = Part2_button.GetComponent<LabelTheme>().Default;
            string part3 = Part3_button.GetComponent<LabelTheme>().Default;
            string part4 = Part4_button.GetComponent<LabelTheme>().Default;
            string host = string.Concat(part1, ".",
                    part2, ".",
                    part3, ".",
                    part4);
            string port = Port_button.GetComponent<LabelTheme>().Default;
            string toWrite = string.Concat(host, ":", port);


            string hostTrimed = string.Concat(TrimValFromLeftZero(part1), ".",
                    TrimValFromLeftZero(part2), ".",
                    TrimValFromLeftZero(part3), ".",
                    TrimValFromLeftZero(part4));
            string portTrimed = TrimValFromLeftZero(Port_button.GetComponent<LabelTheme>().Default);

            IpAdress = hostTrimed;
            Port = portTrimed;

            await FileIO.WriteTextAsync(sampleFile, toWrite);
        });
        task.Start();
        task.Wait();
#else
        string part1= Part1_button.GetComponent<LabelTheme>().Default;
        string part2= Part2_button.GetComponent<LabelTheme>().Default;
        string part3= Part3_button.GetComponent<LabelTheme>().Default;
        string part4= Part4_button.GetComponent<LabelTheme>().Default;
        string host = string.Concat(part1, ".",
                part2, ".",
                part3, ".",
                part4);
        string port = Port_button.GetComponent<LabelTheme>().Default;
        string toWrite = string.Concat(host, ":", port);

        string hostTrimed = string.Concat(TrimValFromLeftZero(part1), ".",
                    TrimValFromLeftZero(part2), ".",
                    TrimValFromLeftZero(part3), ".",
                    TrimValFromLeftZero(part4));
        string portTrimed = TrimValFromLeftZero(Port_button.GetComponent<LabelTheme>().Default);       
        IpAdress = hostTrimed;
        Port = portTrimed;
        File.WriteAllText(Application.streamingAssetsPath + "/ip.txt",toWrite);
#endif


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

    /// <summary>
    /// Parse le caractère et retire 1 à la valeur numérique
    /// </summary>
    private int ParseTextMoins(char text) {
        int x = 0;
        int.TryParse(text.ToString(), out x);
        x--;
        if (x > 9 || x < 0) {
            x = 0;
        }
        return x;
    }
    /// <summary>
    /// Parse le caractère et ajoute 1 à la valeur numérique
    /// </summary>
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
    /// Partie 1
    /// </summary>

    /// <summary>
    /// Augmente le chiffre de centaine
    /// </summary>
    public void UpCentaine1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[0]);
        Debug.Log(x);
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }
    /// <summary>
    /// Diminue le chiffre de centaine
    /// </summary>
    public void DownCentaine1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextMoins(text[0]);
        Debug.Log(x);
        theme.Default = string.Concat(x.ToString(), text[1], text[2]);
    }

    public void UpDizaine1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[1]);
        Debug.Log(x);
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }

    public void DownDizaine1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextMoins(text[1]);
        Debug.Log(x);
        theme.Default = string.Concat(text[0], x.ToString(), text[2]);
    }

    public void UpUnite1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextPlus(text[2]);
        Debug.Log(x);
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }

    public void DownUnite1() {
        LabelTheme theme = Part1_button.GetComponent<LabelTheme>();
        string text = theme.Default;
        int x = ParseTextMoins(text[2]);
        Debug.Log(x);
        theme.Default = string.Concat(text[0], text[1], x.ToString());
    }

    /// <summary>
    /// Partie 2
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
    /// Partie 3
    /// </summary>

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
    /// Partie 4
    /// </summary>

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

    /// <summary>
    /// Partie Port
    /// </summary>

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
