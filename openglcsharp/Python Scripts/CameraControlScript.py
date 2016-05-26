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

def Update():  
    lookSpeed = 0.002

    pitch = (Input.PrevY - Input.CurY) * lookSpeed
    trans.Pitch(pitch)