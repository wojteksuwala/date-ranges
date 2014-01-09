using System;
using DateRangeLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;

namespace DateRaneLibraryTests
{
    [TestClass]
    public class DateRange_DaysToBeginningEndOfMonthTests
    {
        [TestMethod]
        public void FromBeginningOfMonth_Returns1WhenPeriodStarts1st()
        {
            var day = new LocalDate(2000, 1, 1);
            var daysFromBeginMonth = day.DaysFromBeginningOfMonth();
            Assert.AreEqual(1, daysFromBeginMonth);
        }

        [TestMethod]
        public void FromBeginningOfMonth_ReturnsNWhenPeriodStartsNth()
        {
            var day = new LocalDate(2000, 1, 16);
            var daysFromBeginMonth = day.DaysFromBeginningOfMonth();
            Assert.AreEqual(16, daysFromBeginMonth);
        }

        [TestMethod]
        public void ToEndOfMonth_ReturnsOOnLastDayOfMonth()
        {
            var day = new LocalDate(2000, 1, 31);
            var daysFromBeginMonth = day.DaysToEndOfMonth();
            Assert.AreEqual(0, daysFromBeginMonth);
        }

        //[TestMethod]
        //public void ToEndOfMonth_ReturnsNWhenPeriodStartsNth()
        //{
        //    var day = new LocalDate(2000, 1, 16);
        //    var daysFromBeginMonth = p.DaysFromBeginningOfMonth();
        //    Assert.AreEqual(16, daysFromBeginMonth);
        //}
    }
}
