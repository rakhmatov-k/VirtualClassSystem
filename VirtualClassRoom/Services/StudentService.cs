using VirtualClassRoom.Configurations;
using VirtualClassRoom.Extensions;
using VirtualClassRoom.Helpers;
using VirtualClassRoom.Interfaces;
using VirtualClassRoom.Models.Student;

namespace VirtualClassRoom.Services;

public class StudentService : IStudentService
{
    private List<StudentModel> students;

    public async ValueTask<StudentViewModel> CreateAsync(StudentCreationModel student)
    {
        students = await FileIO.ReadAsync<StudentModel>(Constantas.STUDENTS_PATH);
        var existStudent = students.FirstOrDefault(t => t.Email == student.Email);

        if (existStudent != null && existStudent.IsDeleted)
        {
            return await UpdateAsync(existStudent.Id, student.MapTo<StudentUpdateModel>(), true);
        }

        if (existStudent is not null)
            throw new Exception($"This Student is already exist with this email = {student.Email}");

        var createdStudent = students.Create(student.MapTo<StudentModel>());
        await FileIO.WriteAsync(Constantas.STUDENTS_PATH, students);

        return createdStudent.MapTo<StudentViewModel>();
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        students = await FileIO.ReadAsync<StudentModel>(Constantas.STUDENTS_PATH);
        var existStudent = students.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This Student is not found with Id = {id}");

        existStudent.IsDeleted = true;
        existStudent.DeletedAt = DateTime.UtcNow;
        await FileIO.WriteAsync(Constantas.STUDENTS_PATH, students);

        return true;
    }

    public async ValueTask<IEnumerable<StudentViewModel>> GetAllAsync()
    {
        students = await FileIO.ReadAsync<StudentModel>(Constantas.STUDENTS_PATH);

        return students.Where(t => !t.IsDeleted).MapTo<StudentViewModel>();
    }

    public async ValueTask<StudentViewModel> GetByIdAsync(long id)
    {
        students = await FileIO.ReadAsync<StudentModel>(Constantas.STUDENTS_PATH);
        var existStudent = students.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This Student is not found with Id = {id}");

        return existStudent.MapTo<StudentViewModel>();
    }

    public async ValueTask<StudentViewModel> UpdateAsync(long id, StudentUpdateModel student, bool isStudentDeleted = false)
    {
        students = await FileIO.ReadAsync<StudentModel>(Constantas.STUDENTS_PATH);
        var existStudent = new StudentModel();

        if (isStudentDeleted)
        {
            existStudent = students.FirstOrDefault(c => c.Id == id);
            existStudent.IsDeleted = false;
        }
        else
        {
            existStudent = students.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
                ?? throw new Exception($"This Student is not found with Id = {id}");
        }

        existStudent.Email = student.Email;
        existStudent.LastName = student.LastName;
        existStudent.UpdatedAt = DateTime.UtcNow;
        existStudent.Password = student.Password;
        existStudent.FirstName = student.FirstName;

        await FileIO.WriteAsync(Constantas.STUDENTS_PATH, students);

        return existStudent.MapTo<StudentViewModel>();
    }
}
