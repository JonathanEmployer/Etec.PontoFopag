using org.cesar.dmplight.watchComm.api;
using org.cesar.dmplight.watchComm.impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Dimep
{
    public class DimepBioLite : DimepPrintPoint
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
            TCPComm comm = new TCPComm(IP, 0xbb8);
            comm.SetTimeOut(0x3a98);
            _watchComm = new WatchComm(WatchProtocolType.BioLite, comm, 1, accessKey, WatchConnectionType.ConnectedMode, "");
        }
    }
}
