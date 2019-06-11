﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DAL.SQL;

namespace BLL
{
    public class ImportacaoTopPonto
    {
        private DAL.IImportacaoTopPonto dalImportacaoTopPonto;
        private string ConnectionString;
        public ImportacaoTopPonto() : this(null)
        {
            
        }

        public ImportacaoTopPonto(string connString)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalImportacaoTopPonto = new DAL.SQL.ImportacaoTopPonto(new DataBase(ConnectionString));
                    break;
                case 2:
                    dalImportacaoTopPonto = DAL.FB.ImportacaoTopPonto.GetInstancia;
                    break;
            }
        }

        public Hashtable GetHashEmpresa()
        {
            return dalImportacaoTopPonto.GetHashEmpresa();
        }

        public Hashtable GetHashDepartamento()
        {
            return dalImportacaoTopPonto.GetHashDepartamento();
        }

        public Hashtable GetHashFuncaoDescricao()
        {
            return dalImportacaoTopPonto.GetHashFuncaoDescricao();
        }

        public Hashtable GetHashFuncao()
        {
            return dalImportacaoTopPonto.GetHashFuncao();
        }

        public Hashtable GetHashHorario()
        {
            return dalImportacaoTopPonto.GetHashHorario();
        }

        public Hashtable GetHashFuncionario()
        {
            return dalImportacaoTopPonto.GetHashFuncionario();
        }

        public Hashtable GetHashFuncCodigoDscodigo()
        {
            return dalImportacaoTopPonto.GetHashFuncCodigoDscodigo();
        }

        public Hashtable GetHashOcorrenciaDescId()
        {
            return dalImportacaoTopPonto.GetHashOcorrenciaDescId();
        }

        public Hashtable GetHashFuncCodigoId()
        {
            return dalImportacaoTopPonto.GetHashFuncCodigoId();
        }

        public Hashtable GetHashOcorrenciaCodigoId()
        {
            return dalImportacaoTopPonto.GetHashOcorrenciaCodigoId();
        }
    }
}
