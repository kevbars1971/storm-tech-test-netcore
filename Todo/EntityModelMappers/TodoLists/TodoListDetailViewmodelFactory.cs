using System.Linq;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoItems;
using Todo.Models.TodoLists;

namespace Todo.EntityModelMappers.TodoLists
{
    public static class TodoListDetailViewmodelFactory
    {
        public static TodoListDetailViewmodel Create(TodoList todoList, TodoListFilters filters)
        {
            var items = todoList.Items
                .OrderBy(tdl => (int)tdl.Importance)
                .Select(TodoItemSummaryViewmodelFactory.Create)
                .ToList();

            return new TodoListDetailViewmodel(todoList.TodoListId, todoList.Title, items, filters);
        }
    }
}