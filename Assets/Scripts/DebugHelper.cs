using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugHelper : Singleton<DebugHelper> {

    public Text debugText = null;

    private Dictionary<int,string> dictionary;
    private List<string> debugTexts;
    // Use this for initialization
    void Start() {
        if (debugText == null) {
            debugText = GameObject.Find("Text").GetComponent<Text>();
        }
    }

    private void ResetText() {
        debugText.text = "";
    }

    public void AddDebugText(string textToAdd,int id) {
        if (dictionary.ContainsKey(id)) {
            dictionary[id] = textToAdd;
        }
        else {
            dictionary.Add(id, textToAdd);
        }
    }


    // Update is called once per frame
    void Update() {
        string output = ConcatDico();
        debugText.text = output;
    }

    private string ConcatDico() {
        string output = "";
        foreach (KeyValuePair<int, string> entry in dictionary) {
            output +="\n"+ entry.Value;
        }
        return output;
    }
}
