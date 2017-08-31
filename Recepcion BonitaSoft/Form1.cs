using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MetroFramework.Forms;
using DevComponents.DotNetBar;
using System.Configuration;
using Utilities;

namespace Recepcion_BonitaSoft
{
    public partial class Form1 : MetroForm
    {
        Tools sql = new Tools();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = sql.getRecords("SELECT numeroOC, COUNT(numeroOC) AS Orden FROM dbo.RcvProv GROUP BY numeroOC;", null, "Data Source=TDBE10SERVER;Initial Catalog=ERP10DB;Persist Security Info=True;User ID=sa;Password=Epicor123");

            foreach (DataRow row in dt.Rows)
            {
                listOC.Items.Add(dt.Rows[i].ItemArray[0].ToString());
                i++;
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            for (int item = 0; item < listOC.Items.Count; item++)
            {
                listOC.SetItemChecked(item, true);
            }
            btnSeleccionarTodo.Visible = false;
            btnQuitarSeleccion.Visible = true;
        }

        private void btnQuitarSeleccion_Click(object sender, EventArgs e)
        {
            for (int item = 0; item < listOC.Items.Count; item++)
            {
                listOC.SetItemChecked(item, false);
            }
            btnQuitarSeleccion.Visible = false;
            btnSeleccionarTodo.Visible = true;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            MetroFramework.MetroMessageBox.Show(this, "Inicia proceso en Epicor", "Recepción masiva de Orden de Compra", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        private void metroToggle1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void listOC_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (listOC.SelectedItem.Equals("0"))
                MetroFramework.MetroMessageBox.Show(this, "La Orden " + listOC.SelectedItem.ToString() + " no es válida !!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            else
            {
                string queryBuild = String.Format(ConfigurationManager.AppSettings["qGetOrders"].ToString(), listOC.SelectedItem.ToString());
                DataTable d = sql.getRecords(queryBuild, null, ConfigurationManager.AppSettings["epiConnection"].ToString());
                dgvOC.DataSource = d;
            }
        }
        /*
public void mailNotification(string exception)
{
MailMessage Mensaje = new MailMessage(); // Instancia para preparar el cuerpo del correo

// Parámetros y cuerpo del correo 
Mensaje.To.Add(new MailAddress("vorkelball@gmail.com"));
Mensaje.To.Add(new MailAddress("robertogarcia003@hotmail.com"));
Mensaje.From = new MailAddress("asesores@gicaor.com");
Mensaje.Subject = "Excepción encontrada en ControlDevoluciones";
Mensaje.Body = "Buen día !! \n Ha ocurrido una excepción en la aplicación, a continuación se presenta la descripción completa \n\n" + exception;


SmtpClient server = new SmtpClient("mail.gicaor.com", 366); // Especifico el servidor de salida
server.Credentials = new System.Net.NetworkCredential("asesores@gicaor.com", "GICrfv456");
server.EnableSsl = true;

ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
{
return true;
};
server.Send(Mensaje);
}
*/
    }
}
