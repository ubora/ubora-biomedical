public static class StringExtensions
{
    /// <summary>
    /// Removes the "Controller" suffix.
    /// </summary>
    public static string RemoveSuffix(this string controllerName)
    {
        return controllerName.Replace("Controller", "");
    }
}