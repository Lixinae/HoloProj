using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;



#if !UNITY_EDITOR
using Windows.Storage;
using Windows.Storage.Search;
using System.Threading.Tasks;
using Windows.ApplicationModel;
#endif
public class ShowFileInFolder {

    private List<String> fileGLtfList;
    private bool copyOnce = false;

    public ShowFileInFolder() {
        fileGLtfList = new List<string>();
    }
#if !UNITY_EDITOR
    // Permet de copier les fichiers
    private async Task CopyFolderAsync(StorageFolder source, StorageFolder destinationContainer, string desiredName = null) {
        StorageFolder destinationFolder = null;
        destinationFolder = await destinationContainer.CreateFolderAsync(
            desiredName ?? source.Name, CreationCollisionOption.ReplaceExisting);

        foreach (var file in await source.GetFilesAsync()) {
            await file.CopyAsync(destinationFolder, file.Name, NameCollisionOption.ReplaceExisting);
        }
        foreach (var folder in await source.GetFoldersAsync()) {
            await CopyFolderAsync(folder, destinationFolder);
        }
    }
#endif
    // Permet de copier un dossier source dans un dossier destination en gardant l'arborescence
    // D:/bidule/chose -> E:/a/b/destination/bidule/chose
    // Recursivement
    private void CopyFolder(String source,String destination) {
        Directory.CreateDirectory(destination + Path.GetDirectoryName(source));

        foreach(var file in Directory.GetFiles(source)) {
            File.Copy(file, destination + Path.GetFileName(file));
        }
        foreach (var folder in Directory.GetDirectories(source)) {
            CopyFolder(folder,destination);
        }
    }

    private void GetAllFileNames() {
#if UNITY_EDITOR
        String path = Application.streamingAssetsPath + "/3DModels";
        string[] allfiles = Directory.GetFiles(path, "*.gltf", SearchOption.AllDirectories);
        CleanAndAddToList(allfiles);
#else
        // Copier le dossier "path" vers le current.LocalFolder

        // TODO Tester
        String path = Application.streamingAssetsPath + "/3DModels";

        String test2 = ApplicationData.Current.LocalFolder.Path;
        if (!copyOnce) {
            Debug.Log("Copying files");
            CopyFolder(path, test2);
            copyOnce = true;
        }
        Debug.Log("test bidule chose2 : " + test2);
        string[] allfilesTest = Directory.GetFiles(test2, "*.*", SearchOption.AllDirectories);

        string[] allfilesTest2 = Directory.GetFiles(test2, "*.gltf", SearchOption.AllDirectories);
        foreach(string s in allfilesTest) {
            Debug.Log("String :" + s);
        }

        foreach (string s in allfilesTest2) {
            Debug.Log("String gltf only:" + s);
        }
        CleanAndAddToList(allfilesTest2);
        

        // Voir si pas moyen de repliquer le code au dessus plutot que l'espèce de purge en dessous

        /*Task task = new Task(
            async () => {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                // On ne copie qu'une seul fois
                if (!copyOnce) {
                    StorageFolder models = await Package.Current.InstalledLocation.GetFolderAsync("StreamingAssets\\3DModels");
                    Debug.Log("Models folder: " + models.DisplayName);
                    DebugHelper.Instance.AddDebugText("Models folder: " + models.DisplayName, 4);
                    await CopyFolderAsync(models, localFolder);
                    copyOnce = true;
                }


                DebugHelper.Instance.AddDebugText("LocalFolder : " + localFolder.DisplayName, 3);
                Debug.Log("LocalFolder : " + localFolder.DisplayName);
                StorageFolderQueryResult queryResultFolder3D = localFolder.CreateFolderQuery(CommonFolderQuery.DefaultQuery);
                IReadOnlyList<StorageFolder> folderList = await queryResultFolder3D.GetFoldersAsync();
                // On recupère la liste des dossier de local folder
                // Si un dossier porte le nom de "3DModels" on scanne son contenu et on construit la liste des fichiers
                // 

                List<string> allfiles = new List<string>();

                // Correspond a string[] allfiles = Directory.GetFiles(path, "*.gltf", SearchOption.AllDirectories); dans unity
                foreach (StorageFolder folder3D in folderList) {
                    DebugHelper.Instance.AddDebugText("No unity editor Path = " + folder3D.DisplayName, 1);
                    if (folder3D.DisplayName == "3DModels") {
                        StorageFolderQueryResult queryResult = folder3D.CreateFolderQuery(CommonFolderQuery.DefaultQuery);
                        IReadOnlyList<StorageFolder> subfolderList = await queryResult.GetFoldersAsync();
                        foreach (StorageFolder folder in subfolderList) {
                            IReadOnlyList<StorageFile> fileList = await folder.GetFilesAsync();
                            foreach (StorageFile file in fileList) {
                                // Print the name of the file.
                                if (file.Name.EndsWith("*.gltf")) {
                                    string fileName = localFolder.Name + "/" + folder3D.Name + "/" + folder.Name + "/" + file.Name;
                                    Debug.Log("FileName : " + fileName);
                                    DebugHelper.Instance.AddDebugText("FileName : "+ fileName, 1);
                                    allfiles.Add(fileName);
                                }
                            }
                        }
                    }
                    else {
                        continue;
                    }
                }
                CleanAndAddToList(allfiles.ToArray());
            });
        task.Start();
        task.Wait();*/

#endif
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
