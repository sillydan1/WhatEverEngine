using System;

namespace openglcsharp.Engine
{
    public class GameComponent
    {
        protected GameObject owner;

        public GameObject GetOwner
        {
            get
            {
                return owner;
            }
        }

        public virtual void AddToGameObject(GameObject owner)
        {
            this.owner = owner;
        }
        public virtual void RipAwayFromOwner()
        {
            this.owner = null;
        }
        public virtual void Update()
        {

        }
        public virtual void Start()
        {

        }
    }
}
