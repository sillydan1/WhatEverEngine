using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace WhateverEngine.Engine
{
    class NetworkClass
    {
        private ScriptRuntime pyngine;
        private dynamic myScope;
        private Dictionary<string, bool> hasFields = new Dictionary<string, bool>();
        string myScript = @"Python Scripts\Network.py";
        Thread listenThread;

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
                    Program.LogError("Error getting variable '" + variableName + "' on Python script: 'Newwork.py' Are you missing a definition of '" + variableName + "'?");
            }
            catch { }
            hasFields.Add(variableName, br);
            return br;
        }

        private void RecieveData()
        {
            try
            {
                myScope = pyngine.UseFile(myScript);
            }
            catch (Exception e)
            {
                Program.LogError("Loading Pythonscript failed: " + myScript + " CAUSE: \n" + e.Message + "\n");
            }
            myScope.Start();
            while (true)
            {
                if (HasVar<string>("splitMsg"))
                {
                    string result = myScope.ReturnSplitMsg();
                    string[] splitMsg = result.Split('|');
                    if (splitMsg.Count() > 0 && splitMsg[0] != "")
                    {
                        OpenGL.Vector3 v3;
                        switch (splitMsg[0])
                        {
                            case "[string]":
                                Console.WriteLine("NetworkClass says: " + splitMsg[1]);
                                break;
                            case "[position]":
                                v3 = new OpenGL.Vector3(
                                    Convert.ToInt32(splitMsg[2]),
                                    Convert.ToInt32(splitMsg[3]),
                                    Convert.ToInt32(splitMsg[4]));
                                EngineFunctions.GetGameObjectWithId(Convert.ToInt32(splitMsg[1])).Transform.Position = v3;
                                Console.WriteLine("I did pos stuff");
                                break;
                            case "[scale]":
                                v3 = new OpenGL.Vector3(
                                    Convert.ToInt32(splitMsg[2]),
                                    Convert.ToInt32(splitMsg[3]),
                                    Convert.ToInt32(splitMsg[4]));
                                EngineFunctions.GetGameObjectWithId(Convert.ToInt32(splitMsg[1])).Transform.Scale = v3;
                                Console.WriteLine("I did pos stuff");
                                break;
                            case "[rotation]":

                                //EngineFunctions.GetGameObjectWithId(Convert.ToInt32(splitMsg[1])).Transform.SetRotation()
                                break;
                            default:
                                Console.WriteLine("Unknow translation type: " + splitMsg[0]);
                                break;
                        }
                        myScope.SetVariable("splitMsg", "");
                    }
                }
            }
        }

        private void Listener()
        {
            while (true)
            {
                RecieveData();
            }
        }

        public void Start()
        {
            pyngine = Python.CreateRuntime();
            pyngine.LoadAssembly(Assembly.GetAssembly(typeof(OpenGL.Vector3)));
            pyngine.LoadAssembly(Assembly.GetAssembly(typeof(WhateverEngine.Engine.Input)));
            pyngine.LoadAssembly(Assembly.GetAssembly(typeof(System.Random)));

            listenThread = new Thread(() => Listener());
            listenThread.Start();
        }
    }
}
