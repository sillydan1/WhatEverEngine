using System;
using System.Collections.Generic;

namespace openglcsharp.Engine
{
    public class GameObject : IDisposable
    {
        private Transform transform;
        private List<GameComponent> myComponents = new List<GameComponent>();

        public Transform Transform
        {
            get
            {
                return transform;
            }
        }
        //Constructor
        public GameObject()
        {
            transform = new Transform();
            transform.AddToGameObject(this);
        }
        public GameObject(Transform transform)
        {
            this.transform = transform;
            transform.AddToGameObject(this);
        }
        //Called once per frame
        public void Update()
        {
            //Update all attached components
            foreach (GameComponent component in myComponents)
            {
                component.Update();
            }
        }
        //Called when the game starts
        public void Start()
        {
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
            if(myComponents.Contains(whatComponent))
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
                if(item is Renderer)
                {
                    //Dispose of the render data
                    (item as Renderer).Dispose();
                    return;
                }
            }
        }
    }
}
