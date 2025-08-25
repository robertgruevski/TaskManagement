using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
	public class TaskItem
	{
		public int Id { get; set; }
		[Required]
		public string Title { get; set; }
		public string Description { get; set; }
		[DataType(DataType.Date)]
		public DateTime DueDate { get; set; }
		public bool IsCompleted { get; set; }
	}
}
