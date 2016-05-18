using System;
using PhysX;
using OpenGL;

namespace WhateverEngine.Engine
{
    public class PhysicsComponent : GameComponent
    {
        private Material material;
        private RigidActor rigidActor;
        private PhysX.Geometry myGeo;
        private bool isDynamic;

        //Constructors
        public PhysicsComponent()
        {
            ConstructorMethod(new SphereGeometry(1.0f), 10.0f, Program.scene.Physics.CreateMaterial(0.1f, 0.1f, 1.8f), true);
        }
        public PhysicsComponent(PhysX.Geometry geometry)
        {
            ConstructorMethod(geometry, 10.0f, Program.scene.Physics.CreateMaterial(0.1f, 0.1f, 1.8f), true);
        }
        public PhysicsComponent(PhysX.Geometry geometry, float mass)
        {
            ConstructorMethod(geometry, mass, Program.scene.Physics.CreateMaterial(0.1f, 0.1f, 1.8f), true);
        }
        public PhysicsComponent(PhysX.Geometry geometry, float mass, Material mat)
        {
            ConstructorMethod(geometry, mass, mat, false);
        }
        public PhysicsComponent(Material mat)
        {
            ConstructorMethod(new SphereGeometry(1.0f), 10.0f, mat, true);
        }
        public PhysicsComponent(PhysX.Geometry geometry, float mass, Material mat, bool isDynamic)
        {
            ConstructorMethod(geometry, mass, mat, isDynamic);
        }
        private void ConstructorMethod(PhysX.Geometry geometry, float mass, Material mat, bool isDynamic)
        {
            material = mat;
            
            if (isDynamic)
            {
                rigidActor = Program.scene.Physics.CreateRigidDynamic();
                ((RigidDynamic)rigidActor).SetMassAndUpdateInertia(mass);
            }
            else
            {
                rigidActor = Program.scene.Physics.CreateRigidStatic();
                rigidActor.GlobalPose = PhysX.Math.Matrix.RotationAxis(new PhysX.Math.Vector3(0, 0, 3), (float)System.Math.PI / 2);
            }
            this.isDynamic = isDynamic;
            myGeo = geometry;
        }

        public override void Start()
        {
            Shape boxShape = rigidActor.CreateShape(myGeo, material);

            Quaternion q = owner.Transform.Orientation;
            PhysX.Math.Quaternion q2 = new PhysX.Math.Quaternion(q.x, q.y, q.z, q.w);
            
            PhysX.Math.Vector3 physicsStartPos = new PhysX.Math.Vector3(owner.Transform.Position.x, owner.Transform.Position.y, owner.Transform.Position.z);
            rigidActor.GlobalPose = PhysX.Math.Matrix.RotationQuaternion(q2);
            rigidActor.GlobalPose *= PhysX.Math.Matrix.Translation(physicsStartPos);

            Program.scene.AddActor(rigidActor);

            // Spin the cube
            //rigidActor.AddTorque(new PhysX.Math.Vector3(0, 0, 0), ForceMode.Impulse, true);
            base.Start();
        }
        private void ErrorCallBack()
        {

        }
        public override void Update()
        {
            int column = 3;
            PhysX.Math.Vector4 rv = rigidActor.GlobalPose.get_Rows(column);
            PhysX.Math.Vector4 rw = rigidActor.GlobalPose.get_Rows(1);
            Vector3 rigidPos = new Vector3(rv.X, rv.Y, rv.Z);
            Vector4 rot = new Vector4(rw.X, rw.Y, rw.Z, rw.W);
            owner.Transform.SetRotation(new Quaternion(rot));
            owner.Transform.Position = rigidPos;
            
            base.Update();
        }
        public void ChangePosition(Vector3 newPos)
        {
            PhysX.Math.Vector3 v = new PhysX.Math.Vector3(newPos.x, newPos.y, newPos.z);
            rigidActor.GlobalPose = PhysX.Math.Matrix.Translation(v);
            //rigidActor.GlobalPose.set_Rows(3, new PhysX.Math.Vector4(newPos.x, newPos.y, newPos.z, 0.0f));
        }
        public void ChangeRotation(Quaternion newRot)
        {
            PhysX.Math.Quaternion q = new PhysX.Math.Quaternion(newRot.x, newRot.y, newRot.z, newRot.w);
            rigidActor.GlobalPose = PhysX.Math.Matrix.RotationQuaternion(q);
        }
        public void AddForce(Vector3 force)
        {
            if(isDynamic)
            {
                PhysX.Math.Vector3 f = new PhysX.Math.Vector3(force.x, force.y, force.z);
                ((RigidDynamic)rigidActor).AddForce(f);
            }
            else
            {
                Program.LogError("Rigidbody is not dynamic. Forces are not allowed on static rigidactors in PhysX 3.3");
            }
        }
        /// <summary>
        /// Force = 0,
        /// Impulse = 1,
        /// VelocityChange = 2,
        /// Acceleration = 3
        /// </summary>
        public void AddForce(Vector3 force, int forceMode, bool wake)
        {
            if (isDynamic)
            {
                PhysX.Math.Vector3 f = new PhysX.Math.Vector3(force.x, force.y, force.z);
                ((RigidDynamic)rigidActor).AddForce(f, (ForceMode)forceMode, wake);
            }
            else
            {
                Program.LogError("Rigidbody is not dynamic. Forces are not allowed on static rigidactors in PhysX 3.3");
            }
        }
    }
}
