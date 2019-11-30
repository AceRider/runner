﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Create a pool of objects to control quantity
[System.Serializable]
public class PoolItem
{
    public GameObject prefab;
    public int amount;
    public bool expandable;
}

public class Pool : MonoBehaviour
{
    //use a single Pool for all game and keep using again the objects to improved memory use
    public static Pool singleton;
    public List<PoolItem> items;
    public List<GameObject> pooledItems;

    private void Awake()
    {
        singleton = this;
        pooledItems = new List<GameObject>();
        foreach (PoolItem item in items)
        {
            for (int i = 0; i < item.amount; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                pooledItems.Add(obj);
            }
        }
    }

    //get the next avaliable item in list
    public GameObject GetRandom()
    {
        Utils.Shuffle(pooledItems);
        for(int i=0; i< pooledItems.Count; i++)
        {
            if(!pooledItems[i].activeInHierarchy)
            {
                return pooledItems[i];
            }
        }
        
        foreach(PoolItem item in items)
        {
            if(item.expandable)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                pooledItems.Add(obj);
                return obj;
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public static class Utils
{
    public static System.Random r = new System.Random();
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while(n > 1)
        {
            n--;
            int k = r.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

