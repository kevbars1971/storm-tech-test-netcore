using System.Linq;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Models.TodoLists;

namespace Todo.Services
{
    public static class ApplicationDbContextConvenience
    {
        public static IQueryable<TodoList> RelevantTodoLists(this ApplicationDbContext dbContext, string userId)
        {
            return dbContext.TodoLists.Include(tl => tl.Owner)
                .Include(tl => tl.Items)
                .Where(tl => tl.Owner.Id == userId || tl.Items.Any(i => i.ResponsibleParty.Id == userId));
        }

        // TODO: Refactor - filters should operate on an IQueryable rather than materialised lists (let the db sort the data)
        public static TodoList FilteredTodoList(this ApplicationDbContext dbContext, int todoListId, TodoListFilters filters)
        {
            var todoList = dbContext.TodoLists
                .Include(tl => tl.Owner)
                .Include(tl => tl.Items)
                .ThenInclude(ti => ti.ResponsibleParty)
                .Single(tl => tl.TodoListId == todoListId);

            if (filters.HideCompletedTasks)
            {
                todoList.Items = todoList.Items
                    .Where(i => !i.IsDone)
                    .ToList();
            }

            if (filters.OrderByDescendingRank)
            {
                todoList.Items = todoList.Items
                    .OrderByDescending(i => i.Rank)//.ThenBy(i => i.Importance)
                    .ToList();
            }
            else
            {
                todoList.Items = todoList.Items
                    .OrderBy(i => i.Rank)//.ThenBy(i => i.Importance)
                    .ToList();
            }

            return todoList;
        }

        public static TodoList SingleTodoList(this ApplicationDbContext dbContext, int todoListId)
        {
            return dbContext.TodoLists.Include(tl => tl.Owner)
                .Include(tl => tl.Items)
                .ThenInclude(ti => ti.ResponsibleParty)
                .Single(tl => tl.TodoListId == todoListId);
        }

        public static TodoItem SingleTodoItem(this ApplicationDbContext dbContext, int todoItemId)
        {
            return dbContext.TodoItems.Include(ti => ti.TodoList).Single(ti => ti.TodoItemId == todoItemId);
        }
    }
}