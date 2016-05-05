using System;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Reflection;
using System.Collections.Generic;
using OpenGL;

namespace openglcsharp.Engine
{
    public class PythonComponent : GameComponent
    {
        private ScriptRuntime pyngine;
        private string myScriptFileName = "";
        //Meant to tell the component that you shoul reload the python script.
        private bool dirty = true;
        private dynamic myScope;
        private Dictionary<string, bool> hasMethods = new Dictionary<string, bool>();
        private Dictionary<string, bool> hasFields = new Dictionary<string, bool>();

        public PythonComponent(string fileName)
        {
            SetScript(fileName);
        }
        public void SetScript(string fileName)
        {
            myScriptFileName = fileName;
            dirty = true;
        }
        private bool HasMethod(string whatMethod)
        {
            if (hasMethods.ContainsKey(whatMethod))
                return hasMethods[whatMethod];

            bool br = false;
            try
            {
                br = myScope.GetVariable(whatMethod) != null;

                if (!br)
                    Program.LogError("WARNING: Method '" + whatMethod + "' does not exist in: '" + myScriptFileName + "'. Are you missing a definition of '" + whatMethod + "'?\nYou can ignore this message if you don't need the method.");
            }
            catch
            {
                Program.LogError("WARNING: Method '" + whatMethod + "' does not exist in: '" + myScriptFileName + "'. Are you missing a definition of '" + whatMethod + "'?\nYou can ignore this message if you don't need the method.");
            }
            hasMethods.Add(whatMethod, br);
            return br;
        }
        private bool HasVar<T>(string variableName)
        {
            
            if (hasFields.ContainsKey(variableName))
                return hasFields[variableName];

            bool br = false;
            try
            {
                br = myScope.GetVariable(variableName) != null;
                T eh = myScope.GetVariable<T>(variableName);
                if (!br)
                    Program.LogError("Error getting variable '" + variableName + "' on Python script: '" + myScriptFileName + "' Are you missing a definition of '" + variableName + "'?");
            }
            catch (Exception e)
            {
                Program.LogError("Error getting variable '" + variableName + "' on Python script: '" + myScriptFileName + "' Are you missing a definition of '" + variableName + "'?");
            }
            hasFields.Add(variableName, br);
            return br;
        }
        public override void Start()
        {
            base.Start();
            pyngine = Python.CreateRuntime();
            pyngine.LoadAssembly(Assembly.GetAssembly(typeof(OpenGL.Vector3)));
            pyngine.LoadAssembly(Assembly.GetAssembly(typeof(openglcsharp.Engine.Input)));

            if (myScriptFileName != "")
            {
                try
                {
                    myScope = pyngine.UseFile(myScriptFileName);
                }
                catch (Exception e)
                {
                    if(e is Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                    {
                        dirty = false;
                        return;
                    }
                    Program.LogError("Loading Pythonscript failed: " + myScriptFileName + " CAUSE: \n" + e.Message);
                }
                if (HasMethod("Start"))
                    myScope.Start();

                dirty = false;
            }

            //Initial rotation
            if (HasVar<Vector3>("rot"))
            {
                Vector3 result = myScope.GetVariable<Vector3>("rot");
                owner.Transform.Pitch(result.x);
                owner.Transform.Yaw(result.y);
                owner.Transform.Roll(result.z);

            }
        }
        public override void Update()
        {
            base.Update();
            //Update all the engine specific fields.
            if (HasVar<float>("deltaTime"))
            {
                myScope.SetVariable("deltaTime", Program.DeltaTime);
            }
            if (HasVar<Vector3>("pos"))
            {
                Vector3 result = myScope.GetVariable<Vector3>("pos");
                owner.Transform.Position = result;
            }
            if (HasVar<Vector3>("rot"))
            {
                Vector3 result = myScope.GetVariable<Vector3>("rot");
                owner.Transform.Pitch(result.x);
                owner.Transform.Yaw(result.y);
                owner.Transform.Roll(result.z);
            }
            if(HasVar<Transform>("trans"))
            {
                myScope.SetVariable("trans", owner.Transform);
            }
            //Call Update
            if (HasMethod("Update"))
                myScope.Update();

            CheckDirty();
        }
        private void CheckDirty()
        {
            if(dirty)
            {
                Start();
            }
        }
    }
}
