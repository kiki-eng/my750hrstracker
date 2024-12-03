using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.DTOs.Responses;
using _750HrsTracker.Filters;
using _750HrsTracker.Helpers;
using _750HrsTracker.Models.ResponseWrappers;
using _750HrsTracker.Models;
using _750HrsTracker.Providers;
using _750HrsTracker.Repositories.Interfaces;
using _750HrsTracker.Services.Interfaces;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Models.JointEntities;
using Microsoft.Extensions.Options;
using _750HrsTracker.Enums;
using System.IO;
using _750HrsTracker.Extensions;
using Microsoft.AspNetCore.StaticFiles;
using _750HrsTracker.Helpers.Constants;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Diagnostics;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.Net;
using System;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;

namespace _750HrsTracker.Services.Implementations
{
    public class ActivityLogService : BaseService, IActivityLogService
    {
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly IActivityLogActivityRepository _logActivityRepository;
        private readonly IActivityLogSubCategoryRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IActivityLogCategoryService _logCategoryService;
        private readonly AppSettings _appSettings;

        public ActivityLogService(IActivityLogRepository activityLogRepository, IMapper mapper, SessionProvider sessionProvider, 
            IUriService uriService, IUserRepository userRepository, IOptionsSnapshot<AppSettings> appSettings, 
            IActivityLogSubCategoryRepository taskRepository, IActivityLogActivityRepository logActivityRepository, 
            IPropertyRepository propertyRepository, IActivityLogCategoryService logCategoryService) : base(sessionProvider)
        {
            _activityLogRepository = activityLogRepository;
            _mapper = mapper;
            _uriService = uriService;
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
            _taskRepository = taskRepository;
            _logActivityRepository = logActivityRepository;
            _propertyRepository = propertyRepository;
            _logCategoryService = logCategoryService;
        }

        public async Task<ResponseHandler<GetActivityLogResponse>> AddActivityLogAsync(AddActivityLogRequest request)
        {
            ResponseHandler<GetActivityLogResponse> response = new();

            // validate supporting document

            // Convert Base64 string to byte array
            List<Base64FormFile> files = new();

            if(request.SupportingDocuments != null && request.SupportingDocuments!.Count > 0)
            {
                foreach (var ff in request.SupportingDocuments!)
                {
                    byte[] fileBytes = Convert.FromBase64String(ff.Data!);
                    var base64FormFile = new Base64FormFile(ff.FileName!, ff.ContentType!, fileBytes);
                    files.Add(base64FormFile);
                }
            }


            var requestData = _mapper.Map<ActivityLog>(request);
            requestData.TeamId = (Guid)Session.TeamId!;
            requestData.CreatedById = (Guid)Session.UserId!;

            var activityBy = await _userRepository.GetUserAsync(request.ActivityById);

            requestData.ActivityById = activityBy.Id;

            

            if(request.PropertyType == AvailablePropertyType.LTR && request.LogType == ActivityLogType.REAL_ESTATE)
            {
                var properties = await _propertyRepository.GetAllAsync(p => p.TeamId == (Guid)Session.TeamId && p.PropertyType == request.PropertyType);

                if (!request.PropertiesIds!.Any(p => properties.Select(pp => pp.Id).Contains(p)))
                {
                    throw new ApplicationException("Invalid property selection");
                }

                var activities = await _logActivityRepository.GetAllAsync(a => a.AvailablePropertyType == request.PropertyType);

                if (!activities.Any(a => a.Id == request.ActivityLogActivityId))
                {
                    throw new ApplicationException("Invalid activity");
                }

                if (request.TaskId == null)
                {
                    throw new ApplicationException("Task Id is required for LTR logs");
                }
                if (request.TaskId != null)
                {
                    var task = await _taskRepository.GetSingleOrDefaultAsync(t => t.Id == request.TaskId && t.LogActivityId == request.ActivityLogActivityId) 
                        ?? throw new KeyNotFoundException("Invalid task selection");
                    requestData.TaskId = task.Id;
                }
            }
            else if(request.PropertyType == AvailablePropertyType.LTR && request.LogType == ActivityLogType.NON_REAL_ESTATE)
            {
                requestData.ActivityLogActivity = null;
                requestData.LogType = ActivityLogType.NON_REAL_ESTATE;

            }
            else
            {
                requestData.LogType = ActivityLogType.NONE;
                requestData.ActivityLogCategoryId = null;
                requestData.TaskId = null;
            }

            var activityLog = await _activityLogRepository.AddAsync(requestData);

            if(request.SupportingDocuments != null && request.SupportingDocuments!.Count > 0)
            {
                foreach (var f in files)
                {
                    ActivityLogDocument? documentUploadResponse = await Storage.PrepareAndUploadDocumentAsync(f, _appSettings, (Guid)Session.TeamId, DocumentFor.ActivityLog, activityLog.Id.ToString());

                    if (documentUploadResponse != null)
                    {
                        documentUploadResponse!.ActivityLogId = activityLog.Id;
                        documentUploadResponse.TeamId = (Guid)Session.TeamId!;
                        await _activityLogRepository.AttachLogDocumentAsync(documentUploadResponse!);
                    }
                }
            }


            if(request.PropertiesIds != null && request.PropertiesIds!.Count > 0)
            {
                List<ActivityLogProperty> properties = new();
                foreach (var propertyId in request.PropertiesIds!)
                {
                    ActivityLogProperty activityLogProperty = new()
                    {
                        ActivityLogId = activityLog.Id,
                        PropertyId = propertyId,
                    };
                    properties.Add(activityLogProperty);
                }
                await _activityLogRepository.AttachLogPropertyAsync(properties);
            }


            response.Success = true;
            response.Message = "Activity log added successfully";
            response.Data = _mapper.Map<GetActivityLogResponse>(activityLog);

            return response;
        }
        
