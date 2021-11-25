using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Practica.models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Practica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnsaladasController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EnsaladasController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        //consultar ensaladas
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select id,categoria,nombre,img,tokenimg,descrip,precio,actualizarinfo,nomsinespacio
                        from
                        platos
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult(table);
        }

        //insertar ensaladas
        [HttpPost("{categoria},{nombre},{img},{tokenimg},{descrip},{precio}")]
        public object PostEnsaladas(string categoria, string nombre, string img, string tokenimg, string descrip, int precio)
        {
            try
            {
                string query = @"insert into platos (categoria,nombre,img,tokenimg,descrip,precio,actualizarinfo,nomsinespacio)
                            values
                            (@categoria,@nombre,@img,@tokenimg,@descrip,@precio,@actualizarinfo,@nomsinespacio)
                ";
                string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {

                    using (MySqlCommand myCommand = new MySqlCommand())
                    {
                        myCommand.Connection = mycon;
                        myCommand.CommandType = CommandType.Text;
                        myCommand.CommandText = query;
                        myCommand.Parameters.AddWithValue("@categoria", categoria);
                        myCommand.Parameters.AddWithValue("@nombre", nombre);
                        myCommand.Parameters.AddWithValue("@img", "https://firebasestorage.googleapis.com/v0/b/restaurantetic21.appspot.com/o/ensaladas%2F" + img);
                        myCommand.Parameters.AddWithValue("@tokenimg", tokenimg);
                        myCommand.Parameters.AddWithValue("@descrip", descrip);
                        myCommand.Parameters.AddWithValue("@precio", precio);
                        myCommand.Parameters.AddWithValue("@actualizarinfo", "#" + Regex.Replace(nombre, @" ", "_"));
                        myCommand.Parameters.AddWithValue("@nomsinespacio", Regex.Replace(nombre, @" ", "_"));
                        try
                        {
                            mycon.Open();
                            myCommand.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            return ex;
                        }
                    }
                    return null;
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Acutalizar ensaladas
        [HttpPut]
        public JsonResult Put(Ensaladas ens)
        {
           
            string query = @"
                        update platos set 
                        nombre = @EnsaladasNombre,
                        img = @EnsaladasImagen,
                        tokenimg = @tokenimg,
                        descrip =@EnsaladasDescrip,
                        precio = @EnsaladasPrecio,
                        actualizarinfo =@EnsaladasActualizarinfo,
                        nomsinespacio =@Ensaladasnomsinespacio
                        where Id =@EnsaladasId;
                        
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@EnsaladasId", ens.id);
                    myCommand.Parameters.AddWithValue("@EnsaladasNombre", ens.nombre);
                    myCommand.Parameters.AddWithValue("@EnsaladasImagen", "https://firebasestorage.googleapis.com/v0/b/restaurantetic21.appspot.com/o/ensaladas%2F" + ens.url);
                    myCommand.Parameters.AddWithValue("@tokenimg", ens.tokenimg);
                    myCommand.Parameters.AddWithValue("@EnsaladasDescrip", ens.descrip);
                    myCommand.Parameters.AddWithValue("@EnsaladasPrecio", ens.precio);
                    myCommand.Parameters.AddWithValue("@EnsaladasActualizarinfo", "#" + Regex.Replace(ens.nombre, @" ", "_"));
                    myCommand.Parameters.AddWithValue("@Ensaladasnomsinespacio", Regex.Replace(ens.nombre, @" ", "_"));

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        //eliminar un plato de ensalada
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                        delete from platos 
                        where Id=@EnsaladaId;
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@EnsaladaId", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}
