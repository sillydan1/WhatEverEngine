using System;
using Tao.FreeGlut;
using OpenGL;
using System.IO;
using WhateverEngine.Engine;
using PhysX;

namespace WhateverEngine
{
    class Program
    {
        private static int width = 1250;
        private static int height = 720;
        private static float fixedUpdateTime = 0.01f, fixedUpdateTimer = 0.0f;
        private static int framCounter = 0;
        private static ShaderProgram program;
        private static System.Diagnostics.Stopwatch watch;
        private static string VertexShader;
        private static string FragmentShader;
        private static string errorLog = "";
        private static SceneManager sceneMan;
        private static Random random;
        private static float netTime = 0.0f, netTimer = 0.0f;
        private static bool isServer;
        public static Random Random
        {
            get
            {
                if (random == null)
                    random = new Random(128);

                return random;
            }
        }
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
            Input.Start();
            Glut.glutMouseFunc(OnMouse);
            //Glut.glutMotionFunc(OnMove);
            Glut.glutPassiveMotionFunc(OnMove);

            Glut.PassiveMotionCallback moveEvent = new Glut.PassiveMotionCallback(OnMove);


            Glut.glutReshapeFunc(OnReshape);

            // enable depth testing to ensure correct z-ordering of our fragments
            Gl.Enable(EnableCap.DepthTest);
            Gl.Enable(EnableCap.Blend);
            Gl.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            // Load the shaders into memory and compile the shader program
            LoadShaders();
            program = new ShaderProgram(VertexShader, FragmentShader);

            program.Use();

            InitializePhysics();
            StartScene();

            watch = System.Diagnostics.Stopwatch.StartNew();
            Glut.glutMainLoop();
        }

        private static void StartScene()
        {
            //This is where we spawn all of our GameObjects and initialize our Scene Manager.
            sceneMan = new SceneManager();
            isServer = true;
            NetworkClass.Instance.Start(isServer); // Network stuff
            if (isServer)
            {
                ServerScene();
            }
            else
            {
                ClientScene();
            }
        }

        private static void ServerScene()
        {

            //-----------------First person controller------------------

            GameObject player1 = new GameObject("Character", "Player", new Transform(Vector3.Zero));
            player1.AddGameComponent(new PythonComponent(@"Python Scripts\CharacterController.py"));
            player1.AddGameComponent(new Renderer(@"data\sphere.obj"));
            player1.id = 1;
            player1.NetworkStatic = true;

            GameObject player2 = new GameObject(new Transform(Vector3.Zero));
            player2.AddGameComponent(new Renderer(@"data\sphere.obj"));
            player2.id = 2;

            GameObject cameraGO = new GameObject(new Transform(new Vector3(0, 3, 10), Quaternion.FromAngleAxis((float)Math.PI, Vector3.Up), player1.Transform));
            cameraGO.AddGameComponent(new PythonComponent(@"Python Scripts\CameraControlScript.py"));
            cameraGO.AddGameComponent(new CameraComponent());

            GameObject gun = new GameObject(new Transform(new Vector3(0.5f, -0.4f, -0.8f), Quaternion.Identity, new Vector3(0.02f, 0.02f, 0.02f), cameraGO.Transform));
            gun.AddGameComponent(new PythonComponent(@"Python Scripts\PlayerGun.py"));
            gun.AddGameComponent(new PythonComponent(@"Python Scripts\CharacterMouseController.py"));
            gun.AddGameComponent(new Renderer(@"data\rifle.obj"));

            GameObject gunC = new GameObject(new Transform(new Vector3(0.5f, -0.4f, -0.8f), Quaternion.Identity, new Vector3(0.02f, 0.02f, 0.02f), player2.Transform));
            gunC.AddGameComponent(new Renderer(@"data\rifle.obj"));

            GameObject skybox = new GameObject("skybox", "SkyBox", new Transform(Vector3.Up * -10, Quaternion.Identity, new Vector3(200, 200, 200)));
            skybox.AddGameComponent(new Renderer(@"data\skybox2.obj"));

            //-----------------First person controller------------------

            GameObject groundPlane = new GameObject(new Transform(Vector3.Zero, Quaternion.FromAngleAxis((float)Math.PI / 2, new Vector3(0, 0, 3))));
            groundPlane.AddGameComponent(new PhysicsComponent(new PlaneGeometry(), 1.0f, scene.Physics.CreateMaterial(0.1f, 0.1f, 0.1f), false));
            groundPlane.AddGameComponent(new Renderer(@"data\arrow.obj"));

            GameObject ball = new GameObject(new Transform(Vector3.Zero));
            ball.AddGameComponent(new PhysicsComponent(scene.Physics.CreateMaterial(1.0f, 1.0f, 0.0f)));
            ball.AddGameComponent(new Renderer(@"data\sphere.obj"));
            ball.id = 10;
            ball.NetworkStatic = true;

            GameObject floor = new GameObject(new Transform(Vector3.Down));
            floor.Transform.Scale = new Vector3(1000, 0, 1000);
            floor.AddGameComponent(new Renderer(@"data\box.obj"));

            sceneMan.Instantiate(gun);
            sceneMan.Instantiate(gunC);
            sceneMan.Instantiate(skybox);
            sceneMan.Instantiate(player1);
            sceneMan.Instantiate(player2);
            sceneMan.Instantiate(groundPlane);
            sceneMan.Instantiate(cameraGO);
            sceneMan.Instantiate(ball);
            sceneMan.Instantiate(floor);
            sceneMan.CheckAddList();
            Vector3 v3 = new Vector3();
        }

