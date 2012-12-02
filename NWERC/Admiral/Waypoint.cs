using System.Collections.Generic;

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
}