namespace TelnetMock.CrossCutting.Validation
{
    public static class Extensions
    {
        public static Validation<T> RequireArgument<T>(this T item, string argName)
        {
            return new Validation<T>(item, argName);
        }
    }
}
