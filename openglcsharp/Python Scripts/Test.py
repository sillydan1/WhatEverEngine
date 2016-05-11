import sys
from OpenGL import Vector3
from openglcsharp.Engine import Input
from openglcsharp.Engine import Renderer
from openglcsharp.Engine import GameObject
from openglcsharp.Engine import EngineFunctions

pos=Vector3()
rot=Vector3()
deltaTime=0.0

#non-standard stuff
hasSpawned = False
go = GameObject()

def Start():
    global pos
    global rot
    pos += Vector3(0.0, 0.0, -150.0)
    rot = Vector3(0.0, 90.0, 0.0)

def Update():
    global rot
    global pos
    global deltaTime
    rot = Vector3(0.0, 0.1 * deltaTime, 0.0)

    #Instantiate a new object if the player presses 'k'
    if(Input.GetKeyboardKeyUp['k'] is True):
        global go
        global hasSpawned
        if(hasSpawned is False):
            go.AddGameComponent(Renderer("data\\rifle.obj"))
            EngineFunctions.Instantiate(go)
            hasSpawned = True        
        else:
            EngineFunctions.Destroy(go)

    #Rotation controls.
    
    #if (Input.GetKeyboardKey['w'] is True):
    #    rot = Vector3(-1.0 * deltaTime, 0.0, 0.0)
    #if(Input.GetKeyboardKey['s'] is True):
    #    rot = Vector3(1.0 * deltaTime, 0.0, 0.0)

    #if (Input.GetKeyboardKey['q'] is True):
    #    rot = Vector3(0.0, 0.0, 1.0 * deltaTime)
    #if(Input.GetKeyboardKey['e'] is True):
    #    rot = Vector3(0.0, 0.0, -1.0 * deltaTime)

    #if(Input.GetKeyboardKey['a'] is True):
    #    rot = Vector3(0.0, 1.0 * deltaTime, 0.0)
    #if(Input.GetKeyboardKey['d'] is True):
    #    rot = Vector3(0.0, -1.0 * deltaTime, 0.0)
