namespace TelnetMock.CrossCutting.Validation
{
    public class Validation<T>
    {
        public T Value { get; set; }
        public string ArgName { get; set; }
        public Validation(T value, string argName)
        {
            Value = value;
            ArgName = argName;
        }
    }
}
