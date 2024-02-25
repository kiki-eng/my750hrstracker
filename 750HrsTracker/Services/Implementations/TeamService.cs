using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Providers;
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

        public TeamService(ITeamRepository teamRepository,IMapper mapper, IOptionsSnapshot<AppSettings> appSettings,
            IUserRepository userRepository, SessionProvider sessionProvider, INotificationService notificationService) : base(sessionProvider)
        {
            _teamRepository = teamRepository;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            _notificationService = notificationService;
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
            string dataToEncrypt = $"{invitationDetails.Email}|{invitationDetails.Code}";
            var encryptedCode = Encryption.Base64EncodeDecode(dataToEncrypt);
            var invitationNotificationRequest = new InvitationNotificationRequest()
            {
                RecipientEmail = invitationDetails.Email,
                InvitationCode = encryptedCode,
                InviterName = invitationDetails.InviterName,
                InviterEmail = invitationDetails.InviterEmail,
                InvitationLink = string.Concat(_appSettings.AppBaseUrl, $"/user-invite/{encryptedCode}"),
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
            string decrypted = Encryption.Base64EncodeDecode(request.InvitationCode!, "decode")!;
            string[] splitted = decrypted!.Split('|');
            string inviteeEmail = splitted[0];
            string invitationCode = splitted[1];

            await _teamRepository.ValidateInvitationAsync(inviteeEmail, invitationCode);

            response.Success = true;
            response.Message = "Invitation code validation successfully";
            return response;
        }
        public async Task<ResponseHandler<string>> CreateInvitedUserAsync(CreateInvitedUserRequest request)
        {
            var response = new ResponseHandler<string>();
            string decrypted = Encryption.Base64EncodeDecode(request.InvitationCode!, "decode")!;

            string[] splitted = decrypted.Split('|');

            string inviteeEmail = splitted[0];
            string invitationCode = splitted[1];

            var address = new MailAddress(inviteeEmail);
            var user = new User()
            {
                Firstname = request.FirstName,
                Lastname = request.LastName,
                Email = inviteeEmail,
                UserName = $"{address.User}_{Utility.RandomString(4)}",
                PasswordHash = Encryption.HashPassword(request.Password!),
            };

            var newUser = await _teamRepository.CreateInvitedUserAsync(user, invitationCode);


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
    }

}
