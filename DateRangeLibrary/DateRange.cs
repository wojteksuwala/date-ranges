using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace DateRangeLibrary
{
    /// <summary>
    /// Represents date period
    /// </summary>
    public class DateRange : IComparable<DateRange>
    {
        /// <summary>
        /// Firts day of period
        /// </summary>
        public LocalDate From {get;protected set;}
        /// <summary>
        /// Last day of period
        /// </summary>
        public LocalDate To { get; protected set; }

        /// <summary>
        /// Empty period
        /// </summary>
        public static DateRange Empty = new DateRange(new LocalDate(1980,1,1), new LocalDate(1979,12,31));

        /// <summary>
        /// Constructs new period
        /// </summary>
        /// <param name="from">start</param>
        /// <param name="to">end</param>
        public DateRange(LocalDate from, LocalDate to)
        {
            this.From = from;
            this.To = to;
        }

        /// <summary>
        /// Creates new date range between two dates
        /// </summary>
        /// <param name="from">start</param>
        /// <param name="to">end</param>
        /// <returns>new date range between two dates</returns>
        public static DateRange Between(LocalDate from, LocalDate to)
        {
            return new DateRange(from, to);
        }

        /// <summary>
        /// Tells if two periods have any common parts
        /// </summary>
        /// <param name="theOther">other period</param>
        /// <returns>true if two periods share any days</returns>
        public bool Overlaps(DateRange theOther)
        {
            return theOther.Contains(From) || theOther.Contains(To) || this.Contains(theOther);
        }

        /// <summary>
        /// Result of adding two periods. 
        /// </summary>
        /// <param name="theOther">other period</param>
        /// <returns>period or two periods that are sum of this and the other</returns>
        public List<DateRange> Sum(DateRange theOther)
        {
            if (this.IsEmpty()) return new List<DateRange> { theOther.Copy() };
            if (theOther.IsEmpty()) return new List<DateRange> { this.Copy() };

            if (!this.Overlaps(theOther) && !this.Abuts(theOther))
                return new List<DateRange> { this.Copy(), theOther.Copy() };


            return new List<DateRange> { 
                new DateRange(
                    this.From > theOther.From ? theOther.From : this.From,
                    this.To > theOther.To ? this.To : theOther.To
                )
            };
        }

        /// <summary>
        /// Detects common part of two periods.
        /// If two periods do not overlap then empty perdiod is returned
        /// </summary>
        /// <param name="theOther">other period</param>
        /// <returns>common part of two periods</returns>
        public DateRange Intersect(DateRange theOther)
        {
            if (theOther.IsEmpty() || this.IsEmpty())
                return DateRange.Empty;

            if (!this.Overlaps(theOther)) return DateRange.Empty;

            return new DateRange(
                this.From > theOther.From ? this.From : theOther.From,
                this.To > theOther.To ? theOther.To : this.To
                );
        }

        /// <summary>
        /// Returns list of periods that is result of taking common part of 
        /// this period and list of other periods
        /// </summary>
        /// <param name="otherPeriods">other periods</param>
        /// <returns>list of periods that is result of taking common part of 
        /// this period and list of other periods
        /// </returns>
        public List<DateRange> Intersect(List<DateRange> otherPeriods)
        {
            var intersect = new List<DateRange>();

            foreach (var period in otherPeriods) 
            {
                var partialIntersect = this.Intersect(period);
                if (!partialIntersect.IsEmpty())
                    intersect.Add(partialIntersect);
            }
            return intersect.Merge();
        }

        /// <summary>
        /// If cut point is outside period the the same period is returned.
        /// Otherwise returns two periods - one that ends at cut point
        /// and the other that starts next day
        /// </summary>
        /// <param name="cutPoint">day of division</param>
        /// <returns>If cut point is outside period the the same period is returned.
        /// Otherwise returns two periods - one that ends at cut point
        /// and the other that starts next day
        /// </returns>
        public List<DateRange> Divide(LocalDate cutPoint)
        {
            //TODO: what about empty and one day

            if (!this.Contains(cutPoint))
                return new List<DateRange> { this.Copy() };

            return new List<DateRange> { 
                new DateRange(this.From, cutPoint),
                new DateRange(cutPoint.PlusDays(1),this.To)
            };
        }

        /// <summary>
        /// Returns iterator that walks over days in period
        /// </summary>
        /// <exception cref="InvalidOperationException">thrown if trying to iterate over empty period</exception>
        /// <returns>iterator that walks over days in period</returns>
        public IEnumerable<LocalDate> GetDays()
        {
            if (this.IsEmpty())
                throw new InvalidOperationException("Cannot iterate over empty period");

            for (var day = this.From; day <= this.To; day = day.PlusDays(1))
                yield return day;
        }


        /// <summary>
        /// Returns result of comparison between two periods
        /// </summary>
        /// <param name="first">other period</param>
        /// <param name="second">second period</param>
        /// <returns>result of comparison between two periods</returns>
        public static DateRangeDiff Diff(DateRange first,DateRange second)
        {
            var commonParts = new List<DateRange>();
            var intersection = first.Intersect(second);
            if (!intersection.IsEmpty())
                commonParts.Add(intersection);

            return new DateRangeDiff(commonParts, first.Remove(intersection), second.Remove(first));
        }

        /// <summary>
        /// Returns only part of this period that is not included in the Other
        /// </summary>
        /// <param name="theOther">period to remove</param>
        /// <returns>only part of this period that is not included in the Other</returns>
        public List<DateRange> Remove(DateRange theOther) 
        {
            var intersection = this.Intersect(theOther);

            if (intersection.IsEmpty()) return new List<DateRange> { this.Copy() };

            var p1 = DateRange.Between(this.From, intersection.From.PlusDays(-1));
            var p2 = DateRange.Between(intersection.To.PlusDays(1), this.To);

            var leftovers = new List<DateRange>();
            if (!p1.IsEmpty()) leftovers.Add(p1);
            if (!p2.IsEmpty()) leftovers.Add(p2);
            return leftovers;
        }

        /// <summary>
        /// Tells if given date falls between start and end
        /// </summary>
        /// <param name="day">day to check</param>
        /// <returns>true if given date falls between start and end</returns>
        public bool Contains(LocalDate day) 
        {
            return !day.Before(From) && !day.After(To);
        }

        /// <summary>
        /// Tells if given period falls between start and end of this period
        /// </summary>
        /// <param name="theOther">other period</param>
        /// <returns>true if given period falls between start and end of this period</returns>
        public bool Contains(DateRange theOther)
        {
            return this.Contains(theOther.From) && this.Contains(theOther.To);
        }

        /// <summary>
        /// Tells if this period starts before given day
        /// </summary>
        /// <param name="day">day</param>
        /// <returns>true if this period starts before given day</returns>
        public bool StartsBefore(LocalDate day) 
        {
            return this.From.Before(day);
        }

        /// <summary>
        /// Tells if this period starts before the other period
        /// </summary>
        /// <param name="theOther"></param>
        /// <returns>true if this period starts before the other period</returns>
        public bool StartsBefore(DateRange theOther)
        {
            return this.From.Before(theOther.From);
        }

        /// <summary>
        /// Tells if this period ends before given day
        /// </summary>
        /// <param name="day">day</param>
        /// <returns>true if this period ends before given day</returns>
        public bool EndsBefore(LocalDate day)
        {
            return this.To.Before(day);
        }

        /// <summary>
        /// Tells if this periods ends befor start of the other period
        /// </summary>
        /// <param name="theOther">other period</param>
        /// <returns>true if this periods ends befor start of the other period</returns>
        public bool EndsBefore(DateRange theOther)
        {
            return this.To.Before(theOther.From);
        }

        /// <summary>
        /// True if period is empty. One day period is not considered empty.
        /// </summary>
        /// <returns>True if period is empty</returns>
        public bool IsEmpty() 
        {
            return From.After(To);
        }

        /// <summary>
        /// Calculates the gap between two periods
        /// </summary>
        /// <param name="theOther">other period</param>
        /// <returns>the gap between two periods</returns>
        public DateRange Gap(DateRange theOther)
        {
            if (this.Overlaps(theOther)) return DateRange.Empty;
            DateRange lower, higher;
            if (this.CompareTo(theOther) < 0)
            {
                lower = this;
                higher = theOther;
            }
            else
            {
                lower = theOther;
                higher = this;
            }
            return new DateRange(lower.To.PlusDays(1), higher.From.PlusDays(-1));
        }

        /// <summary>
        /// Tells if two periods stand next to eachother
        /// </summary>
        /// <param name="theOther">other period</param>
        /// <returns>true if two periods stand next to eachother</returns>
        public bool Abuts(DateRange theOther)
        {
            return !this.Overlaps(theOther) && this.Gap(theOther).IsEmpty();
        }

        /// <summary>
        /// Tells if two periods are equal.
        /// They are equal if both are empty or both have the same start and end date
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            var otherRange = other as DateRange;
            if (otherRange == null) return false;

            if (this.IsEmpty() && otherRange.IsEmpty()) return true;
            
            return From == otherRange.From && To == otherRange.To;
        }

        public override int GetHashCode()
        {
            return From.GetHashCode();
        }

        public int CompareTo(DateRange other)
        {
            if (!From.Equals(other.From)) return From.CompareTo(other.From);
            return To.CompareTo(other.To);
        }

        /// <summary>
        /// Creates copy of this object
        /// </summary>
        /// <returns>copy of this object</returns>
        public DateRange Copy()
        {
            return new DateRange(this.From, this.To);
        }


        /// <summary>
        /// Calculates lenth of period in months.
        /// It does so by dividing period into parts for each month included
        /// and then takes sum of numer of days in period included in month
        /// divided by number of days in month
        /// </summary>
        /// <returns></returns>
        public decimal LengthInMonths()
        {
            //simple case -> empty period
            if (IsEmpty()) return 0;
            //simple case -> start and end in the same month
            //TODO: add very special case when whole month is covered
            if (From.InTheSameMonth(To)) 
                return decimal.Divide(To.Day - From.Day + 1, DateTime.DaysInMonth(From.Year,From.Month)).Round();

            //in case period spans 2 or more months
            decimal len = 0;
            //calc part from start month here
            len += DateRange.Between(From, From.EndOfMonth()).LengthInMonths();
            //calc months between
            var iterarorDate = From.PlusMonths(1);
            while (!iterarorDate.InTheSameMonth(To))
            {
                len += 1;
                iterarorDate = iterarorDate.PlusMonths(1);
            }
            //calc part from end month here
            len += DateRange.Between(To.BeginningOfMonth(), To).LengthInMonths().Round();

            return len;
        }
    }

    /// <summary>
    /// Class represents result of comparison of two periods.
    /// </summary>
    public class DateRangeDiff
    {
        /// <summary>
        /// Parts present in both periods
        /// </summary>
        public List<DateRange> CommonParts { get; protected set; }
        /// <summary>
        /// Parts present only in first period 
        /// </summary>
        public List<DateRange> OnlyInFirstPeriod { get; protected set; }
        /// <summary>
        /// Parts present in second period
        /// </summary>
        public List<DateRange> OnlyInSecondPeriod { get; protected set; }

        public DateRangeDiff(
            List<DateRange> commonParts,
            List<DateRange> onlyInFirstPeriod,
            List<DateRange> onlyInSecondPeriod
            )
        {
            this.CommonParts = commonParts;
            this.OnlyInFirstPeriod = onlyInFirstPeriod;
            this.OnlyInSecondPeriod = onlyInSecondPeriod;
        }
    }

    /// <summary>
    /// Extension methods over IEnumerable of DateRange
    /// </summary>
    public static class DateReangeListExtensions
    {
        /// <summary>
        /// Creates list of periods by mergin periods in original list.
        /// </summary>
        /// <param name="list">original list</param>
        /// <returns>list of periods by mergin periods in original list</returns>
        public static List<DateRange> Merge(this List<DateRange> list) 
        {
            var toProcess = list.Where(l=>!l.IsEmpty()).ToList();
            var result = new List<DateRange>();
            while (toProcess.Count >= 1) 
            {
                var candidateIsDistinct = true;
                var candidate = toProcess[0];
                for (int i = 1; i < toProcess.Count; i++)
                {
                    var p = toProcess[i];
                    if (candidate.Overlaps(p) || candidate.Abuts(p))
                    {
                        var merge = candidate.Sum(p)[0];
                        toProcess.Remove(p);
                        toProcess.Remove(candidate);
                        toProcess.Add(merge);
                        candidateIsDistinct = false;
                        break;
                    }
                }
                if (candidateIsDistinct)
                {
                    toProcess.Remove(candidate);
                    result.Add(candidate);
                }
            }
            return result;
        }

    }

    public static class LocalDateExtensions
    {
        public static bool Before(this LocalDate thisDate, LocalDate otherDate)
        {
            return thisDate < otherDate;
        }

        public static bool After(this LocalDate thisDate, LocalDate otherDate)
        {
            return thisDate > otherDate;
        }

        public static LocalDate EndOfMonth(this LocalDate thisDate) 
        {
            return new LocalDate(thisDate.Year, thisDate.Month,
                DateTime.DaysInMonth(thisDate.Year, thisDate.Month));
        }

        public static LocalDate BeginningOfMonth(this LocalDate thisDate)
        {
            return new LocalDate(thisDate.Year, thisDate.Month, 1);
        }

        public static int DaysToEndOfMonth(this LocalDate thisDate)
        {
            var numberOfDaysInMonth = DateTime.DaysInMonth(thisDate.Year, thisDate.Month);
            return numberOfDaysInMonth - thisDate.Day;
        }

        public static int DaysFromBeginningOfMonth(this LocalDate thisDate)
        {
            return thisDate.Day;
        }

        public static bool InTheSameMonth(this LocalDate thisDate, LocalDate otherDate) 
        {
            return thisDate.Year == otherDate.Year && thisDate.Month == otherDate.Month;
        }
    }

    /// <summary>
    /// Extension methods to simplify numerics code
    /// </summary>
    public static class NumericExtensions
    {
        /// <summary>
        /// Rounds decimal to specified number of places
        /// </summary>
        /// <param name="thisNumber">number to round</param>
        /// <param name="decimals">number of places (default value is 6)</param>
        /// <returns>rounded decimal</returns>
        public static decimal Round(this decimal thisNumber,int decimals = 6)
        {
            return Decimal.Round(thisNumber, decimals);
        }
    }
}
