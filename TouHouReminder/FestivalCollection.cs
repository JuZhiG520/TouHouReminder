namespace TouHouReminder
{
    public enum FestivalType
    {
        Days,
        DaysEveryMonth,
        DaysEveryWeek,
        Months
    }

    public record FestivalInfo
    {
        public string Time { get; set; } = string.Empty;
        public string Character { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class FestivalCollection
    {
        public List<FestivalInfo> Days { get; set; } = new();
        public List<FestivalInfo> DaysEveryMonth { get; set; } = new();
        public List<FestivalInfo> DaysEveryWeek { get; set; } = new();
        public List<FestivalInfo> Months { get; set; } = new();

        public int GetTotalCount() => Days.Count + DaysEveryMonth.Count + DaysEveryWeek.Count + Months.Count;

        public List<FestivalInfo> this[int index]
        {
            get => index switch
            {
                0 => Days,
                1 => DaysEveryMonth,
                2 => DaysEveryWeek,
                3 => Months,
                _ => throw new Exception(),
            };
        }
    }
}
