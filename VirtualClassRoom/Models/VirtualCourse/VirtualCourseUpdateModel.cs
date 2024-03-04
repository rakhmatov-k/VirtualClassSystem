using VirtualClassRoom.Models.CourseStudents;

namespace VirtualClassRoom.Models.VirtualCourse;

public class VirtualCourseUpdateModel
{
    public long CourceId { get; set; }
    public List<long> Students { get; set; } = new List<long>();
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
