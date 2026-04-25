using System.ComponentModel.DataAnnotations;

namespace IPValidatorAssignment.Models
{
    public class TemporalBlockRequest
    {
        // كود الدولة (مثلاً "US")
        public string CountryCode { get; set; }

        // مدة الحظر بالدقائق
        [Range(1, 1440, ErrorMessage = "Duration must be between 1 and 1440 minutes.")]
        public int DurationMinutes { get; set; }
    }
}
