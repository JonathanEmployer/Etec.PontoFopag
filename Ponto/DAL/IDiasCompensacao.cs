using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public interface IDiasCompensacao : DAL.IDAL
    {

        Modelo.DiasCompensacao LoadObject(int id);
        List<Modelo.DiasCompensacao> LoadPCompensacao(int IdCompensacao);


    }

}