        private static void ClientScene()
        {
            //-----------------First person controller------------------

            GameObject player1 = new GameObject(new Transform(Vector3.Zero));
            player1.AddGameComponent(new Renderer(@"data\sphere.obj"));
            player1.id = 1;

            GameObject player2 = new GameObject("Character", "Player", new Transform(Vector3.Zero));
            player2.AddGameComponent(new PythonComponent(@"Python Scripts\CharacterController.py"));
            player2.AddGameComponent(new Renderer(@"data\sphere.obj"));
            player2.id = 2;
            player2.NetworkStatic = true;

            GameObject cameraGO = new GameObject(new Transform(new Vector3(0, 3, 10), Quaternion.FromAngleAxis((float)Math.PI, Vector3.Up), player2.Transform));
            cameraGO.AddGameComponent(new PythonComponent(@"Python Scripts\CameraControlScript.py"));
            cameraGO.AddGameComponent(new CameraComponent());

            GameObject gun = new GameObject(new Transform(new Vector3(0.5f, -0.4f, -0.8f), Quaternion.Identity, new Vector3(0.02f, 0.02f, 0.02f), player1.Transform));
            gun.AddGameComponent(new Renderer(@"data\rifle.obj"));

            GameObject gunC = new GameObject(new Transform(new Vector3(0.5f, -0.4f, -0.8f), Quaternion.Identity, new Vector3(0.02f, 0.02f, 0.02f), cameraGO.Transform));
            gunC.AddGameComponent(new PythonComponent(@"Python Scripts\PlayerGun.py"));
            gunC.AddGameComponent(new PythonComponent(@"Python Scripts\CharacterMouseController.py"));
            gunC.AddGameComponent(new Renderer(@"data\rifle.obj"));

            GameObject skybox = new GameObject("skybox", "SkyBox", new Transform(Vector3.Up * -10, Quaternion.Identity, new Vector3(200, 200, 200)));
            skybox.AddGameComponent(new Renderer(@"data\skybox2.obj"));

            //-----------------Physics Game objects---------------------

            GameObject groundPlane = new GameObject(new Transform(Vector3.Down * 5.0f, Quaternion.FromAngleAxis((float)Math.PI / 2, new Vector3(0, 0, 3))));
            groundPlane.AddGameComponent(new PhysicsComponent(new PlaneGeometry(), 1.0f, scene.Physics.CreateMaterial(0.1f, 0.1f, 0.1f), false));
            groundPlane.AddGameComponent(new Renderer(@"data\arrow.obj"));

            GameObject ball = new GameObject(new Transform(Vector3.Zero));
            //ball.AddGameComponent(new PhysicsComponent(scene.Physics.CreateMaterial(1.0f, 1.0f, 0.0f)));
            ball.AddGameComponent(new Renderer(@"data\sphere.obj"));
            ball.id = 10;

            GameObject floor = new GameObject(new Transform(Vector3.Down));
            floor.Transform.Scale = new Vector3(1000, 0, 1000);
            floor.AddGameComponent(new Renderer(@"data\box.obj"));

            sceneMan.Instantiate(gun);
            sceneMan.Instantiate(gunC);
            sceneMan.Instantiate(skybox);
            sceneMan.Instantiate(player1);
            sceneMan.Instantiate(player2);
            sceneMan.Instantiate(groundPlane);
            sceneMan.Instantiate(cameraGO);
            sceneMan.Instantiate(ball);
            sceneMan.Instantiate(floor);
            sceneMan.CheckAddList();

        }

