using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NWERC.Cycling
{
    [TestFixture]
    public class CyclingSolverTest
    {
        [Test]
        public void TestParseSegmentsFromString()
        {
            var segments = CyclingSolver.ParseSegmentsFromString(new List<string> { "200.0 15.0 15.0", "225.0 31.0 10.0" });

            Assert.AreEqual(425.0f, segments.Sum(x => x.LengthInMeters));
            Assert.AreEqual(46.0f, segments.Sum(x => x.RedLightTime));
            Assert.AreEqual(25.0f, segments.Sum(x => x.GreenLightTime));
        }
    }
}