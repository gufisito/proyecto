using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web;
using blogSystem;

namespace blogSystem.Models
{
    public class PublicModel
    {
        [Required]
        [Display(Name="Título")]
        public string Titulo { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Categoria")]
        public string Categoria { get; set; }

        [Required]
        [Display(Name="Contenido")]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Contenido { get; set; }

        public void AddPublicacion()
        {

        }
    }
}