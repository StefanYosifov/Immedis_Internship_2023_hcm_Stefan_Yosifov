namespace HCM.Common.AutoMapper
{
    using Data.Models;

    using global::AutoMapper;

    using Helpers;

    using Models.ViewModels.Admin;
    using Models.ViewModels.Countries;
    using Models.ViewModels.Departments;
    using Models.ViewModels.Employees;
    using Models.ViewModels.Genders;
    using Models.ViewModels.Payments;
    using Models.ViewModels.Payments.Bonuses;
    using Models.ViewModels.Positions;
    using Models.ViewModels.Seniorities;
    using Models.ViewModels.Tasks;
    using Models.ViewModels.Tasks.Priority;
    using Models.ViewModels.Tasks.Status;


    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Department, DepartmentViewModel>()
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Position, PositionViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Seniority, SeniorityViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Country, CountryViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Gender, GenderViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Employee, EmployeeTableDataModel>()
                .ForMember(dest => dest.EmployeeUserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EmployeeAge,
                    opt => opt.MapFrom(src => Math.Abs(DateTime.UtcNow.Year - src.BirthDate!.Value.Year)))
                .ForMember(dest => dest.EmployeeFirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.EmployeeLastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.EmployeeDepartmentName, opt => opt.MapFrom(src => src.Department!.Name))
                .ForMember(dest => dest.EmployeePositionName, opt => opt.MapFrom(src => src.Position!.Name));

            CreateMap<Employee, EmployeeGetEditModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.PositionId))
                .ForMember(dest => dest.SeniorityId, opt => opt.MapFrom(src => src.SeniorityId));

            CreateMap<Department, DepartmentGetAllModel>()
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DepartmentCountry, opt => opt.MapFrom(src => src.Country.Name))
                .ForMember(dest => dest.DepartmentEmployeeCount, opt => opt
                    .MapFrom(src => src.Employees.Count(e => e.DepartmentId == src.Id)))
                .ForMember(dest => dest.DepartmentImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DepartmentTotalEmployeeCapacity,
                    opt => opt.MapFrom(src => src.MaxPeopleCount));

            CreateMap<Position, DepartmentGetPositionsModel>()
                .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.EmployeesWithPositionCount, opt => opt.MapFrom(src =>
                    src.Employees.Count(e => e.DepartmentId == src.DepartmentId && e.PositionId == src.Id)
                ));

            CreateMap<Employee, EmployeeSearchModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.AccountCreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => DateCalculator.CalculateAge(src.BirthDate)));

            CreateMap<Employee, SalaryChangeModel>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => DateCalculator.CalculateAge(src.BirthDate)))
                .ForMember(dest => dest.EmployeeFullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.CurrentSalary, opt => opt.MapFrom(src => src.Salary.SalaryAmount))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.Name))
                .ForMember(dest => dest.SeniorityName, opt => opt.MapFrom(src => src.Seniority.Name));

            CreateMap<PositionSeniority, SeniorityViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SeniorityId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Seniority!.Name));

            CreateMap<BonusesReason, BonusReasonModel>()
                .ForMember(dest => dest.ReasonId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ReasonName, opt => opt.MapFrom(src => src.Name));

            CreateMap<DeductionReason, DeductionReasonModel>()
                .ForMember(dest => dest.DeductionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DeductionReason, opt => opt.MapFrom(src => src.Name));

            CreateMap<Priority, PriorityViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PriorityName, opt => opt.MapFrom(src => src.PriorityName));

            CreateMap<Status, StatusViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.StatusName));

            CreateMap<Task, TaskModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest=>dest.EmployeeName,opt=>opt.MapFrom(src=>src.Employee.FirstName + " " +src.Employee.LastName))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.TaskName, opt => opt.MapFrom(src => src.TaskName))
                .ForMember(dest => dest.IssuerId, opt => opt.MapFrom(src => src.IssuerId))
                .ForMember(dest=>dest.IssuerName,opt=>opt.MapFrom(src=>src.Issuer.FirstName+" " + src.Issuer.LastName))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src =>
                    new PriorityViewModel
                    {
                        Id = src.PriorityId,
                        PriorityName = src.Priority.PriorityName
                    }))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                    new StatusViewModel
                    {
                        Id = src.StatusId,
                        StatusName = src.Status.StatusName
                    }));

            CreateMap<CreateTaskModel, Task>()
                .ForMember(dest => dest.TaskName, opt => opt.MapFrom(src => src.TaskName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dest => dest.PriorityId, opt => opt.MapFrom(src => src.PriorityId))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId))
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId));

            CreateMap<AuditLog, AuditLogs>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.EntityName, opt => opt.MapFrom(src => src.EntityName))
                .ForMember(dest => dest.Action, opt => opt.MapFrom(src => src.Action))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp))
                .ForMember(dest => dest.Changes, opt => opt.MapFrom(src => src.Changes));

            CreateMap<Role, RoleViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}