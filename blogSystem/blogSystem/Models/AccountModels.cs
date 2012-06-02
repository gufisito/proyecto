using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Web.Helpers;

namespace blogSystem.Models
{

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class PerfilModel {
        [Display(Name = "Nombre")]
        public string Nombre { get; set;}

        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Display(Name = "Descripcion de Intereses")]
        public string Intereses { get; set; }

        [Display(Name ="Avatar")]
        public string Avatar { get; set; }

        [Display(Name="Ubicación")]
        public string Ubicacion { get; set; }

        [Display(Name = "Sexo")]
        public string Sexo { get; set; }

        [Display(Name = "Estado del Usuario")]
        public string Estado { get; set; }

        linqClassDataContext ctx = new linqClassDataContext();

        public void AddPerfilInfo(PerfilModel model, string name)
        {
            Guid idUs = ctx.aspnet_Users.Where(m => m.UserName == name).Select(m => m.UserId).ToArray()[0];
            int? idPer = (from per in ctx.perfils select per.id).Max();
            int id;
            if (idPer == null)
                id = 1;
            else
            {
                id = idPer.Value;
                id += 1;
            }
            perfil nuevo = new perfil()
            {
                id = id,
                nombre = model.Nombre,
                apellido = model.Apellido,
                avatar = model.Avatar,
                ubicacion = model.Ubicacion,
                intereses = model.Intereses,
                sexo = model.Sexo,
                estado = "Activo",
                idUs = idUs
            };
            ctx.perfils.InsertOnSubmit(nuevo);
            ctx.SubmitChanges();
        }

        public perfil ViewInfo(string name)
        {
            Guid id = (from list in ctx.aspnet_Users where list.UserName == name select list.UserId).ToArray()[0];
            perfil model;
            model = (from datos in ctx.perfils where datos.idUs == id select datos).ToArray()[0];
            return model;
        }
    }
}

