using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DAL
{
    public interface IImportacaoSecullum
    {
        Hashtable GetHashFuncao();
        Hashtable GetHashFuncionario();
        Hashtable GetHashDepartamento();
    }
}
