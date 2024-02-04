using System;
using System.Collections.Generic;
using UnityEngine;

public static class List
{
    public static List<T> Clone<T>(this List<T> _list)
    {
        List<T> result = new List<T>();
        result.AddRange(_list);
        return result;
    }

    public static List<T> Shuffle<T>(this List<T> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            T temp = _list[i];
            int randomIndex = UnityEngine.Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }

        return _list;
    }

    public static T GetPrevious<T>(this List<T> _list, T item)
    {
        int index = _list.IndexOf(item);
        if (index <= -1) return item;
        index--;
        if (index < 0) return _list[_list.Count - 1];
        return _list[index];
    }

    public static T GetNext<T>(this List<T> _list, T item)
    {
        int index = _list.IndexOf(item);
        if (index <= -1) return item;
        index++;
        if (index >= _list.Count) return _list[0];
        return _list[index];
    }

    public static List<string> GetStringByLenght(this List<string> _list, int lenght)
    {
        List<string> ts = new List<string>();
        foreach (string row in _list)
        {
            if (row.Length != lenght) continue;
            ts.Add(row);
        }
        return ts;
    }

    public static List<string> GetStringByLenght(this List<string> _list, int min, int max)
    {
        List<string> ts = new List<string>();
        foreach (string row in _list)
        {
            if (row.Length < min || row.Length > max) continue;
            ts.Add(row);
        }
        return ts;
    }
}
