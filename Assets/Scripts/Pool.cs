using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    //Create a pool of objects to control quantity
    #region Pool Item class
    [System.Serializable]
    public class PoolItem
    {
        #region Public properties
        [Header("Platforms")]
        public GameObject prefab;
        [Tooltip("The amount to spawn before reset all")]
        public int amount;
        [Tooltip("Never run out of this item in the pool")]
        public bool necessary;
        #endregion
    }
    #endregion
    #region Utils class
    public static class Utils
    {
        public static System.Random r = new System.Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = r.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
    #endregion
    public class Pool : MonoBehaviour
    {
        #region Public properties
        //use a single Pool for all game and keep using again the objects to improved memory use
        public static Pool singleton;
        public List<PoolItem> itemList;
        public List<GameObject> pooledItemList;
        #endregion

        private void Awake()
        {
            singleton = this;
            pooledItemList = new List<GameObject>();
            //Store the platforms
            foreach (PoolItem item in itemList)
            {
                for (int i = 0; i < item.amount; i++)
                {
                    GameObject obj = Instantiate(item.prefab);
                    obj.SetActive(false);
                    pooledItemList.Add(obj);
                }
            }
        }

        //get the next avaliable item in list
        public GameObject GetRandom()
        {
            Utils.Shuffle(pooledItemList);
            for (int i = 0; i < pooledItemList.Count; i++)
            {
                if (!pooledItemList[i].activeInHierarchy)
                {
                    return pooledItemList[i];
                }
            }

            foreach (PoolItem item in itemList)
            {
                if (item.necessary)
                {
                    GameObject obj = Instantiate(item.prefab);
                    obj.SetActive(false);
                    pooledItemList.Add(obj);
                    return obj;
                }
            }
            return null;
        }
    }
}

