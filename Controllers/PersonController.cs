using iBudget.Business;
using iBudget.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace iBudget.Controllers
{
    public class PersonController : BaseController
    {
        private readonly PersonBusiness _business;

        public PersonController(PersonBusiness business)
        {
            _business = business;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var people = await _business.GetPeoplePagination(pageNumber);
            return View(people);
        }

        public IActionResult Create()
        {
            return View(new PersonModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(PersonModel person)
        {
            if (!ModelState.IsValid)
                return View(person);

            await _business.AddAsync(person);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            PersonModel person = await _business.GetByIdAsync(id);
            return View(person);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PersonModel person)
        {
            if (person == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return View(person);

            await _business.UpdateAsync(person);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _business.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Search(string search, int? pageNumber)
        {
            TempData[Constants.SearchBoxData] = search ?? "";
            var people = await _business.GetAllLikeAsync(search, pageNumber);
            return View(nameof(Index), people);
        }
    }
}
