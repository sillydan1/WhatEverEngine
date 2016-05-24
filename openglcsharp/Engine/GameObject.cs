using System;
using System.Collections.Generic;

namespace WhateverEngine.Engine
{
    public class GameObject : IDisposable
    {
        private Transform transform;
        private List<GameComponent> myComponents = new List<GameComponent>();
        public string name;
        public int id;
        private string tag;
        public bool NetworkStatic = false;
        public bool LastPosSent = false;
        public bool LastRotSent = false;
        public List<OpenGL.Vector3> Packs = new List<OpenGL.Vector3>();
        float time =1.0f, timer = 0.0f;

        public string GetTag
        {
            get
            {
                return tag;
            }
        }
        public Transform Transform
        {
            get
            {
                return transform;
            }
        }

        //--------------------------------------------------------
        //Properties for IronPython. This segment hurts.
        public PhysicsComponent GetPhysics
        {
            get
            {
                PhysicsComponent p = GetGameComponent<PhysicsComponent>();
                return p;
            }
        }
        //--------------------------------------------------------
        //Constructors
        public GameObject()
        {
            MakeNewGameObject("GameObject", "", new Transform());
        }
        public GameObject(Transform transform)
        {
            MakeNewGameObject("GameObject", "", transform);
        }
        public GameObject(string name)
        {
            MakeNewGameObject(name, "", new Transform());
        }
        public GameObject(string name, string tag, Transform transform)
        {
            MakeNewGameObject(name, tag, transform);
        }
        //Constructor method
        private void MakeNewGameObject(string name, string tag, Transform transform)
        {
            this.name = name;
            this.tag = tag;
            this.transform = transform;
            this.transform.AddToGameObject(this);
        }

        //Called once per frame
        public void Update()
        {
            transform.Update();
            //Update all attached components
            foreach (GameComponent component in myComponents)
            {
                component.Update();
            }
            if (Packs.Count > 2)
            {
                transform.Position = Packs[0];
                transform.MovePrediction = GetSum();
                timer += Program.DeltaTime;
                if (timer >= time)
                {
                    Packs.Clear();
                }
            }
            //transform.previousPos = transform.Position;
            //transform.previousRot = transform.Orientation;

        }
        //Called when the game starts
        public void Start()
        {
            transform.Start();
            //Start all attached components
            foreach (GameComponent component in myComponents)
            {
                component.Start();
            }
        }
        public void AddGameComponent(GameComponent newComponent)
        {
            try
            {
                Transform tr = (Transform)newComponent;
                if (tr != null)
                {
                    Program.LogError("AddGameComponent FAILED: GameObjects can only have one Transform component.");
                    return;
                }
            }
            catch
            {
                //Check if we already have a component of the type that we're trying to add right now.
                myComponents.Add(newComponent);
                newComponent.AddToGameObject(this);
            }
        }
        public void RemoveGameComponent(GameComponent whatComponent)
        {
            if (myComponents.Contains(whatComponent))
            {
                int indexOfComponent = myComponents.IndexOf(whatComponent);
                myComponents.RemoveAt(indexOfComponent);
                whatComponent.RipAwayFromOwner();
            }
            else
                Program.LogError("RemoveGameComponent FAILED: You tried to remove a component that is not attached to this GameObject.");
        }
        public void Dispose()
        {
            foreach (GameComponent item in myComponents)
            {
                if (item is Renderer)
                {
                    //Dispose of the render data
                    (item as Renderer).Dispose();
                    return;
                }
            }
        }
        public T GetGameComponent<T>() where T : GameComponent
        {
            foreach (GameComponent item in myComponents)
            {
                if (item.GetType() == typeof(T))
                {
                    return (T)item;
                }
            }
            return null;
        }

        private OpenGL.Vector3 GetSum()
        {
            float x = 0, y = 0, z = 0;

            x = x * transform.Position.x;
            y = y * transform.Position.y;
            z = z * transform.Position.z;

            OpenGL.Vector3 v3 = (Packs[Packs.Count - 1] - Packs[Packs.Count - 2]);
            v3 *= PhysX.Math.Vector3.Distance(
                new PhysX.Math.Vector3(transform.Position.x, transform.Position.y, transform.Position.z),
                new PhysX.Math.Vector3(v3.x, v3.y, v3.z));
            if (Packs[Packs.Count - 1] == Packs[Packs.Count - 2])
            {
                v3 = OpenGL.Vector3.Zero;
            }
            return v3;
        }
    }
}
