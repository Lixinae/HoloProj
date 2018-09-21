import socket
import sys

# Create a TCP/IP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

outsocket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
# Bind the socket to the port
server_address = ('localhost', 5123)
print ('starting up on %s port %s' % server_address)
sock.bind(server_address)

# todo -> Changer localhost par l'adress ip de la machine
# Hololens sur le pc 192.168.137.71
# Emulateur : 172.16.80.1
outputAddress = ('192.168.137.71',5124)

while True:
    #print ('\nwaiting to receive message')
    #try:
        data, address = sock.recvfrom(4096)
    
        #print ('received %s bytes from %s' % (len(data), address))
        #print (data)
    
        if data:
            sent = outsocket.sendto(data, outputAddress)
            #print ('sent %s bytes back to %s' % (sent, outputAddress))
    #except:
        #pass

