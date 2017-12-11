namespace Ubora.Domain.Projects.StructuredInformations
{
    public class Duration
    {
        public Duration(int days, int months, int years)
        {
            Days = days;
            Months = months;
            Years = years;
        }

        public int Days { get; private set; }
        public int Months { get; private set; }
        public int Years { get; private set; }

        public static Duration CreateEmpty()
        {
            return new Duration(0,0,0);
        }
    }
}