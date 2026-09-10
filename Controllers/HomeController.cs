using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, IEmployeeRepository employeeRepository, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return BadRequest();
                }

                Employee employee = _employeeRepository.GetEmployee(id.Value);
                if (employee == null)
                {
                    return View("EmployeeNotFound", id.Value);
                }

                HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
                {
                    Employee = employee,
                    PageTitle = "Employee Details"
                };

                return View(homeDetailsViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the Details action.");
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string uniqueFileName = ProcessUploadedFile(model);
                    Employee newEmployee = new Employee
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Department = model.Department,
                        Photopath = uniqueFileName
                    };
                    _employeeRepository.Add(newEmployee);
                    return RedirectToAction("Details", new { id = newEmployee.Id });
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the Create action.");
                return RedirectToAction("Error");
            }
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id);
            }
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotopath = employee.Photopath
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Employee employee = _employeeRepository.GetEmployee(model.Id);
                    if (employee == null)
                    {
                        Response.StatusCode = 404;
                        return View("EmployeeNotFound", model.Id);
                    }

                    employee.Name = model.Name;
                    employee.Email = model.Email;
                    employee.Department = model.Department;
                    if (model.Photo != null)
                    {
                        if (model.ExistingPhotopath != null)
                        {
                            string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", model.ExistingPhotopath);
                            System.IO.File.Delete(filePath);
                        }
                        employee.Photopath = ProcessUploadedFile(model);
                    }

                    _employeeRepository.Update(employee);
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the Edit action.");
                return RedirectToAction("Error");
            }
        }
        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            // Implementation for processing uploaded file...
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }
            else
            {
                uniqueFileName = "noimage.jpg";
            }

            return uniqueFileName;
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id);
            }
            return View(employee);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                Employee employee = _employeeRepository.GetEmployee(id);
                if (employee == null)
                {
                    Response.StatusCode = 404;
                    return View("EmployeeNotFound", id);
                }

                // Delete the employee
                _employeeRepository.Delete(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the DeleteConfirmed action.");
                return RedirectToAction("Error");
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
