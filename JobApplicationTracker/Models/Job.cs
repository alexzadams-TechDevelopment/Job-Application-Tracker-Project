namespace JobApplicationTracker.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string JobName { get; set; }
        public string? JobDescription { get; set; }
        public string JobStatus { get; set; }
        public DateTime? DateApplied { get; set; }
        public string? JobUrl { get; set; }
        public string Location { get; set; }
        public string? AdditionalNotes { get; set; }

        public string? UserId { get; set; }

        public Job()
        {
            
        }
    }
}
