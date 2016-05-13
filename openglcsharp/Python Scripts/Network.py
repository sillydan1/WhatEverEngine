#!/usr/bin/python           # This is client.py file
from System import Console
import threading
import socket               # Import socket module
    
class myThread (threading.Thread):
    def __init__(self, threadID, name):
        threading.Thread.__init__(self)
        self.threadID = threadID
        self.name = name
    def run(self):
        print "Starting " + self.name
        if self.name == "ListenThread":
            ListenThread()
        else:
            WritingThread()
        print "Exiting " + self.name

currentTypeString = ""
msg = ""
global splitMsg
splitMsg = ""
s = socket.socket()

def ListenThread():
    global s
    global splitMsg
    print "listen thread started"
    while True:
        if splitMsg == "":
            print "Waiting for message"
            splitMsg = s.recv(1024)
            print "Network.py says: " + splitMsg

def Start():
    print "network started"
    global s
    s = socket.socket()         # Create a socket object
    host = "192.168.43.163"    # Get local machine name
    port = 12345                # Reserve a port for your service.
    s.connect((host, port))
    print "connection found"
    thread1 = myThread(1, "ListenThread")
    thread1.start()

def ReturnSplitMsg():
    global splitMsg
    return splitMsg