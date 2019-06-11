namespace Modelo
{
    public class Inconsistencia : Modelo.ModeloBase
    {   
    
        public string Descricao { get; set; }
        public string Legenda { get; set; }
     
        public override string ToString()
        {
            return Descricao;
        }
    }
}
