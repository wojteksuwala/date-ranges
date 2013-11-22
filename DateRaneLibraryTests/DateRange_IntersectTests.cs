using System;
using System.Collections.Generic;
using DateRangeLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;

namespace DateRaneLibraryTests
{
    [TestClass]
    public class DateRange_IntersectTests
    {
        [TestMethod]
        public void Intersect_withTwoPeriodsInsideReturnsTheseTwoPeriods()
        {
            var initial = new DateRange(new LocalDate(1, 1, 2000), new LocalDate(15, 1, 2000));

            var otherPeriods = new List<DateRange>() { 
                new DateRange(new LocalDate(1, 3, 2000), new LocalDate(7, 1, 2000)),
                new DateRange(new LocalDate(1, 10, 2000), new LocalDate(11, 1, 2000))
            };

            var result = initial.Intersect(otherPeriods);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(otherPeriods[0], result[0]);
            Assert.AreEqual(otherPeriods[1], result[1]);
        }

        [TestMethod]
        public void Intersect_withTwoPeriodsOutsideReturnOneEmptyPeriod()
        {
            var initial = new DateRange(new LocalDate(1, 1, 2000), new LocalDate(15, 1, 2000));

            var otherPeriods = new List<DateRange>() { 
                new DateRange(new LocalDate(1, 3, 1999), new LocalDate(7, 1, 1999)),
                new DateRange(new LocalDate(1, 10, 2001), new LocalDate(11, 1, 2001))
            };

            var result = initial.Intersect(otherPeriods);

            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result[0].IsEmpty());
        }
    }
}
