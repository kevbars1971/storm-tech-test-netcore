using System.Collections.Generic;
using Todo.Models.TodoItems;

namespace Todo.Models.TodoLists
{
    public class TodoListDetailViewmodel
    {
        public int TodoListId { get; }
        public string Title { get; }
        public ICollection<TodoItemSummaryViewmodel> Items { get; }
        public bool HideCompletedTasks { get; }

        public TodoListDetailViewmodel(int todoListId, string title, ICollection<TodoItemSummaryViewmodel> items, bool hideCompletedTasks)
        {
            Items = items;
            TodoListId = todoListId;
            Title = title;
            HideCompletedTasks = hideCompletedTasks;
        }
    }
}