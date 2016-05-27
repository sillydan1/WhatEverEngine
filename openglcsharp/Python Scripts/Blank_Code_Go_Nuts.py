import sys
from OpenGL import Vector3
from WhateverEngine.Engine import Transform 
from WhateverEngine.Engine import Input # you can import Engine specific classes like this. 
from WhateverEngine.Engine import EngineFunctions # Input and EngineFunctions are two very useful classes. 
from WhateverEngine.Engine import WhateverRay #WhateverRay is used for raycasting. This is also a very useful thing.

# standard stuff - these will get updated / read from the engine and applied on the attached GameObject.
trans=Transform() # This is the attached Transform component. You can access all of the public methods on this.
#pos=Vector3() # The raw position of the object.
rot=Vector3() # The raw rotation of the object (as angle axis')
deltaTime=0.0 # Delta time. This is very useful for smooth interpolation stuff

# Start gets called when the GameObject enters the scene 
def Start():
    print "Hello from python"

# Update gets called once per frame.
def Update():
    global deltaTime #remember to use global for engine specified values.
    global trans
    global rot
    gg = rot
    if Input.GetKeyboardKey['t']:
        trans.Move(trans.GetForwardVector() * 10 * deltaTime)
    if Input.GetKeyboardKey['r']:
        trans.Yaw(3*deltaTime)
    
    #Raycasting is done like this
    r = WhateverRay()
    r.CastRay(trans.Position, trans.GetForwardVector(), 1000.0, 1)
    if(r.hit is True):
        print 'rayCast hit something'
        s = r.GetNameOfFirstHit() # NOTE: This is the PhysicsComponent Name value. NOT the GameObject name value.
        if(s == "Basketball"):
            print 'We hit the Basketball!'

