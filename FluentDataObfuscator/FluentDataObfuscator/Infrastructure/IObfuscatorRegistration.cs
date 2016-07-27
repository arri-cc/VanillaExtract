namespace FluentDataObfuscator.Infrastructure
{
    public interface IObfuscatorRegistration
    {
        IObfuscatorRegistration WithField(string field, ObfuscationType type);
    }
}