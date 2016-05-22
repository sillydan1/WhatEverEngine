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

tgglmvmnt = True

def Update():
    speed = 10.0
    translation = Vector3(0.0, 0.0, 0.0)    
    global rot
    global deltaTime
    global trans    
    global tgglmvmnt

    lookSpeed = 0.002

    if (Input.GetKeyboardKey['w'] is True):
        translation += Vector3(0.0, 0.0, speed * deltaTime)
    if(Input.GetKeyboardKey['s'] is True):
        translation -= Vector3(0.0, 0.0, speed * deltaTime)
    if(Input.GetKeyboardKey['q'] is True):
        translation += Vector3(0.0, speed * deltaTime, 0.0)
    if(Input.GetKeyboardKey['e'] is True):
        translation -= Vector3(0.0, speed * deltaTime, 0.0)
    if(Input.GetKeyboardKey['a'] is True):
        translation += Vector3(speed * deltaTime, 0.0, 0.0)
    if(Input.GetKeyboardKey['d'] is True):
        translation -= Vector3(speed * deltaTime, 0.0, 0.0)

    if(Input.GetKeyboardKeyUp[' '] is True):
        tgglmvmnt = not tgglmvmnt
    
    if(tgglmvmnt):    
        trans.MoveRelative(translation);

        yaw = (Input.PrevX - Input.CurX) * lookSpeed
        trans.YawFPS(yaw)

        pitch = (Input.PrevY - Input.CurY) * lookSpeed
        trans.Pitch(pitch) 