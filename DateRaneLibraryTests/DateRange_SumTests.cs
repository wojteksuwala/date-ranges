using System;
using DateRangeLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;

namespace DateRaneLibraryTests
{
    [TestClass]
    public class DateRange_SumTests
    {
        [TestMethod]
        public void Sum_OfEmptyPeriodsIsEmpty()
        {
            var p1 = DateRange.Empty;
            var p2 = DateRange.Empty;
            var sum = p1.Sum(p2);
            Assert.AreEqual(1, sum.Count);
            Assert.IsTrue(sum[0].IsEmpty());
        }

        [TestMethod]
        public void Sum_OfEmptyAndGivenIsEqualToGiven()
        {
            var p1 = DateRange.Empty;
            var p2 = new DateRange(new LocalDate(2000, 1, 2), new LocalDate(2000, 1, 4));
            var sum = p1.Sum(p2);
            Assert.AreEqual(1, sum.Count);
            Assert.AreEqual(sum[0],p2);
        }

        [TestMethod]
        public void Sum_OfPeriodAndPeriodItContainsEqualsToPeriod()
        {
            var p1 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var p2 = new DateRange(new LocalDate(2000, 1, 2), new LocalDate(2000, 1, 4));
            var sum = p1.Sum(p2);
            Assert.AreEqual(1,sum.Count);
            Assert.AreEqual(p1, sum[0]);
        }

        [TestMethod]
        public void Sum_OfPeriodAndPeriodThatContainsItEqualsToContainer()
        {
            var p1 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var p2 = new DateRange(new LocalDate(1999, 1, 2), new LocalDate(2011, 1, 4));
            var sum = p1.Sum(p2);
            Assert.AreEqual(1, sum.Count);
            Assert.AreEqual(p2, sum[0]);
        }

        [TestMethod]
        public void Sum_OfOverappingPeriodsLeftSideIsOneBigPeriod()
        {
            var p1 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var p2 = new DateRange(new LocalDate(1999, 1, 2), new LocalDate(2000, 1, 4));
            var sum = p1.Sum(p2);
            Assert.AreEqual(1, sum.Count);
            Assert.AreEqual(new DateRange(new LocalDate(1999,1,2), new LocalDate(2000,1,5) ), sum[0]);
        }

        [TestMethod]
        public void Sum_OfOverappingPeriodsLeftRightIsOneBigPeriod()
        {
            var p1 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var p2 = new DateRange(new LocalDate(2000, 1, 4), new LocalDate(2000, 1, 8));
            var sum = p1.Sum(p2);
            Assert.AreEqual(1, sum.Count);
            Assert.AreEqual(new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 8)), sum[0]);
        }

        [TestMethod]
        public void Sum_OfPeriodsNextToEachOtherIsOneBigPeriod()
        {
            var p1 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var p2 = new DateRange(new LocalDate(2000, 1, 6), new LocalDate(2000, 1, 14));
            var sum = p1.Sum(p2);
            Assert.AreEqual(1, sum.Count);
            Assert.AreEqual(new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 14)), sum[0]);
        }

        [TestMethod]
        public void Sum_OfNotOverlappingPeriodsThatAreNotNextToEachOtherIsTwoPeriods()
        {
            var p1 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var p2 = new DateRange(new LocalDate(1999, 1, 7), new LocalDate(1999, 1, 14));
            var sum = p1.Sum(p2);
            Assert.AreEqual(2, sum.Count);
            Assert.AreEqual(p1, sum[0]);
            Assert.AreEqual(p2, sum[1]);
        }
    }
}
