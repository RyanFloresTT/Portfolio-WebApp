using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PortfolioWeb.Data;
using PortfolioWeb.Models;

namespace PortfolioWeb.Controllers
{
    public class BlogPostsController : Controller {
        private readonly ApplicationDbContext _context;

        public BlogPostsController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: BlogPosts
        public async Task<IActionResult> Index() {
            var blogPosts = await _context.BlogPost.Include(b => b.Project).ToListAsync();
            foreach (var blogPost in blogPosts)
            {
                blogPost.Tags = await _context.Tag.Where(t => blogPost.TagIds.Contains(t.Id)).ToListAsync();
            }
            ViewBag.Tags = new SelectList(_context.Tag, "Id", "Name");
            return View(blogPosts);
        }

        // GET: BlogPosts/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var blogPost = await _context.BlogPost
                .Include(b => b.Project)
                .Include(b => b.Tags)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null) {
                return NotFound();
            }
            ViewBag.Tags = new SelectList(_context.Tag, "Id", "Name");
            return View(blogPost);
        }

        // GET: BlogPosts/Create
        public IActionResult Create() {
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "Id", "Id");
            ViewBag.Tags = new SelectList(_context.Tag, "Id", "Name");
            ViewBag.Projects = new SelectList(_context.Project, "Id", "Name");
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Body,Summary,CreatedAt,ProjectId,TagIds")] BlogPost blogPost) {
            if (ModelState.IsValid) {
                var selectedTags = await _context.Tag.Where(tag => blogPost.TagIds.Contains(tag.Id)).ToListAsync();
                blogPost.TagIds = selectedTags.Select(tag => tag.Id).ToList();
                blogPost.Tags = await _context.Tag.Where(t => blogPost.TagIds.Contains(t.Id)).ToListAsync();
                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "Id", "Id", blogPost.ProjectId);
            ViewBag.Projects = new SelectList(_context.Project, "Id", "Name");
            ViewBag.Tags = new SelectList(_context.Tag, "Id", "Name");
            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var blogPost = await _context.BlogPost.FindAsync(id); 
            if (blogPost == null) {
                return NotFound();
            }
            ViewBag.Projects = new SelectList(_context.Project, "Id", "Name");
            ViewBag.Tags = new SelectList(_context.Tag, "Id", "Name");
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "Id", "Name", blogPost.ProjectId);
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Body,Summary,CreatedAt,ProjectId,TagIds")] BlogPost blogPost) {
            if (id != blogPost.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    blogPost.Tags = await _context.Tag.Where(t => blogPost.TagIds.Contains(t.Id)).ToListAsync();
                    _context.Update(blogPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!BlogPostExists(blogPost.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Projects = new SelectList(_context.Project, "Id", "Name");
            ViewBag.Tags = new SelectList(_context.Tag, "Id", "Name");
            ViewData["ProjectId"] = new SelectList(_context.Set<Project>(), "Id", "Id", blogPost.ProjectId);
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var blogPost = await _context.BlogPost
                .Include(b => b.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null) {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var blogPost = await _context.BlogPost.FindAsync(id);
            if (blogPost != null) {
                _context.BlogPost.Remove(blogPost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogPostExists(int id) {
            return _context.BlogPost.Any(e => e.Id == id);
        }
    }
}
