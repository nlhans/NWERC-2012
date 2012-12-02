namespace NWERC.Admiral
{
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
}