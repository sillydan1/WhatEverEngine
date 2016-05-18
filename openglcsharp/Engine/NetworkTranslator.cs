using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhateverEngine.Engine
{
    public class NetworkTranslator
    {
        public static string NetPosition(GameObject obj, OpenGL.Vector3 newPos)
        {
            string result = "";
            result += "[position]|";
            result += obj.id + "|";
            result += newPos.x + "|";
            result += newPos.y + "|";
            result += newPos.z;
            return result;
        }

        public static string NetPosition(GameObject obj, PhysX.Math.Vector3 newPos)
        {
            string result = "";
            result += "[position]|";
            result += obj.id + "|";
            result += newPos.X + "|";
            result += newPos.Y + "|";
            result += newPos.Z;
            return result;
        }

        public static string NetRotation(GameObject obj, OpenGL.Vector3 newRotation)
        {
            string result = "";
            result += "[rotation]|";
            result += obj.id + "|";
            result += newRotation.x + "|";
            result += newRotation.y + "|";
            result += newRotation.z;
            return result;
        }

        public static void RecieveData(string data)
        {
            string[] allMsg = data.Split('&');
            if (allMsg.Count() > 1)
            {
                foreach (string item in allMsg)
                {
                    RecieveData(item);
                }
                return;
            }
            string[] splitMsg = data.Split('|');

            if (splitMsg.Count() > 0 && splitMsg[0] != "")
            {
                OpenGL.Vector3 v3;
                switch (splitMsg[0])
                {
                    case "[position]":
                        v3 = new OpenGL.Vector3(
                            Convert.ToSingle(splitMsg[2]),
                            Convert.ToSingle(splitMsg[3]),
                            Convert.ToSingle(splitMsg[4]));
                        EngineFunctions.GetGameObjectWithId(Convert.ToInt32(splitMsg[1])).Transform.Position = v3;
                        Console.WriteLine("I did pos stuff");
                        break;
                    case "[scale]":
                        v3 = new OpenGL.Vector3(
                            Convert.ToSingle(splitMsg[2]),
                            Convert.ToSingle(splitMsg[3]),
                            Convert.ToSingle(splitMsg[4]));
                        EngineFunctions.GetGameObjectWithId(Convert.ToInt32(splitMsg[1])).Transform.Scale = v3;
                        Console.WriteLine("I did scale stuff");
                        break;
                    case "[rotation]":
                        v3 = new OpenGL.Vector3(
                            Convert.ToSingle(splitMsg[2]),
                            Convert.ToSingle(splitMsg[3]),
                            Convert.ToSingle(splitMsg[4]));
                        EngineFunctions.GetGameObjectWithId(Convert.ToInt32(splitMsg[1])).Transform.Pitch(v3.x);
                        EngineFunctions.GetGameObjectWithId(Convert.ToInt32(splitMsg[1])).Transform.Yaw(v3.y);
                        EngineFunctions.GetGameObjectWithId(Convert.ToInt32(splitMsg[1])).Transform.Roll(v3.z);
                        Console.WriteLine("I did rotation stuff");
                        break;
                    default:
                        Console.WriteLine("Unknow translation type: " + splitMsg[0]);
                        break;
                }
            }
        }

    }
}
