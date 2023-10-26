namespace HCM.Common.AutoMapper
{
    using Data.Models;

    using global::AutoMapper;

    using Models.ViewModels.Countries;
    using Models.ViewModels.Departments;
    using Models.ViewModels.Employees;
    using Models.ViewModels.Genders;
    using Models.ViewModels.Positions;
    using Models.ViewModels.Seniorities;

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
                    opt => opt.MapFrom(src => (int)Math.Abs(DateTime.UtcNow.Year - src.BirthDate.Value.Year)))
                .ForMember(dest => dest.EmployeeFirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.EmployeeLastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.EmployeeDepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.EmployeePositionName, opt => opt.MapFrom(src => src.Position.Name));

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


        }
    }
}
