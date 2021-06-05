using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationPasswordProtected.Data;
using WebApplicationPasswordProtected.Models;
using WebApplicationPasswordProtected.ViewModels;

namespace WebApplicationPasswordProtected.Controllers
{
    public class TodosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;


        public TodosController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;


        }

        // GET: Todos
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Todo.Include(f => f.Assigned);

            return View(await _context.Todo.ToListAsync());
        }

        public IActionResult Novo()
        {
           


            return View();
        }






        // GET: Todos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todo.Include("GetFiles")
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (todo == null)
            {
                return NotFound();
            }
            ViewBag.Assigned = _context.Users.ToList(); // chama dropdown usuários
            return View(todo);
        }

        // GET: Todos/Create
        public IActionResult Create()
        {
            ViewBag.Assigned = _context.Users.ToList(); // chama dropdown usuários
            
            return View();
        }

        // POST: Todos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DueDate,Summary,Assigned,Done")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Novo(FilelViewModel model)

        {
                      

            var errors = ModelState
           .Where(x => x.Value.Errors.Count > 0)
           .Select(x => new { x.Key, x.Value.Errors })
           .ToArray();

            

            if (ModelState.IsValid)

            {
                string uniqueFileName = UploadedFile(model);
                

                FileModel file = new FileModel
                {
                    Descricao = model.Descricao,
                    TaskID = model.TaskID,
                    Attachment = uniqueFileName,
                };
                _context.Add(file);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private string UploadedFile(FilelViewModel model)
        {
            string uniqueFileName = null;

            if (model.AttachmentName != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploadfile");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.AttachmentName.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.AttachmentName.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }










        // GET: Todos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todo.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return View(todo);
        }

        // POST: Todos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DueDate,Summary,Assigned,Done")] Todo todo)
        {
            if (id != todo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoExists(todo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: Todos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todo = await _context.Todo.FindAsync(id);
            _context.Todo.Remove(todo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoExists(int id)
        {
            return _context.Todo.Any(e => e.Id == id);
        }
    }
}
