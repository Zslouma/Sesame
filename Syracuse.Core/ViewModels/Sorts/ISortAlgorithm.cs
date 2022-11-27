using System.Collections.Generic;

namespace Syracuse.Mobitheque.Core.ViewModels.Sorts
{
    /*
     * Note : Every class that implements this interface needs to have a default constructor to be registered in the SortAlgorithmFactory.
     */
    public interface ISortAlgorithm
    {
        SortAlgorithm Id { get; }
        IEnumerable<T> Sort<T>(IEnumerable<T> collection, string lookFor);
    }
}
