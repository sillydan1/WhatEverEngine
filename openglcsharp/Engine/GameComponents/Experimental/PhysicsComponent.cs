using System;
using PhysX;
using OpenGL;

namespace WhateverEngine.Engine
{
    class PhysicsComponent : GameComponent
    {
        private Material material;
        private RigidDynamic rigidActor;

        public override void Start()
        {
            material = Program.scene.Physics.CreateMaterial(0.7f, 0.7f, 0.7f);

            rigidActor = Program.scene.Physics.CreateRigidDynamic();

            BoxGeometry boxGeo = new BoxGeometry(1, 1, 1);
            var boxShape = rigidActor.CreateShape(boxGeo, material);

            rigidActor.GlobalPose = PhysX.Math.Matrix.Translation(0, 5, 0);
            rigidActor.SetMassAndUpdateInertia(10);

            Program.scene.AddActor(rigidActor);


            // Spin the cube
            rigidActor.AddTorque(new PhysX.Math.Vector3(10000, 0, 0), ForceMode.Impulse, true);
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
    }
}
