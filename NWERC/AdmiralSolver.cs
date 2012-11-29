using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NWERC.Admiral
{
    public class Waypoint
    {
        public int ID;
        public List<Passage> Routes;

        public Waypoint(int id)
        {
            ID = id;
            Routes = new List<Passage>();
        }
    }
    
    public class RoutePair
    {
        public List<Route> Red;
        public List<Route> Blue;

        public RoutePair(List<Route> red, List<Route> blue)
        {
            Red = red;
            Blue = blue;
        }
    }

    public class Route
    {
        public Waypoint Start;
        public Passage Passage;

        public Route(Waypoint start, Passage passage)
        {
            Start = start;
            Passage = passage;
        }
    }

    public class Passage
    {
        public Waypoint Target;
        public int Cost;

        public Passage(Waypoint target, int cost)
        {
            Target = target;
            Cost = cost;
        }
    }

    public class AdmiralSolver
    {
        private readonly IEnumerable<Waypoint> _map;
        private List<Waypoint> _closedNodes = new List<Waypoint>();

        public AdmiralSolver(IEnumerable<Waypoint> map)
        {
            _map = map;
        }

        private static Waypoint GetOrCreate(int id, SortedList<int, Waypoint> waypoints)
        {
            Waypoint waypoint;

            if (!waypoints.ContainsKey(id))
            {
                waypoint = new Waypoint(id);
                waypoints.Add(id, waypoint);
            }
            else
            {
                waypoint = waypoints[id];
            }

            return waypoint;
        }

        public static IEnumerable<Waypoint> CreateMapFromString(IEnumerable<string> entries)
        {
            var waypoints = new SortedList<int, Waypoint>();

            foreach (string entryPassage in entries)
            {
                var data = entryPassage.Split(' ');
                var startPosition = int.Parse(data[0]);
                var endPosition = int.Parse(data[1]);
                var cost = int.Parse(data[2]);

                var startWaypoint = GetOrCreate(startPosition, waypoints);
                var destWaypoint = GetOrCreate(endPosition, waypoints);
                
                startWaypoint.Routes.Add(new Passage(destWaypoint, cost));
            }

            return waypoints.Select(x => x.Value);
        }



        public RoutePair FindRoute(int targetId)
        {
            var startWaypoint = _map.First();
            var route = new List<Route>();

            var  solutions = Visit(targetId, startWaypoint, route);
            //IOrderedEnumerable<List<Route>> best_routes =solutions.OrderBy(x => x.Sum(c => c.Passage.Cost));

            var routepairs = new List<RoutePair>();

            foreach(var route1 in solutions)
            {
                foreach(var route2 in solutions)
                {
                    if (route2 == route1) continue;

                    var waypoints1 = route1
                        .Where(x => x.Passage.Target.ID != targetId)
                        .Select(r => r.Passage.Target.ID);

                    var waypoints2 = route2
                        .Where(x => x.Passage.Target.ID != targetId)
                        .Select(r => r.Passage.Target.ID);

                    if(waypoints1.Any(waypoints2.Contains))
                    {
                        continue;
                    }

                    if(!routepairs.Any(x => x.Red == route2 && x.Blue == route1))
                    {
                        routepairs.Add(new RoutePair(route1, route2));    
                    }

                    
                }
            }

            return routepairs
                .OrderBy(x => x.Red.Sum(c =>c.Passage.Cost) + x.Blue.Sum(c => c.Passage.Cost))
                .FirstOrDefault();
        }

        public List<List<Route>> Visit(int targetID, Waypoint waypoint, List<Route> route)
        {
            List<List<Route>> routes = new List<List<Route>>();
            
            foreach(Passage passage in waypoint.Routes)
            {
                if (route.Any(x => x.Start == passage.Target))
                {
                    continue;
                }

                List<Route> followedRoute = new List<Route>(route);
                followedRoute.Add(new Route(waypoint, passage));
                if(passage.Target.ID == targetID)
                {
                    routes.Add(followedRoute);
                }
                else
                {
                    routes.AddRange(Visit(targetID, passage.Target, followedRoute));
                }
            }

            return routes;
        }

    }

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
