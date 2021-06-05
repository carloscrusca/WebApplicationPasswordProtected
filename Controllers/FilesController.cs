using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationPasswordProtected.Data;
using WebApplicationPasswordProtected.Models;
using WebApplicationPasswordProtected.ViewModels;

namespace WebApplicationPasswordProtected.Controllers
{
    public class FilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
       

        public FilesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
            
        }
      

        // GET: Files
        public async Task<IActionResult> Index()
        {
           var applicationDbContext = _context.File.Include(f => f.Todo);
           return View(await applicationDbContext.ToListAsync());

            
        }

       


        public IActionResult New(int? id)
        {
            ViewBag.TaskId = _context.Todo.ToList();
            FilelViewModel file = new FilelViewModel();
            file.TaskID = id.Value;
            return View (file);
        }
        


        /// /////// 01 Começo comentário /////
        /// 
        //// GET: Files/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var file = await _context.File
        //        .Include(f => f.Todo)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (file == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(file);
        //}

        //// GET: Files/Create
        //public IActionResult Create()
        //{
        //    ViewData["TaskID"] = new SelectList(_context.Todo, "Id", "Id");
        //    return View();
        //}

        //// POST: Files/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        /// /////// Fim comentário /////


        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> New(FilelViewModel model)
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


       

        // GET: Files/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.File.FindAsync(id);
            if (file == null)
            {
                return NotFound();
            }
            ViewData["TaskID"] = new SelectList(_context.Todo, "Id", "Id", file.TaskID);
            return View(file);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,TaskID,Attachment")] FileModel file)
        {
            if (id != file.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(file);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileExists(file.Id))
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
            ViewData["TaskID"] = new SelectList(_context.Todo, "Id", "Id", file.TaskID);
            return View(file);
        }

        // GET: Files/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.File
                .Include(f => f.Todo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var file = await _context.File.FindAsync(id);
            _context.File.Remove(file);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileExists(int id)
        {
            return _context.File.Any(e => e.Id == id);
        }
    }
}
