namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Models
{
    public class Lecturer
    {
        public int LecturerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        public ICollection<Claim> Claims { get; set; }
    }
}
