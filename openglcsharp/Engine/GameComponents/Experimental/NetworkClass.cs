using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace WhateverEngine.Engine
{
    public class NetworkClass
    {
        private ScriptRuntime pyngine;
        private dynamic myScope;
        private Dictionary<string, bool> hasFields = new Dictionary<string, bool>();
        private string myScript = @"Python Scripts\Network.py";
        private Thread listenThread;
        private static NetworkClass instance;

        public static NetworkClass Instance
        {
            get
            {
                if (instance == null)
                    instance = new NetworkClass();
                return instance;
            }
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
                    Program.LogError("Error getting variable '" + variableName + "' on Python script: 'Newwork.py' Are you missing a definition of '" + variableName + "'?");
            }
            catch { }
            hasFields.Add(variableName, br);
            return br;
        }

        private void RecieveData()
        {
            while (true)
            {
                if (HasVar<string>("splitMsg"))
                {
                    string result = myScope.ReturnSplitMsg();
                    if (result != "")
                    {
                        NetworkTranslator.RecieveData(result);
                        myScope.SetVariable("splitMsg", "");
                    }
                }
            }
        }
        
        private void Listener()
        {
            pyngine = Python.CreateRuntime();
            pyngine.LoadAssembly(Assembly.GetAssembly(typeof(OpenGL.Vector3)));
            pyngine.LoadAssembly(Assembly.GetAssembly(typeof(WhateverEngine.Engine.Input)));
            pyngine.LoadAssembly(Assembly.GetAssembly(typeof(System.Random)));
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
                RecieveData();
            }
        }
        
        public void SendData(string data)
        {
            if (HasVar<string>("sendMsg"))
            {
                if (data != "")
                {
                    myScope.SetVariable("sendMsg", data);
                }
            }
        }

        public void Start()
        {
            listenThread = new Thread(() => Listener());
            listenThread.Start();
        }
    }
}
