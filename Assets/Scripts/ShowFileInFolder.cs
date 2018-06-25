using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ShowFileInFolder {

    private List<String> fileGLtfList;

    public ShowFileInFolder() {
        fileGLtfList = new List<string>();
    }

    private void GetAllFileNames() {
        String path = Application.streamingAssetsPath + "/3DModels";
        string[] allfiles = Directory.GetFiles(path, "*.gltf", SearchOption.AllDirectories);
        CleanAndAddToList(allfiles);
    }

    private void CleanAndAddToList(string[] allfiles) {
        foreach (var s in allfiles) {
            var copy = s.Replace("\\", "/");
            fileGLtfList.Add(copy);
        }
    }
    public List<String> GetFileList() {
        GetAllFileNames();
        return fileGLtfList;
    }
}
