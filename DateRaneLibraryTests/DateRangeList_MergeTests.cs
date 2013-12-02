using System;
using System.Linq;
using System.Collections.Generic;
using DateRangeLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;

namespace DateRaneLibraryTests
{
    [TestClass]
    public class DateRangeList_MergeTests
    {
        [TestMethod]
        public void Merge_OfTwoDistinctPeriodsRetunrnsOriginalList()
        {
            var lst = new List<DateRange> {
                DateRange.Between(new LocalDate(2000,1,1), new LocalDate(2000,1,10)),
                DateRange.Between(new LocalDate(2000,1,12), new LocalDate(2000,1,15))
            };

            var result = lst.Merge();
            Assert.AreEqual(2,result.Count());
        }

        [TestMethod]
        public void Merge_OfTwoPeriodsNextToEachOtherReturnsOnePeriod()
        {
            var lst = new List<DateRange> {
                DateRange.Between(new LocalDate(2000,1,1), new LocalDate(2000,1,10)),
                DateRange.Between(new LocalDate(2000,1,11), new LocalDate(2000,1,15))
            };

            var result = lst.Merge();
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(DateRange.Between(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 15)), result.First());
        }

        [TestMethod]
        public void Merge_OfTwoPeriodsWhereOneContainsTheOtherReturnOnePeriod()
        {
            var lst = new List<DateRange> {
                DateRange.Between(new LocalDate(2000,1,1), new LocalDate(2000,1,10)),
                DateRange.Between(new LocalDate(2000,1,3), new LocalDate(2000,1,5))
            };

            var result = lst.Merge();
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(DateRange.Between(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 10)), result.First());
        }

        [TestMethod]
        public void Merge_OfTwoPeriodsOverlapingTheOtherReturnOnePeriod()
        {
            var lst = new List<DateRange> {
                DateRange.Between(new LocalDate(2000,1,1), new LocalDate(2000,1,10)),
                DateRange.Between(new LocalDate(2000,1,3), new LocalDate(2000,1,15))
            };

            var result = lst.Merge();
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(DateRange.Between(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 15)), result.First());
        
        }

        [TestMethod]
        public void Merge_OfOneElementListReturnsOriginalList()
        {
            var lst = new List<DateRange> {
                DateRange.Between(new LocalDate(2000,1,1), new LocalDate(2000,1,10))
            };

            var result = lst.Merge();
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(DateRange.Between(new LocalDate(2000, 1, 1), new LocalDate(2000, 1, 10)), result.First());
        
        }

        [TestMethod]
        public void Merge_ThreeOverlapingElemsReturnsOneElementList()
        {
            var lst = new List<DateRange> {
                DateRange.Between(new LocalDate(1999,12,1), new LocalDate(2000,1,10)),
                DateRange.Between(new LocalDate(2000,1,1), new LocalDate(2000,1,10)),
                DateRange.Between(new LocalDate(2000,1,3), new LocalDate(2000,1,15))
            };

            var result = lst.Merge();
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(DateRange.Between(new LocalDate(1999, 12, 1), new LocalDate(2000, 1, 15)), result.First());
        
        }

        [TestMethod]
        public void Merge_EliminatesEmptyPeriodsFromList()
        {
            var lst = new List<DateRange> {
                DateRange.Between(new LocalDate(2013,12,1), new LocalDate(2000,1,10)),
                DateRange.Between(new LocalDate(2014,1,1), new LocalDate(2000,1,10)),
                DateRange.Between(new LocalDate(2015,1,3), new LocalDate(2000,1,15))
            };

            var result = lst.Merge();
            Assert.AreEqual(0, result.Count());
            
        }
    }
}
