using FluentDataObfuscator.Infrastructure;

namespace FluentDataObfuscator.Obfuscations
{
    public class FirstNameObfuscation : IObfuscation
    {
        public object Obfuscate()
        {
            return "FirstName";
        }
    }
}