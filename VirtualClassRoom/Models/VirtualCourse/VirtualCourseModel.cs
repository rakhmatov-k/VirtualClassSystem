using VirtualClassRoom.Models.Commons;
using VirtualClassRoom.Models.CourseStudents;

namespace VirtualClassRoom.Models.VirtualCourse;

public class VirtualCourseModel : Auditable
{
    public long CourceId { get; set; }
    public List<long> StudentsId { get; set; }= new List<long>();
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public long studentCount { get; set; }
}
