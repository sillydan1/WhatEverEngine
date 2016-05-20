using System.Collections.Generic;
using OpenGL;

namespace WhateverEngine.Engine
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

                    //Memory leak, but what the heck
                    //obj.Dispose();
                }
                deleteList.Clear();
            }
        }
        public void CheckAddList()
        {
            if (addList.Count > 0)
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
        public GameObject GetGameObjectWithTag(string tag)
        {
            foreach (GameObject item in sceneObjects)
            {
                if (item.GetTag == tag)
                {
                    return item;
                }
            }
            return null;
        }
        public GameObject[] GetGameObjectsWithTag(string tag)
        {
            List<GameObject> gol = new List<GameObject>();
            foreach (GameObject item in sceneObjects)
            {
                if (item.GetTag == tag)
                {
                    gol.Add(item);
                }
            }
            return gol.ToArray();
        }
        public GameObject GetGameObjectWithId(int id)
        {
            List<GameObject> gol = new List<GameObject>();
            foreach (GameObject item in sceneObjects)
            {
                if (item.id == id)
                {
                    return item;
                }
            }
            return null;
        }
        public void SendNetData()
        {
            foreach (GameObject item in sceneObjects)
            {
                // rotation \\
                //if (item.Transform.Orientation != item.Transform.previousRot)
                //{
                //    NetworkClass.Instance.SendData(NetworkTranslator.NetRotation(item, item.Transform.Orientation));
                //    item.Transform.previousRot = item.Transform.Orientation;
                //}

                //// position \\
                //if (item.Transform.Position != item.Transform.previousPos)
                //{
                //    NetworkClass.Instance.SendData(NetworkTranslator.NetPosition(item, item.Transform.Position));
                //    item.Transform.previousPos = item.Transform.Position;
                //}
            }
        }
    }
}
