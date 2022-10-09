using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoNETCORE.Models;
using System.Data;

namespace ProyectoNETCORE.Controllers
{
    public class TrabajadorController : Controller
    {
        private readonly TrabajadoresPruebaContext _context;

        public TrabajadorController(TrabajadoresPruebaContext context)
        {
            _context = context;
        }

        public IActionResult lstTrabajador(String mensaje, Int16? identificador, string sexo)
        {
            ViewBag.mensaje = mensaje;
            ViewBag.identificador = identificador;
            
            SqlConnection cn = (SqlConnection)_context.Database.GetDbConnection();
            SqlCommand cmd = new SqlCommand("usp_listarTrabajadores", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            List<Trabajadores> lista = new List<Trabajadores>();

            while (dr.Read())
            {
                Trabajadores t = new Trabajadores();
                t.Id = Convert.ToInt32(dr["Id"]);
                t.TipoDocumento = dr["TipoDocumento"].ToString();
                t.NumeroDocumento = dr["NumeroDocumento"].ToString();
                t.Nombres = dr["Nombres"].ToString();
                t.Sexo = dr["Sexo"].ToString();

                Departamento dep = new Departamento();
                dep.NombreDepartamento = dr["NombreDepartamento"].ToString();
                t.IdDepartamentoNavigation = dep;

                Provincia prov = new Provincia();
                prov.NombreProvincia = dr["NombreProvincia"].ToString();
                t.IdProvinciaNavigation = prov;

                Distrito dis = new Distrito();
                dis.NombreDistrito = dr["NombreDistrito"].ToString();
                t.IdDistritoNavigation = dis;
                lista.Add(t);

            }

            if (!string.IsNullOrEmpty(sexo))
            {
                lista = lista.Where(x => x.Sexo == sexo).ToList();
            }
            cn.Close();
            
            return View(lista);
        }

        [HttpGet]
        public IActionResult Insertar()
        {
            Trabajadores t = new Trabajadores();

            List<Departamento> lista = new List<Departamento>();
            lista = _context.Departamentos.ToList();
            ViewBag.Departamentos = new SelectList(lista,"Id","NombreDepartamento");

            return PartialView("_InsertarPartial",t);
        }
        [HttpPost]
        public IActionResult Insertar(Trabajadores t)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SqlConnection cn = (SqlConnection)_context.Database.GetDbConnection();
                    SqlCommand cmd = new SqlCommand("usp_crearTrabajador", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tipodoc", t.TipoDocumento);
                    cmd.Parameters.AddWithValue("@numdoc", t.NumeroDocumento);
                    cmd.Parameters.AddWithValue("@nombres", t.Nombres);
                    cmd.Parameters.AddWithValue("@sexo", t.Sexo);
                    cmd.Parameters.AddWithValue("@idDep", t.IdDepartamento);
                    cmd.Parameters.AddWithValue("@idProv", t.IdProvincia);
                    cmd.Parameters.AddWithValue("@idDis", t.IdDistrito);
                    cn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return RedirectToAction("lstTrabajador", new { mensaje = "Trabajador agregado", identificador = 1 });
                }
                else
                {
                    return RedirectToAction("lstTrabajador", new { mensaje = "Modelo no valido", identificador = 2 });
                }
            }
            catch (Exception x)
            {
                return RedirectToAction("lstTrabajador", new { mensaje = x.Message, identificador = 2 });
            }
        }

        [HttpPost]
        public JsonResult LlenarProvJSON(string iddepat)
        {
            var Lista = _context.Provincia.Where(id => id.IdDepartamento == Convert.ToInt32(iddepat)).ToList();
            return Json(new SelectList(Lista, "Id", "NombreProvincia"));
        }

        [HttpPost]
        public JsonResult LlenarDisJSON(string iddis)
        {
            var Lista = _context.Distritos.Where(id => id.IdProvincia == Convert.ToInt32(iddis)).ToList();
            return Json(new SelectList(Lista, "Id", "NombreDistrito"));
        }

        [HttpPost]
        public IActionResult Eliminar(IFormCollection form)
        {
            try
            {
                int idtrabajador = Convert.ToInt32(form["IdTrabajador"]);
                SqlConnection cn = (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = new SqlCommand("usp_eliminarTrabajador", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", idtrabajador);
                cn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.ExecuteNonQuery();
                cn.Close();
                return RedirectToAction("lstTrabajador", new { mensaje = "Trabajador Eliminado", identificador = 1 });
                
            }
            catch (Exception x)
            {
                return RedirectToAction("lstTrabajador", new { mensaje = x.Message, identificador = 2 });
            }
        }

        [HttpGet]
        public IActionResult Actualizar(int Id)
        {
            var trabajador = _context.Trabajadores.Where(id => id.Id == Id).FirstOrDefault();

            List<Departamento> lista = new List<Departamento>();
            lista = _context.Departamentos.ToList();
            ViewBag.Departamentos = new SelectList(lista, "Id", "NombreDepartamento");

            ViewBag.IdProvincia = new SelectList(_context.Provincia.ToList(), "Id", "NombreProvincia", trabajador.IdProvincia);
            ViewBag.IdDistrito = new SelectList(_context.Distritos.ToList(), "Id", "NombreDistrito", trabajador.IdDistrito);
            ViewBag.Sexo = trabajador.Sexo;
            return PartialView("_ActualizarPartial", trabajador);
        }

        [HttpPost]
        public IActionResult Actualizar(Trabajadores t)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SqlConnection cn = (SqlConnection)_context.Database.GetDbConnection();
                    SqlCommand cmd = new SqlCommand("usp_actualizarTrabajador", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", t.Id);
                    cmd.Parameters.AddWithValue("@tipodoc", t.TipoDocumento);
                    cmd.Parameters.AddWithValue("@numdoc", t.NumeroDocumento);
                    cmd.Parameters.AddWithValue("@nombres", t.Nombres);
                    cmd.Parameters.AddWithValue("@sexo", t.Sexo);
                    cmd.Parameters.AddWithValue("@idDep", t.IdDepartamento);
                    cmd.Parameters.AddWithValue("@idProv", t.IdProvincia);
                    cmd.Parameters.AddWithValue("@idDis", t.IdDistrito);
                    cn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return RedirectToAction("lstTrabajador", new { mensaje = "Trabajador actualizado", identificador = 1 });
                }
                else
                {
                    return RedirectToAction("lstTrabajador", new { mensaje = "Modelo no valido", identificador = 2 });
                }
            }
            catch (Exception x)
            {
                return RedirectToAction("lstTrabajador", new { mensaje = x.Message, identificador = 2 });
            }
        }
    }
}
