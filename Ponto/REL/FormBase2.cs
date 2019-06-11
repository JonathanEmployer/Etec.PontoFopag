﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using Modelo;
using System.Xml;
using System.IO;

namespace REL
{
    public partial class FormBase2 : Form
    {
        protected string nomerel;
        protected string ds;
        protected List<Microsoft.Reporting.WinForms.ReportParameter> parametros;
        protected DataTable Dt;

        protected StringBuilder Arquivo { get; set; }
        protected string Titulo { get; set; }

        protected BLL.Empresa bllEmpresa = new BLL.Empresa();
        protected Modelo.Empresa objEmpresa = new Modelo.Empresa();        

        public FormBase2()
        {
            InitializeComponent();

        }

        protected virtual void FormBase_Load(object sender, EventArgs e)
        {
            //Carrega();
        }

        protected virtual void Carrega()
        {
            cbIdEmpresa.Properties.DataSource = bllEmpresa.GetAll();
            setaNomeArquivo(this.Name);
            LeXML();
        }

        protected virtual void btOk_Click(object sender, EventArgs e)
        {
            if ((int)cbIdEmpresa.EditValue != 0)
            {
                Titulo = cbIdEmpresa.Text;

                GravaXML(chbSalvarFiltro.Checked);
            }
            else
            {
                dxErrorProvider1.SetError(cbIdEmpresa, "Campo obrigatório.");
            }
        }

        protected virtual void btCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nome"></param>
        protected void setaNomeArquivo(string nome)
        {
            Arquivo = new StringBuilder(Application.StartupPath + "\\XML\\");
            Arquivo.Append(nome);
            Arquivo.Append(".xml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gravar"></param>
        protected virtual void GravaXML(bool gravar)
        {
            XmlDocument documentoXml = new XmlDocument();

            documentoXml.AppendChild(documentoXml.CreateXmlDeclaration("1.0", "UTF-8", null));

            XmlElement noPai = documentoXml.CreateElement(this.Name);

            XmlElement element = null;
            XmlElement ler = documentoXml.CreateElement("ler");
            ler.InnerText = gravar.ToString();
            noPai.AppendChild(ler);
            if (gravar)
            {
                XmlElement xmlTitulo = documentoXml.CreateElement("titulo");
                xmlTitulo.InnerText = Titulo;
                noPai.AppendChild(xmlTitulo);

                element = documentoXml.CreateElement("filtros");

                XmlElement empresa = documentoXml.CreateElement("empresa");
                empresa.InnerText = cbIdEmpresa.Text;
                element.AppendChild(empresa);

                XmlElement c;
                foreach (Control campo in tabPage1.Controls)
                {
                    if (campo is DevExpress.XtraEditors.CheckEdit)
                    {
                        c = documentoXml.CreateElement(campo.Name);
                        c.InnerText = ((DevExpress.XtraEditors.CheckEdit)campo).Checked.ToString();
                        element.AppendChild(c);
                    }
                    else if (campo is DevExpress.XtraEditors.DateEdit)
                    {
                        c = documentoXml.CreateElement(campo.Name);
                        c.InnerText = ((DevExpress.XtraEditors.DateEdit)campo).EditValue.ToString();
                        element.AppendChild(c);
                    }
                    else if (campo is DevExpress.XtraEditors.BaseEdit)
                    {
                        c = documentoXml.CreateElement(campo.Name);
                        c.InnerText = ((DevExpress.XtraEditors.BaseEdit)campo).EditValue.ToString();
                        element.AppendChild(c);
                    }
                }

                noPai.AppendChild(element);
            }
            documentoXml.AppendChild(noPai);

            documentoXml.Save(Arquivo.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void LeXML()
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                                
                xml.Load(Arquivo.ToString());
                XmlNode noPai = xml.SelectSingleNode(this.Name);

                if (noPai.SelectSingleNode("ler").InnerText == "True")
                {
                    chbSalvarFiltro.Checked = true;

                    XmlNode filtros = noPai.SelectSingleNode("filtros");

                    foreach (Control campo in tabPage1.Controls)
                    {
                        XmlNode xmlCampo = filtros.SelectSingleNode(campo.Name);
                        if (campo is DevExpress.XtraEditors.PopupBaseEdit)
                        {
                            switch (campo.GetType().ToString())
                            {
                                case "Componentes.devexpress.cwk_DevLookup":
                                    ((DevExpress.XtraEditors.PopupBaseEdit)campo).EditValue = Convert.ToInt32(xmlCampo.InnerText);
                                    break;
                                case "DevExpress.XtraEditors.DateEdit":
                                    ((DevExpress.XtraEditors.DateEdit)campo).EditValue = Convert.ToDateTime(xmlCampo.InnerText);
                                    break;
                                default:
                                    ((DevExpress.XtraEditors.PopupBaseEdit)campo).EditValue = xmlCampo.InnerText;
                                    break;
                            }
                        }
                        else if (campo is DevExpress.XtraEditors.CheckEdit)
                        {
                            ((DevExpress.XtraEditors.CheckEdit)campo).Checked = Convert.ToBoolean(xmlCampo.InnerText);
                        }
                        else if (campo is DevExpress.XtraEditors.SpinEdit)
                        {
                            ((DevExpress.XtraEditors.SpinEdit)campo).Value = Convert.ToInt32(xmlCampo.InnerText);
                        }
                        else if (campo is DevExpress.XtraEditors.TextEdit)
                        {
                            ((DevExpress.XtraEditors.TextEdit)campo).EditValue = xmlCampo.InnerText;
                        }
                    }
                    XmlNode xmlEmpresas = filtros.SelectSingleNode("empresa");
                    cbIdEmpresa.EditValue = xmlEmpresas.InnerText;                    
                }
            }
            catch (FileNotFoundException)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string MontaStringEmpresas()
        {
            return cbIdEmpresa.EditValue.ToString();
        }

        protected void gcEmpresas_Click(object sender, EventArgs e)
        {
        }

        private void FormBase_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F9:
                    btOk_Click(sender, e);
                    break;
                case Keys.Escape:
                    btCancelar_Click(sender, e);
                    break;
            }
        }

    }
}
