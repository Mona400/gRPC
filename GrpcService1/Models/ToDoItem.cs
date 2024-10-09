namespace GrpcService1.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Describtion { get; set; }
        public string? ToDoStatus { get; set; } = "New";

      
    }
}
