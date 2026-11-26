using System;
using System.Linq;
using System.Windows.Forms;

namespace YMH219_ChatApp
{
    /// <summary>
    /// Açılış penceresi. Sunucu ve istemci pencerelerini açar.
    /// </summary>
    public partial class MainForm : Form
    {
        private ServerForm? serverForm;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnOpenServer_Click(object sender, EventArgs e)
        {
            // Tek bir sunucu penceresi yeterlidir; zaten açıksa öne getirilir.
            if (serverForm == null || serverForm.IsDisposed)
            {
                serverForm = new ServerForm();
                serverForm.Show();
                return;
            }

            if (serverForm.WindowState == FormWindowState.Minimized)
                serverForm.WindowState = FormWindowState.Normal;

            serverForm.Activate();
        }

        private void btnOpenClient_Click(object sender, EventArgs e)
        {
            // Her tıklamada yeni bir istemci penceresi açılır; böylece birden fazla kullanıcı denenebilir.
            new ClientForm().Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Ana pencere kapanırken açık sunucu ve istemci pencereleri de kapatılır;
            // her biri kendi FormClosing olayında bağlantısını düzgünce sonlandırır.
            foreach (Form form in Application.OpenForms.Cast<Form>().Where(f => f != this).ToList())
                form.Close();
        }
    }
}
