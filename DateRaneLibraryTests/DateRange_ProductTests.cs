using System;
using DateRangeLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;

namespace DateRaneLibraryTests
{
    [TestClass]
    public class DateRange_ProductTests
    {
        [TestMethod]
        public void Product_OfNotOverlappingPeriodsIsEmpty()
        {
            var p1 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var p2 = new DateRange(new LocalDate(2000, 1, 6), new LocalDate(2000, 1, 7));
            var product = p1.Intersect(p2);
            Assert.IsTrue(product.IsEmpty());
        }

        [TestMethod]
        public void Product_OfPeriodAndPeriodContainedInsideInSmallerPeriod()
        {
            var p1 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var p2 = new DateRange(new LocalDate(2000, 1, 2), new LocalDate(2000, 1, 4));
            var product = p1.Intersect(p2);
            Assert.IsFalse(product.IsEmpty());
            Assert.AreEqual(new DateRange(new LocalDate(2000, 1, 2), new LocalDate(2000, 1, 4)), product);
        }

        [TestMethod]
        public void Product_OfPeriodAndPeriodThatContainsItIsGivenPeriod()
        {
            var p1 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var p2 = new DateRange(new LocalDate(1999, 1, 2), new LocalDate(2012, 1, 4));
            var product = p1.Intersect(p2);
            Assert.IsFalse(product.IsEmpty());
            Assert.AreEqual(new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5)), product);
        }

        [TestMethod]
        public void Product_OfPeriodAndPeriodOverlappingLeftIsLeftCommonPart()
        {
            var p1 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var p2 = new DateRange(new LocalDate(1999, 1, 2), new LocalDate(2000, 1, 4));
            var product = p1.Intersect(p2);
            Assert.IsFalse(product.IsEmpty());
            Assert.AreEqual(new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 4)), product);
        }

        [TestMethod]
        public void Product_OfPeriodAndPeriodOverlappingRightIsRightCommonPart()
        {
            var p1 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var p2 = new DateRange(new LocalDate(2000, 1, 4), new LocalDate(2000, 1, 14));
            var product = p1.Intersect(p2);
            Assert.IsFalse(product.IsEmpty());
            Assert.AreEqual(new DateRange(new LocalDate(2000, 1, 4), new LocalDate(2000, 1, 5)), product);
        }

        [TestMethod]
        public void Product_OfPeriodAndTheSamePeriodIsGivenPeriod()
        {
            var p1 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var p2 = new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5));
            var product = p1.Intersect(p2);
            Assert.IsFalse(product.IsEmpty());
            Assert.AreEqual(new DateRange(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 5)), product);
        }

        [TestMethod]
        public void Product_OfPeriodAndEmptyIsAlwaysEmpty()
        {
            var p1 = DateRange.Empty;
            var p2 = new DateRange(new LocalDate(2000, 1, 2), new LocalDate(2000, 1, 4));
            var product = p1.Intersect(p2);
            Assert.IsTrue(product.IsEmpty());
            
        }
    }
}
