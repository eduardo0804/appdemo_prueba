using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using appdemo_prueba.Models;

namespace appdemo_prueba.ViewModel
{
    public class MascotaViewModel
    {
        public Mascota? FormMascota { get; set; }
        public List<Mascota>? ListMascota { get; set; }
    }
}