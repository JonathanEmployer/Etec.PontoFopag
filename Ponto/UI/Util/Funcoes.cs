using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace UI.Util
{
    public static class Funcoes
    {
        public static void ChamaManut(Form pMenu, UI.Base.ManutBase pForm, string pTabela, Modelo.Acao pAcao, List<string> pTelasAbertas)
        {
            if (cwkControleUsuario.Facade.ControleAcesso(pForm))
            {
                if (!pTelasAbertas.Contains(pForm.Name))
                {
                    pTelasAbertas.Add(pForm.Name);
                    pForm.TelasAbertas = pTelasAbertas;
                    pForm.cwTabela = pTabela;
                    pForm.cwAcao = pAcao;
                    if (pMenu != null)
                    {
                        pForm.MdiParent = pMenu;
                        pForm.Show();
                    }
                    else
                    {
                        pForm.ShowDialog();
                    }
                }
                else
                {
                    ToolStripContainer tsc1 = (ToolStripContainer)pMenu.Controls["toolStripContainer1"];
                    if (tsc1.ContentPanel.Controls.ContainsKey(pForm.Name))
                    {
                        tsc1.ContentPanel.Controls[pForm.Name].BringToFront();
                    }
                    pForm.Dispose();
                }
            }
        }

        public static string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static System.Drawing.Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            //a holder for the result
            Bitmap result = new Bitmap(width, height);
            //set the resolutions the same to avoid cropping due to resolution differences
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            //return the resulting bitmap
            return result;
        }

        public static void ChamaGrid(Form pMenu, UI.Base.GridBase pForm, string pTabela, List<string> pTelasAbertas)
        {
            if (!pForm.Text.Contains(pTabela))
            {
                pForm.Text += pTabela;
            }
            if (cwkControleUsuario.Facade.ControleAcesso(pForm))
            {
                if (!pTelasAbertas.Contains(pForm.Name))
                {
                    pTelasAbertas.Add(pForm.Name);
                    pForm.TelasAbertas = pTelasAbertas;
                    pForm.cwId = 0;
                    pForm.cwSelecionar = false;
                    pForm.cwTabela = pTabela;
                    if (pMenu != null)
                    {
                        pForm.MdiParent = pMenu;
                        pForm.Show();
                    }
                    else
                    {
                        pForm.ShowDialog();
                    }
                }
                else
                {
                    ToolStripContainer tsc1 = (ToolStripContainer)pMenu.Controls["toolStripContainer1"];
                    if (tsc1.ContentPanel.Controls.ContainsKey(pForm.Name))
                    {
                        tsc1.ContentPanel.Controls[pForm.Name].BringToFront();
                    }
                    pForm.Dispose();
                }
            }
        }

        public static void ChamaGridSelecao(UI.Base.GridBase pForm)
        {
            if (cwkControleUsuario.Facade.ControleAcesso(pForm))
            {
                pForm.cwSelecionar = true;
                pForm.ShowDialog();
            }
        }

        private static bool ControleAcesso(UI.Base.GridBase pForm)
        {
            BLL.Cw_Acesso bllAcesso = new BLL.Cw_Acesso();
            BLL.Cw_AcessoCampo bllAcessoCampo = new BLL.Cw_AcessoCampo();
            BLL.Cw_Grupo bllGrupo = new BLL.Cw_Grupo();
            
            //Cria o objeto Acesso
            Modelo.Cw_Acesso objAcesso;
            //Cria o objeto e bll para grupo de usuario
            

            foreach (Modelo.Cw_Grupo grupo in bllGrupo.getListaGrupo())
            {
                //Verifica se o formulário possui o registro de Acesso
                if (!bllAcesso.PossuiRegistro(grupo.Id, pForm.Name))
                {
                    //Inclui o registro de acesso do formulário
                    objAcesso = new Modelo.Cw_Acesso();

                    objAcesso.Codigo = bllAcesso.MaxCodigo();
                    objAcesso.IdGrupo = grupo.Id;
                    objAcesso.Formulario = pForm.Name;
                    objAcesso.Acesso = Convert.ToBoolean(grupo.Acesso);

                    bllAcesso.Salvar(Modelo.cwkAcao.Incluir, objAcesso);
                }
                else
                {
                    objAcesso = bllAcesso.LoadObject(grupo.Id, pForm.Name);
                }

                //Percorre todos os controles (Botões) da tela verificando seu acesso
                foreach (Control ctr in pForm.Controls)
                {
                    if (ctr is DevExpress.XtraEditors.SimpleButton)
                    {
                        //Verifica se o botão Possui Registro
                        if (!bllAcessoCampo.PossuiRegistro(objAcesso.Id, ctr.Name))
                        {
                            //Cria o objeto Acesso
                            Modelo.Cw_AcessoCampo objAcessoCampo = new Modelo.Cw_AcessoCampo();

                            //Inclui o registro de acesso do formulário
                            objAcessoCampo.Codigo = bllAcessoCampo.MaxCodigo();
                            objAcessoCampo.IdAcesso = objAcesso.Id;
                            objAcessoCampo.Campo = ctr.Name;
                            objAcessoCampo.Display = ctr.Text;
                            objAcessoCampo.Acesso = Convert.ToBoolean(grupo.Acesso);

                            bllAcessoCampo.Salvar(Modelo.cwkAcao.Incluir, objAcessoCampo);
                        }
                    }
                }
            }

            if (Modelo.cwkGlobal.objUsuarioLogado.Tipo == 0)
            {
                return true;
            }

            //Verifica se o usuario tem acesso a tela
            if (!bllAcesso.PossuiAcesso(Modelo.cwkGlobal.objUsuarioLogado.IdGrupo, pForm.Name))
            {
                MessageBox.Show("Acesso não permitido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            objAcesso = bllAcesso.LoadObject(Modelo.cwkGlobal.objUsuarioLogado.IdGrupo, pForm.Name);

            //Percorre todos os controles (Botões) da tela verificando seu acesso
            foreach (Control ctr in pForm.Controls)
            {
                if (ctr is DevExpress.XtraEditors.SimpleButton)
                {
                    //Verifica se o usuario tem acesso ao campo
                    if (!bllAcessoCampo.PossuiAcesso(objAcesso.Id, ctr.Name))
                    {
                        ctr.Enabled = false;
                    }
                }
            }

            return true;
        }
    }
}