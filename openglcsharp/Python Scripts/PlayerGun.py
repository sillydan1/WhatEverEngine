import sys
import math
from OpenGL import Vector3
from WhateverEngine.Engine import Transform 
from WhateverEngine.Engine import Input # you can import Engine specific classes like this.
from WhateverEngine.Engine import EngineFunctions # Input and EngineFunctions are two very useful classes.
from WhateverEngine.Engine import GameObject
from WhateverEngine.Engine import Renderer
from WhateverEngine.Engine import PythonComponent
from WhateverEngine.Engine import NetworkClass
from WhateverEngine.Engine import NetworkTranslator
from WhateverEngine.Engine import WhateverRay

# standard stuff - these will get updated / read from the engine and applied on
# the attached GameObject.
trans = Transform() # This is the attached Transform component.  You can access all of the publiv
                    # methods on this.
#pos=Vector3() # The raw position of the object.
rot = Vector3() # The raw rotation of the object (as angle axis')
deltaTime = 0.0 # Delta time.  This is very useful for smooth interpolation stuff
normalCD = 0 # normal cooldown
normalCDR = 0.5 # normal cooldown reset
specialCD = 0 # special cooldown
specialCDR = 1.5 # special cooldown reset

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
    if Input.IsMouseLeftButtonDown:
        if normalCD <= 0:
            ShootNormal()
    if Input.IsMouseRightButtonDown:
        if specialCD <= 0:
            ShootSpecial()

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
    global normalCD
    global normalCDR
    ball = EngineFunctions.GetGameObjectWithId(10)
    r = WhateverRay()
    r.CastRay(trans.Position, trans.GetForwardVector(), 10000.0, 1)
    if(r.hit is True):
        s = r.GetNameOfFirstHit()
        if(s == "Basketball"):
                NetworkClass.Instance.SendData(NetworkTranslator.NetAddForce(trans.GetOwner,(ball.Transform.Position-trans.Position) * 2000))
                ball.GetPhysics.AddForce((ball.Transform.Position-trans.Position) * 2000)
                normalCD = normalCDR;

def ShootSpecial():
    global specialCD
    global specialCDR
    r = WhateverRay()
    r.CastRay(trans.Position, trans.GetForwardVector(), 10000.0, 1)
    if(r.hit is True):
        s = r.GetNameOfFirstHit()
        if(s == "Basketball"):
            NetworkClass.Instance.SendData(NetworkTranslator.NetAddForce(trans.GetOwner,(Vector3.Up * 10000)))
            EngineFunctions.GetGameObjectWithId(10).GetPhysics.AddForce(Vector3.Up * 10000)
            specialCD = specialCDR