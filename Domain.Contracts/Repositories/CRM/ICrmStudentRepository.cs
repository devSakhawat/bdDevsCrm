//using bdDevCRM.Entities.DTOs; // Assuming DTOs are here
//using bdDevCRM.Entities.Models;
//using bdDevCRM.Repositories.Contracts;

//namespace bdDevCRM.Repositories.CRM
//{
//  public interface ICrmStudentRepository : IRepositoryBase<CrmStudent>
//  {
//    // EF Core Simple List
//    Task<IEnumerable<CrmStudent>> StudentsAsync(bool trackChanges, CancellationToken cancellationToken = default);

//    // EF Core Single
//    Task<CrmStudent?> StudentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

//    // ADO.NET Example: Complex DTO (Joining Student, Batch, Course)
//    Task<IEnumerable<StudentDetailsDto>> StudentDetailsAsync(CancellationToken cancellationToken = default);

//    // CUD
//    void CreateStudent(CrmStudent student);
//    void UpdateStudent(CrmStudent student);
//    void DeleteStudent(CrmStudent student);
//  }
//}