using Microsoft.VisualBasic;
using VirtualClassRoom.Configurations;
using VirtualClassRoom.Extensions;
using VirtualClassRoom.Helpers;
using VirtualClassRoom.Interfaces;
using VirtualClassRoom.Models.Teacher;

namespace VirtualClassRoom.Services;

public class TeacherService : ITeacherService
{
    private List<TeacherModel> teachers;
    public async ValueTask<TeacherViewModel> CreateAsync(TeacherCreationModel teacher)
    {
        teachers = await FileIO.ReadAsync<TeacherModel>(Constantas.TEACHER_PATH);
        var existTeacher = teachers.FirstOrDefault(t => t.Email == teacher.Email);

        if (existTeacher != null && existTeacher.IsDeleted) 
        {
            return await UpdateAsync(existTeacher.Id, teacher.MapTo<TeacherUpdateModel>(), true);
        }

        if (existTeacher is not null)
            throw new Exception($"This Teacher is already exist with this email = {teacher.Email}");

        var createdTeacher = teachers.Create(teacher.MapTo<TeacherModel>());
        await FileIO.WriteAsync(Constantas.TEACHER_PATH, teachers);

        return createdTeacher.MapTo<TeacherViewModel>();
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        teachers = await FileIO.ReadAsync<TeacherModel>(Constantas.TEACHER_PATH);
        var existTeacher = teachers.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This Teacher is not found with Id = {id}");

        existTeacher.IsDeleted = true;
        existTeacher.DeletedAt = DateTime.UtcNow;
        await FileIO.WriteAsync(Constantas.TEACHER_PATH, teachers);

        return true;
    }

    public async ValueTask<IEnumerable<TeacherViewModel>> GetAllAsync()
    {
        teachers = await FileIO.ReadAsync<TeacherModel>(Constantas.TEACHER_PATH);

        return teachers.Where(t => !t.IsDeleted).MapTo<TeacherViewModel>();
    }

    public async ValueTask<TeacherViewModel> GetByIdAsync(long id)
    {
        teachers = await FileIO.ReadAsync<TeacherModel>(Constantas.TEACHER_PATH);
        var existTeacher = teachers.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This Teacher is not found with Id = {id}");

        return existTeacher.MapTo<TeacherViewModel>();
    }

    public async ValueTask<TeacherViewModel> UpdateAsync(long id, TeacherUpdateModel teacher, bool isTeacherDeleted = false)
    {
        teachers = await FileIO.ReadAsync<TeacherModel>(Constantas.TEACHER_PATH);
        var existTeacher = new TeacherModel();

        if (isTeacherDeleted)
        {
            existTeacher = teachers.FirstOrDefault(c => c.Id == id);
            existTeacher.IsDeleted = false;
        }
        else
        {
            existTeacher = teachers.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
                ?? throw new Exception($"This Teacher is not found with Id = {id}");
        }

        existTeacher.Email = teacher.Email;
        existTeacher.LastName = teacher.LastName;
        existTeacher.UpdatedAt = DateTime.UtcNow;
        existTeacher.Password = teacher.Password;
        existTeacher.FirstName = teacher.FirstName;
        existTeacher.Description = teacher.Description;

        await FileIO.WriteAsync(Constantas.TEACHER_PATH, teachers);

        return existTeacher.MapTo<TeacherViewModel>();
    }
}