        private static void InitializePhysics()
        {
            Foundation foundation = new Foundation();
            physics = new Physics(foundation, checkRuntimeFiles: true);

#if GPU
			var cudaContext = new CudaContextManager(foundation);
#endif

            SceneDesc sceneDesc = new SceneDesc()
            {
                Gravity = new PhysX.Math.Vector3(0, -9.81f, 0),
#if GPU
				GpuDispatcher = cudaContext.GpuDispatcher
#endif
            };

            scene = physics.CreateScene(sceneDesc);

            scene.SetVisualizationParameter(VisualizationParameter.Scale, 2.0f);
            scene.SetVisualizationParameter(VisualizationParameter.CollisionShapes, true);
            scene.SetVisualizationParameter(VisualizationParameter.JointLocalFrames, true);
            scene.SetVisualizationParameter(VisualizationParameter.JointLimits, true);
            scene.SetVisualizationParameter(VisualizationParameter.ParticleSystemPosition, true);
            scene.SetVisualizationParameter(VisualizationParameter.ActorAxes, true);

            // Connect to the remote debugger (if it's there)
            if (physics.RemoteDebugger != null)
            {
                physics.RemoteDebugger.Connect("localhost");
            }
            //CreateGroundPlane();
        }

        private static void CreateGroundPlane()
        {
            var groundPlaneMaterial = scene.Physics.CreateMaterial(0.1f, 0.1f, 0.1f);

            var groundPlane = scene.Physics.CreateRigidStatic();
            groundPlane.GlobalPose = PhysX.Math.Matrix.RotationAxis(new PhysX.Math.Vector3(0, 0, 3), (float)System.Math.PI / 2);

            var planeGeom = new PlaneGeometry();

            groundPlane.CreateShape(planeGeom, groundPlaneMaterial);

            scene.AddActor(groundPlane);
        }

        public static Scene scene { get; private set; }
        public static Physics physics { get; private set; }

        //EngineFunctions functions
        public static void Instantiate(GameObject newGobject)
        {
            sceneMan.Instantiate(newGobject);
        }
        public static GameObject GetGameObjectWithTag(string tag)
        {
            return sceneMan.GetGameObjectWithTag(tag);
        }
        public static GameObject[] GetGameObjectsWithTag(string tag)
        {
            return sceneMan.GetGameObjectsWithTag(tag);
        }
        public static GameObject GetGameObjectWithId(int id)
        {
            return sceneMan.GetGameObjectWithId(id);
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
            physics.Dispose();
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

            fixedUpdateTimer += deltaTime;
            if (fixedUpdateTimer >= fixedUpdateTime)
            {
                scene.Simulate(fixedUpdateTime);
                scene.FetchResults(block: true);
                fixedUpdateTimer = 0;
                OnNetUpdate();
                if (framCounter >= 30)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    framCounter = 0;
                }
                else
                    framCounter++;
            }

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

        private static void OnNetUpdate()
        {
            //netTimer += deltaTime;
            //if (netTimer >= netTime)
            //{
            sceneMan.SendNetData();
            //}
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
