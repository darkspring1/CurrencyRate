using CurrencyRate.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CurrencyRate.Api.ApplicationServices
{

    public class Week
    {
        public Week(DateTime startedOn)
        {
            StartedOn = startedOn;
            
            //если месяц начинается в СБ или ВС
            if (startedOn.DayOfWeek == DayOfWeek.Saturday)
            {
                StartedOn = StartedOn.AddDays(2);
            }
            else if(startedOn.DayOfWeek == DayOfWeek.Sunday)
            {
                StartedOn = StartedOn.AddDays(1);
            }

            
            var daysInMonth = DateTime.DaysInMonth(StartedOn.Year, StartedOn.Month);


            int diff = 5 - (int)StartedOn.DayOfWeek;

            if (diff >= 0)
            {
                int lastDayOfWeekNumber = StartedOn.Day + diff;
                lastDayOfWeekNumber = lastDayOfWeekNumber > daysInMonth ? daysInMonth : lastDayOfWeekNumber;
                FinishedOn = new DateTime(StartedOn.Year, StartedOn.Month, lastDayOfWeekNumber);
            }
        }

        public DateTime StartedOn { get; }

        public DateTime FinishedOn { get; }


        public Week GetNextWeek()
        {
            var nextWeekStartedOn = FinishedOn.AddDays(3);

            if (nextWeekStartedOn.Month != StartedOn.Month)
            {
                return null;
            }

            return new Week(nextWeekStartedOn);
        }
    }

    public class Month
    {
        public Month(int year, int month, Rate[] items)
        {
            var weeks = new List<Week>();
            var week = new Week(new DateTime(year, month, 1));

            do
            {
                weeks.Add(week);
                week = week.GetNextWeek();
            }
            while (week != null);

            Weeks = weeks.ToArray();

            //foreach(var item)
        }

        public Week[] Weeks { get; private set; }
    }
}
