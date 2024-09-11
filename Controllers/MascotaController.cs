using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using appdemo_prueba.Data;
using appdemo_prueba.Models;
using appdemo_prueba.ViewModel;

namespace appdemo_prueba.Controllers
{
    public class MascotaController : Controller
    {
        private readonly ILogger<MascotaController> _logger;
        private readonly ApplicationDbContext _context;

        public MascotaController(ILogger<MascotaController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var mascotas = from o in _context.DataMascota select o;
            
            var viewModel = new MascotaViewModel
            {
                FormMascota = new Mascota(),  // Objeto vacío para el formulario
                ListMascota = mascotas.ToList() // Lista de mascotas para la tabla
            };

            return View(viewModel);
        }

        public IActionResult Editar(int id)
        {
            // Buscar la mascota por su id
            var mascota = _context.DataMascota.FirstOrDefault(masc => masc.Id == id);
            
            if (mascota == null)
            {
                return NotFound();
            }

            var viewModel = new MascotaViewModel
            {
                FormMascota = mascota,
                ListMascota = _context.DataMascota.ToList()
            };

            // Retornar la vista "Index" con los datos de la mascota para editar
            return View("Index", viewModel);
        }

        [HttpPost]
        public IActionResult Guardar(MascotaViewModel viewModel)
        {
            if (viewModel.FormMascota.Id == 0)
            {
                // Crear una nueva mascota
                viewModel.FormMascota.FechaNacimiento = viewModel.FormMascota.FechaNacimiento.Date;
                _context.Add(viewModel.FormMascota);
                TempData["Message"] = "Se ha registrado una nueva mascota.";
            }
            else
            {
                // Editar la mascota existente
                var mascotaExistente = _context.DataMascota.FirstOrDefault(m => m.Id == viewModel.FormMascota.Id);
                if (mascotaExistente != null)
                {
                    mascotaExistente.Nombre = viewModel.FormMascota.Nombre;
                    mascotaExistente.Raza = viewModel.FormMascota.Raza;
                    mascotaExistente.Color = viewModel.FormMascota.Color;
                    mascotaExistente.FechaNacimiento = viewModel.FormMascota.FechaNacimiento.Date;
                    _context.Update(mascotaExistente);
                    TempData["Message"] = "Se ha actualizado la mascota.";
                }
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            var mascota = _context.DataMascota.FirstOrDefault(masc => masc.Id == id);
            if (mascota != null)
            {
                _context.Remove(mascota);
                _context.SaveChanges();
                TempData["Message"] = "Se ha eliminado la mascota.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}



