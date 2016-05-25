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

height = 2.0
heightOffset = 0.0
grav = 0.0
gravityAccelleration = -0.381
velocity = Vector3(0.0, 0.0, 0.0)
grounded = False
moveSpeed = 10.0
jumpForce = 10
lookSpeed = 0.002

# Update gets called once per frame.
def Update():
    global deltaTime
    global trans

    ApplyMouseRotation()
    ApplyGravityAndGrounded()
    ApplyInput()

    trans.Move(velocity * deltaTime)

def ApplyInput():
    global velocity
    global moveSpeed
    global jumpForce
    global grounded
    translation = Vector3(0,0,0)

    if (Input.GetKeyboardKey['w'] is True):
        translation -= Vector3(0.0, 0.0, moveSpeed)
    if(Input.GetKeyboardKey['s'] is True):
        translation += Vector3(0.0, 0.0, moveSpeed)

    if(Input.GetKeyboardKey['a'] is True):
        translation -= Vector3(moveSpeed, 0.0, 0.0)
    if(Input.GetKeyboardKey['d'] is True):
        translation += Vector3(moveSpeed, 0.0, 0.0)  

    if(translation != Vector3.Zero):
        trans.MoveRelative(translation * deltaTime)

    if (Input.GetKeyboardKey[' '] is True & grounded is True):
        velocity += Vector3(0.0, jumpForce, 0.0)
        grounded = False
        
def ApplyMouseRotation():
    global lookSpeed
    yaw = 0.0
    yaw = (Input.PrevX - Input.CurX) * lookSpeed
    trans.YawFPS(-yaw)

def ApplyGravityAndGrounded():
    global velocity
    global trans    
    global grounded

    if(trans.Position.y <= 0):
        grounded = True
        velocity = Vector3(0.0, 0.0, 0.0)
        if(trans.Position.y < 0):
            trans.Position = Vector3(trans.Position.x, 0.0, trans.Position.z)
    
    if(grounded is False):
        velocity += Vector3(0.0, gravityAccelleration, 0.0)
    