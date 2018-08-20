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
#if !UNITY_EDITOR
    // Permet de copier les fichiers
    /*private async Task CopyFolderAsync(StorageFolder source, StorageFolder destinationContainer, string desiredName = null) {
        StorageFolder destinationFolder = null;
        destinationFolder = await destinationContainer.CreateFolderAsync(
            desiredName ?? source.Name, CreationCollisionOption.ReplaceExisting);

        foreach (var file in await source.GetFilesAsync()) {
            await file.CopyAsync(destinationFolder, file.Name, NameCollisionOption.ReplaceExisting);
        }
        foreach (var folder in await source.GetFoldersAsync()) {
            await CopyFolderAsync(folder, destinationFolder);
        }
    }*/
#endif
    // Permet de copier un dossier source dans un dossier destination en gardant l'arborescence
    // D:/bidule/chose -> E:/a/b/destination/bidule/chose
    // Recursivement
    /*private void CopyFolder(String source, String destination) {
        Directory.CreateDirectory(destination + "/" + Path.GetDirectoryName(source));

        foreach (var file in Directory.GetFiles(source)) {
            File.Copy(file, destination + Path.GetFileName(file));
        }
        foreach (var folder in Directory.GetDirectories(source)) {
            CopyFolder(folder, destination);
        }
    }*/

    private void GetAllFileNames() {
#if UNITY_EDITOR
        String path = Application.streamingAssetsPath + "/3DModels";
#else
        String path = ApplicationData.Current.RoamingFolder.Path;
#endif
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
