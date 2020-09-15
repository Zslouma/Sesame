using System;
using System.Collections.Generic;
using System.Linq;

namespace Syracuse.Mobitheque.Core.ViewModels.Sorts
{
    public class DescendingSort : ISortAlgorithm
    {
        public SortAlgorithm Id => SortAlgorithm.DESCENDING;

        public DescendingSort() { }

        public IEnumerable<T> Sort<T>(IEnumerable<T> collection, string lookFor)
        {
            if (collection == null || string.IsNullOrEmpty(lookFor))
                throw new Exception(nameof(Sort));
            return collection.OrderByDescending(x => x.GetType().GetProperty(lookFor));
        }
    }
}