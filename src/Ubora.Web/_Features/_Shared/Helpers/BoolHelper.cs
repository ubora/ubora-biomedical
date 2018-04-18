namespace Ubora.Web._Features._Shared.Helpers
{
    public static class BoolHelper
    {
        public static string ToYesNo(this bool binary)
        {
            return binary ? "Yes" : "No";
        }
    }
}
