using System.Collections.Generic;

namespace DAL
{
    public interface IEquipamentoTipoBiometria : DAL.IDAL
    {
        Modelo.EquipamentoTipoBiometria LoadObject(int id);
        List<Modelo.EquipamentoTipoBiometria> GetAllList();
        List<Modelo.Utils.ItensCombo> GetAllList(int IdEquipamentoTipoBiometria);
    }
}
