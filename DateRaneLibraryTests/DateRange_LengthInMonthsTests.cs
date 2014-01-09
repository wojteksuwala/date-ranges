using System;
using DateRangeLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;

namespace DateRaneLibraryTests
{
    [TestClass]
    public class DateRange_LengthInMonthsTests
    {
        [TestMethod]
        public void LengthInMonths_EmptyPeriodIsZero()
        {
            var p = DateRange.Empty;
            var len = p.LengthInMonths();
            Assert.AreEqual(0, len);
        }

        [TestMethod]
        public void LengthInMonths_OneDayPeriodIsOneDividedByNumberOfDaysInMonth()
        {
            var p = new DateRange(
                new LocalDate(2001, 1, 1), new LocalDate(2001, 1, 1)
                );
            var len = p.LengthInMonths();
            Assert.AreEqual(decimal.Divide(1,31).Round(), len);
        }

        [TestMethod]
        public void LengthInMonths_PeriodInsideOneMonthGivesProperResults()
        {
            var p = new DateRange(
                new LocalDate(2001, 3, 3), new LocalDate(2001, 3, 17)
                );
            var len = p.LengthInMonths();
            Assert.AreEqual(decimal.Divide(15,31).Round(), len);
        }

        [TestMethod]
        public void LengthInMonths_PeriodThatCoversWholeMonthHasLengthOne()
        {
            var p = new DateRange(
                new LocalDate(2001, 1, 1), new LocalDate(2001, 1, 31)
                );
            var len = p.LengthInMonths();
            Assert.AreEqual(1, len);
        }

        [TestMethod]
        public void LengthInMonths_PeriodThatCoversTwoMonthsHasLengthTwo()
        {
            var p = new DateRange(
                new LocalDate(2001, 3, 1), new LocalDate(2001, 4, 30)
                );
            var len = p.LengthInMonths();
            Assert.AreEqual(2, len);
        }

        [TestMethod]
        public void LengthInMonths_PeriodThatCoversThreeMonthsHasLengthThree()
        {
            var p = new DateRange(
                new LocalDate(2001, 3, 1), new LocalDate(2001, 5, 31)
                );
            var len = p.LengthInMonths();
            Assert.AreEqual(3, len);
        }

        [TestMethod]
        public void LengthInMonths_PeriodThatCoversFourMonthsHasLengthFour()
        {
            var p = new DateRange(
                new LocalDate(2001, 3, 1), new LocalDate(2001, 6, 30)
                );
            var len = p.LengthInMonths();
            Assert.AreEqual(4, len);
        }

        [TestMethod]
        public void LengthInMonths_PeriodSpanningTwoMonthsGivesProperNumber()
        {
            var p = new DateRange(
                new LocalDate(2001, 3, 15), new LocalDate(2001, 4, 15)
                );
            var len = p.LengthInMonths();
            Assert.AreEqual( decimal.Divide(17,31).Round() + decimal.Divide(15,30).Round(), len);
        }

        [TestMethod]
        public void LengthInMonths_PeriodSpanningTwoMonthAndAHalfGivesOneAndAHalf()
        {
        }

        [TestMethod]
        public void LengthInMonths_PeriodSpanningThreeMonthGivesProperNumber()
        {
        }

        [TestMethod]
        public void LengthInMonths_PeriodSpanning2YearsGives24()
        {
        }
    }
}
