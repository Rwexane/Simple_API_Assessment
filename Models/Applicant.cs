namespace SimpleA.Models
{
    public class Applicant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Skill> Skills { get; set; } = new List<Skill>();
    }
}
