import sys
from OpenGL import Vector3
from OpenGL import Vector2
from WhateverEngine.Engine import Transform
from WhateverEngine.Engine import PhysicsComponent
from WhateverEngine.Engine import Renderer
from WhateverEngine.Engine import Input
from WhateverEngine.Engine import GameObject
from WhateverEngine.Engine import EngineFunctions
from WhateverEngine.Engine import WhateverRay

trans=Transform()

def Update():
    if(Input.GetKeyboardKeyUp['t'] is True):
        ballgo = EngineFunctions.GetGameObjectWithId(10)
        ballgo.GetPhysics.AddForce(Vector3(0,15.0,0))
    lookSpeed = 0.002
    pitch = (Input.PrevY - Input.CurY) * lookSpeed
    trans.Pitch(pitch)