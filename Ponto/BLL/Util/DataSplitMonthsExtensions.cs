using System;
using System.Collections.Generic;

namespace BLL.Util
{
    public static class DataSplitMonthsExtensions
    {
        public static IEnumerable<Tuple<DateTime, DateTime>> GetIntervalsPerMonths(
            this DateTime from,
            DateTime to)
        {

            var currentFrom = from;
            var currentTo = from.AdvanceToStartOfNextMonth();

            while (currentTo < to)
            {
                yield return Tuple.Create(currentFrom, currentTo);
                currentFrom = currentTo.AddDays(1);
                currentTo = currentFrom.AdvanceToStartOfNextMonth();
            }

            yield return Tuple.Create(currentFrom, to);
        }

        public static DateTime AdvanceToStartOfNextMonth(this DateTime @this)
        {
            var newMonth = @this.Month + 1;
            var newYear = @this.Year;
            if (newMonth == 13)
            {
                newMonth = 1;
                newYear++;
            }
            DateTime dt = new DateTime(newYear, newMonth, 1).AddDays(-1);
            return dt;
        }
    }
}
