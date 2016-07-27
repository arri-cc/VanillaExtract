namespace FluentDataObfuscator.Infrastructure
{
    public interface IObfuscatorRegistration
    {
        IObfuscatorRegistration WithField<TObfuscation>(string field) where TObfuscation : IObfuscation;
    }
}