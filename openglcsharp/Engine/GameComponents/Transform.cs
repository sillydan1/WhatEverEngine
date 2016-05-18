using System;
using OpenGL;

namespace WhateverEngine.Engine
{
    public class Transform : GameComponent
    {
        private Vector3 position;
        private Vector3 localPosition;
        private Quaternion orientation;
        private Quaternion localOrientation;
        private Vector3 scale;
        private bool dirty = false;
        private Transform parent;
        public Vector3 previousPos;
        public Quaternion previousRot;
        private Vector3 movePrediction;
        private Quaternion rotPrediction;

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
        public Vector3 LocalPosition
        {
            get
            {
                return localPosition;
            }
            set
            {
                localPosition = value;
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
        public Vector3 MovePrediction
        {
            get
            {
                return movePrediction;
            }

            set
            {
                movePrediction = value;
            }
        }
        public Quaternion RotPrediction
        {
            get
            {
                return rotPrediction;
            }

            set
            {
                rotPrediction = value;
            }
        }

        public Transform()
        {
            ConstructorMethod(Vector3.Zero, Quaternion.Identity, new Vector3(1, 1, 1), null);
        }
        public Transform(Vector3 position, Transform parent = null)
        {
            ConstructorMethod(position, Quaternion.Identity, new Vector3(1, 1, 1), parent);
        }
        public Transform(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            ConstructorMethod(position, rotation, new Vector3(1, 1, 1), parent);
        }
        public Transform(Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null)
        {
            ConstructorMethod(position, rotation, scale, parent);
        }
        private void ConstructorMethod(Vector3 position, Quaternion rotation, Vector3 scale, Transform parent)
        {
            this.Position = position;
            this.orientation = rotation;
            this.localOrientation = rotation;
            this.Scale = scale;
            this.parent = parent;
            if (parent != null)
            {
                this.localPosition = position - parent.Position;
            }
            previousPos = position;
            previousRot = rotation;
        }

        public void SetParent(Transform parent)
        {
            this.parent = parent;
        }
        public void SetNotDirty()
        {
            dirty = false;
        }
        public void Rotate(Quaternion rotation)
        {
            if (parent != null)
            {
                localOrientation = rotation * localOrientation;
                Orientation = rotation * orientation;
            }
            else
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
        public void SetRotation(Quaternion newRot)
        {
            orientation = newRot;
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
            if (parent != null)
            {
                localPosition += by;
            }
            else
            {
                Position += by;
                if (GetOwner.GetPhysics != null)
                {
                    GetOwner.GetPhysics.ChangePosition(position);
                }
            }
        }
        /// <summary>
        /// Moves the camera taking into account the orientation of the camera.
        /// This is useful if you want to move in the direction that the camera is facing.
        /// </summary>
        /// <param name="by">The amount to move the position by, relative to the camera.</param>
        public void MoveRelative(Vector3 by)
        {
            if (parent != null)
            {
                localPosition += Orientation * by;
            }
            else
            {
                Position += Orientation * by;

                if (GetOwner.GetPhysics != null)
                    GetOwner.GetPhysics.ChangePosition(position);
            }
        }
        public override void Start()
        {
            base.Start();
        }
        public override void Update()
        {
            base.Update();
            if (parent != null)
            {
                position = (parent.position + (parent.Orientation * localPosition));
                //orientation = localOrientation * parent.Orientation;
            }
            if (movePrediction != Vector3.Zero)
            {
                Move(movePrediction * Program.DeltaTime);
            }
            if (rotPrediction != Quaternion.Zero)
            {
                Rotate(rotPrediction * Program.DeltaTime);
            }
        }
    }
}
