﻿using System.Collections.Generic;

namespace Cesxhin.AnimeManga.Modules.Generic
{
    public static class ConvertGeneric<T> where T : class
    {
        public static List<T> ConvertIEnurableToListCollection(IEnumerable<T> list)
        {
            List<T> result = new();

            foreach (T item in list)
            {
                result.Add(item);
            }
            return result;
        }
    }
}
