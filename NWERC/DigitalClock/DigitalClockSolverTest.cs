using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NWERC.DigitalClock
{

    [TestFixture]
    public class DigitalClockSolverTest
    {
        [Test]
        public void ParseInput()
        {
            IEnumerable<Sample> obvs = GetObvs();

            var ob = obvs.FirstOrDefault();

            Assert.AreEqual(3, obvs.Count());
            Assert.AreEqual(4, ob.Segments.Count);
            Assert.AreEqual(7, ob.Segments.FirstOrDefault().Digit);
        }

        [Test]
        public void TestPermutations()
        {
            IEnumerable<Sample> obvs = GetObvs();

            var ob = obvs.FirstOrDefault();

            var possiblities = DigitalClockSolver.GeneratePermutations(ob);

            Assert.AreEqual(10, possiblities.Count());
        }

        private IEnumerable<Sample> GetObvs()
        {
            string input = "3 71:57 71:57 71:07";
            return DigitalClockSolver.ParseObservations(input);
        }
    }
}