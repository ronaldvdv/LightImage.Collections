using LightImage.Collections.Sorting;
using System.Collections.Generic;
using System.Linq;

namespace LightImage.Util.Sorting
{
    public static class NameGenerator
    {
        public static IComparer<string> Comparer = NaturalSorter.Instance;

        public static IEnumerable<string> Generate(string pattern)
        {
            var nwi = NameWithIndex.Parse(pattern);
            if (nwi.HasIndex)
                return GenerateDynamic(nwi.Name, nwi.Index, nwi.IndexSize);
            else
                return GenerateStatic(pattern);
        }

        public static IEnumerable<string> Generate(string pattern, int count)
        {
            return Generate(pattern).Take(count);
        }

        private static IEnumerable<string> GenerateDynamic(string baseName, int startIndex, int indexWidth)
        {
            var index = startIndex;
            while (true)
            {
                yield return baseName + index.ToString().PadLeft(indexWidth, '0');
                index++;
            }
        }

        private static IEnumerable<string> GenerateStatic(string name)
        {
            while (true)
                yield return name;
        }
    }
}