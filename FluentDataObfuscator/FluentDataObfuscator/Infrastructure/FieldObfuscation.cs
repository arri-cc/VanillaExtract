namespace FluentDataObfuscator.Infrastructure
{
    public class FieldObfuscation
    {
        public string Field { get; set; }
        public IObfuscation Obfuscation { get; set; }

        public FieldObfuscation(string field, IObfuscation obfuscation)
        {
            Field = field.ToLower();
            Obfuscation = obfuscation;
        }
    }
}