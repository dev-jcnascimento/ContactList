namespace ContactList.Core.Arguments.Contact
{
    public class ConvertToParameters
    {
        public string Parameter { get; set; }

        public string Value { get; set; }
        public ConvertToParameters(string param, string value)
        {
            Parameter = param;
            Value = value;
        }
    }
}
