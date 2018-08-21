using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using HoloToolkit.Unity;

#if !UNITY_EDITOR
using System.Threading.Tasks;
using Windows.Storage;
#endif

public class ReceiveAndWriteFile : Singleton<ReceiveAndWriteFile> {

    private byte[] Data = null;

    // Valeurs par défaut
    private string host = "192.168.137.1"; 
    private string port = "5125";

#if !UNITY_EDITOR
    private Windows.Networking.Sockets.StreamSocket socket;
    private Task exchangeTask;
    private Stream stream;

#endif
    private bool stopListening = false;
    /// <summary>
    /// Read-only property which returns the folder path where mesh files are stored.
    /// </summary>
    public string FolderDataName {
        get {
#if !UNITY_EDITOR
            return ApplicationData.Current.RoamingFolder.Path;
#else
                return Application.persistentDataPath;
#endif
        }
    }

    public void StartService(string host, string port) {
        ConnectUWP(host, port);
    }

#if UNITY_EDITOR
    private void ConnectUWP(string host, string port)
#else
    private async void ConnectUWP(string host, string port)
#endif
    {
#if UNITY_EDITOR
        Debug.Log("UWP TCP client used in Unity!");
#else
        try {
            socket = new Windows.Networking.Sockets.StreamSocket();
            Windows.Networking.HostName serverHost = new Windows.Networking.HostName(host);
            await socket.ConnectAsync(serverHost, port);

            stream = socket.InputStream.AsStreamForRead();
            //Initialize();
#if !UNITY_EDITOR
            exchangeTask = Task.Run(() => Read_Data());
#endif

            Debug.Log("Connected!");
        }
        catch (Exception e) {
            Debug.Log(e.ToString());
        }
#endif
    }
/*
    private byte[] GetAllData() {
        byte[] data = null;
#if !UNITY_EDITOR
        exchangeTask = Task.Run(() => Read_Data());
#endif
        return data;
    }
    */

#if !UNITY_EDITOR
    private void Read_Data() {
        stopListening = false;
        try {
            while (!stopListening) {
                byte[] receiveBytes = new Byte[1024];
                int length;
                int index = 0;
                /*
                int currentOffset = 0;
                // Avec écriture dans le fichier directement
                while ((length = stream.Read(receiveBytes, 0, receiveBytes.Length)) != 0) {
                    // Nouveau fichier en cours d'envoie
                    // On ajouter donc
                    if (length == 7) { // Correspond à la taille de la chaine "NewFile"
                        continue;
                    }
                    var data = new Byte[length];
                    Array.Copy(receiveBytes, 0, data, 0, length);
                    string fileName = "";
                    // On sais que le 1er fichier reçu sera toujours le fichier bin et que le second sera toujours le fichier gltf
                    if (index == 0) {
                        fileName = "scene.bin";
                    }
                    else if (index == 1) {
                        fileName = "scene.gltf";
                    }
                    string path = Path.Combine(FolderDataName, fileName);
                    using (FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write)) {// File.Create(path)) {
                                                                                                             // On écrit bloc par bloc
                        fileStream.Write(data, currentOffset, data.Length);
                        currentOffset += data.Length;
                    }
                }*/

                List<List<Byte[]>> allFileDataList = new List<List<byte[]>>();
                List<Byte[]> dataList = new List<byte[]>();
                // Sans l'écriture dans un fichier directement
                while ((length = stream.Read(receiveBytes, 0, receiveBytes.Length)) != 0) {
                    // Nouveau fichier en cours d'envoie
                    // On ajouter donc
                    if (length == 7) { // Correspond à la taille de la chaine "NewFile"
                        allFileDataList.Add(dataList);
                        // On remet à 0 la datalist
                        dataList = new List<byte[]>();
                        continue;
                    }
                    var data = new Byte[length];
                    Array.Copy(receiveBytes, 0, data, 0, length);

                    dataList.Add(data);
                }


                index = 0;

                // Soucis pour le moment -> 1 seul dossier et donc 1 objet 3D sur le casque à la fois.


                // Reconstruction des données reçu , moche mais ça devrais marcher
                // Possible car les fichiers sont petits, la solution serait mauvaise pour des fichiers de plus grosse taille
                foreach (var dataL in allFileDataList) {
                    // Calcul de la taille total de chaque fichier
                    int totalSize = 0;
                    foreach (var data in dataL) {
                        totalSize += data.Length;
                    }
                    byte[] endData = new byte[totalSize];
                    int previousLength = 0;
                    // Copy vers le buffer endData
                    foreach (var data in dataL) {
                        Array.Copy(data, 0, endData, previousLength, length);
                        previousLength += data.Length;
                    }
                    string fileName = "scene";
                    string fileExtension = "";
                    // On sais que le 1er fichier reçu sera toujours le fichier bin et que le second sera toujours le fichier gltf
                    if (index == 0) {
                        fileExtension = ".bin";
                    }
                    else if (index == 1) {
                        fileExtension = ".gltf";
                    }
                    string folderName = "3DObject";
                    string path = Path.Combine(FolderDataName + folderName, fileName + fileExtension);

                    // sauvegarde du fichier
                    SaveFile(folderName, fileName, fileExtension, endData);

                    // Ecriture du fichier sur le casque dans le dossier voulu
                    /*using (FileStream fileStream = File.Create(path)) {
                        // On écrit tout d'un coup
                        fileStream.Write(endData, 0, endData.Length);
                    }*/
                    index++;
                }

            }
        }
        catch (Exception e) {
            Debug.Log(e);
            Debug.Log("[polhemus] PlStream terminated in PlStream::read_liberty()");
            Console.WriteLine("[polhemus] PlStream terminated in PlStream::read_liberty().");
        }
        throw new NotImplementedException();
    }
#endif
    /// <summary>
    /// Sauvegarde le fichier avec l'extension voulu dans le dossier correspondant
    /// </summary>
    /// <param name="folderName"> Nom du dossier où stocker le fichier </param> 
    /// <param name="fileName"> Nom du fichier à stocker</param>
    /// <param name="fileExtension"> Extension du fichier voulu </param>
    /// <param name="dataToWrite"> Données à écrire dans le fichier </param>
    /// <returns></returns>
    public string SaveFile(string folderName, string fileName, string fileExtension, byte[] dataToWrite) {
        if (string.IsNullOrEmpty(folderName)) {
            throw new ArgumentException("Must specify a valid folderName.");
        }
        if (string.IsNullOrEmpty(fileExtension)) {
            throw new ArgumentException("Must specify a valid fileExtension.");
        }
        if (string.IsNullOrEmpty(fileName)) {
            throw new ArgumentException("Must specify a valid fileName.");
        }

        // Create the file.
        string folderFullName = FolderDataName + folderName;
        Debug.Log(String.Format("Saving file: {0}", Path.Combine(folderName, fileName + fileExtension)));

        using (Stream stream = OpenFileForWrite(folderName, fileName + fileExtension)) {
            stream.Write(dataToWrite, 0, dataToWrite.Length);
            stream.Flush();
        }

        Debug.Log("File saved.");

        return Path.Combine(folderName, fileName + fileExtension);
    }

    /// <summary>
    /// Opens the specified file for writing.
    /// </summary>
    /// <param name="folderName">The name of the folder containing the file.</param>
    /// <param name="fileName">The name of the file, including extension.</param>
    /// <returns>Stream used for writing the file's data.</returns>
    /// <remarks>If the specified file already exists, it will be overwritten.</remarks>
    private static Stream OpenFileForWrite(string folderName, string fileName) {
        Stream stream = null;

#if !UNITY_EDITOR
        Task task = new Task(
                        async () => {
                            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(folderName);
                            StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                            stream = await file.OpenStreamForWriteAsync();
                        });
        task.Start();
        task.Wait();
#else
            stream = new FileStream(Path.Combine(folderName, fileName), FileMode.Create, FileAccess.Write);
#endif
        return stream;
    }


    /// <summary>
    /// Permet de déconnecter le client du serveur
    /// </summary>
    private void OnApplicationQuit() {
        try {
            // signal shutdown
            stopListening = true;
#if !UNITY_EDITOR
            if (exchangeTask != null) {
                exchangeTask.Wait();
                socket.Dispose();
                stream.Close();
                socket = null;
                exchangeTask = null;
            }
#endif

        }
        catch (Exception e) {
            Debug.Log(e);
        }
    }
}
