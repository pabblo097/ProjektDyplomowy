namespace ProjektDyplomowy.Helpers
{
    public class TimeHelpers
    {
        public static string HowMuchTimePassed(DateTime date)
        {
            var span = DateTime.Now - date;
            var secondsQuantity = span.TotalSeconds;
            double value;

            if (secondsQuantity < 0)
                return "Jestem z przyszłości!";
            else if (secondsQuantity < 60)
            {
                value = Math.Round(span.TotalSeconds);
                return $"{value} sekund{GetTimeOrYearExtension(value, true)}";
            }
            else if (secondsQuantity < 3600)
            {
                value = Math.Round(span.TotalMinutes);
                return $"{value} minut{GetTimeOrYearExtension(value, true)}";
            }
            else if (secondsQuantity < 86400)
            {
                value = Math.Round(span.TotalHours);
                return $"{value} godzin{GetTimeOrYearExtension(value, true)}";
            }
            else if (secondsQuantity < 604800)
            {
                value = Math.Round(span.TotalDays);
                if (value == 1)
                    return $"{value} dzień";
                else
                    return $"{value} dni";
            }
            else if (secondsQuantity < 2629744)
            {
                value = Math.Round(span.TotalDays / 7);
                if (value == 1)
                    return $"{value} tydzień";
                else
                    return $"{value} tygodnie";
            }
            else if (secondsQuantity < 31556926)
            {
                value = Math.Round(span.TotalDays / 30);
                if (value == 1)
                    return $"{value} miesiąc";
                else if (value >= 2 && value <= 4)
                    return $"{value} miesiące";
                else
                    return $"{value} miesięcy";
            }
            else
            {
                value = Math.Round(span.TotalDays / 365);
                return $"{value} {GetTimeOrYearExtension(value, false)}";
            }
        }

        private static string GetTimeOrYearExtension(double value, bool isValueATime)
        {
            if (value == 0)
                return isValueATime ? "" : "lat";
            if (value == 1)
                return isValueATime ? "a" : "rok";
            else if (value < 5)
                return isValueATime ? "y" : "lata";
            else if (value < 22)
                return isValueATime ? "" : "lat";
            else if (value % 10 >= 2 && value % 10 <= 4)
                return isValueATime ? "y" : "lata";
            else
                return isValueATime ? "" : "lat";
        }
    }
}
