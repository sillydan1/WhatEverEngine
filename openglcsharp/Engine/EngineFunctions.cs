using PhysX;
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
    
    public class WhateverRay
    {
        public bool hit = false;
        public RaycastHit[] rayHit;

        public void CastRay(OpenGL.Vector3 origin, OpenGL.Vector3 direction, float distance, int maximumHits)
        {
            System.Func<PhysX.RaycastHit[], bool> hitCall = ReturnHitResults;
            PhysX.Math.Vector3 o = new PhysX.Math.Vector3(origin.x, origin.y, origin.z);
            PhysX.Math.Vector3 d = new PhysX.Math.Vector3(direction.x, direction.y, direction.z);
            bool b = Program.scene.Raycast(o, d, distance, maximumHits, hitCall);
            hit = b;
        }
        public string GetNameOfFirstHit()
        {
            if(rayHit != null)
            {
                if(rayHit[0] != null)
                {
                    return rayHit[0].Actor.Name;
                }
            }
            string s = "ERROR: rayhit is null.";
            Program.LogError(s);
            return s;
        }
        private bool ReturnHitResults(RaycastHit[] b)
        {
            rayHit = b;
            if (b != null)
            {
                if (b[0] != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
