using System;

namespace NWERC.BeerPressure
{
    public class Pub : ICloneable
    {
        public int Votes;
        public int Id;
        public double Chance;

        public Pub(int votes, int id, double chance)
        {
            Votes = votes;
            Id = id;
            Chance = chance;
        }

        public object Clone()
        {
            return new Pub(Votes, Id, Chance);

        }
    }
}