using System;

namespace BorderlessGaming.Logic.Extensions;

public static class CollectionExtensions
{
    public static void Add<T>(ref T[] array, T item)
    {
        if (array == null)
        {
            array = new T[1] { item };
        }
        else
        {
            Array.Resize(ref array, array.Length + 1);
            array[^1] = item;
        }
    }

    public static void AddRange<T>(ref T[] array, T[] items)
    {
        if (array == null)
        {
            array = items;
        }
        else
        {
            var oldLength = array.Length;
            Array.Resize(ref array, array.Length + items.Length);
            Array.Copy(items, 0, array, oldLength, items.Length);
        }
    }

    public static void Remove<T>(ref T[] array, T item) where T : IEquatable<T>
    {
        ArgumentNullException.ThrowIfNull(array);

        int index = Array.IndexOf(array, item);
        if (index < 0)
            return;  // Item not found, nothing to remove

        for (int i = index + 1; i < array.Length; i++)
        {
            array[i - 1] = array[i];
        }

        Array.Resize(ref array, array.Length - 1);
    }
}