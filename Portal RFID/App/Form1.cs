using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class FormPrincipal : Form
    {
        /// <summary>
        /// Cliente do serviço Portal_ID.
        /// </summary>
        static ServiceReference1.LeitorClient  client;

        /// <summary>
        /// Tipo utilizado para fazer a leitura das tags RFIDs.
        /// </summary>
        static List<ServiceReference1.TagRfidType> tagsRfids;

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        public FormPrincipal()
        {
            InitializeComponent();

            /* Inicialização dos dados. */
            cmbBoxAnt.SelectedIndex = 0;
            client = new ServiceReference1.LeitorClient();
            tagsRfids = new List<ServiceReference1.TagRfidType>();
        }

        /// <summary>
        /// Método de tratamento do clique do botão "btnLer".
        /// </summary>
        /// 
        /// Este método é executado sempre que o usuário clica no botão "btnLer".
        /// 
        /// <param name="sender">Objeto gerador do evento.</param>
        /// <param name="e">Argumentos do evento.</param>
        private void btnLer_Click(object sender, EventArgs e)
        {
            var oldText = btnLer.Text;
            btnLer.Text = "Aguarde...";
            Application.DoEvents();
            int potencia = int.Parse(txtBoxPotencia.Text);
            tagsRfids = client.GetTagsRfid((cmbBoxAnt.SelectedIndex + 1), txtBoxIpPorta.Text, txtFiltro.Text,  Convert.ToInt32("0"+txtTempoLeitura.Text) * 1000 , false, potencia, Convert.ToInt32("0" + txtPotAnt1.Text), Convert.ToInt32("0" + txtPotAnt2.Text), Convert.ToInt32("0" + txtPotAnt3.Text), Convert.ToInt32("0" + txtPotAnt4.Text));
            dgRead.DataSource = tagsRfids == null ? null :  tagsRfids.OrderByDescending(o=>o.EpcValue).ToList();
            dgRead.Refresh();
            if (tagsRfids != null)
            {
                lblQtdTagsUltimaLeitura.Text = lblQtdTagsLidas.Text;
                lblQtdTagsLidas.Text = tagsRfids.Count.ToString();
                
            }
            else
            {
                lblQtdTagsLidas.Text = "0";
                MessageBox.Show("ERRO!");
            }
            btnLer.Text = oldText;


        }

        /// <summary>
        /// Método de tratamento do clique do botão "btnAtualizar".
        /// </summary>
        /// 
        /// Este método é executado sempre que o usuário clica no botão "btnAtualizar".
        /// 
        /// <param name="sender">Objeto gerador do evento.</param>
        /// <param name="e">Argumentos do evento.</param>
        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            try {
                lblLeitorOkVal.Text = client.IsReaderOk(txtBoxIpPorta.Text) ? "SIM" : "NÃO";
            }
            catch(Exception)
            {
                lblLeitorOkVal.Text = "NÃO";
            }
        }

        /// <summary>
        /// Método de tratamento do clique do botão "btnEcoOk".
        /// </summary>
        /// 
        /// Este método é executado sempre que o usuário clica no botão "btnAtualizar".
        /// 
        /// <param name="sender">Objeto gerador do evento.</param>
        /// <param name="e">Argumentos do evento.</param>
        private void btnEcoOk_Click(object sender, EventArgs e)
        {
            try {
                lblRetornoEcoVal.Text = client.GetEcho(txtBoxEco.Text);
            }
            catch(Exception ex)
            {
                lblRetornoEcoVal.Text = "";
            }
        }

    }
}
