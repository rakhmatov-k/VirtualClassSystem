using VirtualClassRoom.Models.Teacher;

namespace VirtualClassRoom.Interfaces;

public interface ITeacherService
{
    ValueTask<TeacherViewModel> CreateAsync(TeacherCreationModel teacher);
    ValueTask<TeacherViewModel> UpdateAsync(long id, TeacherUpdateModel teacher, bool isTeacherDeleted = false);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<TeacherViewModel> GetByIdAsync(long id);
    ValueTask<IEnumerable<TeacherViewModel>> GetAllAsync();
}
