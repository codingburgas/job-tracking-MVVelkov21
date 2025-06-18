namespace JobTracking.Domain.Enums
{
    public enum ApplicationStatus
    {
        Submitted,            // Application has been sent
        ApprovedForInterview, // Candidate is approved for an interview
        Rejected              // Application has been rejected
    }
}