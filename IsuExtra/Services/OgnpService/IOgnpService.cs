using System.Collections.Generic;
using Isu.Services;

namespace IsuExtra.Services.OgnpService
{
    public interface IOgnpService
    {
        void AddNewOgnp(Ognp ognp);
        void EnrollStudentToOgnp(Student student, Ognp ognp, StreamNumber streamNumber);
        void RemoveStudentsEnrollmentToOgnp(Student student, Ognp ognp, StreamNumber streamNumber);
        IReadOnlyList<StudyStream> GetStudyStreams(Institute institute);
        IReadOnlyList<Student> GetStudentsFromOgnp(Institute institute, StreamNumber streamNumber);
        IReadOnlyList<Student> GetNonEnrolledStudents(Group group);
        int GetStudentOgnpsCount(int studentId);
    }
}