        public async Task<ResponseHandler<GetActivityLogResponse>> AddSTRActivityLogAsync(BaseAddActivityLogRequest request)
        {
            ResponseHandler<GetActivityLogResponse> response = new();

            // validate supporting document

            // Convert Base64 string to byte array
            List<Base64FormFile> files = new();

            if(request.SupportingDocuments != null && request.SupportingDocuments!.Count > 0)
            {
                foreach (var ff in request.SupportingDocuments!)
                {
                    byte[] fileBytes = Convert.FromBase64String(ff.Data!);
                    var base64FormFile = new Base64FormFile(ff.FileName!, ff.ContentType!, fileBytes);
                    files.Add(base64FormFile);
                }
            }


            var requestData = _mapper.Map<ActivityLog>(request);
            requestData.TeamId = (Guid)Session.TeamId!;
            requestData.CreatedById = (Guid)Session.UserId!;

            var activityBy = await _userRepository.GetUserAsync(request.ActivityById);

            requestData.ActivityById = activityBy.Id;

            var activities = await _logActivityRepository.GetAllAsync(a => a.AvailablePropertyType == request.PropertyType);

            if(!activities.Any(a => a.Id == request.ActivityLogActivityId))
            {
                throw new ApplicationException("Invalid activity");
            }

            requestData.LogType = ActivityLogType.NONE;
            requestData.ActivityLogCategoryId = null;
            requestData.TaskId = null;

            var activityLog = await _activityLogRepository.AddAsync(requestData);

            if(request.SupportingDocuments != null && request.SupportingDocuments!.Count > 0)
            {
                foreach (var f in files)
                {
                    ActivityLogDocument? documentUploadResponse = await Storage.PrepareAndUploadDocumentAsync(f, _appSettings, (Guid)Session.TeamId, DocumentFor.ActivityLog);

                    if (documentUploadResponse != null)
                    {
                        documentUploadResponse!.ActivityLogId = activityLog.Id;
                        documentUploadResponse.TeamId = (Guid)Session.TeamId!;
                        await _activityLogRepository.AttachLogDocumentAsync(documentUploadResponse!);
                    }
                }
            }


            List<ActivityLogProperty> properties = new(); 
            foreach(var propertyId in request.PropertiesIds!)
            {
                ActivityLogProperty activityLogProperty = new() 
                { 
                    ActivityLogId = activityLog.Id,
                    PropertyId = propertyId,
                };
                properties.Add(activityLogProperty);
            }
            await _activityLogRepository.AttachLogPropertyAsync(properties);


            response.Success = true;
            response.Message = "Activity log added successfully";
            response.Data = _mapper.Map<GetActivityLogResponse>(activityLog);

            return response;
        }


        public async Task<ResponseHandler<string>> DeleteActivityLogAsync(Guid id)
        {
            ResponseHandler<string> response = new();

            var activityLog = await _activityLogRepository.SoftDeleteAsync(p => p.Id == id && p.TeamId == Session.TeamId);

            response.Success = true;
            response.Message = "Activity log deleted successfully";

            return response;
        }

