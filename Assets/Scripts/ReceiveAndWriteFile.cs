﻿using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using HoloToolkit.Unity;
using System.Net.Sockets;
using System.Threading;

#if !UNITY_EDITOR
using System.Threading.Tasks;
using Windows.Storage;
#endif

public class ReceiveAndWriteFile : Singleton<ReceiveAndWriteFile> {

    private byte[] Data = null;

    private bool stopListening = false;
    // Valeurs par défaut
    private string host = "192.168.137.1";
    private string port = "5125";

#if !UNITY_EDITOR
    private bool _useUWP = true;
    private Windows.Networking.Sockets.StreamSocket socket;
    private Task exchangeTask;
    private Stream stream;
#endif

#if UNITY_EDITOR
    private bool _useUWP = false;
    private TcpClient tcpClient; // PC ajout
    private NetworkStream stream;
    private Thread conThread;
#endif

    public void SetupHostAndPort(string host, string port) {
        this.host = host;
        this.port = port;
    }

    // TODO Rajouter un bouton pour demander l'actualisation

    public void ConnectAndGetFile(string host, string port) {
        if (_useUWP) {
            ConnectUWP(host, port);
        }
        else {
            ConnectUnity(host, port);
        }
    }

    public void ConnectAndGetFile() {
        if (_useUWP) {
            ConnectUWP(host, port);
        }
        else {
            ConnectUnity(host, port);
        }
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

    private void ConnectUnity(string host, string port) {
#if !UNITY_EDITOR
        Debug.Log("Unity TCP client used in UWP!");
#else
        try {
            tcpClient = new System.Net.Sockets.TcpClient(host, Int32.Parse(port));
            stream = tcpClient.GetStream();
            Debug.Log("Connected!");
            conThread = new Thread(new ThreadStart(Read_Data));
            // start the read thread
            conThread.Start();
        }
        catch (Exception e) {
            Debug.Log(e.ToString());
        }
#endif
    }


    private void Read_DataUpdated() {

    }

    /// <summary>
    /// Lis les données en entrée
    /// </summary>
    private void Read_Data() {
        stopListening = false;
        try {
            while (!stopListening) {
                //Debug.Log("No stop listening");
                byte[] receiveBytes = new Byte[1024];
                int length;
                int index = 0;

                
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
                    string path = Path.Combine("FolderDataName", fileName); // todo cjhange
                    using (FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write)) {// File.Create(path)) {
                                                                                                             // On écrit bloc par bloc
                        fileStream.Write(data, currentOffset, data.Length);
                        currentOffset += data.Length;
                    }
                }
                /*
                List<List<Byte[]>> allFileDataList = new List<List<byte[]>>();
                List<Byte[]> dataList = new List<byte[]>();
                // Bugs persistent

                // Sans l'écriture dans un fichier directement
                while ((length = stream.Read(receiveBytes, 0, receiveBytes.Length)) != 0) {
                    // Nouveau fichier en cours d'envoie
                    // On ajouter donc
                    Debug.Log("Received data size :" + length);
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

                //Debug.Log("Received all data, processing");
                index = 0;

                // Soucis pour le moment -> 1 seul dossier et donc 1 objet 3D sur le casque à la fois.

                // Todo Problème avec ordre de reception des données !!!!
                // Voir comment corriger ça


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
                        // string receivedString = System.Text.Encoding.UTF8.GetString(endData)
                        //Debug.Log(receivedString);
                    }
                    string folderName = "\\3DObject\\";
                    //string path = Path.Combine(FolderDataName + folderName, fileName + fileExtension);

                    // sauvegarde du fichier
#if !UNITY_EDITOR
                    SaveFile(folderName, fileName, fileExtension, endData);
#endif

#if UNITY_EDITOR
                    
                    string folderFullName = ".\\testFolder" + folderName;
                    if(!Directory.Exists(folderFullName)){
                        Directory.CreateDirectory(folderFullName);
                    }
                    Debug.Log(String.Format("Saving file: {0}", Path.Combine(folderFullName, fileName + fileExtension)));
                    using (var fs = File.Create(folderFullName + fileName + fileExtension)) {
                        fs.Write(endData, 0, endData.Length);
                        fs.Flush();
                    }
#endif
                    // Ecriture du fichier sur le casque dans le dossier voulu
                    using (FileStream fileStream = File.Create(path)) {
                        // On écrit tout d'un coup
                        fileStream.Write(endData, 0, endData.Length);
                    }
                    index++;*/
                

            }
        }
        catch (Exception e) {
            Debug.Log(e);
            Debug.Log("[polhemus] PlStream terminated in PlStream::read_liberty()");
            Console.WriteLine("[polhemus] PlStream terminated in PlStream::read_liberty().");
        }
    }

    /// <summary>
    /// Sauvegarde le fichier avec l'extension voulu dans le dossier correspondant
    /// </summary>
    /// <param name="folderName"> Nom du dossier où stocker le fichier </param> 
    /// <param name="fileName"> Nom du fichier à stocker</param>
    /// <param name="fileExtension"> Extension du fichier voulu </param>
    /// <param name="dataToWrite"> Données à écrire dans le fichier </param>
    /// <returns></returns>
#if !UNITY_EDITOR
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
        string folderFullName = ApplicationData.Current.RoamingFolder.Path + folderName;
        Debug.Log(String.Format("Saving file: {0}", Path.Combine(folderFullName, fileName + fileExtension)));

        using (Stream stream = OpenFileForWrite(folderName, fileName + fileExtension)) {
            stream.Write(dataToWrite, 0, dataToWrite.Length);
            stream.Flush();
        }

        Debug.Log("File saved.");

        return Path.Combine(folderFullName, fileName + fileExtension);
    }
#endif
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
                            // Peut etre devoir creer le dossier avant l'ouverture du fichier
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