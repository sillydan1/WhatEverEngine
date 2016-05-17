import sys
from OpenGL import Vector3
from openglcsharp.Engine import Transform 
from openglcsharp.Engine import Input # you can import Engine specific classes like this.
from openglcsharp.Engine import EngineFunctions # Input and EngineFunctions are two very useful classes.
from openglcsharp.Engine import GameObject
from openglcsharp.Engine import Renderer
from openglcsharp.Engine import PythonComponent

# standard stuff - these will get updated / read from the engine and applied on
# the attached GameObject.
trans = Transform() # This is the attached Transform component.  You can access all of the publiv
                    # methods on this.
#pos=Vector3() # The raw position of the object.
rot = Vector3() # The raw rotation of the object (as angle axis')
deltaTime = 0.0 # Delta time.  This is very useful for smooth interpolation stuff

# Start gets called when the GameObject enters the scene
def Start():
    print "Hello from python"

# Update gets called once per frame.
def Update():
    global deltaTime
    global trans
    global rot
    gg = rot
    if Input.GetKeyboardKey['t']:
        SpawnNormalBullet()

def SpawnNormalBullet():
    global trans
    #try:
    go = GameObject(Transform(trans.Position * trans.GetForwardVector() * 5))
    go.AddGameComponent(Renderer("data\\box.obj")) # kan give exception hvis forkert syntaks og/eller filen ikke findes.
    go.AddGameComponent(PythonComponent("Python Scripts\\NormalBullet.py"))
    EngineFunctions.Instantiate(go)
    #except:
    print "Something went wrong when loading and spawning a gun object!"