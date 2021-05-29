using System.ComponentModel;

namespace Extensions
{
    public enum DatePostfix
    {
        [Description("ثانیه")]
        Seconds,

        [Description("دقیقه")]
        Minute,

        [Description("ساعت")]
        Hours,

        [Description("روز")]
        Days,

        [Description("هفته")]
        Week,

        [Description("ماه")]
        Month,

        [Description("سال")]
        Year
    }
}
