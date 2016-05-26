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

    if(Input.GetKeyboardKeyUp['r'] is True):
        r = WhateverRay()
        r.CastRay(trans.Position, trans.GetForwardVector(), 10.0, 1)
        if(r.hit is True):
            print 'rayCast hit something'
            s = r.GetNameOfFirstHit()
            if(s == "Basketball"):
                ballgo = EngineFunctions.GetGameObjectWithTag("ball")
                ballgo.GetPhysics.AddForce(Vector3(0,15.0,0))
                print 'ball goes up!'
        else:
            print 'rayCast didnt hit anything'

    if(Input.GetKeyboardKeyUp['t'] is True):
        ballgo = EngineFunctions.GetGameObjectWithTag("ball")
        ballgo.GetPhysics.AddForce(Vector3(0,15.0,0))
        print 'ball goes up!'

    lookSpeed = 0.002

    pitch = (Input.PrevY - Input.CurY) * lookSpeed
    trans.Pitch(pitch) 

    
