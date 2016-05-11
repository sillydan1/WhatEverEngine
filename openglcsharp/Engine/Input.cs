using System;
using Tao.FreeGlut;
using System.Collections.Generic;
using OpenGL;

namespace openglcsharp.Engine
{
    public static class Input
    {
        static bool mouselbDown, mouserbDown;
        static int prevX;
        static int prevY;
        static int curX;
        static int curY; //Hehe... Curry
        static Dictionary<string, bool> keyboardKeyIsDown = null;
        static Dictionary<string, bool> keyboardKeyIsUp = null;

        public static Dictionary<string, bool> GetKeyboardKey
        {
            get
            {
                if(keyboardKeyIsDown == null)
                {
                    keyboardKeyIsDown = new Dictionary<string, bool>();
                    for (byte i = 0; i < byte.MaxValue; i++)
                    {
                        keyboardKeyIsDown.Add(((char)i).ToString(), false);
                    }
                }
                return keyboardKeyIsDown;
            }
        }
        public static Dictionary<string, bool> GetKeyboardKeyUp
        {
            get
            {
                if (keyboardKeyIsUp == null)
                {
                    keyboardKeyIsUp = new Dictionary<string, bool>();
                    for (byte i = 0; i < byte.MaxValue; i++)
                    {
                        keyboardKeyIsUp.Add(((char)i).ToString(), false);
                    }
                }
                return keyboardKeyIsUp;
            }
        }
        public static int PrevY
        {
            get
            {
                return prevY;
            }
        }
        public static int PrevX
        {
            get
            {
                return prevX;
            }
        }
        public static int CurY
        {
            get
            {
                return curY;
            }
        }
        public static int CurX
        {
            get
            {
                return curX;
            }
        }
        public static bool IsMouseLeftButtonDown
        {
            get
            {
                return mouselbDown;
            }
        }
        public static bool IsMouseRightButtonDown
        {
            get
            {
                return mouserbDown;
            }
        }

        public static void OnMouseButton(int button, int state, int x, int y)
        {
            switch (button)
            {
                case Glut.GLUT_LEFT_BUTTON:
                    mouselbDown = (state == Glut.GLUT_DOWN);
                    break;
                case Glut.GLUT_RIGHT_BUTTON:
                    mouserbDown = (state == Glut.GLUT_DOWN);
                    break;
                default:
                    break;
            }
            curX = x;
            curY = y;
            prevX = curX;
            prevY = curY;
        }
        public static void OnMouseMove(int x, int y)
        {
            curX = x;
            curY = y;
            // if the mouse move event is caused by glutWarpPointer then do nothing
            if (curX == prevX && curY == prevY)
            {
                return;
            }

            if (x < 0) Glut.glutWarpPointer((prevX = Program.GetWidth), y);
            else if (x > Program.GetWidth) Glut.glutWarpPointer((int)(prevX = 0), y);

            if (y < 0) Glut.glutWarpPointer(x, (prevY = Program.GetHeight));
            else if (y > Program.GetHeight) Glut.glutWarpPointer(x, (prevY = 0));
        }
        public static void OnEndOfFrame()
        {
            prevX = CurX;
            prevY = CurY;

            if (keyboardKeyIsUp != null)
            {
                for (byte i = 0; i < byte.MaxValue; i++)
                {
                    keyboardKeyIsUp[((char)i).ToString()] = false;
                }
            }
        }
        public static void OnKeyboardDown(byte key, int x, int y)
        {
            GetKeyboardKey[((char)key).ToString()] = true;
        }
        public static void OnKeyboardUp(byte key, int x, int y)
        {
            GetKeyboardKey[((char)key).ToString()] = false;
            GetKeyboardKeyUp[((char)key).ToString()] = true;
        }
    }
}
