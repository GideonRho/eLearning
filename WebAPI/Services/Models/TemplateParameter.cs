namespace WebAPI.Services.Models
{
    public class TemplateParameter
    {
        
        public string Variable { get; }
        public string Value { get; }

        public TemplateParameter(string variable, string value)
        {
            Variable = variable;
            Value = value;
        }

        public string FormattedVariable => "{" + Variable + "}";

    }
}