using System.ComponentModel.DataAnnotations.Schema;

namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }
        public int LecturerId { get; set; }

        [ForeignKey("LecturerId")]
        public Lecturer Lecturer { get; set; }
        public string LecturerName { get; set; }
        public string LecturerEmail { get; set; }
        public double? HoursWorked { get; set; }
        public double? HourlyRate { get; set; }
        public string? AdditionalNotes { get; set; }
        public string? SupportingDocuments { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string? InvoicePath { get; set; }

    }
}
