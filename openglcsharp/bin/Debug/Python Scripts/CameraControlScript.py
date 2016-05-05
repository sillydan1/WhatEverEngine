﻿import sys
from OpenGL import Vector3
from OpenGL import Vector2
from openglcsharp.Engine import Transform
from openglcsharp.Engine import Input

trans=Transform()
rot=Vector3()
deltaTime=0.0

def Update():
    speed = 50.0
    translation = Vector3(0.0, 0.0, 0.0)    
    global rot
    global deltaTime
    global trans    

    #First Person movement
    #Movement
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

    #MouseLook
    MouseLook()    

def MouseLook():
    global trans
    lookSpeed = 0.002
    
    if(Input.IsMouseRightButtonDown is True):
        yaw = (Input.PrevX - Input.CurX) * lookSpeed
        trans.Yaw(yaw)
        
        pitch = (Input.PrevY - Input.CurY) * lookSpeed
        trans.Pitch(pitch)
        
    
