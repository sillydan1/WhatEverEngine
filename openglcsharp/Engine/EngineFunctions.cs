
namespace WhateverEngine.Engine
{
    public static class EngineFunctions
    {
        public static void Instantiate(GameObject obj)
        {
            Program.Instantiate(obj);
        }        
        public static void Destroy(GameObject obj)
        {
            Program.Destroy(obj);
        }
        public static GameObject GetGameObjectWithTag(string tag)
        {
            return Program.GetGameObjectWithTag(tag);
        }
        public static GameObject[] GetGameObjectsWithTag(string tag)
        {
            return Program.GetGameObjectsWithTag(tag);
        }
        public static GameObject GetGameObjectWithId(int id)
        {
            return Program.GetGameObjectWithId(id);
        }
    }
}
