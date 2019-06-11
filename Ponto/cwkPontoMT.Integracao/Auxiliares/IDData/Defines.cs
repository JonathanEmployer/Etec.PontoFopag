using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Auxiliares.IDData
{
    public static class CDefines
    {
        public const string DLL_VERSION = "1.5.0.1";
        public const string SW_TITLE = "IDSysR30 for C#";
    }

    #region enum

    public enum enuDDL_IDSysR30_Functions
    {
        None,
        AddUser,
        ChangeUserData,
        DeleteUser,
        ReadUserData,
        ReadEmployerData,
        SetEmployer,
        SetDateTime,
        SetREPCommunication,
        ReadREPCommunication,
        RequestEventByNSR,
        RequestNFR,
        RequestTotalNSR,
        RequestTotalUsers,
        RequestUserByIndex,
        PacketAvail,
        GetUserData,
        GetEmployerData,
        GetLogType2,
        GetLogType3,
        GetLogType4,
        GetLogType5,
        GetREPCommunication,
        GetNFR,
        GetTotalNSR,
        GetTotalUsers,
        IsHamsterConnected,
        GetTemplates_FIM01,
        GetTemplates_FIM10,
        FIM01_To_FIM10,
        FIM10_To_FIM01,
        CreateFile,
        FileOpen,
        FileClose,
        EncryptBuffer,
        DecryptBuffer,
        ExportUbsUser,
        ImportUbsUser,
        FileOpenAFD,
        ImportAFD
    }

    public enum enuConnectionState
    {
        None,
        AttemptConnection,
        AttemptConnectionFail,
        Connected,
        SendingData,
        DataReceived,
        Disconnected,
        DataReceivedError,
        ConnectionError
    }

    #endregion
}
