using OpenGL;
using System.Collections.Generic;

namespace WhateverEngine.Engine
{
    public class MeshFilter
    {
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
            myMeshIndex = ObjCacher.SafeGetNewObjLoader(fileName, Program.ShaderProg);
        }

        public void Dispose()
        {
            //Ech..
        }
    }
}
