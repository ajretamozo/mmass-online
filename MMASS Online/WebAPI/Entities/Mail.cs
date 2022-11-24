using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using WebApi.Helpers;

namespace WebApi.Entities
{
    public class Mail
    {
        public int IdMail { get; set; }
        public string DirMail { get; set; }
        public string Pass { get; set; }
        public string Servidor { get; set; }
        public string Puerto { get; set; }
        public string Nombre { get; set; }


        public static Mail getMail(DataRow item)
        {
            Mail mi = new Mail();
            mi.IdMail = DB.DInt(item["idmsgcta"].ToString());
            mi.DirMail = item["msgfromaddr"].ToString();
            mi.Pass = item["msgpass"].ToString();
            mi.Servidor = item["msgserver"].ToString();
            mi.Puerto = item["msgport"].ToString(); 
            mi.Nombre = item["msgfromname"].ToString();
            return mi;
        }

        public static Mail getMailCta()
        {
            string sqlCommand = " SELECT * FROM dg_msgcta";

            Mail resultado = new Mail(); ;
            DataTable t = DB.Select(sqlCommand);

            if (t.Rows.Count == 1)
            {
                resultado = getMail(t.Rows[0]);
            }
            return resultado;
        }

        public bool saveMailCta()
        {
            bool respuesta = true;

            string sql = "";

            if (IdMail == 0)
            {
                sql = @"INSERT INTO dg_msgcta (descmsgcta, msgserver, msguser, msgpass, msgport, msgauth, msgfromname, msgfromaddr,
                        msgempresa, msggeneral, msgcertifica, msgfactura, borrado, msgportPOP3, msghostPOP3) 
                        VALUES ('Alerta MMASS Online', @servidor, 'mmassOnline', @pass, @puerto, 'atLogin', 'MMASS Online',
                        @dirmail, 0, 0, 0, 0, 0, 0, '')";
            }
            else
            {
                sql = "UPDATE dg_msgcta SET msgserver = @servidor, msgpass = @pass, msgport = @puerto, msgfromaddr = @dirmail WHERE idmsgcta = @idMail";
            }

            List<SqlParameter> parametros = new List<SqlParameter>()
                {
                    new SqlParameter()
                    { ParameterName="@idMail", SqlDbType = SqlDbType.Int, Value = IdMail },
                    new SqlParameter()
                    { ParameterName="@servidor",SqlDbType = SqlDbType.NVarChar, Value = Servidor },
                    new SqlParameter()
                    { ParameterName="@pass",SqlDbType = SqlDbType.NVarChar, Value = Pass },
                    new SqlParameter()
                    { ParameterName="@puerto",SqlDbType = SqlDbType.NVarChar, Value = Puerto },
                    new SqlParameter()
                    { ParameterName="@dirmail",SqlDbType = SqlDbType.NVarChar, Value = DirMail }
                };
            try
            {
                DB.Execute(sql, parametros);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                respuesta = false;
            }
            return respuesta;
        }

    }
}
