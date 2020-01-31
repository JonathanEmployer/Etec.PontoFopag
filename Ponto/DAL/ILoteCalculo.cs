using System;
using System.Collections.Generic;
using Modelo;

namespace DAL
{
    public interface ILoteCalculo
    {
        Guid Adicionar(DateTime dataInicio, DateTime DataFim);
    }
}
