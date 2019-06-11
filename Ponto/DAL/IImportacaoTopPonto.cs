using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DAL
{
    public interface IImportacaoTopPonto
    {
        Hashtable GetHashEmpresa();

        Hashtable GetHashDepartamento();

        Hashtable GetHashFuncaoDescricao();

        Hashtable GetHashFuncao();

        Hashtable GetHashHorario();

        Hashtable GetHashFuncionario();

        Hashtable GetHashFuncCodigoDscodigo();

        Hashtable GetHashOcorrenciaDescId();

        Hashtable GetHashFuncCodigoId();

        Hashtable GetHashOcorrenciaCodigoId();
    }
}
