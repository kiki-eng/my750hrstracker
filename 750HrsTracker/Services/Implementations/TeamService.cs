using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Providers;
using _750HrsTracker.Repositories.Implementations;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Net.Mail;

namespace _750HrsTracker.Services.Implementations
{
    public class TeamService : BaseService, ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly INotificationService _notificationService;
        private readonly IUriService _uriService;

        public TeamService(ITeamRepository teamRepository, IMapper mapper, IOptionsSnapshot<AppSettings> appSettings,
            IUserRepository userRepository, SessionProvider sessionProvider, INotificationService notificationService, IUriService uriService) : base(sessionProvider)
        {
            _teamRepository = teamRepository;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _uriService = uriService;
        }
        public async Task<ResponseHandler<GetRoleResponse>> AddTeamRoleAsync(AddRoleRequest request)
        {
            ResponseHandler<GetRoleResponse> response = new();

            var permissions = request.Permissions!.Select(p => new Permission { Id = p.Id }).ToList();

            var teamRole =
                await _teamRepository.AddTeamRoleAsync((Guid)Session.TeamId!, request.RoleName!, (Guid)Session.UserId!, permissions);

            response.Success = true;
            response.Message = "Role created successfully";
            response.Data = new GetRoleResponse
            {
                Id = teamRole.Id,
                Name = teamRole.Name,
                Slug = teamRole.Slug,
                Default = teamRole.Default,
            };
            return response;
        }

        public async Task<PagedResponseHandler<List<GetRoleResponse>>> GetTeamRolesAsync(PaginationFilter filter, string route)
        {
            var teamRoles = await _teamRepository.GetTeamRolesAsync((Guid)Session.TeamId!);
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var records = teamRoles.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();

            var pagedData = records.Select(sn => new GetRoleResponse
            {
                Id = sn.Id,
                Name = sn.Name,
                Slug = sn.Slug,
                Default = sn.Default,
            }).ToList();

            var totalCount = teamRoles.Count;

            PagedResponseHandler<List<GetRoleResponse>> response =
                PaginationHelper.CreatePagedResponse(pagedData, validFilter, totalCount, _uriService, route);

            response.Success = true;
            response.Message = "Roles retrieved successfully";
            return response;
        }
        public async Task<ResponseHandler<GetRoleResponse>> UpdatetRoleAsync(Guid roleId, UpdateRoleRequest request)
        {
            ResponseHandler<GetRoleResponse> response = new();

            var teamRole =
                await _teamRepository.UpdateTeamRoleAsync((Guid)Session.TeamId!, roleId, (Guid)Session.UserId!, new Role { Name = request.RoleName });

            response.Success = true;
            response.Message = "Role updated successfully";
            response.Data = new GetRoleResponse
            {
                Id = teamRole.Id,
                Name = teamRole.Name,
                Slug = teamRole.Slug,
                Default = teamRole.Default,
            };
            return response;
        }
        public async Task<ResponseHandler<string>> DeleteRoleAsync(Guid roleId)
        {

            ResponseHandler<string> response = new();
            var deletedRole =
                await _teamRepository.DeleteRoleAsync((Guid)Session.TeamId!, roleId);

            response.Success = true;
            response.Message = "Role deleted successfully";
            return response;
        }

        public async Task<ResponseHandler<string>> UpdateRolePermissionsAsync(Guid roleId, UpdateRolePermissionsRequest request)
        {
            ResponseHandler<string> response = new ResponseHandler<string>();
            var teamRole =
                await _teamRepository.UpdateRolePermissionsAsync((Guid)Session.TeamId!, roleId, (Guid)Session.UserId!, request.Permissions!.Select(p => new Permission { Id = p.Id }).ToList());
            response.Success = true;
            response.Message = "Role permissions updated successfully";

            return response;
        }


