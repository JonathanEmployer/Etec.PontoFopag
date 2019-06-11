using cwkPontoMT.Integracao.Relogios.Dimep;
using org.cesar.dmplight.watchComm.api;
using org.cesar.dmplight.watchComm.impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Madis
{
    public class MDREPPrintPoint : DimepPrintPoint
    {
        protected override void InstanciaWatchComm()
        {
            string accessKey = "";
            if (File.Exists(CaminhoEXE() + @"\key.txt"))
            {
                StreamReader reader = File.OpenText(CaminhoEXE() + @"\key.txt");
                accessKey = reader.ReadLine();
                reader.Close();
            }
            TCPComm comm = new TCPComm(IP, 3000);
            comm.SetTimeOut(15000);
            _watchComm = new WatchComm(WatchProtocolType.PrintPoint, comm, 1, accessKey, WatchConnectionType.ConnectedMode, "01.00.0000");
        }
    }
}
