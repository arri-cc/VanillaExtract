using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentDataObfuscator.Infrastructure
{
    public class ObfuscatorConfiguration
    {
        private readonly IList<ObfuscatorRegistration> _registrations = new List<ObfuscatorRegistration>();
        private readonly ITableRepository _tableRepository;

        public IEnumerable<ObfuscatorRegistration> Registrations => _registrations.AsEnumerable();

        public ObfuscatorConfiguration(ITableRepository repository)
        {
            _tableRepository = repository;
        }

        public IObfuscatorRegistration ForTable(string table)
        {
            _registrations.Add(new ObfuscatorRegistration(table));
            return _registrations.Last();
        }

        public Obfuscator GetObfuscatorFor(string table)
        {
            return _registrations
                .Where(x => x.Table == table.ToLower())
                .Select(x => new Obfuscator(x.Table, x.Obfuscations))
                .Single();
        }

        public bool AssertConfigurationIsValid()
        {
            var registeredTables = _registrations.Select(x => x.Table).ToList();

            var remoteTables = _tableRepository.GetSelected(registeredTables).ToList();

            var nonExistentTables = registeredTables.Except(remoteTables.Select(x => x.Name), StringComparer.InvariantCultureIgnoreCase).ToList();

            if (nonExistentTables.Any())
                throw new InvalidOperationException(
                    "Configuration Error: The following registered tables do not exist on remote server => " + string.Join(", ", nonExistentTables));

            foreach (var reg in _registrations)
            {
                var remoteColumns = remoteTables.Where(x => x.Name == reg.Table).SelectMany(x => x.Columns).Select(x => x.Name);

                var nonExistentColumns = reg.Obfuscations.Select(x => x.Field).Except(remoteColumns).ToList();

                if (nonExistentColumns.Any())
                    throw new InvalidOperationException(
                        $"Configuration Error: The following registered columns do not exist for table '{reg.Table}' on remote server => " + string.Join(", ", nonExistentColumns));
            }

            return true;
        }
    }
}