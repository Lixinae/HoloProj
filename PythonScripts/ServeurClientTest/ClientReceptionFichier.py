
import socket
import os

import socket

### -----------------------------------------------------

def int_to_bytes(x):
    return x.to_bytes((x.bit_length() + 7) // 8, 'big')
### -----------------------------------------------------
def bytes_to_int(xbytes):
    return int.from_bytes(xbytes, 'big')

# Create a TCP/IP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# Connect the socket to the port where the server is listening
server_address = ('localhost', 5125)
print ('connecting to %s port %s' % server_address)
sock.connect(server_address)

# Manque 16 1er bytes du fichier gltf
try:
    index = 0
    while True:
        amount_received = 0
        data = sock.recv(16)
        if not data:
            break
        print(len(data))
        amount_expected = bytes_to_int(data)
        print(amount_expected)
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
        
        
finally:
    print ('closing socket')
    sock.close()


