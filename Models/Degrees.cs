namespace ResumeApp.Models
{
    public class Degrees
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.Now;

        public ICollection<Candidates>? Candidates { get; set; }
    }
}
