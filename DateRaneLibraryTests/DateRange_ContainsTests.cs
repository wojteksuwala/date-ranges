using System;
using DateRangeLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;

namespace DateRaneLibraryTests
{
    [TestClass]
    public class DateRange_ContainsTests
    {
        DateRange subjectOfTest;

        [TestInitialize]
        public void Setup()
        {
            subjectOfTest = new DateRange(
                new LocalDate(2000,1,1),
                new LocalDate(2000,1,15)
                );
        }

        [TestMethod]
        public void Contains_DateInsidePeriod()
        {
            Assert.IsTrue(subjectOfTest.Contains(new LocalDate(2000,1,13)));
        }

        [TestMethod]
        public void Contains_DateOutsidePeriod()
        {
            Assert.IsFalse(subjectOfTest.Contains(new LocalDate(2000,1,16)));
        }

        [TestMethod]
        public void Contains_DateEqualPeriodStart()
        {
            Assert.IsTrue(subjectOfTest.Contains(subjectOfTest.From));
        }

        [TestMethod]
        public void Contains_DateEqualPeriodEnd()
        {
            Assert.IsTrue(subjectOfTest.Contains(subjectOfTest.To));
        }

        [TestMethod]
        public void Contains_ContainsTheSamePeriod() 
        {
            Assert.IsTrue(subjectOfTest.Contains(subjectOfTest));
        }

        [TestMethod]
        public void Contains_ContainsInsidePeriod()
        {
            var smallerPeriod = new DateRange(new LocalDate(2000, 1, 5), new LocalDate(2000, 1, 7));
            Assert.IsTrue(subjectOfTest.Contains(smallerPeriod));
        }

        [TestMethod]
        public void Contains_DoesNotContainPeriodBefore()
        {
            var smallerPeriod = new DateRange(new LocalDate(1999, 1, 5), new LocalDate(1999, 12, 31));
            Assert.IsFalse(subjectOfTest.Contains(smallerPeriod));
        }

        [TestMethod]
        public void Contains_DoesNotContainPeriodAfter()
        {
            var smallerPeriod = new DateRange(new LocalDate(2001, 1, 1), new LocalDate(2001, 1, 7));
            Assert.IsFalse(subjectOfTest.Contains(smallerPeriod));
        }

        [TestMethod]
        public void Contains_DoesNotContainPeriodPartialyBefore()
        {
            var smallerPeriod = new DateRange(new LocalDate(1999, 1, 5), new LocalDate(2000, 1, 3));
            Assert.IsFalse(subjectOfTest.Contains(smallerPeriod));
        }

        [TestMethod]
        public void Contains_DoesNotContainPeriodPartialyAfter()
        {
            var smallerPeriod = new DateRange(new LocalDate(2000, 2, 1), new LocalDate(2000, 2, 7));
            Assert.IsFalse(subjectOfTest.Contains(smallerPeriod));
        }

        [TestMethod]
        public void Contains_DoesNotContainPeriodThatCompletelyOverlaps()
        {
            var biggerPeriod = new DateRange(new LocalDate(1970, 1, 1), new LocalDate(2012, 1, 7));
            Assert.IsFalse(subjectOfTest.Contains(biggerPeriod));
        }
    }
}
