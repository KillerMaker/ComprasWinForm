
using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace ComprasWinForm.Modelos
{
    public class Excel
    {
        private readonly string nombre = "";
        private readonly _Application excel = new _Excel.Application();
        private Workbook wb;
        private Worksheet ws;
        public Excel(string nombre = "report")
        {
            this.nombre = nombre + " " + DateTime.Now.ToString().Replace(":", "_").Replace("\\", "-").Replace("/", "-");
            wb = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);

            try
            {
                wb.SaveAs(this.nombre);
                wb.Close();
            }
            catch (Exception e)
            {
                wb.Close();
                throw new Exception(e.Message);
            }
        }
        public async Task Write(System.Data.DataTable table)
        {
            new Task(() => 
            {
                wb = excel.Workbooks.Open(nombre);

                try
                {
                    ws = wb.Worksheets[1];
                    int i = 1;
                    int j = 1;

                    foreach (DataColumn columna in table.Columns)
                    {
                        ws.Cells[i, j].Value2 = columna.ColumnName;
                        j++;
                    }

                    i = 2;
                    j = 1;

                    for (int k = 0; k < table.Rows.Count; k++)
                    {
                        for (int l = 0; l < table.Columns.Count; l++)
                        {
                            ws.Cells[i, j].Value2 = table.Rows[k][l].ToString();
                            j++;
                        }
                        j = 1;
                        i++;
                    }

                    wb.Save();
                    wb.Close();
                    MessageBox.Show("Reporte Creado correctamente en Documentos");
                }
                catch (Exception e)
                {
                    wb.Close();
                    throw new Exception(e.Message);
                }
            }).Start();

            await Task.WhenAll();
           
        }
    }
}
