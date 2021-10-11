using System.Collections.Generic;
using System.Linq;
using IsuExtra.Services.Tools;

namespace IsuExtra.Services.OgnpService
{
    public class Ognp
    {
        private readonly string _name;
        private readonly List<StudyStream> _streams;

        public Ognp(string name, Institute institute)
        {
            _name = name;
            Institute = institute;
            _streams = new List<StudyStream>();
        }

        public Institute Institute { get; }

        public void AddStudyStream(StudyStream stream)
        {
            if (_streams.Any(p => p.StreamName.StreamNumber.Number.Equals(stream.StreamName.StreamNumber.Number)))
            {
                throw new IsuExtraException(
                    $"Study stream with stream number {stream.StreamName.StreamNumber} already exists");
            }

            _streams.Add(stream);
        }

        public StudyStream FindStream(StreamNumber streamNumber)
        {
            return _streams.FirstOrDefault(p => p.StreamName.StreamNumber.Number.Equals(streamNumber.Number));
        }

        public bool ContainsStream(StreamNumber streamNumber)
        {
            return FindStream(streamNumber) != null;
        }

        public IReadOnlyList<StudyStream> GetStudyStreams()
        {
            return _streams;
        }
    }
}