        public async Task<PagedResponseHandler<List<GetActivityLogResponse>>> GetAllActivityLogAsync(AvailablePropertyType propertyType, PaginationFilter filter, ActivityLogFilter activityLogFilter, string route)
        {
            var validFilters = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var validActivityLogFilters = new ActivityLogFilter(activityLogFilter.Activity.ToString(), activityLogFilter.Property.ToString(), activityLogFilter.Member.ToString(), 
                activityLogFilter.AllSupportingDocument, activityLogFilter.HasSupportingDocument, activityLogFilter.WithDocuments, activityLogFilter.StartDate, activityLogFilter.EndDate);

            var properties = await _activityLogRepository.GetAllLogsAsync(validFilters, validActivityLogFilters, propertyType, (Guid)Session.TeamId!);


            var pagedData = (properties.Records!.Select(sn => MappedResponse(sn))).ToList();

            PagedResponseHandler<List<GetActivityLogResponse>> response =
                PaginationHelper.CreatePagedResponse(pagedData, validFilters, properties.TotalCount, _uriService, route);

            response.Success = true;
            response.Message = "All activity logs retrieved successfully";
            return response;
        }

        public async Task<ResponseHandler<GetActivityLogResponse>> GetActivityLogAsync(Guid id)
        {
            ResponseHandler<GetActivityLogResponse> response = new();

            var activityLog = await _activityLogRepository.GetLogByIdAsync(id, (Guid)Session.TeamId!);

            if (activityLog == null)
            {
                throw new KeyNotFoundException("Activity log not found");
            };

            var responseData = MappedResponse(activityLog);


            var documents = await  _activityLogRepository.GetDocumentsAsync(activityLog.Id);
            List<Base64FileModel> supportDocuments = new();

            if (documents != null && documents.Count > 0)
            {
                foreach( var document in documents)
                {
                    byte[] fileBytes = await Storage.DownloadDocumentAsStream(_appSettings, document.RemoteDirectoryName!);

                    Base64FileModel fileModel = new()
                    {
                        ContentType = Utility.GetMimeType(document.DocumentName!),
                        FileExtension = Path.GetExtension(document.DocumentName!),
                        Data = Convert.ToBase64String(fileBytes),
                        FileName = document.DocumentName,
                        DocumentId = document.Id
                    };

                    supportDocuments.Add(fileModel);
                }

            }

            responseData.SupportingDocuments = supportDocuments;

            response.Success = true;
            response.Message = "Activity log retrieved successfully";
            response.Data = responseData;

            return response;

        }

        public async Task<ResponseHandler<List<GetActivityLogResponse>>> SearchActivityLogAsync([NotNull] string keyword, AvailablePropertyType propertyType)
        {

            ResponseHandler<List<GetActivityLogResponse>> response = new();

            var ActivityLog = await _activityLogRepository.SearchEntityAsync(p => 
                p.TeamId == Session.TeamId && 
                p.Description!.ToLower().Contains(keyword.ToLower()) && 
                p.PropertyType == propertyType);

            response.Success = true;
            response.Message = "Activity logs retrieved successfully";
            response.Data = ActivityLog.Select(p => _mapper.Map<GetActivityLogResponse>(p)).ToList();

            return response;
        }

