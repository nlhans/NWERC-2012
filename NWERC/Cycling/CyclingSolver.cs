using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NWERC.Cycling
{
    public class CyclingSolver
    {
        public static IEnumerable<Segment> ParseSegmentsFromString(IEnumerable<string> lines)
        {
            return lines
                .Select(line => line.Split(' '))
                .Select(sl => new Segment(float.Parse(sl[0], new CultureInfo("en-US")), float.Parse(sl[1], new CultureInfo("en-US")), float.Parse(sl[2], new CultureInfo("en-US"))));
        }

    }
}
