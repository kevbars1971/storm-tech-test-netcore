﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Data.Entities;
using Todo.EntityModelMappers.TodoLists;
using Todo.Models.TodoLists;
using Todo.Services;

namespace Todo.Controllers
{
    [Authorize]
    public class TodoListController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IUserStore<IdentityUser> userStore;

        public TodoListController(ApplicationDbContext dbContext, IUserStore<IdentityUser> userStore)
        {
            this.dbContext = dbContext;
            this.userStore = userStore;
        }

        public IActionResult Index()
        {
            var userId = User.Id();
            var todoLists = dbContext.RelevantTodoLists(userId);
            var viewmodel = TodoListIndexViewmodelFactory.Create(todoLists);
            return View(viewmodel);
        }

        public IActionResult Detail(int todoListId)
        {
            var todoList = dbContext.SingleTodoList(todoListId);
            var viewmodel = TodoListDetailViewmodelFactory.Create(todoList, new TodoListFilters());
            return View(viewmodel);
        }

        [HttpGet]
        public IActionResult FilterTodoItems([FromQuery]int todoListId, [FromQuery]bool hideCompletedTasks, [FromQuery]bool orderByDecendingRank)
        {
            var todoList = dbContext.SingleTodoList(todoListId);

            if (hideCompletedTasks)
            {
                // Normally I'd filter these in the repository or service by modifying the IQueryable
                todoList.Items = todoList.Items
                    .Where(i => !i.IsDone)
                    .ToList();
            }

            if (orderByDecendingRank)
            {
                todoList.Items = todoList.Items
                    .OrderByDescending(i => i.Rank)
                    .ToList();
            }
            else
            {
                todoList.Items = todoList.Items
                    .OrderBy(i => i.Rank)
                    .ToList();
            }

            var filters = new TodoListFilters
            {
                HideCompletedTasks = hideCompletedTasks,
                OrderByDescendingRank = orderByDecendingRank
            };

            var viewmodel = TodoListDetailViewmodelFactory.Create(todoList, filters);
            return View("Detail", viewmodel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TodoListFields());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoListFields fields)
        {
            if (!ModelState.IsValid) { return View(fields); }

            var currentUser = await userStore.FindByIdAsync(User.Id(), CancellationToken.None);

            var todoList = new TodoList(currentUser, fields.Title);

            await dbContext.AddAsync(todoList);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Create", "TodoItem", new { todoList.TodoListId });
        }
    }
}