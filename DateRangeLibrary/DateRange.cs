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
            if (!this.Overlaps(theOther)) return DateRange.Empty;

            return new DateRange(
                this.From > theOther.From ? this.From : theOther.From,
                this.To > theOther.To ? theOther.To : this.To
                );
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
        /// Returns part of this period not included in theOther
        /// </summary>
        /// <param name="theOther">other period</param>
        /// <returns>part of this period not included in theOther</returns>
        public List<DateRange> Diff(DateRange theOther)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public static int DaysFromBeginningOfMonth(this LocalDate thisDate)
        {
            throw new NotImplementedException();
        }
    }
}
