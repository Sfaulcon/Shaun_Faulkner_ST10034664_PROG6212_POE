namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Models
{
    public class ClaimsDashboardViewModel
    {
        public List<Claim> PendingClaims { get; set; }
        public List<Claim> ApprovedClaims { get; set; }
        public List<Claim> DeniedClaims { get; set; }
    }
}
