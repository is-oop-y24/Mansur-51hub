using System;
using System.Collections.Generic;

namespace IsuExtra.Services.OgnpService
{
    public class Institute
    {
        private readonly List<string> _isuPrefixName;

        public Institute(string name, List<string> isuPrefixName)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _isuPrefixName = isuPrefixName;
        }

        public string Name { get; }
        public IReadOnlyList<string> GetInstitutePrefixNames()
        {
            return _isuPrefixName;
        }
    }
}