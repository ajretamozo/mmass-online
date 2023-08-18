using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using WebApi.Helpers;
using System.Configuration;

namespace WebApi.Entities
{
    public class Producto
    {
        public int Id_producto { get; set; }
        public Competitivo competitivo { get; set; }
        public string Desc_producto { get; set; }
        public bool Es_borrado { get; set; }
        public Tabla imagen_marca { get; set; }
        public Target_Producto target { get; set; }
        public Tipo_Producto tipo_producto { get; set; }
        public string Alias_producto { get; set; }
        public int No_vinculable { get; set; }
        public string Email { get; set; }
        public DateTime? Fecha_modificacion { get; set; }
        public string Transferido { get; set; }


        public static Producto getProducto1(DataRow item)
        {
            Producto mi = new Producto
            {
                Id_producto = int.Parse(item["id_producto"].ToString()),
                Desc_producto = item["desc_producto"].ToString(),
                Es_borrado = (item["es_borrado"].ToString() == "1"),
                Alias_producto = item["alias_producto"].ToString(),
                No_vinculable = DB.DInt(item["no_vinculable"].ToString()),
                Email = item["email"].ToString(),
                Fecha_modificacion = DB.DFecha(item["Fecha_Modificacion"].ToString())
            };
            return mi;
        }

        public static Producto getProducto2(DataRow item)
        {
            Producto mi = new Producto
            {
                Id_producto = int.Parse(item["id_producto"].ToString()),
                Desc_producto = item["desc_producto"].ToString(),
                Es_borrado = (item["Es_borrado"].ToString() == "1"),
                Alias_producto = item["alias_producto"].ToString(),
                No_vinculable = DB.DInt(item["No_vinculable"].ToString()),
                Email = item["email"].ToString(),
                Fecha_modificacion = DB.DFecha(item["fecha_modificacion"].ToString()),
                Transferido = item["transferido"].ToString()
            };
            return mi;
        }

        public static Producto getById(int Id)
        {
            int BD = int.Parse(Dg_parametro.getById(1).Valor);
            string sqlCommand = "select * from productos where id_producto = " + Id.ToString();
            Producto resultado;
            resultado = new Producto();
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                if (BD == 1)
                {
                    resultado = getProducto1(t.Rows[0]);
                }
                else
                {
                    resultado = getProducto2(t.Rows[0]);
                }
            }
            return resultado;
        }

        public static List<Producto> GetAll()
        {
            int BD = int.Parse(Dg_parametro.getById(1).Valor);
            string sqlCommand = "select * from productos p " +
                                " where p.es_borrado = 0 ";
            List<Producto> col = new List<Producto>();
            Producto elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                if (BD == 1)
                {
                    elem = getProducto1(item);
                }
                else
                {
                    elem = getProducto2(item);
                }

                elem.competitivo = new Competitivo();
                elem.imagen_marca = new Tabla();
                elem.target = new Target_Producto();
                elem.tipo_producto = new Tipo_Producto();
                col.Add(elem);
            }
            return col;
        }

        public static List<Producto> GetProductosPorAnunciante(int IdAnunciante)
        {
            string sqlCommand = "";
            int BD = int.Parse(Dg_parametro.getById(1).Valor);
            if (BD == 1)
            {
                sqlCommand = "select p.id_producto, desc_producto, p.es_borrado, p.alias_producto, p.no_vinculable, p.email, p.Fecha_modificacion, " +
                                    " c.id_competitivo, c.desc_competitivo, " +
                                    " p.id_imagen_marca, im.desc_tabla as image_marca, " +
                                    " t.id_target_producto, t.target_producto, " +
                                    " tp.id_tipo_producto, tp.desc_tipo_producto " +
                                    " from productos p " +
                                    " left outer join competitivos c on c.id_competitivo = p.id_competitivo " +
                                    " left outer join tabla im on im.id_tabla = p.id_imagen_marca " +
                                    " left outer join target_producto t on t.id_target_producto = p.id_target " +
                                    " left outer join tipo_producto tp on tp.id_tipo_producto = p.id_tipo_producto " +
                                    " where p.es_borrado = 0 " +
                                    " and exists(select* from anun_prod ap where ap.id_producto = p.id_producto and ap.id_anunciante = " + IdAnunciante.ToString() + ")";
            }
            else if (BD == 2)
            {
                sqlCommand = "select p.id_producto, desc_producto, p.es_borrado, p.alias_producto, p.no_vinculable, p.email, " +
                                    " c.id_competitivo, c.desc_competitivo, " +
                                    " p.id_imagen_marca, im.desc_tabla as image_marca, " +
                                    " t.id_target_producto, t.desc_target_producto, " +
                                    " tp.id_tipo_producto, tp.desc_tipo_producto " +
                                    " from productos p " +
                                    " left outer join competitivos c on c.id_competitivo = p.id_competitivo " +
                                    " left outer join tabla im on im.id_tabla = p.id_imagen_marca " +
                                    " left outer join target_producto t on t.id_target_producto = p.id_target " +
                                    " left outer join tipo_producto tp on tp.id_tipo_producto = p.id_tipo_producto " +
                                    " where p.es_borrado = 0 " +
                                    " and exists(select* from anun_prod ap where ap.id_producto = p.id_producto and ap.id_anunciante = " + IdAnunciante.ToString() + ")";
            }

            List<Producto> col = new List<Producto>();
            Producto elem;
            DataTable t = DB.Select(sqlCommand);

            foreach (DataRow item in t.Rows)
            {
                if (BD == 1)
                {
                    elem = getProducto1(item);
                }
                else
                {
                    elem = getProducto2(item);
                }

                elem.competitivo = new Competitivo();
                elem.competitivo.Id_competitivo = DB.DInt(item["id_competitivo"].ToString());
                elem.competitivo.Desc_competitivo = item["desc_competitivo"].ToString();

                elem.imagen_marca = new Tabla();
                elem.imagen_marca.Id_tabla = DB.DInt(item["id_imagen_marca"].ToString());
                elem.imagen_marca.Desc_tabla = item["image_marca"].ToString();

                elem.target = new Target_Producto();
                elem.target.Id_target_producto = DB.DInt(item["id_target_producto"].ToString());
                if (BD == 1)
                {
                    elem.target.Desc_target_producto = item["target_producto"].ToString();
                }
                else if (BD == 2)
                {
                    elem.target.Desc_target_producto = item["desc_target_producto"].ToString();
                }
                elem.tipo_producto = new Tipo_Producto();
                elem.tipo_producto.Id_tipo_producto = DB.DInt(item["id_tipo_producto"].ToString());
                elem.tipo_producto.Desc_tipo_producto = item["desc_tipo_producto"].ToString();

                col.Add(elem);
            }
            return col;
        }
    }
}
