using System.Collections.Generic;
using UnityEngine;

public static class ListExtension
{
    public static void Shuffle<T>(this List<T> list)
    {
        int n = list.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + Random.Range(0, n - i);
            T temp = list[r];
            list[r] = list[i];
            list[i] = temp;
        }
    }

    public static List<T> GetShuffled<T>(this List<T> list)
    {
        List<T> shuffledList = new List<T>(list);
        int n = shuffledList.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + Random.Range(0, n - i);
            T temp = shuffledList[r];
            shuffledList[r] = shuffledList[i];
            shuffledList[i] = temp;
        }
        return shuffledList;
    }
}