namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public string LecturerId { get; set; }
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string AdditionalNotes { get; set; }
        public string SupportingDocumentsPath { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Status { get; set; }

        public Lecturer Lecturer { get; set; }
    }
}
