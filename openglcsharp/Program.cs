using System;
using Tao.FreeGlut;
using OpenGL;
using System.IO;
using openglcsharp.Engine;

namespace openglcsharp
{
    class Program
    {
        private static int width = 1280, height = 720;
        private static ShaderProgram program;
        private static System.Diagnostics.Stopwatch watch;
        private static string VertexShader;
        private static string FragmentShader;
        private static string errorLog = "";
        private static SceneManager sceneMan;

        public static int GetWidth
        {
            get
            {
                return width;
            }
        }
        public static int GetHeight
        {
            get
            {
                return height;
            }
        }

        private static float deltaTime;
        public static float DeltaTime
        {
            get
            {
                return deltaTime;
            }
        }

        static void Main(string[] args)
        {
            // create an OpenGL window
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH | Glut.GLUT_ALPHA | Glut.GLUT_STENCIL | Glut.GLUT_MULTISAMPLE);
            Glut.glutInitWindowSize(width, height);
            Glut.glutCreateWindow("Whatever Engine");

            // provide the Glut callbacks that are necessary for running this tutorial
            Glut.glutIdleFunc(OnRenderFrame);
            Glut.glutDisplayFunc(OnDisplay);
            Glut.glutCloseFunc(OnClose);
            Glut.glutKeyboardFunc(OnKeyboardDown);
            Glut.glutKeyboardUpFunc(OnKeyboardUp);
            //Mouse move callbacks
            Glut.glutMouseFunc(OnMouse);
            Glut.glutMotionFunc(OnMove);

            Glut.glutReshapeFunc(OnReshape);

            // enable depth testing to ensure correct z-ordering of our fragments
            Gl.Enable(EnableCap.DepthTest);
            Gl.Enable(EnableCap.Blend);
            Gl.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            // Load teh shaders into memory and compile the shader program
            LoadShaders();
            program = new ShaderProgram(VertexShader, FragmentShader);

            // set the view and projection matrix
            program.Use();
            //program["model_matrix"].SetValue(Matrix4.Identity);

            //program["light_direction"].SetValue(new Vector3(0,0,1));
            StartScene();

            watch = System.Diagnostics.Stopwatch.StartNew();

            Glut.glutMainLoop();
        }

        private static void StartScene()
        {
            //This is where we spawn all of our GameObjects and initialize our Scene Manager.
            sceneMan = new SceneManager();
            GameObject newGobject = new GameObject();
            newGobject.AddGameComponent(new Renderer(@"data\box.obj"));
            newGobject.AddGameComponent(new PythonComponent(@"Python Scripts\Test.py"));

            GameObject cameraGameObject = new GameObject();
            cameraGameObject.AddGameComponent(new CameraComponent());
            cameraGameObject.AddGameComponent(new PythonComponent(@"Python Scripts\CameraControlScript.py"));

            GameObject gunSpawner = new GameObject();
            gunSpawner.AddGameComponent(new PythonComponent(@"Python Scripts\GunSpawnerScript.py"));

            GameObject dbHandler = new GameObject();
            gunSpawner.AddGameComponent(new PythonComponent(@"Python Scripts\DatabaseBullshit.py"));

            GameObject demoObject = new GameObject();
            demoObject.AddGameComponent(new PythonComponent(@"Python Scripts\Blank_Code_Go_Nuts.py"));

            sceneMan.Instantiate(cameraGameObject);
            sceneMan.Instantiate(newGobject);
            sceneMan.Instantiate(gunSpawner);
            sceneMan.Instantiate(dbHandler);
            sceneMan.CheckAddList();
            //sceneMan.Start();
        }

        public static void Instantiate(GameObject newGobject)
        {
            sceneMan.Instantiate(newGobject);
        }

        public static void Destroy(GameObject objToDestroy)
        {
            sceneMan.Destroy(objToDestroy);
        }

        private static void LoadShaders()
        {
            VertexShader = File.ReadAllText(@"Shaders\VertexShader.glsl");
            FragmentShader = File.ReadAllText(@"Shaders\FragmentShader.glsl");
        }

        private static void OnClose()
        {
            // dispose of all of the resources that were created
            program.DisposeChildren = true;
            program.Dispose();
        }

        private static void OnKeyboardDown(byte key, int x, int y)
        {
            if (key == 27) Glut.glutLeaveMainLoop();

            Input.OnKeyboardDown(key, x, y);
        }

        private static void OnKeyboardUp(byte key, int x, int y)
        {
            Input.OnKeyboardUp(key, x, y);
        }

        private static void OnMouse(int button, int state, int x, int y)
        {
            Input.OnMouseButton(button, state, x, y);
        }

        private static void OnMove(int x, int y)
        {
            Input.OnMouseMove(x, y);
        }

        private static void OnReshape(int width, int height)
        {
            Program.width = width;
            Program.height = height;

            CameraComponent.OnWindowResize();
            program.Use();
        }

        private static void OnDisplay()
        {

        }

        private static void OnRenderFrame()
        {
            // calculate how much time has elapsed since the last frame
            watch.Stop();
            deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
            watch.Restart();

            Gl.Enable(EnableCap.Multisample);

            // set up the OpenGL viewport and clear both the color and depth bits
            Gl.Viewport(0, 0, width, height);
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Gl.UseProgram(program);
            program["model_matrix"].SetValue(Matrix4.Identity);

            //Update all the graphics elements.
            sceneMan.Update();

            Glut.glutSwapBuffers();
            Input.OnEndOfFrame();
        }

        public static ShaderProgram ShaderProg
        {
            get
            {
                return program;
            }
        }

        public static void LogError(string message)
        {
            errorLog += message + "\n";
            Console.WriteLine(message);
        }
    }
}
