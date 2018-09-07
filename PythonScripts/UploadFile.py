
import socket
import os


def get_filepaths(directory):
    """
    This function will generate the file names in a directory 
    tree by walking the tree either top-down or bottom-up. For each 
    directory in the tree rooted at directory top (including top itself), 
    it yields a 3-tuple (dirpath, dirnames, filenames).
    """
    file_paths = []  # List which will store all of the full filepaths.

    # Walk the tree.
    for root, directories, files in os.walk(directory):
        for filename in files:
            # Join the two strings in order to form the full filepath.
            filepath = os.path.join(root, filename)
            file_paths.append(filepath)  # Add it to the list.

    return file_paths  # Self-explanatory.


def askFolder():
    folder = "";
    while folder == "" or not os.path.isdir(folder):
      folder = input("Donner le chemin absolu vers le dossier à transferer :");
      if not os.path.isdir(folder):
          print("Ceci n'est pas un dossier valide, veuillez entrer un chemin valide")
    return folder


# Socket TCP créé pour se connecter dessus
socketTCP = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
host = ""

port = 5125
socketTCP.bind((host,port))
socketTCP.listen(5)


'''
folder = askFolder()
full_file_paths = get_filepaths(folder)
print(full_file_paths)
full_file_paths =  [x for x in full_file_paths if ((("bin" in x) or ("gltf" in x)) and ("meta" not in x))]
print(full_file_paths)


for file in full_file_paths:
    print (file)
      # Envoie du fichier
    print(os.path.getsize(file))
    #with open(file,"rb") as f:
        #print(f.read(1024))
    
# Pour chaque fichier
# Envoyer la taille
# Envoyer le fichier

# Sur unity
# -> Une taille octet lu, nouveau fichier
'''

# Voir serveur echo pour exemple de serveur echo
# Décommenter après les test

print("Starting transfer server on ("+host+","+str(port)+")")
while True:
  print("Waiting connect")
  client, address = socketTCP.accept()

  try :
    print ("{} connected".format( address ))
    # On ne demande le nom du dossier que si le client est connecté
    folder = askFolder()

    full_file_paths = get_filepaths(folder)
    # On ne garde que les fichiers voulu pour le transfert
    full_file_paths =  [x for x in full_file_paths if x.endswith(".bin") or x.endswith(".gltf")]
    if(len(full_file_paths) == 0):
        print("Erreur, aucun fichier gltf ou bin dans ce dossier, veuillez choisir un autre dossier")
    '''
# Tester ce format la
    while True:
        for file in full_file_paths:
            print (file)
            # Envoie du fichier
            with open(file,"r") as f:
                try :
                    #print(f.read(1024))
                    data = f.read(1024).encode()
                    if data:
                        #print("blabla")
                        client.send(data) # Envoie des données au client connecté
                        print("Data size :"+str(len(data)) + " sent to "+str(client))
                    else :
                        print("Error in data flow, no more data to send")
                        break
                    except:
                        break;
            data = "NewFile".encode()
            client.send(data) # Envoie l'information qu'un nouveau fichier arrive
    '''        
    for file in full_file_paths:
        print (file)
      # Envoie du fichier
        with open(file,"rb") as f:
            while True:
              try :
                #print(f.read(512))
                data = f.read(512)#.encode()
                if data:
                  #print("blabla")
                  client.send(data) # Envoie des données au client connecté
                  print("Data size :"+str(len(data)) + " sent to "+str(client))
                else :
                  print("Error in data flow, no more data to send")
                  break
              except:
                break;
  finally :
    # En cas de deconnection on ferme l'accès au client
    print ("{} disconnected".format( address ))
    client.close()
print ("Close")
socketTCP.close()
