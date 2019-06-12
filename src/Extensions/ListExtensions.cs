using System.Collections.Generic;

namespace System
{
    public static class ListExtensions
    {
        public static void Swap<T>(this List<T> source, T first, T second)
        {
            Swap(source, source.IndexOf(first), source.IndexOf(second));
        }

        public static void Swap<T>(this List<T> source, int indexA, int indexB)
        {
            T tmp = source[indexA];
            source[indexA] = source[indexB];
            source[indexB] = tmp;
        }
    }
}
