import sys
from OpenGL import Vector3
from OpenGL import Vector2
from WhateverEngine.Engine import Transform
from WhateverEngine.Engine import PhysicsComponent
from WhateverEngine.Engine import Renderer
from WhateverEngine.Engine import Input
from WhateverEngine.Engine import GameObject
from WhateverEngine.Engine import EngineFunctions

trans=Transform()
rot=Vector3()
deltaTime=0.0

#class specificly for handling the mouse look controls
class MouseLookClass:
    def MouseLook(self):
        global trans
        

# script specific vars
msLook=MouseLookClass()

def Update():
    speed = 5.0
    translation = Vector3(0.0, 0.0, 0.0)    
    global rot
    global deltaTime
    global trans    

    if(Input.GetKeyboardKeyUp['p'] is True):
        go = GameObject(Transform(Vector3(0,10,1))) 
        go.AddGameComponent(Renderer("data\\sphere.obj")) # kan give exception hvis forkert syntaks og/eller filen ikke findes.
        go.AddGameComponent(PhysicsComponent())
        EngineFunctions.Instantiate(go)
    
    if (Input.GetKeyboardKey['w'] is True):
        translation -= Vector3(0.0, 0.0, speed * deltaTime)
    if(Input.GetKeyboardKey['s'] is True):
        translation += Vector3(0.0, 0.0, speed * deltaTime)
    if(Input.GetKeyboardKey['q'] is True):
        translation -= Vector3(0.0, speed * deltaTime, 0.0)
    if(Input.GetKeyboardKey['e'] is True):
        translation += Vector3(0.0, speed * deltaTime, 0.0)
    if(Input.GetKeyboardKey['a'] is True):
        translation -= Vector3(speed * deltaTime, 0.0, 0.0)
    if(Input.GetKeyboardKey['d'] is True):
        translation += Vector3(speed * deltaTime, 0.0, 0.0)

    trans.MoveRelative(translation)

    #use the MouseLooker class
    lookSpeed = 0.002

    

    #if(Input.IsMouseRightButtonDown is True):
    #yaw = (Input.PrevX - Input.CurX) * lookSpeed
    #trans.Yaw(yaw)
        
    #pitch = (Input.PrevY - Input.CurY) * lookSpeed
    #trans.Pitch(pitch) 
