using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using blogSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace blogSystem.Models
{
    public class AddBookModel
    {
        linqClassDataContext ctx = new linqClassDataContext();

        [Display(Name = "Autor")]
        public string autor { get; set; }

        [Display(Name = "Idioma")]
        public string idioma { get; set; }

        [Display(Name = "Páginas")]
        public string paginas { get; set; }

        [Display(Name = "Tema")]
        public string tema { get; set; }

        [Display(Name = "Portada del libro")]
        public string avatar { get; set; }

        public void AddBook(string tam, string name, int idpub)
        {
            Guid idUs = (from datos in ctx.aspnet_Users where datos.UserName == name select datos.UserId).ToArray()[0];
            int? idPer = (from per in ctx.libros select per.id).Max();
            int id;
            if (idPer == null)
                id = 1;
            else
            {
                id = idPer.Value;
                id += 1;
            }
            libro book = new libro() {
                id = id,
                autor = autor,
                idioma = idioma,
                tamano = tam,
                paginas = paginas,
                tema = tema,
                avatar = avatar,
                id_publicacion = idpub
            };

            ctx.libros.InsertOnSubmit(book);
            ctx.SubmitChanges();
        }
    }
}