using System;
using DateRangeLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;

namespace DateRaneLibraryTests
{
    [TestClass]
    public class DateRange_IsEmptyTests
    {
        [TestMethod]
        public void IsEmpty_EmptyConstIsEmpty()
        {
            Assert.IsTrue(DateRange.Empty.IsEmpty());
        }

        [TestMethod]
        public void IsEmpty_DetectEmptyPeriod()
        {
            var empty = new DateRange(new LocalDate(1999, 1, 1), new LocalDate(1998, 1, 1));
            Assert.IsTrue(empty.IsEmpty());
        }

        [TestMethod]
        public void IsEmpty_DetectNonEmptyPeriod()
        {
            var nonempty = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 2, 1));
            Assert.IsFalse(nonempty.IsEmpty());
        }

        [TestMethod]
        public void IsEmpty_OneDayPeriodIsNotEmpty()
        {
            var oneDay = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 1));
            Assert.IsFalse(oneDay.IsEmpty());
        }
    }
}
