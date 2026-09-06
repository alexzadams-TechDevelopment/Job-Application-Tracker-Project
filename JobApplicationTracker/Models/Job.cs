namespace JobApplicationTracker.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string JobName { get; set; }
        public string JobDescription { get; set;}
        public String JobStatus { get; set; }


        public Job()
        {
            
        }
    }
}
