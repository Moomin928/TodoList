using TodoApi.Module.Labels.Dtos;

namespace TodoApi.Module.TaskItems.Dtos
{
    public class TaskItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public TaskLabelDto? Label { get; set; }
    }
}
