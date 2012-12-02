namespace NWERC.Admiral
{
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
}