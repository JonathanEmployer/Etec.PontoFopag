using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Text;

namespace UI
{
    partial class sobre : Form
    {
        private BLL.Empresa bllEmpresa;
        public sobre()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            //this.Text = String.Format("About {0} {0}", AssemblyTitle);
            //this.labelProductName.Text = AssemblyProduct;
            //this.labelVersion.Text = String.Format("Version {0} {0}", AssemblyVersion);
            //this.labelCopyright.Text = AssemblyCopyright;
            //this.labelCompanyName.Text = AssemblyCompany;
            //this.textBoxDescription.Text = AssemblyDescription;

            this.Text = "Sobre o Cwork Ponto MT";

            this.labelVersion.Text = "Versão: " + Modelo.Global.Versao;
            this.labelRevision.Text = "Revisão: " + Modelo.Global.Revisao;

            IList<Modelo.Empresa> listaObjEmpresa = bllEmpresa.GetAllList();
            //Modelo.Empresa objEmpresa = bllEmpresa.GetEmpresaPrincipal();

            StringBuilder str = new StringBuilder();
            foreach (var objEmpresa in listaObjEmpresa)
            {
                str.AppendLine("Registrado para: " + objEmpresa.Nome);
                string licenca = "";
                switch (objEmpresa.TipoLicenca)
                {
                    case 0:
                        licenca = "Demonstração";
                        break;
                    case 1:
                        licenca = "Empresas";
                        break;
                    case 2:
                        licenca = "Funcionários";
                        break;
                }
                str.AppendLine("Tipo de Licença: " + licenca);

                if (objEmpresa.TipoLicenca == 2)
                {
                    str.AppendLine("Quantidade de Funcionários: " + bllEmpresa.GetQuantidadeMaximaDeFuncionarios());
                }

                str.AppendLine("Número de Série: " + objEmpresa.Numeroserie);
                str.Append(Environment.NewLine);

            }
            this.textBoxDescription.Text = str.ToString();

            
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sobre_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;                
            }
        }

    }
}
