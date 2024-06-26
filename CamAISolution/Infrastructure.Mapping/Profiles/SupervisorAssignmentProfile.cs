using AutoMapper;
using Core.Domain.DTO;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Interfaces.Mappings;
using Core.Domain.Interfaces.Services;

namespace Infrastructure.Mapping.Profiles;

public class SupervisorAssignmentProfile : Profile
{
    public SupervisorAssignmentProfile()
    {
        CreateMap<SupervisorAssignment, SupervisorAssignmentDto>()
            .ForMember(x => x.Incidents, opts => opts.MapFrom(assignment => new List<IncidentDto>()));
    }

    public static async Task<SupervisorAssignmentDto> ToSupervisorAssignmentDto(
        IBaseMapping mapping,
        IAccountService accountService,
        IEmployeeService employeeService,
        SupervisorAssignment supervisorAssignment
    )
    {
        var dto = mapping.Map<SupervisorAssignment, SupervisorAssignmentDto>(supervisorAssignment);
        var inCharge = supervisorAssignment.Supervisor ?? accountService.GetCurrentAccount();
        var inChargeRole = inCharge.Role;
        dto.InChargeAccount = mapping.Map<Account, AccountDto>(inCharge);
        dto.InChargeAccountId = inCharge.Id;
        dto.InChargeAccountRole = inChargeRole;
        var employee = await employeeService.GetEmployeeAccount(inCharge.Id);
        dto.InChargeEmployeeId = employee?.Id;
        return dto;
    }
}
