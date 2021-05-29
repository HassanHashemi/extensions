namespace Extensions
{
    public class PersianDateWrapper
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int? Hours { get; set; }
        public int? Minutes { get; set; }
        public int? Seconds { get; set; }

        public string Full => $"{this.Year}/{this.Month}/{this.Day}";
    }
}
