using System.Collections.Generic;
using System.Linq;

namespace NWERC.Admiral
{
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
}
