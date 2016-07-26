namespace FluentDataObfuscator.Infrastructure
{
    public class DataGenerator : IDataGenerator
    {
        public string FirstName()
        {
            return Faker.Name.First();
        }

        public string LastName()
        {
            return Faker.Name.Last();
        }

        public string Email()
        {
            return Faker.Internet.Email();
        }

        public string Ssn()
        {
            return "000-00-0000";
        }
    }
}