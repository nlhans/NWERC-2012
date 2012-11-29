using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;

namespace NWERC
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

    public class Poll
    {
        public int Contenders;

        public List<Pub> Pubs = new List<Pub>();
    }

    public class BeerPressure
    {
        public static Poll ParseLines(string pubs, string votes)
        {
            Poll poll = new Poll();

            string[] pubs_split = pubs.Split(' ');
            string[] votes_split = votes.Split(' ');
            poll.Contenders = int.Parse(pubs_split[1]);

            for (int i = 0; i < int.Parse(pubs_split[0]); i++)
            {
                poll.Pubs.Add(new Pub(int.Parse(votes_split[i]), i + 1, 0.0));
            }
            return poll;
        }

        public static Poll DeterminePollResult(Poll poll)
        {
            var result = VoteStudent(poll);
            var total_permutations =  result.Sum(x => x.Value);
            poll.Pubs.ForEach(x => x.Chance = result[x.Id] / total_permutations);

            return poll;
        }

        public static Dictionary<int, double> VoteStudent(Poll poll)
        {
            Dictionary<int, double> total_result = new Dictionary<int, double>();
            poll.Pubs.ForEach(x => total_result.Add(x.Id, 0.0));

            int students = poll.Pubs.Sum(x => x.Votes);

            if (students == poll.Contenders)
            {
                // All votes are casted
                var maximumVotes = poll.Pubs.Max(x => x.Votes);
                var pubsWithMaximumVotes = poll.Pubs.Count(x => x.Votes == maximumVotes);
                foreach (Pub p in poll.Pubs.Where(x => x.Votes == maximumVotes))
                    total_result[p.Id] += 1.0/pubsWithMaximumVotes;


            }
            else
            {
                // Vote for this student with all permutations
                foreach (Pub pub in poll.Pubs)
                {
                    for (int i = 0; i < pub.Votes; i++)
                    {
                        Poll p = new Poll();
                        p.Contenders = poll.Contenders;
                        p.Pubs = poll.Pubs.Select(x => x.Clone()).Cast<Pub>().ToList();
                        p.Pubs.FirstOrDefault(x => x.Id == pub.Id).Votes++;

                        var result = VoteStudent(p);

                        foreach (var r in result)
                        {
                            total_result[r.Key] += r.Value;
                        }
                    }

                }
            }

            return total_result;
        }

    }
    
    public class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comprarer;

        public LambdaEqualityComparer(Func<T, T, bool> comprarer)
        {
            _comprarer = comprarer;
        }

        public bool Equals(T x, T y)
        {
            return _comprarer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return 0;
        }
    }

    [TestFixture]
    public class BeerPressureTest
    {
        [Test]
        public void ParsePubs()
        {
            Poll poll = ParsePoll();

            Assert.AreEqual(3, poll.Pubs.Count);
            Assert.AreEqual(5, poll.Pubs.Sum(x => x.Votes));
            Assert.AreEqual(7, poll.Contenders);

        }

        private Poll ParsePoll()
        {
            string pubs = "3 7";
            string votes = "3 1 1";

            return BeerPressure.ParseLines(pubs,votes);
        }


        [Test]
        public void RunPoll()
        {
            Poll poll = ParsePoll();

            Poll result = BeerPressure.DeterminePollResult(poll);

            Assert.AreEqual(28.0 / 30, result.Pubs.FirstOrDefault(x => x.Id == 1).Chance);
            Assert.AreEqual(1.0 / 30, result.Pubs.FirstOrDefault(x => x.Id == 2).Chance);
            Assert.AreEqual(1.0 / 30, result.Pubs.FirstOrDefault(x => x.Id == 3).Chance);

        }

    }
}
