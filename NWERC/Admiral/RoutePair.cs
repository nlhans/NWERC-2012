using System.Collections.Generic;

namespace NWERC.Admiral
{
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
}