using VirtualClassRoom.Models.Student;

namespace VirtualClassRoom.Interfaces;

public interface IStudentService
{
    ValueTask<StudentViewModel> CreateAsync(StudentCreationModel student);
    ValueTask<StudentViewModel> UpdateAsync(long id, StudentUpdateModel student, bool isStudentDeleted = false);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<StudentViewModel> GetByIdAsync(long id);
    ValueTask<IEnumerable<StudentViewModel>> GetAllAsync();
}
