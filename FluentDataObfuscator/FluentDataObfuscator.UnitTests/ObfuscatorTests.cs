using FluentDataObfuscator.Infrastructure;
using FluentDataObfuscator.Obfuscations;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentDataObfuscator.UnitTests
{
    public class ObfuscatorTests
    {
        private Mock<ITableRepository> _repo;
        private ObfuscatorConfiguration _config;

        [SetUp]
        public void Setup()
        {
            _repo = new Mock<ITableRepository>();
            _repo.Setup(x => x.GetSelected(It.IsAny<IEnumerable<string>>()))
                .Returns(new[]
                {
                    new Table
                    {
                        Name = "Users",
                        Columns = new[]
                        {
                            new Column {Name = "Username"},
                            new Column {Name = "FirstName"}
                        }
                    }
                });

            _config = new ObfuscatorConfiguration(_repo.Object);
        }

        [Test]
        public void Configuration_Works()
        {
            _config
                .ForTable("table1")
                .WithField<FirstNameObfuscation>("field")
                .WithField<SsnObfuscation>("ssn");

            _config
                .ForTable("table2")
                .WithField<FirstNameObfuscation>("field");

            Assert.That(_config.Registrations.Count(), Is.EqualTo(2));
            Assert.That(_config.GetObfuscatorFor("table1").Obfuscations.Select(x => x.Obfuscation).First(), Is.TypeOf<FirstNameObfuscation>());
            Assert.That(_config.GetObfuscatorFor("table1").Obfuscations.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Obfuscation_Works()
        {
            _config
                .ForTable("table1")
                .WithField<FirstNameObfuscation>("field")
                .WithField<SsnObfuscation>("ssn");

            var obfuscator = _config.GetObfuscatorFor("table1");

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
            _config
                .ForTable("table1")
                .WithField<FirstNameObfuscation>("field")
                .WithField<SsnObfuscation>("ssn");

            var obfuscator = _config.GetObfuscatorFor("table1");

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

        [Test]
        public void AssertConfigurationIsValid_ThrowsException_WhenNonExistentTables()
        {
            _config.ForTable("NonExistentTable");
            _config.ForTable("ThrowsErrors");

            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                _config.AssertConfigurationIsValid();
            });

            var expectedMessage =
                "Configuration Error: The following registered tables do not exist on remote server => nonexistenttable, throwserrors";

            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void AssertConfigurationIsValid_ThrowsException_WhendNonExistentColumns()
        {
            _config.ForTable("Users")
                .WithField<FirstNameObfuscation>("NonExistentField")
                .WithField<SsnObfuscation>("SSNfield");

            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                _config.AssertConfigurationIsValid();
            });

            var expectedMessage =
                "Configuration Error: The following registered columns do not exist for table 'users' on remote server => nonexistentfield, ssnfield";

            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }
    }
}