
import socket
import os


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

while True:
  print("Waiting connect")
  client, address = socketTCP.accept()
  try :
    print ("{} connected".format( address ))
    while True:
        #data = "Bidule".encode()
        rec = client.recv(16)
        if rec:
            print("Received %s"%rec)
            client.send(rec)
        else:
            break

  finally :
    # En cas de deconnection on ferme l'accès au client
    print ("{} disconnected".format( address ))
    client.close()
print ("Close")
socketTCP.close()
