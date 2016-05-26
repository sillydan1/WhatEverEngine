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
            normalCD = normalCDR
    if Input.IsMouseRightButtonDown:
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
    ball = EngineFunctions.GetGameObjectWithId(10)

    vec1 = trans.GetForwardVector()
    vec2 = ball.Transform.Position - trans.Position
    vec1 = vec1.Normalize()
    vec2 = vec2.Normalize()
 ##Get the dot product
 #   dot = Vector3.Dot(vec1,vec2);
 ## Divide the dot by the product of the magnitudes of the vectors
 #   dot = dot/(math.sqrt(vec1.x * vec1.x + vec1.y * vec1.y + vec1.z * vec1.z)*math.sqrt(vec2.x * vec2.x + vec2.y * vec2.y + vec2.z * vec2.z))
 ##Get the arc cosin of the angle, you now have your angle in radians 
 #   acos = math.acos(dot)
 ##Multiply by 180/Mathf.PI to convert to degrees
 #   angle = acos*180/math.pi
 ##Congrats, you made it really hard on yourself.
 #   angle = angle * math.copysign(Vector3.Cross(vec1, vec2).y)
    nr = ball.Transform.Position - trans.Position
    nrf = math.sqrt(math.pow(nr.x,2) +math.pow(nr.z,2) +math.pow(nr.z,2)) * 0.1
    print vec2
    vec2 = Vector3(vec2.x*nrf,vec2.y*nrf,vec2.z*nrf)
    print vec2
    angle = Vector3.CalculateAngle(vec1,vec2)
    print angle
    if angle > 0:
        if  angle < 45:
            NetworkClass.Instance.SendData(NetworkTranslator.NetAddForce(trans.GetOwner,(ball.Transform.Position-trans.Position) * 1))
            ball.GetPhysics.AddForce((ball.Transform.Position-trans.Position) * 1)

def ShootSpecial():
    #r = OpenGL.Ray(trans.Position, trans.Position * Vector3.Forward * 9999);
    #if r:
    #    r.GameObject.GetPhysics.AddForce(trans.Position, trans.Position * Vector3.Forward * 5)
    NetworkClass.Instance.SendData(NetworkTranslator.NetAddForce(trans.GetOwner,(Vector3.Up * 10000)))
    EngineFunctions.GetGameObjectWithId(10).GetPhysics.AddForce(Vector3.Up * 10000)