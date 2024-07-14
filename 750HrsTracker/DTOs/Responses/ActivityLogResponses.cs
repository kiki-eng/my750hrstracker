using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.Enums;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace _750HrsTracker.DTOs.Responses
{
    public class GetActivityLogResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public GetActivityLogCategoryResponse? Category { get; set; }
        public GetActivityLogActivityResponse? Activity { get; set; }
        public GetLogActivitySubCategoryResponse? Task { get; set; }
        public DateTime ActivityDate { get; set; }
        public int HoursSpent { get; set; }
        public int MinutesSpent { get; set; }
        public int SecondsSpent { get; set; }
        public string? Description { get; set; }
        public ActivityLogType? LogType { get; set; }

        public GetUsersOnlyResponse? ActivityBy {  get; set; }
        public List<GetPropertyResponse>? Properties { get; set; }   
        public List<Base64FileModel>? SupportingDocuments { get; set; }
    }

    public class FileImportDto
    {
        public string? ActivityDate { get; set; }
        public string? Description { get; set; }
        public string? Hours { get; set; }
        public string? Minutes { get; set; }
        public string? Seconds { get; set; }
        public string? Property { get; set; }
        public string? TeamMemberEmail { get; set; }
        public string? LogType { get; set; }
        public string? Material { get; set; }
        public string? Activity { get; set; }
        public string? Task { get; set; }
    }

    public static class FileImportValidator
    {
        public static List<string> Validate(List<FileImportDto> dtos, AvailablePropertyType propertyType)
        {
            List<string> errors = new List<string>();

            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];

                // Validate Hours
                if (!IsValidInteger(dto.Hours))
                {
                    errors.Add($"Row {i + 1}: Hours must be an integer value.");
                }

                // Validate Minutes
                if (!IsValidInteger(dto.Minutes))
                {
                    errors.Add($"Row {i + 1}: Minutes must be an integer value.");
                }

                // Validate Seconds
                if (!IsValidInteger(dto.Seconds))
                {
                    errors.Add($"Row {i + 1}: Seconds must be an integer value.");
                }

                // Validate TeamMemberEmail
                if (!IsValidEmail(dto.TeamMemberEmail))
                {
                    errors.Add($"Row {i + 1}: Team member email must be a valid email address.");
                }

                // Validate ActivityDate
                if (!IsValidDate(dto.ActivityDate))
                {
                    errors.Add($"Row {i + 1}: Activity date must be a valid date.");
                }

                // Validate Log Type
                if (!IsValidLogType(dto.LogType, propertyType))
                {
                    errors.Add($"Row {i + 1}: Log Type must be REAL_ESTATE or NON_REAL_ESTATE");
                }
            }

            return errors;
        }

        private static bool IsValidInteger(string? value)
        {
            return int.TryParse(value, out _);
        }

        private static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }

        private static bool IsValidDate(string? date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return false;
            }

            return DateTime.TryParseExact(
                date,
                "d/M/yyyy", // Adjust the format according to your date format
                 CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _);
        }

        private static bool IsValidLogType(string? logType, AvailablePropertyType propertyType)
        {
            if(string.IsNullOrEmpty(logType))
            {
                return false;
            }

            if(propertyType == AvailablePropertyType.LTR &&  (logType.ToUpper() == ActivityLogType.REAL_ESTATE.ToString() || logType.ToUpper() == ActivityLogType.NON_REAL_ESTATE.ToString()))
            {
                return true;
            }

            return false;
        }
    }

}
