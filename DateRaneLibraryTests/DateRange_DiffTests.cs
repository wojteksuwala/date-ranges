using System;
using DateRangeLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;

namespace DateRaneLibraryTests
{
    [TestClass]
    public class DateRange_DiffTests
    {
        [TestMethod]
        public void Diff_WithPeriodBeforeReturnsEmptyPeriod()
        {
            var p1 = DateRange.Between(
                new LocalDate(2000,1,1), new LocalDate(2000,1,14)
                );

            var p2 = DateRange.Between(
                new LocalDate(1999, 1, 1), new LocalDate(1999, 1, 14)
                );

            var diffResult = DateRange.Diff(p1, p2);

            Assert.AreEqual(1, diffResult.OnlyInFirstPeriod.Count);
            Assert.AreEqual(p1, diffResult.OnlyInFirstPeriod[0]);

            Assert.AreEqual(1, diffResult.OnlyInSecondPeriod.Count);
            Assert.AreEqual(p2, diffResult.OnlyInSecondPeriod[0]);

            Assert.AreEqual(0, diffResult.CommonParts.Count);
        }

        [TestMethod]
        public void Diff_WithPeriodAfterReturnsEmptyPeriod()
        {
            var p1 = DateRange.Between(
                new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 14)
                );

            var p2 = DateRange.Between(
                new LocalDate(2000, 1, 15), new LocalDate(1999, 1, 21)
                );

            var diffResult = DateRange.Diff(p1, p2);

            Assert.AreEqual(1, diffResult.OnlyInFirstPeriod.Count);
            Assert.AreEqual(p1, diffResult.OnlyInFirstPeriod[0]);

            Assert.AreEqual(1, diffResult.OnlyInSecondPeriod.Count);
            Assert.AreEqual(p2, diffResult.OnlyInSecondPeriod[0]);

            Assert.AreEqual(0, diffResult.CommonParts.Count);
        }

        [TestMethod]
        public void Diff_WithPeriodEndingOnThisStartReturnsOneDayPeriod()
        {
            var p1 = DateRange.Between(
                new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 14)
                );

            var p2 = DateRange.Between(
                new LocalDate(2000, 1, 14), new LocalDate(2000, 1, 16)
                );

            var diffResult = DateRange.Diff(p1, p2);

            Assert.AreEqual(1, diffResult.OnlyInFirstPeriod.Count);
            Assert.AreEqual(
                DateRange.Between(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 13)), 
                diffResult.OnlyInFirstPeriod[0]);

            Assert.AreEqual(1, diffResult.OnlyInSecondPeriod.Count);
            Assert.AreEqual(DateRange.Between(new LocalDate(2000, 1, 15), new LocalDate(2000, 1, 16)), 
                diffResult.OnlyInSecondPeriod[0]);

            Assert.AreEqual(1, diffResult.CommonParts.Count);
            Assert.AreEqual(
                DateRange.Between(new LocalDate(2000, 1, 14), new LocalDate(2000, 1, 14)), 
                diffResult.CommonParts[0]);
        }

        [TestMethod]
        public void Diff_WithPeriodOneCoveredByPeriodTwoResultsInPeriodOneBeingCommonPart()
        {
            var p1 = DateRange.Between(
                new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 14)
                );

            var p2 = DateRange.Between(
                new LocalDate(1999, 1, 1), new LocalDate(2001, 1, 14)
                );

            var diffResult = DateRange.Diff(p1, p2);

            Assert.AreEqual(0, diffResult.OnlyInFirstPeriod.Count);

            Assert.AreEqual(2, diffResult.OnlyInSecondPeriod.Count);

            Assert.AreEqual(1, diffResult.CommonParts.Count);
            Assert.AreEqual(
                p1, 
                diffResult.CommonParts[0]);
            
        }


    }
}
