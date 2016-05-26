import sys
from OpenGL import Vector3
from WhateverEngine.Engine import Transform 
from WhateverEngine.Engine import Input # you can import Engine specific classes like this.
from WhateverEngine.Engine import EngineFunctions # Input and EngineFunctions are two very useful classes.
from WhateverEngine.Engine import GameObject
from WhateverEngine.Engine import Renderer
from WhateverEngine.Engine import PythonComponent
from WhateverEngine.Engine import NetworkClass
from WhateverEngine.Engine import NetworkTranslator

# standard stuff - these will get updated / read from the engine and applied on
# the attached GameObject.
trans = Transform() # This is the attached Transform component.  You can access all of the publiv
                    # methods on this.
#pos=Vector3() # The raw position of the object.
rot = Vector3() # The raw rotation of the object (as angle axis')
deltaTime = 0.0 # Delta time.  This is very useful for smooth interpolation stuff
normalCD = 0 # normal cooldown
normalCDR = 1 # normal cooldown reset
specialCD = 0 # special cooldown
specialCDR = 1 # special cooldown reset

# Start gets called when the GameObject enters the scene
def Start():
    print "Hello from python"

# Update gets called once per frame.
def Update():
    global deltaTime
    global trans
    global rot
    global normalCD
    global normalCDR
    global specialCD
    global specialCDR

    gg = rot
    normalCD -= deltaTime
    specialCD -= deltaTime
    if Input.GetKeyboardKey['c']:
        if Input.GetKeyboardKey['y']:
            if normalCD <= 0:
                ShootNormal()
                normalCD = normalCDR
        if Input.GetKeyboardKey['u']:
            if specialCD <= 0:
                ShootSpecial()
                specialCD = specialCDR

def SpawnNormalBullet():
    global trans
    #try:
    go = GameObject(Transform(trans.Position * trans.GetForwardVector() * 5))
    go.AddGameComponent(Renderer("data\\box.obj")) # kan give exception hvis forkert syntaks og/eller filen ikke findes.
    go.AddGameComponent(PythonComponent("Python Scripts\\NormalBullet.py"))
    EngineFunctions.Instantiate(go)
    #except:
    print "Something went wrong when loading and spawning a gun object!"

def ShootNormal():
    #r = OpenGL.Ray(trans.Position, trans.Position * Vector3.Forward * 9999);
    #if r:
    #    r.GameObject.GetPhysics.AddForce(trans.Position, trans.Position * Vector3.Forward * 5)
    NetworkClass.Instance.SendData(NetworkTranslator.NetAddForce(trans.GetOwner,(EngineFunctions.GetGameObjectWithId(10).Transform.Position-trans.Position) * 100))
    EngineFunctions.GetGameObjectWithId(10).GetPhysics.AddForce((EngineFunctions.GetGameObjectWithId(10).Transform.Position-trans.Position) * 100)

def ShootSpecial():
    #r = OpenGL.Ray(trans.Position, trans.Position * Vector3.Forward * 9999);
    #if r:
    #    r.GameObject.GetPhysics.AddForce(trans.Position, trans.Position * Vector3.Forward * 5)
    NetworkClass.Instance.SendData(NetworkTranslator.NetAddForce(trans.GetOwner,(Vector3.Up * 10000)))
    EngineFunctions.GetGameObjectWithId(10).GetPhysics.AddForce(Vector3.Up * 10000)