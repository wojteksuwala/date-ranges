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
            var initial = new DateRange(new LocalDate(2000,1,1), new LocalDate(2000,1,15));

            var otherPeriods = new List<DateRange>() { 
                new DateRange(new LocalDate(2000,1,3), new LocalDate(2000,1,7)),
                new DateRange(new LocalDate(2000,1,10), new LocalDate(2000,1,11))
            };

            var result = initial.Intersect(otherPeriods);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(otherPeriods[0], result[0]);
            Assert.AreEqual(otherPeriods[1], result[1]);
        }

        [TestMethod]
        public void Intersect_withTwoPeriodsOutsideReturnOneEmptyPeriod()
        {
            var initial = new DateRange(new LocalDate(2000,1,1), new LocalDate(2000,1,15));

            var otherPeriods = new List<DateRange>() { 
                new DateRange(new LocalDate(1999,3,1), new LocalDate(1999,7,1)),
                new DateRange(new LocalDate(2001,1,20), new LocalDate(2001,11,1))
            };

            var result = initial.Intersect(otherPeriods);

            Assert.AreEqual(0, result.Count);
            //Assert.IsTrue(result[0].IsEmpty());
        }
    }
}
