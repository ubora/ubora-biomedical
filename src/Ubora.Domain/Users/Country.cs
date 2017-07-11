namespace Ubora.Domain.Users
{
    public class Country
    {
        public string Code { get; private set; }

        public Country(string code)
        {
            Code = code;
        }
    }
}
