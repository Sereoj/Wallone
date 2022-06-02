namespace Wallone.Core.Models
{
    public class Parameter
    {
        public Parameter()
        {
        }

        public Parameter(string type, string name, string value)
        {
            Type = type;
            Name = name;
            Value = value;
        }

        public Parameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }
}