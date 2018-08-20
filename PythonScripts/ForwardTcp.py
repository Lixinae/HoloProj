import socket

# Socket UDP de l'application "UnityExporter"
sockUDP = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sockUDP.bind(('', 5123))

# Socket TCP créé pour se connecter dessus
socketTCP = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
host = ""
port = 5124
socketTCP.bind((host,port))
socketTCP.listen(5)

print("Starting transfer server on ("+host+","+str(port)+")")
while True:
  print("Waiting connect")
  client, address = socketTCP.accept()
  try :
    print ("{} connected".format( address ))
    while True:
      data, addr = sockUDP.recvfrom(40) #on recoit les messages polhemus
      #print("Data :"+str(data))
      try :
        if data:
          client.send(data) # Envoie des données au client connecté
        else :
          print("Error in data flow")
          break
      except:
        break;
  finally :
    # En cas de deconnection on ferme l'accès au client
    print ("{} disconnected".format( address ))
    client.close()
print ("Close")
socketTCP.close()

