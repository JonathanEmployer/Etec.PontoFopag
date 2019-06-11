using cwkPontoMT.Integracao;
using cwkPontoMT.Integracao.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;

namespace BLL.IntegracaoRelogio
{
    public class ColetaBilhetes
    {
        private Modelo.REP objRep;
        private Modelo.Cw_Usuario usuarioLogado;
        private string conexao;
        private BLL.REP bllRep;
        public ColetaBilhetes(Modelo.REP rep, string conexao, Modelo.Cw_Usuario usuarioLogado)
        {
            this.bllRep = new BLL.REP(conexao, usuarioLogado);
            this.objRep = rep;
            this.conexao = conexao;
            this.usuarioLogado = usuarioLogado;
        }

        public ResultadoImportacao ImportarAFDRep()
        {
            return ImportarAFDRep(new List<int>());
        }

        public ResultadoImportacao ImportarAFDRep(List<int> idsFuncsImportar)
        {
            ResultadoImportacao resultado = new ResultadoImportacao();
            resultado.InicioProcessamento = DateTime.Now;
            string comando = "Importação de Bilhetes";

            List<RegistroAFD> registros = GetRegistrosRelogio(comando);

            ProcessarRegistroAFD processarRegistros = new ProcessarRegistroAFD(objRep, conexao, usuarioLogado);
            return processarRegistros.ProcessarImportacao(idsFuncsImportar, registros);
        }

        private List<RegistroAFD> GetRegistrosRelogio(string comando)
        {
            Relogio relogio = RelogioFactory.GetRelogio((TipoRelogio)objRep.Relogio);
            if (objRep.TipoIP == 1)
            {
                IPHostEntry hostEntry;
                hostEntry = Dns.GetHostEntry(objRep.IP);

                if (hostEntry.AddressList.Length > 0)
                {
                    objRep.IP = hostEntry.AddressList[0].ToString();
                }
            }
            relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha, (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);

            if (objRep.EquipamentoHomologado.EquipamentoHomologadoInmetro)
            {
                if (String.IsNullOrEmpty(usuarioLogado.Cpf) || String.IsNullOrEmpty(usuarioLogado.LoginRep) || String.IsNullOrEmpty(usuarioLogado.SenhaRep))
                {
                    throw new Exception("Para comunicação com REPs Homologados pelo INMETRO é obrigatório informar o Login, Senha e CPF para comunicação com o Equipamento. "
                        + "Verifique o cadastro deste usuário (" + usuarioLogado.Nome + ") e realize o preenchimento dos campos \"Login REP\", \"Senha REP\" e \"CPF\".");
                }

                relogio.UsuarioREP = usuarioLogado.LoginRep;
                relogio.SenhaUsuarioREP = usuarioLogado.SenhaRep;
                relogio.Cpf = usuarioLogado.Cpf;
            }

            BLL.Empresa bllEmp = new BLL.Empresa(conexao, usuarioLogado);
            Modelo.Empresa emp = bllEmp.LoadObject(objRep.IdEmpresa);
            cwkPontoMT.Integracao.Entidades.Empresa empregador = new cwkPontoMT.Integracao.Entidades.Empresa();
            empregador.RazaoSocial = emp.Nome;
            empregador.TipoDocumento = String.IsNullOrEmpty(emp.Cnpj) ? TipoDocumento.CPF : TipoDocumento.CNPJ;
            empregador.Documento = String.IsNullOrEmpty(emp.Cnpj) ? emp.Cpf : emp.Cnpj;
            empregador.CEI = emp.CEI;
            empregador.Local = emp.Endereco;

            relogio.SetEmpresa(empregador);
            relogio.SetNumeroSerie(objRep.NumSerie);
            relogio.Senha = objRep.Senha;
            List<RegistroAFD> registros = new List<RegistroAFD>();
            if (objRep.UltimoNSR == 0)
            {
                cwkPontoMT.Integracao.Util.GravaLogCentralCliente(objRep.NumSerie, comando, 2, "Iniciando coleta por período", "Comunicador", "Período = " + objRep.DataInicioImportacao.ToString("dd/MM/yyyy HH:mm") + " a " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                registros = relogio.GetAFD(objRep.DataInicioImportacao, DateTime.Now);
            }
            else
            {
                cwkPontoMT.Integracao.Util.GravaLogCentralCliente(objRep.NumSerie, comando, 2, "Iniciando coleta por NSR", "Comunicador", "NSR a partir de " + objRep.UltimoNSR);
                registros = relogio.GetAFDNsr(objRep.DataInicioImportacao, DateTime.Now, (int)objRep.UltimoNSR + 1, int.MaxValue, false);
            }

            int qtdLidos = registros.Count;
            if (qtdLidos > 0)
            {
                cwkPontoMT.Integracao.Util.GravaLogCentralCliente(objRep.NumSerie, comando, 0, qtdLidos + " Registro(s) coletado(s) com sucesso", "Comunicador", "");
            }
            else
            {
                cwkPontoMT.Integracao.Util.GravaLogCentralCliente(objRep.NumSerie, comando, 0, "Não foram encontrados novos registros", "Comunicador", "");
            }
            return registros;
        }
    }
}
