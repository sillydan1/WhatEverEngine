using System;
using OpenGL;

namespace WhateverEngine.Engine
{
    public class CameraComponent : GameComponent
    {
        private static float FoV = 0.90f;

        private Matrix4 viewMatrix;
        public Matrix4 ViewMatrix
        {
            get
            {
                if(owner.Transform.IsDirty)
                {
                    viewMatrix = Matrix4.CreateTranslation(-owner.Transform.Position) * owner.Transform.Orientation.Matrix4;
                    owner.Transform.SetNotDirty();
                }
                return viewMatrix;
            }
        }

        public override void Update()
        {
            base.Update();
            Program.ShaderProg["view_matrix"].SetValue(ViewMatrix);
        }
        public override void Start()
        {
            base.Start();
            Program.ShaderProg["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.90f, (float)Program.GetWidth / Program.GetHeight, 1.0f, 1000f));
        }
        public static void OnWindowResize()
        {
            Program.ShaderProg["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(FoV, (float)Program.GetWidth / Program.GetHeight, 1.0f, 1000f));
        }
    }
}
