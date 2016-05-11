using System;
using OpenGL;

namespace openglcsharp.Engine
{
    public class Transform : GameComponent
    {
        private Vector3 position;
        private Quaternion orientation;
        private Vector3 scale;
        private bool dirty = false;

        public Vector3 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
                dirty = true;
            }
        }
        public Vector3 Scale
        {
            get
            {
                return scale;
            }

            set
            {
                scale = value;
            }
        }
        public Quaternion Orientation
        {
            get { return orientation; }
            set
            {
                orientation = value;
                dirty = true;
            }
        }
        public bool IsDirty
        {
            get
            {
                return dirty;
            }
        }

        public Transform()
        {
            Position = Vector3.Zero;
            Orientation = Quaternion.Identity;
            Scale = new Vector3(1, 1, 1);
        }
        public Transform(Vector3 position)
        {
            this.Position = position;
            Orientation = Quaternion.Identity;
            Scale = new Vector3(1, 1, 1);
        }
        public Transform(Vector3 position, Quaternion rotation)
        {
            this.Position = position;
            this.Orientation = rotation;
            this.Scale = new Vector3(1,1,1);
        }
        public Transform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.Position = position;
            this.orientation = rotation;
            this.Scale = scale;
        }

        public void SetNotDirty()
        {
            dirty = false;
        }
        public void Rotate(Quaternion rotation)
        {
            Orientation = rotation * orientation;
        }
        public void Rotate(float angle, Vector3 axis)
        {
            Rotate(Quaternion.FromAngleAxis(angle, axis));
        }
        public void Roll(float angle)
        {
            Vector3 axis = orientation * Vector3.UnitZ;
            Rotate(angle, axis);
        }
        public void Yaw(float angle)
        {
            //Convert to angles from radians.
            //angle = angle * (float)(180 / Math.PI);
            // this method assumes that the y direction will always be 'up', so we've fixed the yaw
            // which is more useful for FPS games, etc.  For flight simulators, or other applications
            // of an unfixed yaw, simply replace Vector3.Up with (orientation * Vector3.UnitY)
            Rotate(angle, Vector3.Up);
        }
        public void Pitch(float angle)
        {
            Vector3 axis = orientation * Vector3.UnitX;
            Rotate(angle, axis);
        }
        public Vector3 GetForwardVector()
        {
            Quaternion q = Orientation.Inverse();
            return q * Vector3.Forward;
        }
        public Vector3 GetRightVector()
        {
            return new Vector3(1 - 2 * (Orientation.y * Orientation.y + Orientation.z * Orientation.z),
                    2 * (Orientation.x * Orientation.y + Orientation.w * Orientation.z),
                    2 * (Orientation.x * Orientation.z - Orientation.w * Orientation.y));
        }
        /// <summary>
        /// Moves the camera by modifying the position directly.
        /// </summary>
        /// <param name="by">The amount to move the position by.</param>
        public void Move(Vector3 by)
        {
            Position += by;
        }
        /// <summary>
        /// Moves the camera taking into account the orientation of the camera.
        /// This is useful if you want to move in the direction that the camera is facing.
        /// </summary>
        /// <param name="by">The amount to move the position by, relative to the camera.</param>
        public void MoveRelative(Vector3 by)
        {
            Position += Orientation * by;
        }
        public override void Start()
        {
            base.Start();
        }
        public override void Update()
        {
            base.Update();
        }
    }
}
