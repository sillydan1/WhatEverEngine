using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhateverEngine.Engine
{
    public class NetworkTranslator
    {
        //private static OpenGL.Vector3[] packs = new OpenGL.Vector3[byte.MaxValue];

        public static string NetPosition(GameObject obj, OpenGL.Vector3 newPos)
        {
            string result = "";
            result += "[position]|";
            result += obj.id + "|";
            result += newPos.x + "|";
            result += newPos.y + "|";
            result += newPos.z + "&";
            result = result.Replace('.', ',');
            return result;
        }

        public static string NetPosition(GameObject obj, PhysX.Math.Vector3 newPos)
        {
            string result = "";
            result += "[position]|";
            result += obj.id + "|";
            result += newPos.X + "|";
            result += newPos.Y + "|";
            result += newPos.Z + "&";
            result = result.Replace('.', ',');
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
            result += newRotation.w + "&";
            result = result.Replace('.', ',');
            return result;
        }

        public static void RecieveData(string data)
        {
            string[] allMsg = data.Split('&');
            if (allMsg.Count() > 2)
            {
                foreach (string item in allMsg)
                {
                    RecieveData(item);
                }
                return;
            }
            string[] splitMsg = allMsg[0].Split('|');


            if (splitMsg.Count() > 0 && splitMsg[0] != "")
            {
                OpenGL.Vector3 v3;
                OpenGL.Vector4 v4;
                GameObject g;
                switch (splitMsg[0])
                {
                    case "[position]":
                        if (splitMsg.Count() != 5) return;
                        v3 = new OpenGL.Vector3(
                            Convert.ToSingle(splitMsg[2]),
                            Convert.ToSingle(splitMsg[3]),
                            Convert.ToSingle(splitMsg[4]));
                        g = EngineFunctions.GetGameObjectWithId(Convert.ToInt32(splitMsg[1]));
                        g.Packs.Add(v3);
                        break;
                    case "[scale]":
                        if (splitMsg.Count() != 5) return;
                        v3 = new OpenGL.Vector3(
                            Convert.ToSingle(splitMsg[2]),
                            Convert.ToSingle(splitMsg[3]),
                            Convert.ToSingle(splitMsg[4]));
                        EngineFunctions.GetGameObjectWithId(Convert.ToInt32(splitMsg[1])).Transform.Scale = v3;
                        break;
                    case "[rotation]":
                        if (splitMsg.Count() == 6)
                        {
                            v4 = new OpenGL.Vector4(
                                Convert.ToSingle(splitMsg[2]),
                                Convert.ToSingle(splitMsg[3]),
                                Convert.ToSingle(splitMsg[4]),
                                Convert.ToSingle(splitMsg[5]));
                            EngineFunctions.GetGameObjectWithId(Convert.ToInt32(splitMsg[1])).Transform.SetRotation(new OpenGL.Quaternion(v4.x, v4.y, v4.z, v4.w));
                            g = EngineFunctions.GetGameObjectWithId(Convert.ToInt32(splitMsg[1]));
                            g.Transform.Orientation = new OpenGL.Quaternion(v4.x, v4.y, v4.z, v4.w);
                            if (g.Transform.RotPrediction == OpenGL.Quaternion.Zero)
                            {
                                g.Transform.RotPrediction = new OpenGL.Quaternion(v4.x, v4.y, v4.z, v4.w);
                            }
                            else
                            {
                                g.Transform.RotPrediction = OpenGL.Quaternion.Zero;
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Unknow translation type: " + splitMsg[0]);
                        break;
                }
            }
        }
    }
}
