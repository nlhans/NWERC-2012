using System.Linq;
using NUnit.Framework;

namespace NWERC.BeerPressure
{
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