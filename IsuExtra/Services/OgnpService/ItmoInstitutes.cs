using System.Collections.Generic;
using System.Linq;
using Isu.Services;
using IsuExtra.Services.Tools;

namespace IsuExtra.Services.OgnpService
{
    public class ItmoInstitutes
    {
        private static readonly List<Institute> _institutes = new List<Institute>()
        {
            new Institute("School of Computer Technologies and Control", new List<string>()
            {
                "N3",
                "P3",
                "R3",
                "H3",
            }),
            new Institute("School of Biotechnology and Cryogenic Systems", new List<string>()
            {
                "W3",
            }),
            new Institute("School of Life Sciences", new List<string>()
            {
                "O3",
            }),
            new Institute("School of Physics and Engineering", new List<string>()
            {
                "B3",
                "T3",
                "L3",
                "Z3",
            }),
            new Institute("School of Translational Information Technologies", new List<string>()
            {
                "K3",
                "M3",
            }),
        };

        public static Institute GetInstituteFromPrefix(string name)
        {
            var groupName = new IsuGroupName(name);
            Institute findingInstitute = _institutes
                .FirstOrDefault(p => p.GetInstitutePrefixNames()
                    .Any(prefix => prefix.Equals(groupName.Name[..2])));
            if (findingInstitute == null)
            {
                throw new IsuExtraException("Error. Could not find prefix of group name");
            }

            return findingInstitute;
        }

        public IReadOnlyList<Institute> GetInstitutes()
        {
            return _institutes;
        }
    }
}