using OpenGL;
using System.Collections.Generic;

namespace openglcsharp.Engine
{
    public class MeshFilter
    {
        public static List<ObjLoader> allLoadedOBJs = new List<ObjLoader>();

        private int myMeshIndex;
        public int GetMeshIndex
        {
            get
            {
                return myMeshIndex;
            }
        }

        public MeshFilter()
        {
            myMeshIndex = -1;
        }
        public MeshFilter(string fileName)
        {
            MeshFilter.allLoadedOBJs.Add(new ObjLoader(fileName, Program.ShaderProg));
            myMeshIndex = allLoadedOBJs.Count - 1;
        }
    }
}
