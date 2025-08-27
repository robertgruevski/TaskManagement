using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
	public class TaskItem
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Title is required.")]
		[StringLength(50, ErrorMessage = "Title cannot be longer than 50 characters.")]
		public string Title { get; set; } = string.Empty;
		[StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters.")]
		public string Description { get; set; } = string.Empty;
		[DisplayName("Due Date")]
		[DataType(DataType.Date)]
		[Required(ErrorMessage = "Due date is required.")]
		public DateTime DueDate { get; set; }
		[DisplayName("Complete")]
		public bool IsCompleted { get; set; }
	}
}