        public async Task<ResponseHandler<string>> InviteUserAsync(InviteUserRequest request, HttpRequest httpRequest)
        {
            var response = new ResponseHandler<string>();
            var currentUserDetail = await _userRepository.GetUserAsync((Guid)Session.UserId!);

            if (currentUserDetail.Email.Trim() == request.EmailAddress!.Trim())
            {
                throw new ApplicationException("Self invitation is not allowed");
            }

            if (await _teamRepository.IsInvitedUserConfirmed(request.EmailAddress!))
            {
                throw new ApplicationException("User already confirmed invitation");
            }


            var invitationDetails = await _teamRepository.InviteUserAsync((Guid)Session.TeamId!, (Guid)Session.UserId!, request.EmailAddress!, request.RoleId);
           
            var invitationNotificationRequest = new InvitationNotificationRequest()
            {
                RecipientEmail = invitationDetails.Email,
                InvitationCode = invitationDetails.Code,
                InviterName = invitationDetails.InviterName,
                InviterEmail = invitationDetails.InviterEmail,
                TeamName = invitationDetails.TeamName,
                Origin = _appSettings.NotificationOrigin,
                OriginIpAddress = Utility.GetRequestIPAddress(httpRequest)
            };

            await _notificationService.SendInvitationNotification(invitationNotificationRequest);
            response.Success = true;
            response.Message = "User invitation sent successfully";
            return response;
        }
        public async Task<ResponseHandler<string>> ValidateInvitationAsync(ValidateInvitationRequest request)
        {
            var response = new ResponseHandler<string>();

            await _teamRepository.ValidateInvitationAsync(request!.InvitationCode!);

            response.Success = true;
            response.Message = "Invitation code validated successfully";
            return response;
        }
        public async Task<ResponseHandler<string>> CreateInvitedUserAsync(CreateInvitedUserRequest request)
        {
            var response = new ResponseHandler<string>();           

            var user = new User()
            {
                Firstname = request.FirstName,
                Lastname = request.LastName,
                PasswordHash = Encryption.HashPassword(request.Password!),
            };

            var newUser = await _teamRepository.CreateInvitedUserAsync(user, request.InvitationCode!);


            response.Success = true;
            response.Message = "User added successfully to team";
            return response;
        }

        public async Task<ResponseHandler<List<PendingUserInvitationResponse>>> GetPendingUserInvitationsAsync()
        {
            var response = new ResponseHandler<List<PendingUserInvitationResponse>>();
            var pendingInvitations = await _teamRepository.GetPendingUserInvitationsAsync((Guid)Session.TeamId!);

            response.Success = true;
            response.Message = "Pending invitations retrieved successfully";
            response.Data = pendingInvitations.Select(pi => new PendingUserInvitationResponse()
            {
                InviterEmail = pi.InviterEmail!,
                InviterName = pi.InviterName!,
                InvitedEmail = pi.Email!,
                RoleId = pi.RoleId!,
                RoleName = pi.RoleName!,
                InvitedAt = pi.CreatedAt
            }).ToList();

            return response;
        }

        public async Task<PagedResponseHandler<List<GetUserResponse>>> GetTeamUsersAsync(PaginationFilter filter, string route)
        {
            var validFilters = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var users = await _teamRepository.GetTeamUsersAsync((Guid)Session.TeamId!, filter);

            var pagedData = (users.Records!.Select(sn => _mapper.Map<GetUserResponse>(sn))).ToList();

            PagedResponseHandler<List<GetUserResponse>> response =
                PaginationHelper.CreatePagedResponse(pagedData, validFilters, users.TotalCount, _uriService, route);

            response.Success = true;
            response.Message = "All users retrieved successfully";
            return response;
        }
        
        
        public async Task<ResponseHandler<List<GetUserResponse>>> GetTeamUsersAsync()
        {
            ResponseHandler<List<GetUserResponse>> response = new();

            var users = await _teamRepository.GetTeamUsersAsync((Guid)Session.TeamId!);

            response.Success = true;
            response.Message = "All users retrieved successfully";
            response.Data = users.Select(u => _mapper.Map<GetUserResponse>(u)).ToList();
            return response;
        }

        public Task<ResponseHandler<string>> DeactivateAccountAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseHandler<GetUserResponse>> MaKeSpouseRequestAsync(MakeSpouseRequest request)
        {
            ResponseHandler<GetUserResponse> response = new();

            var teamSpouse = await _teamRepository.MakeSpouseAsync((Guid)Session.TeamId!, request.UserId);

            response.Success = true;
            response.Message = "User has been made spouse";
            response.Data = _mapper.Map<GetUserResponse>(teamSpouse);

            return response;
        }
        
        public async Task<ResponseHandler<GetUserResponse>> ActivateDeactivateUsersAsync(MakeSpouseRequest request)
        {
            ResponseHandler<GetUserResponse> response = new();

            var teamSpouse = await _teamRepository.ActivateDeactivateUsersAsync((Guid)Session.TeamId!, request.UserId);

            response.Success = true;
            response.Message = "User has been updated";
            response.Data = _mapper.Map<GetUserResponse>(teamSpouse);

            return response;
        }
    }

}
