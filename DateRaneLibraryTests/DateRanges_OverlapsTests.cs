using System;
using DateRangeLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;

namespace DateRaneLibraryTests
{
    [TestClass]
    public class DateRanges_OverlapsTests
    {
        DateRange subjectOfTest;

        [TestInitialize]
        public void Setup()
        {
            subjectOfTest = new DateRange(
                new LocalDate(2000, 1, 1),
                new LocalDate(2000, 1, 15)
                );
        }

        [TestMethod]
        public void Overlaps_OverlapsPeriodInside()
        {
            var periodInside = new DateRange(new LocalDate(2000,1,3), new LocalDate(2000,1,5));
            Assert.IsTrue(subjectOfTest.Overlaps(periodInside));
        }

        [TestMethod]
        public void Overlaps_OverlapsPeriodOnLeft()
        {
            var period = new DateRange(new LocalDate(2000, 1, 3), new LocalDate(2000, 1, 21));
            Assert.IsTrue(subjectOfTest.Overlaps(period));
        }


        [TestMethod]
        public void Overlaps_OverlapsPeriodOnRight()
        {
            var period = new DateRange(new LocalDate(1999, 12, 30), new LocalDate(2000, 1, 3));
            Assert.IsTrue(subjectOfTest.Overlaps(period));
        }

        [TestMethod]
        public void Overlaps_OverlapsPeriodContainingThis()
        {
            var period = new DateRange(new LocalDate(1999, 1, 3), new LocalDate(2012, 1, 21));
            Assert.IsTrue(subjectOfTest.Overlaps(period));
        }

        [TestMethod]
        public void Overlaps_DoesntOverlapsPeriodBefore()
        {
            var period = new DateRange(new LocalDate(1999, 1, 3), new LocalDate(1999, 1, 21));
            Assert.IsFalse(subjectOfTest.Overlaps(period));
        }


        [TestMethod]
        public void Overlaps_DoesntOverlapsPeriodAfter()
        {
            var period = new DateRange(new LocalDate(2001, 1, 3), new LocalDate(2001, 1, 21));
            Assert.IsFalse(subjectOfTest.Overlaps(period));
        }
    }
}
