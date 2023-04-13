using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoVenta.Datos;
using ProyectoVenta.Models;

namespace ProyectoVenta.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        DA_Usuario _daUsuario = new DA_Usuario();
        public IActionResult Usuarios()
        {
            return View();
        }


        [HttpGet]
        public JsonResult ListaUsuario()
        {
            List<Usuario> oLista = new List<Usuario>();
            oLista = _daUsuario.Listar();
            return Json(new { data = oLista });
        }



        [HttpPost]
        public JsonResult GuardarUsuario([FromBody] Usuario obj)
        {
            string operacion = Request.Headers["operacion"];
            bool respuesta;

            if (operacion == "crear")
            {
                respuesta = _daUsuario.Guardar(obj);
            }
            else
            {
                respuesta = _daUsuario.Editar(obj);
            }


            return Json(new { respuesta = respuesta });
        }

        [HttpDelete]
        public JsonResult EliminarUsuario(int idUsuario)
        {
            bool respuesta;
            respuesta = _daUsuario.Eliminar(idUsuario);
            return Json(new { respuesta = respuesta });
        }
    }
}
