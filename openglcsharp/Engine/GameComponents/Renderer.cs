using System;
using OpenGL;

namespace WhateverEngine.Engine
{
    public class Renderer : GameComponent
    {
        private MeshFilter meshFilter;

        public Renderer()
        {
            meshFilter = new MeshFilter();
        }
        public Renderer(string meshFilePath)
        {
            meshFilter = new MeshFilter(meshFilePath);
        }

        public override void Update()
        {
            if(meshFilter.GetMeshIndex != -1)
            {
                Matrix4 modelMatrix = Matrix4.CreateScaling(owner.Transform.Scale) * owner.Transform.Orientation.Matrix4 * Matrix4.CreateTranslation(owner.Transform.Position);
                if (ObjCacher.AlreadyLoadedObjects[meshFilter.GetMeshIndex].Objects[0].Material != null)
                {
                    ObjCacher.AlreadyLoadedObjects[meshFilter.GetMeshIndex].Objects[0].Material.Program["model_matrix"].SetValue(modelMatrix);
                }
                else
                {
                    Program.ShaderProg["model_matrix"].SetValue(modelMatrix);
                }
                ObjCacher.AlreadyLoadedObjects[meshFilter.GetMeshIndex].Draw();
                Gl.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }
        }
        public override void Start()
        {

        }
        public void Dispose()
        {
            meshFilter.Dispose();
        }
    }
}
