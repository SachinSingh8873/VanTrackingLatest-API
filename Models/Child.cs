namespace VanDriverAPI.Models
{
    public class Child
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; } = "Not Picked";
        public string ParentPhone { get; set; }
    }
}
