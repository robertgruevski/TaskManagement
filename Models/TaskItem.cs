using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
	public class TaskItem
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Title is required")]
		[StringLength(50, ErrorMessage = "Title cannot be longer than 50 characters")]
		public string Title { get; set; }
		[StringLength(300, ErrorMessage = "Description cannot be longer than 200 characters")]
		public string Description { get; set; }
		[DataType(DataType.Date)]
		[Required(ErrorMessage = "Due date is required")]
		public DateTime DueDate { get; set; }
		public bool IsCompleted { get; set; }
	}
}
