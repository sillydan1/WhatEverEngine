import sys
from OpenGL import Vector3
from WhateverEngine.Engine import Transform 
from WhateverEngine.Engine import Input # you can import Engine specific classes like this. 
from WhateverEngine.Engine import EngineFunctions # Input and EngineFunctions are two very useful classes.
from WhateverEngine.Engine import GameObject
from WhateverEngine.Engine import NetworkTranslator
from WhateverEngine.Engine import NetworkClass

# standard stuff - these will get updated / read from the engine and applied on the attached GameObject.
trans=Transform() # This is the attached Transform component. You can access all of the publiv methods on this.
#pos=Vector3() # The raw position of the object.
rot=Vector3() # The raw rotation of the object (as angle axis')
deltaTime=0.0 # Delta time. This is very useful for smooth interpolation stuff
gameObject = GameObject

# Start gets called when the GameObject enters the scene 
def Start():
    print "Hello from python"

# Update gets called once per frame.
def Update():
    global deltaTime
    global trans
    global rot
    global gameObject
    gg = rot
    if Input.GetKeyboardKey['t']:
        trans.Move(trans.GetForwardVector() * 10 * deltaTime)
        NetworkClass.Instance.SendData[NetworkTranslator.NetPosition[gameObject.Id,trans.Position.x,trans.Position.y,trans.Position.z]]
    if Input.GetKeyboardKey['r']:
        trans.Yaw(3*deltaTime)

