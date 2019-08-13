using Todo.Data.Entities;
using Todo.Models.TodoItems;

namespace Todo.EntityModelMappers.TodoItems
{
    public class TodoItemCreateFieldsFactory
    {
        public static TodoItemCreateFields Create(TodoList todoList, string defaultResponsibleUserId, int defaultRank)
        {
            return new TodoItemCreateFields(todoList.TodoListId, todoList.Title, defaultResponsibleUserId, defaultRank);
        }
    }
}