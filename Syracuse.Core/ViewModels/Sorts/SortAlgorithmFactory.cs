using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Syracuse.Mobitheque.Core.ViewModels.Sorts
{
    public enum SortAlgorithm
    {
        ASCENDING = 0,
        DESCENDING = 1,
    }

    public static class SortAlgorithmFactory
    {
        private static readonly List<ISortAlgorithm> algorithms = new List<ISortAlgorithm>();

        public static void RegisterAlgorithms()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var reflectedAlgorithms = assembly.GetTypes()?.ToList()?.Where(x => typeof(ISortAlgorithm).IsAssignableFrom(x) && !x.IsInterface);

            foreach (var algo in reflectedAlgorithms)
                SortAlgorithmFactory.algorithms.Add(Activator.CreateInstance(algo) as ISortAlgorithm);
        }

        public static ISortAlgorithm GetAlgorithm(SortAlgorithm id) => SortAlgorithmFactory.algorithms.FindLast(x => x.Id == id);
    }
}
