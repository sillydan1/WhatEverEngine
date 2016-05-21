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

# Update gets called once per frame.
def Update():
    global deltaTime
    global trans
    global physcomp
    speed = 1.0
    jumpForce = 50
    
    #if (Input.GetKeyboardKeyUp[' '] is True):
    #    physcomp.AddForce(Vector3(0,1,0) * jumpForce, 1, True)
    
    yaw = 0.0
    #if(Input.IsMouseRightButtonDown is True):
    if(Input.GetKeyboardKey['l'] is True):
        yaw = (-speed) * deltaTime
    if(Input.GetKeyboardKey['j'] is True):
        yaw = (speed) * deltaTime

    trans.Yaw(yaw)

