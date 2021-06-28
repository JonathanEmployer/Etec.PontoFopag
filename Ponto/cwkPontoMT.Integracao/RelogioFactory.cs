using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao
{
    public class RelogioFactory
    {
        public static Relogio GetRelogio(TipoRelogio tipoRelogio)
        {
            switch (tipoRelogio)
            {
                case TipoRelogio.InnerRep:
                    return new Relogios.TopData.InnerRep();
                case TipoRelogio.Orion6:
                    return new Relogios.Henry.Orion6();
                case TipoRelogio.RepTrilobit:
                    return new Relogios.REPTrilobit();
                case TipoRelogio.Henry:
                    return new Relogios.Henry.PrismaSuperFacil();
                case TipoRelogio.ControlID:
                    return new Relogios.ControlID.IDXBio();
                case TipoRelogio.ZPM_R130:
                    return new Relogios.ZPM.ZPM_R130();
                case TipoRelogio.REP_BI_01:
                    return new Relogios.IDData.IDRep_BI01();
                case TipoRelogio.Kurumin_REP_II_Max:
                    return new Relogios.Proveu.KuruminRepIIMax();
                case TipoRelogio.BioProx_C:
                    return new Relogios.RWTech.Pointline_BIOPROX_C();
                case TipoRelogio.Dimep_PrintPoint:
                    return new Relogios.Dimep.DimepPrintPoint();
                case TipoRelogio.Dimep_MiniPrint:
                    throw new NotImplementedException();
                case TipoRelogio.Dimep_BioLite:
                    throw new NotImplementedException();
                case TipoRelogio.Dimep_BioPoint:
                    throw new NotImplementedException();
                case TipoRelogio.DIXI_IDNOX:
                    return new Relogios.DIXI.IDNOX();
                case TipoRelogio.InnerRepBarras2i:
                    return new Relogios.TopData.InnerRep();
                case TipoRelogio.Henry_Hexa:
                    return new Relogios.Henry.Hexa();
                case TipoRelogio.Ahgora:
                    return new Relogios.Ahgora.Ah10();
                case TipoRelogio.MDREPPrintPoint:
                    return new Relogios.Madis.MDREPPrintPoint();
                case TipoRelogio.MDREPMiniPrint:
                    return new Relogios.Madis.MDREPMiniPrint();
                case TipoRelogio.MDREPPrintPointLi:
                    return new Relogios.Madis.MDREPPrintPointLi();
                case TipoRelogio.InnerRepPlus:
                    return new Relogios.TopData.InnerRepPlus();
                case TipoRelogio.MDREPPrintPointIII:
                    return new Relogios.Dimep.DimepPrintPointIII();
                case TipoRelogio.Telematica:
                    return new Relogios.Telematica.ConexRep();
                case TipoRelogio.IDClass:
                    return new Relogios.ControlID.IDClass();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
