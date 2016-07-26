namespace FluentDataObfuscator.Infrastructure
{
    public interface IDataGenerator
    {
        string FirstName();
        string LastName();
        string Email();
        string Ssn();
    }
}