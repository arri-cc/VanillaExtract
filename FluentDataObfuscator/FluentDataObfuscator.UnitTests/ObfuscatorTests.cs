using FluentDataObfuscator.Infrastructure;
using FluentDataObfuscator.Obfuscations;
using NUnit.Framework;
using System.Linq;

namespace FluentDataObfuscator.UnitTests
{
    public class ObfuscatorTests
    {
        [Test]
        public void Works()
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
    }
}
