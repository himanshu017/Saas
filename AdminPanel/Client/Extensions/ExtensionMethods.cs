using AdminPanel.Shared;

namespace AdminPanel.Client.Extensions
{
    public static class Extentions
    {
        public static List<T> GetClone<T>(this List<T> source)
        {
            return source.GetRange(0, source.Count);
        }

        public static DateTime GetNextDeliveryDate(this DateTime current, TimeSpan cutoffTime, IEnumerable<string> permittedDays, bool isSameDay)
        {
            var currentTime = DateTime.Now.TimeOfDay;
            DateTime date = new DateTime();

            if (currentTime < cutoffTime)
            {
                if (isSameDay)
                {
                    date = current;
                    date = CalculateNextDate(permittedDays, date);
                }
                else
                {
                    date = current.AddDays(isSameDay ? 1 : 2);
                    date = CalculateNextDate(permittedDays, date);
                }
            }
            else
            {
                date = current.AddDays(1);
                date = CalculateNextDate(permittedDays, date);
            }

            return date;
        }

        private static DateTime CalculateNextDate(IEnumerable<string> permittedDays, DateTime date)
        {
            var validDays = ValidDeliveryDays(permittedDays.ToList());

            bool dateSet = false;
            while (!dateSet)
            {
                if (validDays.Contains(date.DayOfWeek))
                {
                    dateSet = true;
                }
                else
                {
                    date = date.AddDays(1);
                }
            }

            return date;
        }

        static DateTime GetDate(DateTime date, DayOfWeek[] validDays, bool addDay = false)
        {
            bool dateSet = false;
            while (!dateSet)
            {
                if (validDays.Contains(date.DayOfWeek))
                {
                    dateSet = true;
                }
                else
                {
                    date = date.AddDays(1);
                }
            }

            return date;
        }

        static DayOfWeek[] ValidDeliveryDays(List<string> days)
        {
            DayOfWeek[] daysOfWeeks = new DayOfWeek[days.Count()];

            for (int i = 0; i < days.Count(); i++)
            {
                daysOfWeeks[i] = (DayOfWeek)((int)(WeekDay)Enum.Parse(typeof(WeekDay), days[i]));
            }

            return daysOfWeeks;
        }
    }
}
