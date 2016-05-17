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
                    NetworkTranslator.RecieveData(result);
                    myScope.SetVariable("splitMsg", "");
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
