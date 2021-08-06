using System.Text.RegularExpressions;

namespace LightImage.Collections.Sorting
{
    public struct NameWithIndex
    {
        public NameWithIndex(string full, string name, int index, int indexSize = 0)
        {
            Full = full;
            Name = name;
            Index = index;
            IndexSize = indexSize;
            HasIndex = indexSize > 0;
        }

        public string Full { get; }

        public bool HasIndex { get; }

        public int Index { get; }
        public int IndexSize { get; }
        public string Name { get; }

        public static NameWithIndex Parse(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                return new NameWithIndex(pattern, pattern, 0, 0);
            var match = Regex.Match(pattern, @"^(?<base>.*?)(?<start>[0-9]+)\s*$");
            if (match.Success)
            {
                var index = match.Groups["start"].Value;
                return new NameWithIndex(pattern, match.Groups["base"].Value, ParseIndex(index), index.Length);
            }
            else
                return new NameWithIndex(pattern, pattern, 0, 0);
        }

        private static int ParseIndex(string value)
        {
            value = value?.TrimStart('0');
            if (string.IsNullOrWhiteSpace(value))
                return 0;
            return int.Parse(value);
        }
    }
}