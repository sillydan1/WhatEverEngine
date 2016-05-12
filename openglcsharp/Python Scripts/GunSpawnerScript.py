from OpenGL import Vector3
from WhateverEngine.Engine import Renderer
from WhateverEngine.Engine import PythonComponent
from WhateverEngine.Engine import Transform
from WhateverEngine.Engine import GameObject
from WhateverEngine.Engine import EngineFunctions

deltaTime=0.0

#non-standard stuff
timer=0.0

def Start():
    print "This is the Gunspawner Script. Let's spawn some guns!"
    #print (db.version)
    #print (db.sqlite_version)

def Update():
    global deltaTime
    global timer
    timer += deltaTime
    if(timer > 0.5):
        SpawnNewGun()
        timer = 0
    
def SpawnNewGun():
    try:
        go = GameObject(Transform(Vector3(0,0,-130))) 
        go.AddGameComponent(Renderer("data\\rifle.obj")) # kan give exception hvis forkert syntaks og/eller filen ikke findes.
        go.AddGameComponent(PythonComponent("Python Scripts\\KillGunScript.py"))
        EngineFunctions.Instantiate(go)
    except:
        print "Something went wrong when loading and spawning a gun object!"
    