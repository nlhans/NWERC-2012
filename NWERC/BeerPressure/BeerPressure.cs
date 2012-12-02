using System.Collections.Generic;
using System.Linq;

namespace NWERC.BeerPressure
{
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
}
