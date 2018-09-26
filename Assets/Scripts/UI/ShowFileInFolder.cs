using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

#if !UNITY_EDITOR
using Windows.Storage;
using Windows.Storage.Search;
using System.Threading.Tasks;
using Windows.ApplicationModel;
#endif

public class ShowFileInFolder {

    private List<String> fileGLtfList;
    //private bool copyOnce = false;

    public ShowFileInFolder() {
        fileGLtfList = new List<string>();
    }

    private void GetAllFileNames() {
#if UNITY_EDITOR
        String path = Application.streamingAssetsPath + "/3DModels/";
#else
        String path = ApplicationData.Current.LocalFolder.Path + "/3DModels/";
#endif
        // Si le dossier n'existe pas, on le crée pour éviter les erreurs
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
        string[] allfiles = Directory.GetFiles(path, "*.gltf", SearchOption.AllDirectories);
        CleanAndAddToList(allfiles);

    }

    public List<String> GetFileList() {
        GetAllFileNames();

        return fileGLtfList;
    }

    private void CleanAndAddToList(string[] allfiles) {
        foreach (var s in allfiles) {
            var copy = s.Replace("\\", "/");
            fileGLtfList.Add(copy);
        }
    }
}
