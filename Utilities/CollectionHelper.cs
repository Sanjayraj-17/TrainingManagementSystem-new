
using System;
using System.Collections.Generic;

namespace TrainingManagementSystem.Utilities
{
    public static class CollectionHelper
    {
        // 1. Generic method to display collection items
        public static void DisplayItems<T>(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                Console.WriteLine(item);
            }
        }

        // 2. Generic search method using a comparison function
        public static T? FindItem<T>(
            IEnumerable<T> items,
            string searchText,
            Func<T, string> getSearchValue)
        {
            foreach (T item in items)
            {
                string value = getSearchValue(item);

                if (string.Equals(
                    value,
                    searchText,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }

            return default;
        }
    }
}

