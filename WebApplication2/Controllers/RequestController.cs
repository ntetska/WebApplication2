using Microsoft.AspNetCore.Mvc;
using WebApplication2.Domain;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    public class RequestController
        : Controller
    {
        private IRepository<RegistrationRequest> _requestRepository;
        public RequestController(IRepository<RegistrationRequest> requestService)
        {
            _requestRepository = requestService;
        }
        public async Task<IActionResult> GetAllAsync()
        {
            return View(await _requestRepository.GetAllAsync());
        }
        [HttpGet("/Request/{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            return View(await _requestRepository.GetByIdAsync(id));
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            await _requestRepository.AddAsync(request);

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Update()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            await _requestRepository.AddAsync(request);

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            await _requestRepository.AddAsync(request);

            return RedirectToAction(nameof(Index));
        }
    }

}