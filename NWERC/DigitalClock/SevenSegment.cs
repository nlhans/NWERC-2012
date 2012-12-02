using System.Collections.Generic;
using System.Linq;

namespace NWERC.DigitalClock
{
    public class SevenSegment
    {
        private static readonly List<IEnumerable<SevenSegment>> digitToDeratives = new List<IEnumerable<SevenSegment>>
            {
                CreateDeratives(new[] {0, 8}),
                CreateDeratives(new[] {1, 3, 4, 7, 8, 9}),
                CreateDeratives(new[] {2, 8}),
                CreateDeratives(new[] {3, 8, 9}),
                CreateDeratives(new[] {4, 8, 9}),
                CreateDeratives(new[] {5, 6, 8}),
                CreateDeratives(new[] {6, 8}),
                CreateDeratives(new[] {7, 0, 3, 8, 9}),
                CreateDeratives(new[] { 8 }),
                CreateDeratives(new[] {9, 8})
            };

        private static IEnumerable<SevenSegment> CreateDeratives(IEnumerable<int> deratives)
        {
            return deratives.Select(digit => new SevenSegment(digit));
        }


        public int Digit;

        public SevenSegment(int digit)
        {
            Digit = digit;
        }

        public IEnumerable<SevenSegment> FindDeratives()
        {
            return digitToDeratives[Digit];
        }
    }
}