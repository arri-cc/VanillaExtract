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
                .WithField("field", ObfuscationType.FirstName)
                .WithField("ssn", ObfuscationType.Ssn);

            config
                .ForTable("table2")
                .WithField("field", ObfuscationType.FirstName);

            Assert.That(config.Obfuscators.Count(), Is.EqualTo(2));
            Assert.That(config.Obfuscators.First().Obfuscations["field"], Is.TypeOf<FirstNameObfuscation>());
            Assert.That(config.Obfuscators.First().Obfuscations.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Obfuscation_Works()
        {
            var config = new ObfuscatorConfiguration();

            config
                .ForTable("table1")
                .WithField("field", ObfuscationType.FirstName)
                .WithField("ssn", ObfuscationType.Ssn);

            var obfuscator = config.Obfuscators.First(x => x.Table == "table1");

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
                .WithField("field", ObfuscationType.FirstName)
                .WithField("ssn", ObfuscationType.Ssn);

            var obfuscator = config.Obfuscators.First(x => x.Table == "table1");

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
