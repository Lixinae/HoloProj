using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


#if !UNITY_EDITOR
using Windows.Storage;
using Windows.Storage.Search;
using System.Threading.Tasks;
#endif
public class ShowFileInFolder {

    private List<String> fileGLtfList;

    public ShowFileInFolder() {
        fileGLtfList = new List<string>();
    }

    private void GetAllFileNames() {
#if UNITY_EDITOR
        String path = Application.streamingAssetsPath + "/3DModels";
        string[] allfiles = Directory.GetFiles(path, "*.gltf", SearchOption.AllDirectories);
        CleanAndAddToList(allfiles);
#else
        Task task = new Task(
            async () => {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
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
        task.Wait();

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
