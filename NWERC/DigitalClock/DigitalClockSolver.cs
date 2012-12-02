using System;
using System.Collections.Generic;

namespace NWERC.DigitalClock
{
    public class DigitalClockSolver
    {
        public static IEnumerable<Sample> ParseObservations(string input)
        {
            List<Sample> obvs = new List<Sample>();

            string[] data = input.Split(' ');
            int samples = int.Parse(data[0]);

            for (int i = 1; i < data.Length; i++)
            {
                Sample ob = new Sample();

                foreach (var digit in data[i])
                {
                    if(digit!=':'){
                    ob.Segments.Add(new SevenSegment(int.Parse(digit.ToString())));}
                }

                obvs.Add(ob);
            }

            return obvs;
        }

        public static IEnumerable<Sample> GeneratePermutations(Sample ob)
        {
            return ob.CreateDeratives();
        }
    }
}