        public async Task<ResponseHandler<GetActivityLogResponse>> UpdateActivityLogAsync(Guid id, AddActivityLogRequest request)
        {
            ResponseHandler<GetActivityLogResponse> response = new();

            var existingActivityLog = await _activityLogRepository.GetSingleOrDefaultAsync(id) ?? throw new KeyNotFoundException("Activity log not found");
           
            // Convert Base64 string to byte array
            List<Base64FormFile> files = new();

            if (request.SupportingDocuments != null && request.SupportingDocuments!.Count > 0)
            {
                foreach (var ff in request.SupportingDocuments!)
                {
                    byte[] fileBytes = Convert.FromBase64String(ff.Data!);
                    var base64FormFile = new Base64FormFile(ff.FileName!, ff.ContentType!, fileBytes);
                    files.Add(base64FormFile);
                }
            }


            var requestData = _mapper.Map<ActivityLog>(request);
            requestData.ActivityLogCategoryId = request.ActivityLogCategoryId ?? Guid.Empty;
            requestData.ActivityLogActivityId = request.ActivityLogActivityId ?? Guid.Empty;
            requestData.TeamId = (Guid)Session.TeamId!;

            var activityBy = await _userRepository.GetUserAsync(request.ActivityById);

            requestData.ActivityById = activityBy.Id;

            if (request.PropertyType == AvailablePropertyType.LTR && request.TaskId == null && request.LogType == ActivityLogType.REAL_ESTATE)
            {
                throw new ApplicationException("Task Id is required for LTR logs");
            }

            if (request.TaskId != null)
            {
                var tasks = await _taskRepository.GetAllAsync(t => t.LogActivityId == request.ActivityLogActivityId);
                var task = tasks.ToList().FirstOrDefault(t => t.Id == request.TaskId) ?? throw new KeyNotFoundException("Invalid task selection");
                requestData.TaskId = task.Id;
            }

            var activityLog = await _activityLogRepository.UpdateAsync(id, (Guid)Session.TeamId!, requestData);

            List<ActivityLogDocument> docuemnts = new();
            if (request.SupportingDocuments != null && request.SupportingDocuments!.Count > 0)
            {
                // purge old data
                await _activityLogRepository.DetachLogDocumentAsync(id);
                await Storage.RemoveDocumentsAsync(_appSettings, DocumentFor.ActivityLog, "", Session.TeamId!.ToString()!, id.ToString());


                foreach (var f in files)
                {
                    ActivityLogDocument? documentUploadResponse = await Storage.PrepareAndUploadDocumentAsync(f, _appSettings, (Guid)Session.TeamId, DocumentFor.ActivityLog);

                    if (documentUploadResponse != null)
                    {
                        documentUploadResponse!.ActivityLogId = activityLog.Id;
                        documentUploadResponse.TeamId = (Guid)Session.TeamId!;

                        docuemnts.Add(documentUploadResponse!);
                    }
                }
            }

            await _activityLogRepository.UpdateAttachedLogDocumentAsync(existingActivityLog.Id, docuemnts!);


            List<ActivityLogProperty> properties = new();
            foreach (var propertyId in request.PropertiesIds!)
            {
                ActivityLogProperty activityLogProperty = new()
                {
                    ActivityLogId = activityLog.Id,
                    PropertyId = propertyId,
                };
                properties.Add(activityLogProperty);
            }
            await _activityLogRepository.UpdateAttachedLogPropertyAsync(existingActivityLog.Id, properties);


            response.Success = true;
            response.Message = "Activity log update successfully";
            response.Data = MappedResponse(activityLog);

            return response;
        }

        public async Task<ResponseHandler<GetDashboardResponse>> GetDashboardDataAsync(AvailablePropertyType availablePropertyType)
        {
            ResponseHandler<GetDashboardResponse> response = new();
            GetDashboardResponse responseData = await _activityLogRepository.GetRecentActivityLogsAsync((Guid) Session.TeamId!, (Guid)Session.UserId!,  availablePropertyType);

            response.Success = true;
            response.Message = "Dashboard data retrieved successfully";
            response.Data = responseData;
            return response;


        }

        public async Task<ResponseHandler<Base64FileModel>> DownloadActivityLogImportTemplateAsync()
        {
            ResponseHandler<Base64FileModel> response = new();

            var directoryName = LogCategoryConstants.TemplatesDirectory + "/" + LogCategoryConstants.LogImportTemplateFileName;
            byte[] fileBytes = await Storage.DownloadDocumentAsStream(_appSettings, directoryName);

            Base64FileModel fileModel = new()
            {
                ContentType = Utility.GetMimeType(LogCategoryConstants.LogImportTemplateFileName),
                FileExtension = Path.GetExtension(LogCategoryConstants.LogImportTemplateFileName),
                Data = Convert.ToBase64String(fileBytes),
                FileName = LogCategoryConstants.LogImportTemplateFileName
            };

            response.Success = true;
            response.Message = "File retrieved successfully";
            response.Data = fileModel;
            return response;
        }

