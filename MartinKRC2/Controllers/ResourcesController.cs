using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MartinKRC2.Data;
using MartinKRC2.Models;
using MartinKRC2.Models.ResourcesViewModels;

namespace MartinKRC2.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResourcesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Resources
        public async Task<IActionResult> Index()
        {
            return View(await _context.Resource.ToListAsync());
        }

        // GET: Resources/Create
        public async Task<IActionResult> Create()
        {
            var resourceGroups = new List<SelectListItem>();
            foreach (var item in await _context.ResourceGroup.ToListAsync())
            {
                resourceGroups.Add(new SelectListItem() { Text = item.Title, Value = item.Id.ToString() });
            }

            var vm = new CreateViewModel() {
                Resource = new Resource(),
                ResourceGroups = resourceGroups
            };

            return View(vm);
        }

        // POST: Resources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,FAIconClass,ResourceGroupId,ShortUrl,TargetUrl,Title,VisibleOnSite")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resource);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(resource);
        }

        // GET: Resources/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resource = await _context.Resource.SingleOrDefaultAsync(m => m.Id == id);
            if (resource == null)
            {
                return NotFound();
            }

            var resourceGroups = new List<SelectListItem>();
            foreach (var item in await _context.ResourceGroup.ToListAsync())
            {
                var selectListItem = new SelectListItem()
                {
                    Text = item.Title,
                    Value = item.Id.ToString(),
                    Selected = (item.Id == resource.ResourceGroupId)
                };
                resourceGroups.Add(selectListItem);
            }

            var vm = new EditViewModel()
            {
                Resource = resource,
                ResourceGroups = resourceGroups
            };

            return View(vm);
        }

        // POST: Resources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,FAIconClass,ResourceGroupId,ShortUrl,TargetUrl,Title,VisibleOnSite")] Resource resource)
        {
            if (id != resource.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResourceExists(resource.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(resource);
        }

        // GET: Resources/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resource = await _context.Resource.SingleOrDefaultAsync(m => m.Id == id);
            if (resource == null)
            {
                return NotFound();
            }

            return View(resource);
        }

        // POST: Resources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resource = await _context.Resource.SingleOrDefaultAsync(m => m.Id == id);
            _context.Resource.Remove(resource);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ResourceExists(int id)
        {
            return _context.Resource.Any(e => e.Id == id);
        }
    }
}
