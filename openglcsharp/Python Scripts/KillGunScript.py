import sys
from OpenGL import Vector3
from WhateverEngine.Engine import Transform
from WhateverEngine.Engine import EngineFunctions

trans=Transform()
deltaTime=0.0

rotatioSpeed=3.0
timer=0.0
gravity=0.0
velocity=Vector3(0,0,0)

def Start():
    global velocity
    trans.Yaw(90.0)
    velocity=Vector3(0.0, 0.05, 0.05)

def Update():
    global timer
    global deltaTime
    global gravity
    global velocity    
    #rotate the object
    #trans.Roll(rotatioSpeed * deltaTime)
    #Move downwards
    velocity += Vector3(0.0, -gravity, 0.0)
    trans.Move(velocity)
    #tick tock. Time's ticking...
    timer += deltaTime
    gravity += 0.000098 * deltaTime
    if(timer > 7):
        EngineFunctions.Destroy(trans.GetOwner) #Self destruct.
        timer = 0


