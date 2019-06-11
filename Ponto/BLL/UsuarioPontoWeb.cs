using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class UsuarioPontoWeb : IcwkBLL<Modelo.UsuarioPontoWeb>
    {
        DAL.IUsuarioPontoWeb dalUsuario;
        private string ConnectionString;

        public UsuarioPontoWeb(string connString)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalUsuario = new DAL.SQL.UsuarioPontoWeb(new DataBase(ConnectionString));

        }

        public int MaxCodigo()
        {
            return dalUsuario.MaxCodigo();
        }

        public List<Modelo.UsuarioPontoWeb> GetAllList()
        {
            return dalUsuario.GetAllList();
        }

        public List<Modelo.UsuarioPontoWeb> GetAllListWeb()
        {
            return dalUsuario.GetAllListWeb();
        }

        public Modelo.UsuarioPontoWeb LoadObjectLogin(string pLogin)
        {
            return dalUsuario.LoadObjectLogin(pLogin);
        }

        public Modelo.UsuarioPontoWeb LoadObject(int id)
        {
            return dalUsuario.LoadObject(id);
        }

        public Modelo.UsuarioPontoWeb LoadObjectByCodigo(int codigo)
        {
            return dalUsuario.LoadObjectByCodigo(codigo);
        }

        public string GetIdAdmin()
        {
            return Convert.ToString(dalUsuario.GetIdAdmin());
        }

        public Dictionary<string, string> Salvar(Modelo.cwkAcao pAcao, Modelo.UsuarioPontoWeb objeto)
        {
            Dictionary<string, string> erros = new Dictionary<string, string>();
            erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.cwkAcao.Incluir:
                        dalUsuario.Incluir(objeto);
                        break;
                    case Modelo.cwkAcao.Alterar:
                        dalUsuario.Alterar(objeto);
                        break;
                    case Modelo.cwkAcao.Excluir:
                        dalUsuario.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public System.Data.DataTable GetAll()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.UsuarioPontoWeb objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (!String.IsNullOrEmpty(objeto.Cpf))
            {
                if (!ValidarCPF(objeto.Cpf))
                {
                    ret.Add("txtCpf", "CPF Inválido");
                }
            }
            if (!String.IsNullOrEmpty(objeto.LoginRep) && String.IsNullOrEmpty(objeto.SenhaRep))
            {
                ret.Add("txtSenhaRep", "Senha Inválida");
            }
            if (String.IsNullOrEmpty(objeto.LoginRep) && !String.IsNullOrEmpty(objeto.SenhaRep))
            {
                ret.Add("txtLoginRep", "Login Inválido");
            }

            if (objeto.Login.Length > 20)
            {
                ret.Add("Login", "O login deve possuir no máximo 20 caracteres.");
            }
            return ret;
        }

        private bool ValidarCPF(string cpf)
        {
            string[] bloqueados = new string[] 
            { 
                "00000000000", "11111111111", "22222222222", 
                "33333333333", "44444444444", "55555555555", 
                "66666666666", "77777777777", "88888888888", 
                "99999999999" 
            };
            cpf = cpf.Replace(".", "").Replace("-", "").Replace("_", "");
            cpf = cpf.Trim();
            if (cpf.Length != 11 && cpf.Length != 0)
            {
                return false;
            }

            if (bloqueados.ToList().Contains(cpf))
            {
                return false;
            }

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Replace(".", "").Replace("-", "").Replace("_", "");
            cpf = cpf.Trim();

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            }

            resto = soma % 11;
            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            }
            resto = soma % 11;

            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }
            digito = digito + resto.ToString();

            if (!cpf.EndsWith(digito))
            {
                return false;
            }
            return true;
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            throw new NotImplementedException();
        }
    }
}
