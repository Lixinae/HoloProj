import socket

sockUDP = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sockUDP.bind(('', 5123))


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
          client.send(data)
        else :
          print("Error in data flow")
          break
      except:
        break;
      #socketTCP.send(data) #et on les envoie au client/hololens connecté
  finally :
    print ("{} disconnected".format( address ))
    client.close()
print ("Close")
socketTCP.close()

