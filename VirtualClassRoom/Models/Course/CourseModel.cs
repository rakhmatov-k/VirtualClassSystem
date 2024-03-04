using VirtualClassRoom.Models.Commons;

namespace VirtualClassRoom.Models.Course;

public class CourseModel : Auditable
{
    public string CourseName { get; set; }
    public string Description { get; set; }
    public long TeacherId { get; set; }
}
