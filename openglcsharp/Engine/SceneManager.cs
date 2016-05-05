using System.Collections.Generic;
using OpenGL;

namespace openglcsharp.Engine
{
    public class SceneManager
    {
        private List<GameObject> sceneObjects;
        private List<GameObject> deleteList;
        private List<GameObject> addList;

        public SceneManager()
        {
            sceneObjects = new List<GameObject>();
            deleteList = new List<GameObject>();
            addList = new List<GameObject>();
        }

        public void Update()
        {
            UpdateSceneObjects();
            ClearOutDeleteList();
            CheckAddList();
        }
        public void Start()
        {
            for (int i = 0; i < sceneObjects.Count; i++)
            {
                sceneObjects[i].Start();
            }
        }
        private void UpdateSceneObjects()
        {
            for (int i = 0; i < sceneObjects.Count; i++)
            {
                sceneObjects[i].Update();
            }
        }
        private void ClearOutDeleteList()
        {
            if (deleteList.Count > 0)
            {
                foreach (GameObject obj in deleteList)
                {
                    if (sceneObjects.Contains(obj))
                        sceneObjects.Remove(obj);
                }
                deleteList.Clear();
            }
        }
        public void CheckAddList()
        {
            if(addList.Count > 0)
            {
                for (int i = 0; i < addList.Count; i++)
                {
                    sceneObjects.Add(addList[i]);
                    addList[i].Start();
                }
                addList.Clear();
            }
        }
        public void Instantiate(GameObject obj)
        {
            addList.Add(obj);
        }
        public void Destroy(GameObject obj)
        {
            if (sceneObjects.Contains(obj))
                deleteList.Add(obj);
        }
    }
}
