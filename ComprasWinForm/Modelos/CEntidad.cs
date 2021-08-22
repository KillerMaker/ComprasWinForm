using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ComprasWinForm.Modelos
{
    public abstract class CEntidad
    {
        public static readonly string stringConnection = "Data Source=DESKTOP-7V51383\\SQLEXPRESS;Initial Catalog=COMPRAS-PROJECT;Integrated Security=True";
        
        public abstract Task<int> Insert();
        public abstract Task<int> Update();
        public abstract Task<int> Delete();

        public static bool validaCedula(string pCedula)
        {
            int vnTotal = 0;
            string vcCedula = pCedula.Replace("-", "");
            int pLongCed = vcCedula.Trim().Length;
            int[] digitoMult = new int[11] { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1 };


            if (pLongCed < 11 || pLongCed > 11 || vcCedula== "00000000000")
            //if (pLongCed < 11 || pLongCed > 11)
                return false;

            for (int vDig = 1; vDig <= pLongCed; vDig++)
            {
                int vCalculo = Int32.Parse(vcCedula.Substring(vDig - 1, 1)) * digitoMult[vDig - 1];
                if (vCalculo < 10)
                    vnTotal += vCalculo;
                else
                    vnTotal += Int32.Parse(vCalculo.ToString().Substring(0, 1)) + Int32.Parse(vCalculo.ToString().Substring(1, 1));
            }

            if (vnTotal % 10 == 0)
                return true;
            else
                return false;
        }

        public async Task<int> ExecuteCommand(string query,List<SqlParameter>parameters=null)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(stringConnection))
                {
                    await con.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        if(parameters!=null)
                            command.Parameters.AddRange(parameters.ToArray());

                        return await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<SqlDataReader>ExecuteReader(string query,SqlConnection con)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    await con.OpenAsync();
                    return await command.ExecuteReaderAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public static async Task<IEnumerable<string>> fillLists(string query)
        {
            List<string> list = new List<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(stringConnection))
                {
                    await con.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        using(SqlDataReader reader=await command.ExecuteReaderAsync())
                        {

                            while (await reader.ReadAsync())
                                list.Add(reader[0].ToString());
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public abstract List<SqlParameter> GetParameters();
    }
}
