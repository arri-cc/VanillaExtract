using FluentDataObfuscator.Infrastructure;

namespace FluentDataObfuscator.Obfuscations
{
    public class SsnObfuscation : IObfuscation
    {
        public object Obfuscate()
        {
            return "000-00-0000";
        }
    }
}