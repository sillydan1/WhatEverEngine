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
    global trans    
    lookSpeed = 0.002
    # The camera's axes are apparently inverted... Don't ask why. (And don't change it in CameraComponent. Trust me. It's better to just live with it and compensate)
    pitch = (Input.PrevY - Input.CurY) * lookSpeed
    trans.Pitch(-pitch) 

    