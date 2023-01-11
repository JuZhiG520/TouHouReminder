using System.Text;
using System.Text.RegularExpressions;

namespace TouHouReminder
{
    internal static class TimeConverter
    {
        public struct MatchResult
        {
            public bool IsCompletelyMatched { get; set; }
            public int Hour { get; set; }
            public int Minute { get; set; }

            public MatchResult()
            {
                IsCompletelyMatched = true;
                Hour = 0;
                Minute = 0;
            }
        }

        private static readonly string[] MONTH_ARRAY =
            { "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December" };

        private static readonly string[] WEEKDAY_ARRAY =
            { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        private static string NumToMonth(int num) => num switch
        {
            1 => "January",
            2 => "February",
            3 => "March",
            4 => "April",
            5 => "May",
            6 => "June",
            7 => "July",
            8 => "August",
            9 => "September",
            10 => "October",
            11 => "November",
            12 => "December",
            _ => throw new Exception()
        };

        private static int MonthToNum(string str) => str switch
        {
            "January" => 1,
            "February" => 2,
            "March" => 3,
            "April" => 4,
            "May" => 5,
            "June" => 6,
            "July" => 7,
            "August" => 8,
            "September" => 9,
            "October" => 10,
            "November" => 11,
            "December" => 12,
            _ => throw new Exception()
        };

        private static string OrdinalNumToRadix(int num)
        {
            if (num < 0)
            {
                throw new Exception();
            }

            if ((num < 10 || num > 20) && num % 10 >= 1 && num % 10 <= 3)
            {
                return (num % 10) switch
                {
                    1 => num + "st",
                    2 => num + "nd",
                    3 => num + "rd",
                    _ => throw new Exception()
                };
            }
            else
            {
                return num + "th";
            }
        }

        private static int RadixToOrdinalNum(string str)
        {
            if (Regex.IsMatch(str, "\\d+"))
            {
                return Convert.ToInt32(Regex.Match(str, "\\d+").Value);
            }
            else
            {
                throw new Exception();
            }
        }

        private static string CNWeekdayToEN(string str) => str switch
        {
            "日曜日（星期日）" => "Sunday",
            "月曜日（星期一）" => "Monday",
            "火曜日（星期二）" => "Tuesday",
            "水曜日（星期三）" => "Wednesday",
            "木曜日（星期四）" => "Thursday",
            "金曜日（星期五）" => "Friday",
            "土曜日（星期六）" => "Saturday",
            _ => throw new Exception()
        };

        private static DayOfWeek ENWeekdayToDayOfWeek(string str) => str switch
        {
            "Sunday" => DayOfWeek.Sunday,
            "Monday" => DayOfWeek.Monday,
            "Tuesday" => DayOfWeek.Tuesday,
            "Wednesday" => DayOfWeek.Wednesday,
            "Thursday" => DayOfWeek.Thursday,
            "Friday" => DayOfWeek.Friday,
            "Saturday" => DayOfWeek.Saturday,
            _ => throw new Exception()
        };

        private static string CNDateToENMonth(string str)
        {
            int numMonth = Convert.ToInt32(str[..str.IndexOf('月')]);
            return NumToMonth(numMonth);
        }

        private static string CNDateToEN(string str)
        {
            StringBuilder builder = new();

            builder.Append(CNDateToENMonth(str));
            builder.Append(' ');

            int num = Convert.ToInt32(str[(str.IndexOf('月') + 1)..str.IndexOf('日')]);
            builder.Append(OrdinalNumToRadix(num));

            return builder.ToString();
        }

        public static string GetENFestivalTime(string timeText)
        {
            StringBuilder builder = new();

            if (Regex.IsMatch(timeText, "^\\d{1,2}月\\d{1,2}日(左右)?$"))
            {
                if (timeText.Contains("左右"))
                {
                    builder.Append("About ");
                    builder.Append(CNDateToEN(timeText[..^2]));
                }
                else
                {
                    builder.Append(CNDateToEN(timeText));
                }
            }
            else if (Regex.IsMatch(timeText, "^\\d{1,2}月(\\d{1,2}·?)+日$"))
            {
                builder.Append(CNDateToENMonth(timeText));
                builder.Append(' ');

                string[] strArray = timeText[(timeText.IndexOf('月') + 1)..timeText.IndexOf('日')].Split('·');

                for (int i = 0; i < strArray.Length; i++)
                {
                    builder.Append(OrdinalNumToRadix(Convert.ToInt32(strArray[i])));

                    if (i < strArray.Length - 1)
                    {
                        builder.Append('·');
                    }
                }
            }
            else if (Regex.IsMatch(timeText, "^\\d{1,2}月\\d{1,2}-\\d{1,2}日$"))
            {
                builder.Append(CNDateToENMonth(timeText));
                builder.Append(' ');

                string[] strArray = timeText[(timeText.IndexOf('月') + 1)..timeText.IndexOf('日')].Split('-');

                builder.Append(OrdinalNumToRadix(Convert.ToInt32(strArray[0])));
                builder.Append('-');
                builder.Append(OrdinalNumToRadix(Convert.ToInt32(strArray[1])));
            }
            else if (Regex.IsMatch(timeText, "^\\d{1,2}月每日$"))
            {
                builder.Append("Every day in ");
                builder.Append(CNDateToENMonth(timeText));
            }
            else if (Regex.IsMatch(timeText, "^(\\d{1,2}月\\d{1,2}日)-(\\d{1,2}月\\d{1,2}日)$"))
            {
                string[] strArray = timeText.Split('-');

                builder.Append(CNDateToEN(strArray[0]));
                builder.Append(" to ");
                builder.Append(CNDateToEN(strArray[1]));
            }
            else if (Regex.IsMatch(timeText, "^\\d{1,2}月\\d{1,2}日\\d{1,2}时\\d{1,2}分$"))
            {
                builder.Append(CNDateToEN(Regex.Match(timeText, "\\d{1,2}月\\d{1,2}日").Value));
                builder.Append(", ");
                builder.Append(Convert.ToUInt32(timeText[(timeText.IndexOf('日') + 1)..timeText.IndexOf('时')]));
                builder.Append(':');
                builder.Append(timeText[(timeText.IndexOf('时') + 1)..timeText.IndexOf('分')].PadLeft(2, '0'));
            }
            else if (Regex.IsMatch(timeText, "^每月\\d{1,2}日$"))
            {
                int num = Convert.ToInt32(timeText[(timeText.IndexOf('月') + 1)..timeText.IndexOf('日')]);
                builder.Append($"The {OrdinalNumToRadix(num)} day of every month");
            }
            else if (Regex.IsMatch(timeText, "^\\S+（\\S+）$"))
            {
                builder.Append($"Every {CNWeekdayToEN(timeText)}");
            }
            else if (Regex.IsMatch(timeText, "^\\d{1,2}月$"))
            {
                builder.Append(CNDateToENMonth(timeText));
            }
            else
            {
                throw new Exception();
            }

            return builder.ToString();
        }

        public static bool CompareTimeWithSpanString(DateTime dateTime, string timeSpan, out MatchResult matchResult)
        {
            matchResult = new MatchResult();

            if (Regex.IsMatch(timeSpan, "^(About )?[A-Z][a-z]+ \\d{1,2}[a-z]{2}$"))
            {
                if (timeSpan.Contains("About "))
                {
                    timeSpan = Regex.Replace(timeSpan, "^(About )?", string.Empty);
                }

                return dateTime.Month == MonthToNum(Regex.Match(timeSpan, "^[A-Z][a-z]+").Value)
                    && dateTime.Day == RadixToOrdinalNum(Regex.Match(timeSpan, "\\d{1,2}[a-z]{2}$").Value);
            }
            else if (Regex.IsMatch(timeSpan, "^[A-Z][a-z]+ (\\d{1,2}[a-z]{2}·?)+$"))
            {
                if (dateTime.Month != MonthToNum(Regex.Match(timeSpan, "^[A-Z][a-z]+").Value))
                {
                    return false;
                }

                string[] strArray = Regex.Match(timeSpan, "(\\d{1,2}[a-z]{2}·?)+$").Value.Split('·');
                foreach (string str in strArray)
                {
                    if (dateTime.Day == RadixToOrdinalNum(str))
                    {
                        return true;
                    }
                }

                return false;
            }
            else if (Regex.IsMatch(timeSpan, "^[A-Z][a-z]+ \\d{1,2}[a-z]{2}-\\d{1,2}[a-z]{2}$"))
            {
                if (dateTime.Month != MonthToNum(Regex.Match(timeSpan, "^[A-Z][a-z]+").Value))
                {
                    return false;
                }

                string[] strArray = Regex.Match(timeSpan, "\\d{1,2}[a-z]{2}-\\d{1,2}[a-z]{2}$").Value.Split('·');
                for (int i = RadixToOrdinalNum(strArray[0]); i <= RadixToOrdinalNum(strArray[^1]); i++)
                {
                    if (dateTime.Day == i)
                    {
                        return true;
                    }
                }

                return false;
            }
            else if (Regex.IsMatch(timeSpan, "^Every day in [A-Z][a-z]+$"))
            {
                return dateTime.Month == MonthToNum(Regex.Match(timeSpan, "[A-Z][a-z]+$").Value);
            }
            else if (Regex.IsMatch(timeSpan, "^[A-Z][a-z]+ \\d{1,2}[a-z]{2} to [A-Z][a-z]+ \\d{1,2}[a-z]{2}$"))
            {
                MatchCollection monthCollection = Regex.Matches(timeSpan, "[A-Z][a-z]+");
                MatchCollection dayCollection = Regex.Matches(timeSpan, "\\d{1,2}[a-z]{2}");

                for (int i = MonthToNum(monthCollection[0].Value); i <= MonthToNum(monthCollection[^1].Value); i++)
                {
                    if (dateTime.Month != i)
                    {
                        return false;
                    }
                }

                for (int i = MonthToNum(dayCollection[0].Value); i <= MonthToNum(dayCollection[^1].Value); i++)
                {
                    if (dateTime.Day != i)
                    {
                        return false;
                    }
                }

                return true;
            }
            else if (Regex.IsMatch(timeSpan, "^[A-Z][a-z]+ \\d{1,2}[a-z]{2}, \\d{1,2}:\\d{1,2}$"))
            {
                matchResult.IsCompletelyMatched = false;

                string[] strArray = Regex.Match(timeSpan, "\\d{1,2}:\\d{1,2}$").Value.Split(':');
                matchResult.Hour = Convert.ToInt32(strArray[0]);
                matchResult.Minute = Convert.ToInt32(strArray[1]);

                return dateTime.Month == MonthToNum(Regex.Match(timeSpan, "^[A-Z][a-z]+").Value)
                    && dateTime.Day == RadixToOrdinalNum(Regex.Match(timeSpan, "\\d{1,2}[a-z]{2}").Value);
            }
            else if (Regex.IsMatch(timeSpan, "^The \\d{1,2}[a-z]{2} day of every month$"))
            {
                return dateTime.Day == RadixToOrdinalNum(Regex.Match(timeSpan, "\\d{1,2}[a-z]{2}").Value);
            }
            else
            {
                if (timeSpan.Contains("Every "))
                {
                    if (WEEKDAY_ARRAY.Contains(timeSpan[6..]))
                    {
                        return dateTime.DayOfWeek == ENWeekdayToDayOfWeek(timeSpan[6..]);
                    }
                }
                else if (MONTH_ARRAY.Contains(timeSpan))
                {
                    return dateTime.Month == MonthToNum(timeSpan);
                }

                throw new Exception();
            }
        }
    }
}
