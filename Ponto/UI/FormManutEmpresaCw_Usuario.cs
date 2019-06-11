using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using DevExpress.XtraGrid.Views.Grid;

namespace UI
{
    public partial class FormManutEmpresaCw_Usuario : UI.Base.ManutBase
    {
        private BLL.EmpresaCw_Usuario bllEmpresaCw_Usuario;
        private BLL.Empresa bllEmpresa;
        private BLL.Cw_Usuario bllCw_Usuario;
        private readonly List<Modelo.Cw_Usuario> todosUsuarios;
        private readonly List<Modelo.EmpresaCw_Usuario> usuariosEmpresa;
        private readonly List<Modelo.Cw_Usuario> usuariosNaoAdicionados = new List<Modelo.Cw_Usuario>();
        private readonly List<Modelo.Cw_Usuario> usuariosAdicionados = new List<Modelo.Cw_Usuario>();

        private string Empresa
        {
            get { return txtEmpresa.Text; }
            set { txtEmpresa.Text = value; }
        }
        private int idEmpresa;
        private int IdEmpresa
        {
            get { return idEmpresa; }
            set
            {
                idEmpresa = value;
                if (idEmpresa > 0)
                {
                    var objEmpresa = bllEmpresa.LoadObject(idEmpresa);
                    if (!String.IsNullOrEmpty(objEmpresa.Cnpj))
                        Empresa = objEmpresa.Cnpj;
                    else
                        Empresa = objEmpresa.Cpf;
                    Empresa += " | " + objEmpresa.Codigo + " | " + objEmpresa.Nome;
                }
                else
                    Empresa = String.Empty;
            }
        }

        public FormManutEmpresaCw_Usuario(int pIdEmpresa)
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            bllEmpresaCw_Usuario = new BLL.EmpresaCw_Usuario();
            bllCw_Usuario = new BLL.Cw_Usuario();
            IdEmpresa = pIdEmpresa;
            todosUsuarios = bllCw_Usuario.GetAllList();
            usuariosEmpresa = bllEmpresaCw_Usuario.GetListaPorEmpresa(IdEmpresa);
            AtualizaListasUsuarios();
        }

        private void FormManutEmpresaCw_Usuario_Shown(object sender, EventArgs e)
        {
            gcUsuariosAdicionados.DataSource = usuariosAdicionados;
            gcUsuariosNaoAdicionados.DataSource = usuariosNaoAdicionados;
        }

        private void AtualizaListasUsuarios()
        {
            usuariosAdicionados.Clear();
            usuariosAdicionados.AddRange((from u in todosUsuarios
                                          where u.Tipo != 0 && usuariosEmpresa
                                          .Where(e => e.IdCw_Usuario == u.Id
                                                 && e.Acao != Modelo.Acao.Excluir).Count() > 0
                                          select u).ToList());

            usuariosNaoAdicionados.Clear();
            usuariosNaoAdicionados.AddRange((from u in todosUsuarios
                                             where u.Tipo != 0 && !usuariosAdicionados.Contains(u)
                                             select u)
                                       .ToList());
        }

        private void AtualizarGrids()
        {
            gcUsuariosAdicionados.RefreshDataSource();
            gvUsuariosAdicionados.MoveFirst();
            gcUsuariosNaoAdicionados.RefreshDataSource();
            gvUsuariosNaoAdicionados.MoveFirst();
        }

        private void sbAdicionar_Click(object sender, EventArgs e)
        {
            AdicionarSelecionados();
        }

        private void sbRemover_Click(object sender, EventArgs e)
        {
            RemoverSelecionados();
        }

        private void AdicionarSelecionados()
        {
            foreach (Modelo.Cw_Usuario item in GetRegistrosSelecionado(gvUsuariosNaoAdicionados))
            {
                Modelo.EmpresaCw_Usuario empresaCw_Usuario;
                var result = usuariosEmpresa.Where(u => u.IdCw_Usuario == item.Id);
                if (result.Count() > 0)
                {
                    empresaCw_Usuario = result.First();
                    empresaCw_Usuario.Acao = empresaCw_Usuario.Id > 0 ? Modelo.Acao.Alterar : Modelo.Acao.Incluir;
                }
                else
                {
                    empresaCw_Usuario = new Modelo.EmpresaCw_Usuario() { Acao = Modelo.Acao.Incluir };
                    empresaCw_Usuario.Codigo = bllEmpresaCw_Usuario.MaxCodigo(usuariosEmpresa);
                    empresaCw_Usuario.IdCw_Usuario = item.Id;
                    empresaCw_Usuario.IdEmpresa = IdEmpresa;
                    usuariosEmpresa.Add(empresaCw_Usuario);
                }
            }
            AtualizaListasUsuarios();
            AtualizarGrids();
        }

        private void RemoverSelecionados()
        {
            foreach (Modelo.Cw_Usuario item in GetRegistrosSelecionado(gvUsuariosAdicionados))
            {
                Modelo.EmpresaCw_Usuario empresaCw_Usuario;
                var result = usuariosEmpresa.Where(u => u.IdCw_Usuario == item.Id);
                if (result.Count() > 0)
                {
                    empresaCw_Usuario = result.First();
                    empresaCw_Usuario.Acao = Modelo.Acao.Excluir;
                }
            }
            AtualizaListasUsuarios();
            AtualizarGrids();
        }

        protected IEnumerable<Modelo.Cw_Usuario> GetRegistrosSelecionado(GridView gridView)
        {
            foreach (var rowHandle in gridView.GetSelectedRows())
                yield return (Modelo.Cw_Usuario)gridView.GetRow(rowHandle);
        }

        private void sbAdicionarTodos_Click(object sender, EventArgs e)
        {
            gvUsuariosNaoAdicionados.SelectAll();
            AdicionarSelecionados();
        }

        private void sbRemoverTodos_Click(object sender, EventArgs e)
        {
            gvUsuariosAdicionados.SelectAll();
            RemoverSelecionados();
        }

        public override Dictionary<string, string> Salvar()
        {
            var ret = new Dictionary<string, string>();

            foreach (var item in usuariosEmpresa
                    .Where(i => !(i.Acao == Modelo.Acao.Excluir && i.Id == 0)))
            {
                ret.Concat(bllEmpresaCw_Usuario.Salvar(item.Acao, item));
            }
            return ret;
        }
    }
}
