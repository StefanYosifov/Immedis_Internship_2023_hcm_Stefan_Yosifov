namespace Huamn_Capital_Managment.Common
{
    using AutoMapper;

    using Human_Capital_Managment.Data.Models;
    using Human_Capital_Managment.ViewModels.AuthenticationViewModels;
    using Human_Capital_Managment.ViewModels.ProjectViewModels;
    using Human_Capital_Managment.ViewModels.UserDetailViewModels;

    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {

            CreateMap<RegisterViewModel, Employee>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<UserDetailsResponseModel,EmployeeDetail>()
                .ForMember(dest=>dest.EmployeeId,opt=>opt.MapFrom(src=>src.EmployeeId))
                .ForMember(dest=>dest.GenderId,opt=>opt.MapFrom(src=>src.GenderId))
                .ForMember(dest=>dest.CountryOfBirthId,opt=>opt.MapFrom(src=>src.CountryOfBirth))
                .ForMember(dest=>dest.CountryOfResidenceId,opt=>opt.MapFrom(src=>src.CountryOfResidenceId));

            CreateMap<Project, AllMyProjectsRequestViewModel>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.Employees.Count));

            CreateMap<Project, AllMyDepartmentProjectsViewModel>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }

    }
}
