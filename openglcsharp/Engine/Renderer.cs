using System;
using OpenGL;

namespace openglcsharp.Engine
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
                Matrix4 modelMatrix = owner.Transform.Orientation.Matrix4 * Matrix4.CreateTranslation(owner.Transform.Position);
                if (MeshFilter.allLoadedOBJs[meshFilter.GetMeshIndex].Objects[0].Material != null)
                {
                    MeshFilter.allLoadedOBJs[meshFilter.GetMeshIndex].Objects[0].Material.Program["model_matrix"].SetValue(modelMatrix);
                }
                else
                {
                    Program.ShaderProg["model_matrix"].SetValue(modelMatrix);
                }
                MeshFilter.allLoadedOBJs[meshFilter.GetMeshIndex].Draw();
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
