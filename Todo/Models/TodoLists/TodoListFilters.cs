namespace Todo.Models.TodoLists
{
    public class TodoListFilters
    {
        public bool HideCompletedTasks { get; set; } = false;
        public bool OrderByDescendingRank { get; set; } = false;

        #region Assumption
        // The sort modification created in task 2 interferes with the rank sort so to avoid potential confusion, 
        // give the user a choice so they understand why the data is presented as it is.
        // 
        // Typically, this would probably trigger a quick discussion to decide on the desired behaviour and the
        // scope of the task would then be updated to keep the sprint on track.
        #endregion

        public bool OrderByDescendingImportance { get; set; } = true;
    }
}
