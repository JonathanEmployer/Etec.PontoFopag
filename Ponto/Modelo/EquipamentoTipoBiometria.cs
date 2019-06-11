using System;
using System.Collections.Generic;
using System.Text;

namespace Modelo
{
    public class EquipamentoTipoBiometria : ModeloBase
    {
        public int Id { get; set; }
        public EquipamentoHomologado EquipamentoHomologado { get; set; }
        public TipoBiometria TipoBiometria { get; set; }
    }
}
