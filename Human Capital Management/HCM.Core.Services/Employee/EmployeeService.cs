namespace HCM.Core.Services.Employee
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using BCrypt.Net;
    using Common.Constants;
    using Common.Helper;
    using Common.Manager;
    using Countries;
    using Data;

    using Department;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models.ViewModels.Countries;
    using Models.ViewModels.Departments;
    using Models.ViewModels.Employees;
    using Models.ViewModels.Employees.Enum;
    using Models.ViewModels.Files;
    using Models.ViewModels.Genders;
    using Models.ViewModels.Roles;
    using System.Text;

    using Data.Models;

    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext context;
        private readonly ICountryService countryService;
        private readonly IDepartmentService departmentService;
        private readonly IEmployeeManager employeeManager;
        private readonly IMapper mapper;

        public EmployeeService(
            ApplicationDbContext context,
            IMapper mapper,
            IEmployeeManager employeeManager,
            IDepartmentService departmentService,
            ICountryService countryService)
        {
            this.context = context;
            this.mapper = mapper;
            this.employeeManager = employeeManager;
            this.departmentService = departmentService;
            this.countryService = countryService;
        }

        public async Task<EmployeeTableModel> GetEmployeeTable(int page, EmployeeQueryTableFilters query)
        {
            var employeeTable = context.Employees
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.SearchEmployeeName))
            {
                var wildcard = $"%{query.SearchEmployeeName.ToLower()}%";

                employeeTable = employeeTable
                    .Where(c => EF.Functions.Like(c.Username.ToLower(), wildcard));
            }

            if (query.GenderId > 0)
            {
                employeeTable = employeeTable.Where(e => e.GenderId == query.GenderId);
            }

            if (query.DepartmentId > 0)
            {
                employeeTable = employeeTable.Where(e => e.DepartmentId == query.DepartmentId);
            }

            if (query.PositionId > 0)
            {
                employeeTable = employeeTable.Where(e => e.PositionId == query.PositionId);
            }

            employeeTable = query.Sort switch
            {
                EmployeeTableSortEnum.FirstNameAccending => employeeTable.OrderBy(e => e.FirstName),
                EmployeeTableSortEnum.FirstNameDeccending => employeeTable.OrderByDescending(e => e.FirstName),
                EmployeeTableSortEnum.LastNameAccending => employeeTable.OrderBy(e => e.LastName),
                EmployeeTableSortEnum.LastNameDeccending => employeeTable.OrderByDescending(e => e.LastName),
                EmployeeTableSortEnum.UsernameAccending => employeeTable.OrderBy(e => e.Username),
                EmployeeTableSortEnum.UsernameDeccending => employeeTable.OrderByDescending(e => e.Username),
                EmployeeTableSortEnum.Youngest => employeeTable.OrderBy(e => e.BirthDate),
                EmployeeTableSortEnum.Oldest => employeeTable.OrderByDescending(e => e.BirthDate),
                EmployeeTableSortEnum.DepartmentNameAccending => employeeTable.OrderBy(e => e.Department.Name),
                EmployeeTableSortEnum.DepartmentDeccending => employeeTable.OrderByDescending(e => e.Department.Name),
                EmployeeTableSortEnum.PositionNameAccending => employeeTable.OrderBy(e => e.Position.Name),
                EmployeeTableSortEnum.PositionDeccending => employeeTable.OrderByDescending(e => e.Position.Name),
                _ => employeeTable
            };

            var totalPages = (int)Math.Ceiling(
                (decimal)await employeeTable.CountAsync() / ValidationConstants.PaginationConstants.ItemsPerPage);

            var mappedTable = new EmployeeTableModel();

            var employeesData = employeeTable.ProjectTo<EmployeeTableDataModel>(mapper.ConfigurationProvider);

            var pagination = await Pagination<EmployeeTableDataModel>
                .CreateAsync(employeesData, page, ValidationConstants.PaginationConstants.ItemsPerPage);

            var paginatedResult = new EmployeeTableDataModel[pagination.Count];

            for (var index = 0; index < paginatedResult.Length; index++)
            {
                var element = pagination[index];
                paginatedResult[index] = element;
            }

            mappedTable.CurrentPage = page;
            mappedTable.TotalPages = totalPages;
            mappedTable.Employees = paginatedResult;

            return mappedTable;
        }

        public async Task<ICollection<CountryViewModel>> GetCountries()
        {
            return await context.Countries
                .ProjectTo<CountryViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public async Task<ICollection<GenderViewModel>> GetGenders()
        {
            return await context.Genders
                .ProjectTo<GenderViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public Task<EmployeeCreateRequestModel> GetEmployeeCreationOptions()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateEmployee(EmployeeCreateResponseModel model)
        {
            var random = new Random();

            var employee = new Employee
            {
                FirstName = model.Firstname,
                LastName = model.Lastname,
                Email = model.Email,
                GenderId = model.GenderId,
                BirthDate = model.BirthDate,
                PhoneNumber = model.PhoneNumber,
                DepartmentId = model.DepartmentId,
                PositionId = model.PositionId,
                NationalityId = model.NationalityId,
                SeniorityId = model.SeniorityId,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "name",
                Username = model.Firstname[..2] + model.Lastname[..3] +
                           random.Next(0, 999), //Compiler suggestion replacing .SubString(0,2)
                PasswordHash = GenerateRandomPassword(random.Next(8, 15), random)
            };

            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<string> CreateFileFromEmployees([FromForm] IFormFile file)
        {
            if (file.Length == 0)
            {
                throw new ArgumentException("No data");
            }

            var getFileExtension = Path.GetExtension(file.FileName);
            var convertIntoByteArr = await FileHelper.ReadAsByteArrAsync(file);

            var factory = new CreateEmployeeFileFactory();
            var fileFromFactory = factory.CreateEmployeeFile(getFileExtension);
            var employees = fileFromFactory.ProcessFile(convertIntoByteArr);

            await context.Employees.AddRangeAsync(employees);
            await context.SaveChangesAsync();

            return "Success";
        }

        public async Task<EmployeeGetEditModel> GetEmployeeToEdit(string employeeId)
        {
            var currentUserId = employeeManager.GetUserId();

            Console.WriteLine(employeeManager.IsInRole(RolesEnum.HR));

            if (employeeId != currentUserId && employeeManager.IsInRole(RolesEnum.HR) == false)
            {
                throw new ArgumentException("Invalid user");
            }

            var findEmployee = await context.Employees.FindAsync(employeeId);

            var mappedEmployeeDetails = mapper.Map<EmployeeGetEditModel>(findEmployee);

            mappedEmployeeDetails.Deparments = await context.Departments
                .ProjectTo<DepartmentViewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            mappedEmployeeDetails.Positions =
                await departmentService.GetPositionsByDepartmentId(mappedEmployeeDetails.DepartmentId);

            mappedEmployeeDetails.Senioritys =
                await departmentService.GetSenioritiesByPositionId(mappedEmployeeDetails.PositionId);

            mappedEmployeeDetails.Nationalities = await countryService.GetCountries();

            return mappedEmployeeDetails;
        }

        public async Task<string> EditEmployee(string employeeId, EmployeeSendEditModel model)
        {
            var findEmployee = await context.Employees.FindAsync(employeeId);

            if (findEmployee == null)
            {
                throw new InvalidOperationException("An error occured, couldn't find Employee");
            }

            context.Entry(findEmployee).CurrentValues.SetValues(model);
            await context.SaveChangesAsync();
            return "Successfully edited the employee";
        }

        private string GenerateRandomPassword(int length, Random random)
        {
            var sb = new StringBuilder();

            var validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";

            for (var i = 0; i < length; i++)
            {
                var generatedChar = validChars[random.Next(0, validChars.Length)];
                sb.Append(generatedChar);
            }

            return BCrypt.HashPassword(sb.ToString());
        }
    }
}