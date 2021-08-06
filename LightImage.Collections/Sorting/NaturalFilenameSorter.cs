using System.Collections.Generic;
using System.IO;

namespace LightImage.Collections.Sorting
{
    public class NaturalFilenameSorter : IComparer<string>
    {
        public static NaturalFilenameSorter Instance = new NaturalFilenameSorter();

        private static readonly IComparer<string> _default = Comparer<string>.Default;
        private readonly NaturalSorter _nestedSorter = new NaturalSorter();

        public NaturalFilenameSorter()
        {
        }

        public int Compare(string x, string y)
        {
            if (string.IsNullOrWhiteSpace(x) || string.IsNullOrWhiteSpace(y))
                return _default.Compare(x, y);

            var nameX = Path.GetFileNameWithoutExtension(x);
            var nameY = Path.GetFileNameWithoutExtension(y);
            return _nestedSorter.Compare(nameX, nameY);
        }
    }
}