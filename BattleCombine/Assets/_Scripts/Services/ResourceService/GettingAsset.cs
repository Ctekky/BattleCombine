using System;
using UnityEngine;

[Serializable]
public class GettingAsset<T> 
{
    [SerializeField]  private T[] array;


    public T GetBy(int index)
    {
        return array[index];
    }

    public T[] GetAll()
    {
        return array;
    }

}