        public async Task<byte[]> ExportDocumentsAsync(ExportDocumentFilter filter)
        {
            var validFilter = new ExportDocumentFilter(filter.year!);

            var documents = await _activityLogRepository.GetDocumentsByTeamIdAsync((Guid)Session.TeamId!, validFilter);

            if (documents != null && documents.Count > 0)
            {
                using MemoryStream zipStream = new();
                using (ZipArchive archive = new(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach (var document in documents)
                    {
                        byte[] fileBytes = await Storage.DownloadDocumentAsStream(_appSettings, document.RemoteDirectoryName!);

                        if (fileBytes == null || fileBytes.Length == 0)
                        {
                            continue;
                        }
                        // Add blob content to zip file
                        ZipArchiveEntry entry = archive.CreateEntry(document.DocumentName!, CompressionLevel.Fastest);
                        using Stream entryStream = entry.Open();
                        await entryStream.WriteAsync(fileBytes, 0, fileBytes.Length);
                    }
                }

                    zipStream.Position = 0;

                byte[] zipBytes = zipStream.ToArray();

                return zipBytes;

            }
            else
            {
                throw new ApplicationException("No document to download");
            }


        }
       
        private GetActivityLogResponse MappedResponse(ActivityLog activityLog)
        {
            var response = _mapper.Map<GetActivityLogResponse>(activityLog);

            if(activityLog.ActivityLogActivity != null)
            {
                response.Activity = new GetActivityLogActivityResponse
                {
                    Name = activityLog.ActivityLogActivity.Name,
                    Id = activityLog.ActivityLogActivity.Id
                };

                //response.Activity = activityLog.ActivityLogActivity.Name;
            }
            
            if(activityLog.Task != null)
            {
                response.Task = new GetLogActivitySubCategoryResponse()
                {
                    Id = activityLog.Task.Id,
                    Name = activityLog.Task.Name,
                    Slug = activityLog.Task.Slug,
                };
            }
            
            if(activityLog.ActivityLogActivity != null && activityLog.ActivityLogActivity.ActivityLogCategory != null)
            {
                response.Category = new GetActivityLogCategoryResponse
                {
                    Name = activityLog.ActivityLogActivity.ActivityLogCategory.Name,
                    Id = activityLog.ActivityLogActivity.ActivityLogCategory.Id,
                };

                //response.Category = activityLog.ActivityLogActivity.ActivityLogCategory.Name;
            }
            
            if(activityLog.ActivityLogProperties != null && activityLog.ActivityLogProperties.Count > 0)
            {
                List<GetPropertyResponse> properties = new List<GetPropertyResponse>();

                foreach(var property in activityLog.ActivityLogProperties)
                {
                    GetPropertyResponse propertyResponse = new()
                    {
                        Name = property.Property?.Name,
                        Description = property.Property?.Description,
                        Id = property.PropertyId,
                    };

                    properties.Add(propertyResponse);
                }

                response.Properties = properties; 
            }

            return response;
        }

        public async Task<ResponseHandler<string>> ImportActivityLogAsync(AvailablePropertyType propertyType,  ImportActivityLogRequest request)
        {

            ResponseHandler<string> response = new();

            if (request.SupportingDocuments!.Count < 1 || request.SupportingDocuments!.Count > 1)
            {
                throw new ApplicationException("You can only upload one file at a time");
            }
            
            if (propertyType.Equals(AvailablePropertyType.ALL))
            {
                throw new ApplicationException("Invalid property type");
            }

            var file = request.SupportingDocuments!.First()!;
            byte[] fileBytes = Convert.FromBase64String(file.Data!);

            var logData = Utility.ParseImportedFileAsync(new MemoryStream(fileBytes));

            var errors = FileImportValidator.Validate(logData, propertyType);

            if (errors.Count > 0)
            {
                throw new RequestValidationException($"Errors importing data. {string.Join("|", errors)}");
            }

            var logActivities = await _logActivityRepository.GetAllAsync(l => l.AvailablePropertyType == propertyType);
            var logTypeAndCategories = await _logCategoryService.GetAllActivityLogCategoryAsync();



            List<ActivityLog> activityLogs = new();
          
            // fetch non real estate and real estate data 
            var realEstateOptions = logTypeAndCategories!.Data!.FirstOrDefault(a => a.LogTypeValue! == ActivityLogType.REAL_ESTATE);

            if (logData.Count < 1)
            {
                throw new ApplicationException("No activity log data to process");
            }

            for (int i = 0; i < logData.Count; i++)
            {
                ActivityLog activityLog = new();
                var data = logData[i];


                activityLog.TeamId = (Guid)Session.TeamId!;
                activityLog.CreatedById = (Guid)Session.UserId!;

                var activityBy = await _userRepository.GetUserByEmailAsync(data.TeamMemberEmail!);
                activityLog.ActivityById = activityBy.Id;

                AvailableProperty property = null;

                if (propertyType == AvailablePropertyType.LTR && data.LogType!.ToUpper() == ActivityLogType.REAL_ESTATE.ToString())
                {
                    property = await _propertyRepository.GetSingleOrDefaultAsync(p => p.Code == data.Property) ?? throw new ApplicationException($"Property on Row {i + 1} not found");

                    var isMaterial = data.Material!.ToLower() == "yes";
                    GetLogCategoryResponse categoryData = new();

                    if (isMaterial)
                    {
                        categoryData = realEstateOptions!.Categories!.FirstOrDefault(c => c.Slug == LogCategoryConstants.MaterialParticipationSlug)!;
                    }
                    else
                    {
                        categoryData = realEstateOptions!.Categories!.FirstOrDefault(c => c.Slug == LogCategoryConstants.GeneralRealEstateSlug)!;
                    }

                    // validate activity and task
                    var activity = categoryData.Activities!.FirstOrDefault(cd => cd.Slug == data.Activity)
                        ?? throw new ApplicationException($"Activity on Row {i + 1} is not valid");

                    var task = activity.Tasks!.FirstOrDefault(t => t.Slug == data.Task) ?? throw new ApplicationException($"Task on Row {i + 1} is not valid");

                    activityLog.ActivityLogCategoryId = categoryData.Id;
                    activityLog.ActivityLogActivityId = activity.Id;
                    activityLog.TaskId = task.Id;

                    //activityLog.ActivityLogProperties = new List<ActivityLogProperty>()
                    //    {
                    //        new ActivityLogProperty
                    //        {
                    //            Id = property.Id
                    //        }
                    //    };
                    activityLog.LogType = ActivityLogType.REAL_ESTATE;

                }
                else if (propertyType == AvailablePropertyType.LTR && data.LogType!.ToUpper() == ActivityLogType.NON_REAL_ESTATE.ToString())
                {
                    activityLog.LogType = ActivityLogType.NON_REAL_ESTATE;

                }
                else
                {
                    property = await _propertyRepository.GetSingleOrDefaultAsync(p => p.Code == data.Property) ?? throw new ApplicationException($"Property on Row {i + 1} not found");
                    activityLog.ActivityLogProperties = new List<ActivityLogProperty>()
                    {
                        new ActivityLogProperty
                        {
                            Id = property.Id
                        }
                    };
                    // validate activity and task
                    var activity = logActivities.FirstOrDefault(cd => cd.Slug == data.Activity)
                        ?? throw new ApplicationException($"Activity on Row {i + 1} is not valid");

                    activityLog.LogType = ActivityLogType.NONE;

                }

                string format = "d/M/yyyy";     
                DateTime dateTime;

                if (DateTime.TryParseExact(data.ActivityDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                {
                    activityLog.ActivityDate = dateTime;

                }
                else
                {
                    throw new ApplicationException($"Date on Row {i + 1} is not valid");
                }

                activityLog.PropertyType = propertyType;
                activityLog.HoursSpent = Convert.ToInt32(data.Hours);
                activityLog.MinutesSpent = Convert.ToInt32(data.Minutes);
                activityLog.SecondsSpent = Convert.ToInt32( data.Seconds);
                activityLog.Description = data.Description;

                activityLogs.Add(activityLog);

                var newLog = await _activityLogRepository.AddAsync(activityLog);


                if ((propertyType == AvailablePropertyType.LTR && data.LogType!.ToUpper() == ActivityLogType.REAL_ESTATE.ToString()) || propertyType == AvailablePropertyType.STR)
                {
                    List<ActivityLogProperty> properties = new()
                    {
                        new()
                        {
                            ActivityLogId = newLog.Id,
                            PropertyId = property!.Id,
                        } };
                    
                    await _activityLogRepository.AttachLogPropertyAsync(properties);
                }
            }



            response.Success = true;
            response.Message = $"{logData.Count} records uploaded successfully";

            return response;

        }

        public async Task<LogReportDownloadResponse> DownloadActivityLogReportAsync(AvailablePropertyType propertyType, PaginationFilter filter, ActivityLogFilter activityLogFilter)
        {
            LogReportDownloadResponse response = new();

            string? fileName;
            filter.PageNumber = 1;
            filter.PageSize = 0;

            var records = await _activityLogRepository.GetAllLogsAsync(filter, activityLogFilter, propertyType, (Guid)Session.TeamId!, true);

            var pagedData = (records.Records!.Select(sn => MappedResponse(sn))).ToList();

            fileName = $"{LogCategoryConstants.ReportDownloadFileName}_{DateTime.Now:dd-MM-yyyy}.xlsx";
            var xcel = ReportHelper.ExportActivityLogReportToExcel(pagedData, fileName);

            if (xcel.FileStatus.Equals(true))
            {
                response = xcel;
                response.FileStatus = true;
                response.FileName  = fileName;
            }
            else
                response.FileStatus = false;


            return response;
        }
    }   
}
