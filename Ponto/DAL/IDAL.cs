using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public interface IDAL
    {
        Modelo.Cw_Usuario UsuarioLogado { get; set; }
        int MaxCodigo();
        DataTable GetAll();
        void Adicionar(Modelo.ModeloBase obj, bool Codigo);
        void Incluir(Modelo.ModeloBase obj);
        void Alterar(Modelo.ModeloBase obj);
        void Excluir(Modelo.ModeloBase obj);
        int getId(int pValor, string pCampo, int? pValor2);
        bool ExecutaComandosLote(List<string> comandos, int limite);
        void AtualizarRegistros<T>(List<T> list, SqlTransaction trans);
        void AtualizarRegistros<T>(List<T> list);
        void InserirRegistros<T>(List<T> list, SqlTransaction trans);
        void InserirRegistros<T>(List<T> list);
    }
}
