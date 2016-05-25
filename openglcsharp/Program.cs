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
        private static NetworkClass nwc;
        private static float netTime = 0.0f, netTimer = 0.0f;
        private static bool isServer;
        public static NetworkClass Nwc
        {
            get
            {
                return nwc;
            }
        }
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
        public static Scene scene { get; private set; }
        public static Physics physics { get; private set; }

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

            GameObject cameraGO = new GameObject(new Transform(new Vector3(0, 3, 10)));
            cameraGO.AddGameComponent(new CameraComponent());
            cameraGO.AddGameComponent(new PythonComponent(@"Python Scripts\CameraControlScript.py"));

            GameObject physicsGO = new GameObject("Character", "Player", new Transform(Vector3.Zero, cameraGO.Transform));
            physicsGO.AddGameComponent(new Renderer(@"data\sphere.obj"));
            physicsGO.id = 1;
            physicsGO.NetworkStatic = true;

            GameObject physicsGOC = new GameObject(new Transform(Vector3.Zero));
            physicsGOC.AddGameComponent(new Renderer(@"data\sphere.obj"));
            physicsGOC.id = 2;

            GameObject gun = new GameObject(new Transform(new Vector3(1, 0, 0), Quaternion.Identity, new Vector3(0.02f, 0.02f, 0.02f), physicsGO.Transform));
            gun.AddGameComponent(new Renderer(@"data\rifle.obj"));

            GameObject gun2 = new GameObject(new Transform(new Vector3(-1, 0, 0), Quaternion.Identity, new Vector3(0.02f, 0.02f, 0.02f), physicsGO.Transform));
            gun2.AddGameComponent(new Renderer(@"data\rifle.obj"));

            GameObject gunC = new GameObject(new Transform(new Vector3(1, 0, 0), Quaternion.Identity, new Vector3(0.02f, 0.02f, 0.02f), physicsGOC.Transform));
            gunC.AddGameComponent(new Renderer(@"data\rifle.obj"));

            GameObject gun2C = new GameObject(new Transform(new Vector3(-1, 0, 0), Quaternion.Identity, new Vector3(0.02f, 0.02f, 0.02f), physicsGOC.Transform));
            gun2C.AddGameComponent(new Renderer(@"data\rifle.obj"));

            GameObject skybox = new GameObject("skybox", "SkyBox", new Transform(Vector3.Up * -10, Quaternion.Identity, new Vector3(200, 200, 200)));
            skybox.AddGameComponent(new Renderer(@"data\skybox2.obj"));

            //-----------------First person controller------------------

            GameObject refferenceGo = new GameObject(new Transform(cameraGO.Transform.Position + cameraGO.Transform.GetForwardVector() * 11));
            refferenceGo.AddGameComponent(new Renderer(@"data\box.obj"));

            GameObject groundPlane = new GameObject(new Transform(Vector3.Zero, Quaternion.FromAngleAxis((float)Math.PI / 2, new Vector3(0, 0, 3))));
            groundPlane.AddGameComponent(new PhysicsComponent(new PlaneGeometry(), 1.0f, scene.Physics.CreateMaterial(0.1f, 0.1f, 0.1f), false));
            groundPlane.AddGameComponent(new Renderer(@"data\arrow.obj"));

            GameObject netCube = new GameObject(new Transform(Vector3.Zero));
            netCube.AddGameComponent(new PythonComponent(@"Python Scripts\NetCubeTest.py"));
            netCube.AddGameComponent(new Renderer(@"data\box.obj"));
            netCube.id = 5;
            netCube.NetworkStatic = true;

            GameObject netCube2 = new GameObject(new Transform(Vector3.Zero));
            netCube2.AddGameComponent(new Renderer(@"data\box.obj"));
            netCube2.id = 6;

            sceneMan.Instantiate(gun);
            sceneMan.Instantiate(gun2);
            sceneMan.Instantiate(skybox);
            sceneMan.Instantiate(physicsGO);
            sceneMan.Instantiate(physicsGOC);
            sceneMan.Instantiate(netCube);
            sceneMan.Instantiate(netCube2);
            sceneMan.Instantiate(groundPlane);
            sceneMan.Instantiate(cameraGO);
            sceneMan.CheckAddList();
        }

        private static void ClientScene()
        {
            //-----------------First person controller------------------

            GameObject cameraGO = new GameObject(new Transform(new Vector3(0, 3, 10)));
            cameraGO.AddGameComponent(new CameraComponent());
            cameraGO.AddGameComponent(new PythonComponent(@"Python Scripts\CameraControlScript.py"));

            GameObject physicsGO = new GameObject("Character", "Player", new Transform(Vector3.Zero));
            physicsGO.AddGameComponent(new Renderer(@"data\sphere.obj"));
            physicsGO.id = 1;

            GameObject physicsGOC = new GameObject(new Transform(Vector3.Zero, cameraGO.Transform));
            physicsGOC.AddGameComponent(new Renderer(@"data\sphere.obj"));
            physicsGOC.id = 2;
            physicsGOC.NetworkStatic = true;

            GameObject gun = new GameObject(new Transform(new Vector3(1, 0, 0), Quaternion.Identity, new Vector3(0.02f, 0.02f, 0.02f), physicsGO.Transform));
            gun.AddGameComponent(new Renderer(@"data\rifle.obj"));

            GameObject gun2 = new GameObject(new Transform(new Vector3(-1, 0, 0), Quaternion.Identity, new Vector3(0.02f, 0.02f, 0.02f), physicsGO.Transform));
            gun2.AddGameComponent(new Renderer(@"data\rifle.obj"));

            GameObject gunC = new GameObject(new Transform(new Vector3(1, 0, 0), Quaternion.Identity, new Vector3(0.02f, 0.02f, 0.02f), physicsGOC.Transform));
            gunC.AddGameComponent(new Renderer(@"data\rifle.obj"));

            GameObject gun2C = new GameObject(new Transform(new Vector3(-1, 0, 0), Quaternion.Identity, new Vector3(0.02f, 0.02f, 0.02f), physicsGOC.Transform));
            gun2C.AddGameComponent(new Renderer(@"data\rifle.obj"));

            GameObject skybox = new GameObject("skybox", "SkyBox", new Transform(Vector3.Up * -10, Quaternion.Identity, new Vector3(200, 200, 200)));
            skybox.AddGameComponent(new Renderer(@"data\skybox2.obj"));

            //-----------------First person controller------------------

            GameObject refferenceGo = new GameObject(new Transform(cameraGO.Transform.Position + cameraGO.Transform.GetForwardVector() * 11));
            refferenceGo.AddGameComponent(new Renderer(@"data\box.obj"));

            GameObject groundPlane = new GameObject(new Transform(Vector3.Zero, Quaternion.FromAngleAxis((float)Math.PI / 2, new Vector3(0, 0, 3))));
            groundPlane.AddGameComponent(new PhysicsComponent(new PlaneGeometry(), 1.0f, scene.Physics.CreateMaterial(0.1f, 0.1f, 0.1f), false));
            groundPlane.AddGameComponent(new Renderer(@"data\arrow.obj"));

            //GameObject netCube = new GameObject(new Transform(Vector3.Zero));
            //netCube.AddGameComponent(new PythonComponent(@"Python Scripts\NetCubeTest.py"));
            //netCube.AddGameComponent(new Renderer(@"data\box.obj"));
            //netCube.id = 5;
            //netCube.NetworkStatic = true;

            //GameObject netCube2 = new GameObject(new Transform(Vector3.Zero));
            //netCube2.AddGameComponent(new Renderer(@"data\box.obj"));
            //netCube2.id = 6;

            sceneMan.Instantiate(gun);
            sceneMan.Instantiate(gun2);
            sceneMan.Instantiate(skybox);
            sceneMan.Instantiate(physicsGO);
            sceneMan.Instantiate(physicsGOC);
            //sceneMan.Instantiate(netCube);
            //sceneMan.Instantiate(netCube2);
            sceneMan.Instantiate(groundPlane);
            sceneMan.Instantiate(cameraGO);
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
            OnNetUpdate();

            fixedUpdateTimer += deltaTime;
            if (fixedUpdateTimer >= fixedUpdateTime)
            {
                scene.Simulate(fixedUpdateTime);
                scene.FetchResults(block: true);
                fixedUpdateTimer = 0;
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
            netTimer += deltaTime;
            if (netTimer >= netTime)
            {
                sceneMan.SendNetData();
                netTimer = 0.0f;
            }
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
