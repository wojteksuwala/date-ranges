using System;
using DateRangeLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;

namespace DateRaneLibraryTests
{
    [TestClass]
    public class DateRange_GapTests
    {
        [TestMethod]
        public void Gap_BetweenTwoOverlapingPeriodsIsEmpty()
        {
            var p1 = new DateRange(new LocalDate(2000,1,1), new LocalDate(2000,1,5));
            var p2 = new DateRange(new LocalDate(2000, 1, 3), new LocalDate(2000, 1, 7));
            var gap = p1.Gap(p2);
            Assert.IsTrue(gap.IsEmpty());
        }

        [TestMethod]
        public void Gap_BetweenTwoPeriodsNextToEachOtherIsEmpty()
        {
            var p1 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var p2 = new DateRange(new LocalDate(2000, 1, 6), new LocalDate(2000, 1, 10));
            var gap = p1.Gap(p2);
            Assert.IsTrue(gap.IsEmpty());
        }

        [TestMethod]
        public void Gap_BetweenTwoPeriodsWithOneDayBreakIsOneDayPeriod()
        {
            var p1 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var p2 = new DateRange(new LocalDate(2000, 1, 7), new LocalDate(2000, 1, 15));
            var gap = p1.Gap(p2);
            Assert.IsFalse(gap.IsEmpty());
            Assert.AreEqual(new LocalDate(2000, 1, 6), gap.From);
            Assert.AreEqual(new LocalDate(2000, 1, 6), gap.To);
        }

        [TestMethod]
        public void Gap_BetweenTwoPeriodsEqualIsEmpty()
        {
            var p1 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var p2 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var gap = p1.Gap(p2);
            Assert.IsTrue(gap.IsEmpty());
        }

        [TestMethod]
        public void Gap_BetweenContaingPeriodsIsEmpty()
        {
            var p1 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var p2 = new DateRange(new LocalDate(1999, 1, 1), new LocalDate(2012, 1, 5));
            var gap = p1.Gap(p2);
            Assert.IsTrue(gap.IsEmpty());
        }

        [TestMethod]
        public void Gap_InversingPeriodsOrderDoesNotChangeTheGap()
        {
            var p1 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var p2 = new DateRange(new LocalDate(2000, 1, 10), new LocalDate(2000, 1, 15));
            var gap1 = p1.Gap(p2);
            var gap2 = p2.Gap(p1);
            Assert.IsFalse(gap1.IsEmpty());
            Assert.AreEqual(new LocalDate(2000, 1, 6), gap1.From);
            Assert.AreEqual(new LocalDate(2000, 1, 9), gap1.To);
            Assert.IsFalse(gap2.IsEmpty());
            Assert.AreEqual(new LocalDate(2000, 1, 6), gap2.From);
            Assert.AreEqual(new LocalDate(2000, 1, 9), gap2.To);
        }
    }
}
