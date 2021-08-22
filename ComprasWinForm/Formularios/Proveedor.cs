using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComprasWinForm.Modelos;

namespace ComprasWinForm.Formularios
{
    public partial class Proveedor : Form
    {
        CProveedor proveedor;
        Form form;
        public Proveedor()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void cmbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private async void btnInsertar_Click(object sender, EventArgs e)
        {
            try
            {
                proveedor = new CProveedor
                (
                    null, mtxtRnc.Text,
                    txtNombreComercial.Text,
                    cmbEstado.SelectedIndex + 1
                );

                if (await proveedor.Insert() > 0)
                    MessageBox.Show("Proveedor insertado correctamente");
                else
                    MessageBox.Show("Algo ha salido mal");

                dataGridView1.DataSource = await CProveedor.Select();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Algo ha salido mal la momento de Insertar");
            }

        }
        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                proveedor = new CProveedor
                (
                   int.Parse(txtId.Text),
                   mtxtRnc.Text,
                   txtNombreComercial.Text,
                   cmbEstado.SelectedIndex + 1
                );

                if (await proveedor.Update() > 0)
                    MessageBox.Show("Proveedor insertado correctamente");
                else
                    MessageBox.Show("Algo ha salido mal");

                dataGridView1.DataSource = await CProveedor.Select();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Algo ha salido mal la momento de Actualizar");
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int id = (dataGridView1.SelectedRows.Count.Equals(1)) ?
                int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()) : 0;

                if (id == 0)
                    MessageBox.Show("Seleccione un registro de la lista por favor");
                else
                    proveedor = new CProveedor(id);

                await proveedor.Delete();
                dataGridView1.DataSource = await CProveedor.Select();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Algo ha salido mal la momento de Eliminar");
            }
            
        }

        private async void btnBusqueda_Click(object sender, EventArgs e)
        {
            try
            {
                string searchString = $"WHERE ID = {int.Parse(txtBusqueda.Text)}";
                dataGridView1.DataSource = await CProveedor.Select(searchString);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Algo ha salido mal la momento de Buscar");
            }

        }

        private void btnCargarData_Click(object sender, EventArgs e)
        {
            txtId.Text = (dataGridView1.SelectedRows.Count.Equals(1)) ?
                dataGridView1.SelectedRows[0].Cells[0].Value.ToString() : txtId.Text;
        }

        private async void Proveedor_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = await CProveedor.Select();

            foreach (string nombre in await CEntidad.fillLists("SELECT NOMBRE FROM ESTADO"))
                cmbEstado.Items.Add(nombre);
        }

        private void empleadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form = new Empleado();
            Close();
            form.Show();
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form = new Cliente();
            Close();
            form.Show();
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form = new Proveedor();
            Close();
            form.Show();
        }

        private void solicitudToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form = new Solicitud();
            Close();
            form.Show();
        }

        private void ordenDeCompraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form = new OrdenCompra();
            Close();
            form.Show();
        }

        private void marcaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form = new Marca();
            Close();
            form.Show();
        }

        private void unidadDeMedidaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form = new UnidadMedida();
            Close();
            form.Show();
        }

        private void articuloToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form = new Articulo();
            Close();
            form.Show();
        }

        private void departamentosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form = new Departamento();
            Close();
            form.Show();
        }

        private async void btnExportar_Click(object sender, EventArgs e)
        {
            Excel reporte = new Excel("OrdenDeCompra");
            await reporte.Write((DataTable)dataGridView1.DataSource);
        }
    }

        
    
}
