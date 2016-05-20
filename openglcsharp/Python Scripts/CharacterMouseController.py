import sys
from OpenGL import Vector3
from WhateverEngine.Engine import Transform 
from WhateverEngine.Engine import GameComponent 
from WhateverEngine.Engine import GameObject 
from WhateverEngine.Engine import PhysicsComponent
from WhateverEngine.Engine import Input # you can import Engine specific classes like this. 
from WhateverEngine.Engine import EngineFunctions # Input and EngineFunctions are two very useful classes. 

# standard stuff - these will get updated / read from the engine and applied on the attached GameObject.
trans=Transform() # This is the attached Transform component. You can access all of the publiv methods on this.
#pos=Vector3() # The raw position of the object.
#rot=Vector3() # The raw rotation of the object (as angle axis')
deltaTime=0.0 # Delta time. This is very useful for smooth interpolation stuff

physcomp = PhysicsComponent()

def Start():
    global physcomp
    pc = trans.GetOwner
    physcomp = pc.GetPhysics
    print(physcomp)

# Update gets called once per frame.
def Update():
    global deltaTime
    global trans
    global physcomp
    speed = 10.0
   
    pitch = 0.0
    if(Input.GetKeyboardKey['i'] is True):
        pitch = (speed) * 0.002
    if(Input.GetKeyboardKey['k'] is True):
        pitch = (-speed) * 0.002

    trans.Pitch(pitch) 

