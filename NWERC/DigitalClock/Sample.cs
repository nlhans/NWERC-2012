using System.Collections.Generic;
using System.Linq;

namespace NWERC.DigitalClock
{
    public class Sample
    {
        public List<SevenSegment> Segments; // 4 segments

        public int Hours { get { return Segments[0].Digit * 10 + Segments[1].Digit; } }
        public int Minutes { get { return Segments[2].Digit * 10 + Segments[3].Digit; } }

        public Sample()
        {
            Segments = new List<SevenSegment>();
        }

        public Sample(List<SevenSegment> segments)
        {
            Segments = segments;
        }

        public List<Sample> CreateDeratives()
        {
            var hours10_deratives = Segments[0].FindDeratives();
            var hours1_deratives = Segments[1].FindDeratives();
            var mins10_deratives = Segments[2].FindDeratives();
            var mins1_deratives = Segments[3].FindDeratives();

            return (from hour10 in hours10_deratives
                    from hour1 in hours1_deratives
                    from mins10 in mins10_deratives
                    from mins1 in mins1_deratives
                    let hour = hour10.Digit*10 + hour1.Digit
                    let min = mins10.Digit*10 + mins1.Digit
                    where hour <= 23 && min <= 59
                    select new List<SevenSegment> {hour10, hour1, mins10, mins1}
                    into derativeSegments select new Sample(derativeSegments)).ToList();
            
            List<Sample> deratives = new List<Sample>();
            foreach(SevenSegment hour10 in hours10_deratives)
            {
                foreach(SevenSegment hour1 in hours1_deratives)
                {
                    foreach(SevenSegment mins10 in mins10_deratives)
                    {
                        foreach(SevenSegment mins1 in mins1_deratives)
                        {
                            int hour = hour10.Digit*10 + hour1.Digit;
                            int min = mins10.Digit*10 + mins1.Digit;

                            if (hour <= 23 && min <= 59)
                            {
                                var derativeSegments = new List<SevenSegment> {hour10, hour1, mins10, mins1};

                                deratives.Add(new Sample(derativeSegments) );
                            }

                        }
                    }
                }
            }
            return deratives;
        }
    }
}