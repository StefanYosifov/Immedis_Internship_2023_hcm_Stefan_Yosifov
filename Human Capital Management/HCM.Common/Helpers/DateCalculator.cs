namespace HCM.Common.Helpers
{
    public static class DateCalculator
    {
        public static int CalculateAge(DateTime? date)
        {
            return (int)((DateTime.Now - date).Value.Days / 365.242199);
        }

        public static int CalculateDays(DateTime? date)
        {
            return (DateTime.Now - date).Value.Days;
        }

    }
}