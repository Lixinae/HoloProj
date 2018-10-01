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
        string path = Application.streamingAssetsPath + "/3DModels/";
#else
        string path2 = ApplicationData.Current.LocalFolder.Path + "/" + "3DModels";
        //string path = KnownFolders.VideosLibrary.Path + "/3DModels/";

        // Cette ligne n'est vraiment que pour la démo !
        // On ne l'utilisera pas
        string path = Application.streamingAssetsPath + "/" + "3DModels"; // UNIQUEMENT POUR UTILISER LE DOSSIER streaming assets et donc pas le transfert de fichier

#endif
        // Faire une fonction pour iterer sur plusieurs path
        // maFonction(string... paths)
        // for (string path in paths){

        //}

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
