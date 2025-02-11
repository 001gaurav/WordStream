using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WordStream.web.Data;
using WordStream.web.Models.Domain;
using WordStream.web.Models.ViewModels;
using WordStream.web.Repositories;

namespace WordStream.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            ValidateAddTagRequest(addTagRequest);
            if (ModelState.IsValid == false)
            {
                return View();
            }

            // Mapping AddTagRequest to Tag domain model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };
            await tagRepository.AddAsync(tag);

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List(string? searchQuery, 
            string? sortBy, 
            string? sortDirection,
            int pageSize = 3,
            int pageNumber = 1)
        {
            var totalRecords = await tagRepository.CountAsync();
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            ViewBag.TotalPages = totalPages;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;

            if(pageNumber > totalPages)
            {
                pageNumber--;
            }
            if(pageNumber < 1)
            {
                pageNumber++;
            }

            var tags = await tagRepository.GetAllAsync(searchQuery, sortBy, sortDirection, pageSize, pageNumber);

            return View(tags);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await tagRepository.GetAsync(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            var updatedTag = await tagRepository.UpdateAsync(tag);

            if (updatedTag != null)
            {
            }
            else
            {
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedTag = await tagRepository.DeleteAsync(editTagRequest.Id);
            if (deletedTag != null)
            {
                // Show Success notification
                return RedirectToAction("List");
            }
            // Show error notification
            return RedirectToAction("List", new { editTagRequest.Id });
        }

        private void ValidateAddTagRequest(AddTagRequest request)
        {
            if (request.Name is not null && request.DisplayName is not null)
            {
                if (request.Name == request.DisplayName)
                {
                    ModelState.AddModelError("DisplayName", "Name cannot be same as DisplayName");
                }
            }

        }
    }
}