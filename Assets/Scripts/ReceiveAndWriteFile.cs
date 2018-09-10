using System.Collections.Generic;
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
			/*
			        amount_received = 0
					data = sock.recv(16)
					if not data:
						break
					#print(len(data))
					amount_expected = bytes_to_int(data)
					#print(amount_expected)
					if index == 0:
						file ="scene.bin"
					elif index == 1:
						file = "scene.gltf"
					with open(file,"wb") as f:
						while amount_received < amount_expected:
							data = sock.recv(512)
							if data:
								amount_received += len(data)
								#print(amount_received)
								f.write(data)
							else:
								break
							#print ('received "%s"' % data)
						print("Received full file")
						print("Expected = "+ str(amount_expected))
						print("Recieved = "+ str(amount_received))
						print("Size = " + str(os.path.getsize(file)))
					index+=1
			*/
			
            while (!stopListening) {
                //Debug.Log("No stop listening");
                byte[] sizeBytes = new Byte[16];
                int length;
                int index = 0;
				int fileSize = 0;
				length = stream.Read(sizeBytes, 0, sizeBytes.Length)
				if(length != 0){
					fileSize = BitConverter.ToInt32(sizebytes, 0);
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
				
				// Todo tester la methode
                int currentOffset = 0;
				byte[] receiveBytes = new Byte[512];
				string folderDataName = "" ; // Todo
				string folderName = "/3DObject";
				
				string path = Path.Combine(folderDataName + folderName, fileName + fileExtension);
				using (FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write)) {
					
					while ((length = stream.Read(receiveBytes, 0, receiveBytes.Length)) != 0) {
					// On aura toujours la taille exact de la data avec cette methode
						var data = new Byte[length];
						Array.Copy(receiveBytes, 0, data, 0, length);
						fileStream.Write(data, currentOffset, data.Length);
						currentOffset += data.Length;
					}
                }
				index++;
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
