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

global splitMsg
global sendMsg
global s
sendMsg = ""
splitMsg = ""
s = socket.socket()

def ListenThread():
    global s
    global splitMsg
    print "listen thread started"
    try:
        while True:
            if splitMsg == "":
                print "Waiting for message"
                splitMsg = s.recv(1024)
                print "Network.py says: " + splitMsg
    except :
        print "ListenThread died"

def WritingThread():
    global sendMsg
    global c
    try:
        while True:
            if sendMsg != "":
                c.send(sendMsg)
                sendMsg = ""
    except :
        print "ListenThread died"

def GetConnection():
    global s
    global c
    print "waiting for connection..."
    s = socket.socket()         # Create a socket object
    host = socket.gethostname()    # Get local machine name
    port = 12345                # Reserve a port for your service.l
    s.bind((host, port))
    s.listen(5)
    c, addr = s.accept()

def Start():
    print "network started"
    #global s
    #s = socket.socket()         # Create a socket object
    #host = "192.168.43.163"    # Get local machine name
    #port = 12345                # Reserve a port for your service.
    #s.connect((host, port))
    while True:
        try:
            GetConnection()
            print "connection found"
            thread1 = myThread(1, "ListenThread")
            thread2 = myThread(2, "WritingThread")
            thread1.start()
            thread2.start()
        except :
            pass

def ReturnSplitMsg():
    global splitMsg
    return splitMsg