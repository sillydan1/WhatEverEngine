import sys
sys.path.append(r"C:\Program Files(x86)\IronPython 2.7\Lib") #doing this because Ironpython is too stupid to know where os is...
import os	#and yet. os is still missing - check the game log for yourself.
import clr	#I can't use sqlite3 because I can't use clr to add the refference to the DLL because I can't use os to find the local folder.
clr.AddReferenceToFileAndPath("F:\\Documents\\Skole\\Python\\openglcsharp\\openglcsharp\\bin\\Debug\\IronPython.SQLite.dll") # This works for shit. (hence why I need 'os')
import _sqlite3 # <- F@!# you Ironpython

# It's 4 AM now and I still have to do the report! 
# (doing this so late, because I can't wake up before 12 to do the rest, so I'm doing it now.)
# I've been googling this issue around for 3 hours now, it's apparently a very common problem.
# My last effort is to make a completely different project JUST for the SQLite-python stuff. Just to prove that I actually know how to do this. Ironpython is just being a bitch

# You can't say I didn't try...

