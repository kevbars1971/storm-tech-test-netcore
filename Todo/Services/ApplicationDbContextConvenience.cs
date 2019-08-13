using System.Collections.Generic;
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

            IEnumerable<TodoItem> sortedItems = todoList.Items;

            if (filters.HideCompletedTasks)
            {
                sortedItems = sortedItems
                    .Where(i => !i.IsDone);
            }

            sortedItems = filters.OrderByDescendingRank
                ? sortedItems.OrderByDescending(i => i.Rank)
                : sortedItems.OrderBy(i => i.Rank);

            sortedItems = filters.OrderByDescendingImportance
                ? sortedItems.OrderByDescending(i => (int) i.Importance)
                : sortedItems.OrderBy(i => (int) i.Importance);

            todoList.Items = sortedItems.ToList();

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