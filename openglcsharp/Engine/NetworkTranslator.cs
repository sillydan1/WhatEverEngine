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
            result += newPos.z + "|";
            return result;
        }

        public static string NetPosition(GameObject obj, PhysX.Math.Vector3 newPos)
        {
            string result = "";
            result += "[position]|";
            result += obj.id + "|";
            result += newPos.X + "|";
            result += newPos.Y + "|";
            result += newPos.Z + "|";
            return result;
        }

        public static string NetRotation(GameObject obj, OpenGL.Quaternion newRotation)
        {
            string result = "";
            result += "[rotation]|";
            result += obj.id + "|";
            result += newRotation.x + "|";
            result += newRotation.y + "|";
            result += newRotation.z + "|";
            result += newRotation.w + "|";
            return result;
        }

        public static void RecieveData(string data)
        {
            string[] splitMsg = data.Split('|');

            if (splitMsg.Count() > 0 && splitMsg[0] != "")
            {
                OpenGL.Vector3 v3;
                OpenGL.Vector4 v4;
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
                        v4 = new OpenGL.Vector4(
                            Convert.ToSingle(splitMsg[2]),
                            Convert.ToSingle(splitMsg[3]),
                            Convert.ToSingle(splitMsg[4]),
                            Convert.ToSingle(splitMsg[5]));
                        EngineFunctions.GetGameObjectWithId(Convert.ToInt32(splitMsg[1])).Transform.SetRotation(new OpenGL.Quaternion(v4.x, v4.y, v4.z, v4.w));
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
