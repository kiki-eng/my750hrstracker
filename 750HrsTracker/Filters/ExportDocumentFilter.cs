using System.ComponentModel.DataAnnotations;

namespace _750HrsTracker.Filters
{
    public class ExportDocumentFilter
    {
        [MaxLength(4)]
        [MinLength(4)]
        public string? year;
        public ExportDocumentFilter()
        {
            
        }

        public ExportDocumentFilter(string year)
        {
            this.year = year;
        }
    }
}
