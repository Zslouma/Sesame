using System;
using System.Collections.Generic;
using System.Linq;

namespace Syracuse.Mobitheque.Core.ViewModels.Sorts
{
    public class AscendingSort : ISortAlgorithm
    {
        public SortAlgorithm Id => SortAlgorithm.ASCENDING;

        /*
         * Default constructor.
         */
        public AscendingSort() { }

        public IEnumerable<T> Sort<T>(IEnumerable<T> collection, string lookFor)
        {
            if (collection == null || string.IsNullOrEmpty(lookFor))
                throw new Exception(nameof(Sort));
            return collection.OrderBy(x => x.GetType().GetProperty(lookFor));
        }
    }
}
