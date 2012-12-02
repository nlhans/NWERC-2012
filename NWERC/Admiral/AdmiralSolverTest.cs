using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NWERC.Admiral
{
    [TestFixture]
    public class AdmiralSolverTest
    {
        [Test]
        public void CreateMap()
        {
            IEnumerable<Waypoint> map = BuildMap();
            Assert.AreEqual(6, map.Count());

            var firstWayPoint = map.FirstOrDefault(x => x.ID == 1);

            Assert.NotNull(firstWayPoint);
            Assert.AreEqual(3, firstWayPoint.Routes.Count);

        }

        private IEnumerable<Waypoint> BuildMap()
        {
            var entries = new List<string>();

            entries.Add("1 2 23");
            entries.Add("1 3 12");
            entries.Add("1 4 99");
            entries.Add("2 5 17");
            entries.Add("2 6 73");
            entries.Add("3 5 3");
            entries.Add("3 6 21");
            entries.Add("4 6 8");
            entries.Add("5 2 33");
            entries.Add("5 4 5");
            entries.Add("6 5 20");

            return AdmiralSolver.CreateMapFromString(entries);
        }


        [Test]
        public void TestVisit()
        {
            var map = BuildMap();
            var solver = new AdmiralSolver(map);
            var route = solver.FindRoute(6);

            Assert.AreEqual(86, route.Blue.Sum(x => x.Passage.Cost) + route.Red.Sum(x => x.Passage.Cost));
        }
    }
}