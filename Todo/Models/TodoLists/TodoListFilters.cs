namespace Todo.Models.TodoLists
{
    public class TodoListFilters
    {
        public bool HideCompletedTasks { get; set; } = false;
        public bool OrderByDescendingRank { get; set; } = false;
    }
}
