using FluentDataObfuscator.Infrastructure;
using FluentDataObfuscator.Obfuscations;
using NUnit.Framework;
using System.Linq;

namespace FluentDataObfuscator.UnitTests
{
    public class ObfuscatorTests
    {
        [Test]
        public void Configuration_Works()
        {
            var config = new ObfuscatorConfiguration();

            config
                .ForTable("table1")
                .WithField<FirstNameObfuscation>("field")
                .WithField<SsnObfuscation>("ssn");

            config
                .ForTable("table2")
                .WithField<FirstNameObfuscation>("field");

            Assert.That(config.Registrations.Count(), Is.EqualTo(2));
            Assert.That(config.GetObfuscatorFor("table1").Obfuscations.Select(x => x.Obfuscation).First(), Is.TypeOf<FirstNameObfuscation>());
            Assert.That(config.GetObfuscatorFor("table1").Obfuscations.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Obfuscation_Works()
        {
            var config = new ObfuscatorConfiguration();

            config
                .ForTable("table1")
                .WithField<FirstNameObfuscation>("field")
                .WithField<SsnObfuscation>("ssn");

            var obfuscator = config.GetObfuscatorFor("table1");

            var input = new
            {
                Field = "field",
                Ssn = "ssn"
            };

            var output = obfuscator.Obfuscate(input);

            Assert.That(output.ContainsKey("field"), Is.True);
            Assert.That(output["field"], Is.EqualTo("FirstName"));
            Assert.That(output.ContainsKey("ssn"), Is.True);
            Assert.That(output["ssn"], Is.EqualTo("000-00-0000"));
        }

        [Test]
        public void Obfuscation_WithMissingField_Works()
        {
            var config = new ObfuscatorConfiguration();

            config
                .ForTable("table1")
                .WithField<FirstNameObfuscation>("field")
                .WithField<SsnObfuscation>("ssn");

            var obfuscator = config.GetObfuscatorFor("table1");

            var input = new
            {
                Field = "field",
                Ssn = "ssn",
                FullName = "full name"
            };

            var output = obfuscator.Obfuscate(input);

            Assert.That(output.ContainsKey("fullname"), Is.True);
            Assert.That(output["fullname"], Is.EqualTo("full name"));
        }
    }
}
