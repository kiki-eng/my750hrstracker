using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace _750HrsTracker.Filters
{
    public class ActivityLogFilter
    {
        public Guid Property {  get; set; }
        public Guid Activity {  get; set; }
        public Guid Member { get; set; }
        public bool WithDocuments {  get; set; }
        public bool AllSupportingDocument {  get; set; }
        public bool HasSupportingDocument {  get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ActivityLogFilter()
        {
            
        }

        public ActivityLogFilter(string activity, string property, string member, bool allSupportingDocument, bool hasSupportingDocument, bool withDocuments, DateTime? startDate = null, DateTime? endDate = null)
        {
            Property = string.IsNullOrEmpty(property) || !IsValidGuid(property) ? Guid.Empty : Guid.Parse(property);
            Activity = string.IsNullOrEmpty(activity) || !IsValidGuid(activity) ? Guid.Empty : Guid.Parse(activity);
            Member = string.IsNullOrEmpty(member) || !IsValidGuid(member) ? Guid.Empty : Guid.Parse(member);

            AllSupportingDocument = allSupportingDocument;
            HasSupportingDocument = hasSupportingDocument;
            WithDocuments = withDocuments;
            StartDate = startDate;
            EndDate = endDate;
        }

        private bool IsValidGuid(string value)
        {
            return Guid.TryParse(value, out var id);
        }
    }
}
