using System;

namespace Modelo
{
    public class LayoutImportacao : Modelo.ModeloBase
    {
        public tipo Tipo { get; set; }
        public Int16 Tamanho { get; set; }
        public Int16 Posicao { get; set; }
        public Char Delimitador { get; set; }
        public campo Campo { get; set; }
        
        public LayoutImportacao()
        {
            Acao = Acao.Consultar;
        }
    }

    public enum campo
    {
        Código,
        Matrícula,
        CodFolha,
        NomeFuncionário,
        CTPS,
        CódDepto,
        DescDepto,
        CódFunção,
        DescFunção,
        DataAdmissão,
        DiaAdmissão,
        MesAdmissão,
        AnoAdmissão,
        DataDemissão,
        DiaDemissão,
        MesDemissão,
        AnoDemissão,
       // CódigoDS,
        PIS
    }

    public enum tipo 
    {
        Fixo,
        Variavel
    }
}
