﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PontoWeb.EnvioEmail {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EmailRequest", Namespace="http://schemas.datacontract.org/2004/07/EnviaEmail")]
    [System.SerializableAttribute()]
    public partial class EmailRequest : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte[] Arq_AnexoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Cod_Email_SistemaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Des_EmailField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Des_Email_AssuntoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Des_Email_CopiaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Des_Email_Copia_OcultaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Des_Email_DestinoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Des_Email_RemetenteField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Des_Tipo_AnexoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Des_Titulo_RemetenteField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<System.DateTime> Dta_AgendamentoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Nme_Arquivo_AnexoField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] Arq_Anexo {
            get {
                return this.Arq_AnexoField;
            }
            set {
                if ((object.ReferenceEquals(this.Arq_AnexoField, value) != true)) {
                    this.Arq_AnexoField = value;
                    this.RaisePropertyChanged("Arq_Anexo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Cod_Email_Sistema {
            get {
                return this.Cod_Email_SistemaField;
            }
            set {
                if ((object.ReferenceEquals(this.Cod_Email_SistemaField, value) != true)) {
                    this.Cod_Email_SistemaField = value;
                    this.RaisePropertyChanged("Cod_Email_Sistema");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Des_Email {
            get {
                return this.Des_EmailField;
            }
            set {
                if ((object.ReferenceEquals(this.Des_EmailField, value) != true)) {
                    this.Des_EmailField = value;
                    this.RaisePropertyChanged("Des_Email");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Des_Email_Assunto {
            get {
                return this.Des_Email_AssuntoField;
            }
            set {
                if ((object.ReferenceEquals(this.Des_Email_AssuntoField, value) != true)) {
                    this.Des_Email_AssuntoField = value;
                    this.RaisePropertyChanged("Des_Email_Assunto");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Des_Email_Copia {
            get {
                return this.Des_Email_CopiaField;
            }
            set {
                if ((object.ReferenceEquals(this.Des_Email_CopiaField, value) != true)) {
                    this.Des_Email_CopiaField = value;
                    this.RaisePropertyChanged("Des_Email_Copia");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Des_Email_Copia_Oculta {
            get {
                return this.Des_Email_Copia_OcultaField;
            }
            set {
                if ((object.ReferenceEquals(this.Des_Email_Copia_OcultaField, value) != true)) {
                    this.Des_Email_Copia_OcultaField = value;
                    this.RaisePropertyChanged("Des_Email_Copia_Oculta");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Des_Email_Destino {
            get {
                return this.Des_Email_DestinoField;
            }
            set {
                if ((object.ReferenceEquals(this.Des_Email_DestinoField, value) != true)) {
                    this.Des_Email_DestinoField = value;
                    this.RaisePropertyChanged("Des_Email_Destino");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Des_Email_Remetente {
            get {
                return this.Des_Email_RemetenteField;
            }
            set {
                if ((object.ReferenceEquals(this.Des_Email_RemetenteField, value) != true)) {
                    this.Des_Email_RemetenteField = value;
                    this.RaisePropertyChanged("Des_Email_Remetente");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Des_Tipo_Anexo {
            get {
                return this.Des_Tipo_AnexoField;
            }
            set {
                if ((object.ReferenceEquals(this.Des_Tipo_AnexoField, value) != true)) {
                    this.Des_Tipo_AnexoField = value;
                    this.RaisePropertyChanged("Des_Tipo_Anexo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Des_Titulo_Remetente {
            get {
                return this.Des_Titulo_RemetenteField;
            }
            set {
                if ((object.ReferenceEquals(this.Des_Titulo_RemetenteField, value) != true)) {
                    this.Des_Titulo_RemetenteField = value;
                    this.RaisePropertyChanged("Des_Titulo_Remetente");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<System.DateTime> Dta_Agendamento {
            get {
                return this.Dta_AgendamentoField;
            }
            set {
                if ((this.Dta_AgendamentoField.Equals(value) != true)) {
                    this.Dta_AgendamentoField = value;
                    this.RaisePropertyChanged("Dta_Agendamento");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nme_Arquivo_Anexo {
            get {
                return this.Nme_Arquivo_AnexoField;
            }
            set {
                if ((object.ReferenceEquals(this.Nme_Arquivo_AnexoField, value) != true)) {
                    this.Nme_Arquivo_AnexoField = value;
                    this.RaisePropertyChanged("Nme_Arquivo_Anexo");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="EnvioEmail.IEnviaEmail")]
    public interface IEnviaEmail {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEnviaEmail/EnviaEmail", ReplyAction="http://tempuri.org/IEnviaEmail/EnviaEmailResponse")]
        void EnviaEmail(string _Des_Email_Remetente, string _Des_Email_Destino, string _Des_Email_Assunto, string _Des_Email, string _Nme_Arquivo_Anexo, byte[] _Arq_Anexo, string _Des_Titulo_Remetente, string _Des_Email_Copia, string _Des_Email_Copia_Oculta, string _Des_Tipo_Anexo, string _Cod_Email_Sistema, System.Nullable<System.DateTime> _Dta_Agendamento);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEnviaEmail/EnviaEmail", ReplyAction="http://tempuri.org/IEnviaEmail/EnviaEmailResponse")]
        System.Threading.Tasks.Task EnviaEmailAsync(string _Des_Email_Remetente, string _Des_Email_Destino, string _Des_Email_Assunto, string _Des_Email, string _Nme_Arquivo_Anexo, byte[] _Arq_Anexo, string _Des_Titulo_Remetente, string _Des_Email_Copia, string _Des_Email_Copia_Oculta, string _Des_Tipo_Anexo, string _Cod_Email_Sistema, System.Nullable<System.DateTime> _Dta_Agendamento);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEnviaEmail/EnviaListEmail", ReplyAction="http://tempuri.org/IEnviaEmail/EnviaListEmailResponse")]
        void EnviaListEmail(PontoWeb.EnvioEmail.EmailRequest[] lstEmail);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEnviaEmail/EnviaListEmail", ReplyAction="http://tempuri.org/IEnviaEmail/EnviaListEmailResponse")]
        System.Threading.Tasks.Task EnviaListEmailAsync(PontoWeb.EnvioEmail.EmailRequest[] lstEmail);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEnviaEmail/EnviaEmailArquivo", ReplyAction="http://tempuri.org/IEnviaEmail/EnviaEmailArquivoResponse")]
        void EnviaEmailArquivo(byte[] arquivo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEnviaEmail/EnviaEmailArquivo", ReplyAction="http://tempuri.org/IEnviaEmail/EnviaEmailArquivoResponse")]
        System.Threading.Tasks.Task EnviaEmailArquivoAsync(byte[] arquivo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEnviaEmail/EnviaEmailDataSet", ReplyAction="http://tempuri.org/IEnviaEmail/EnviaEmailDataSetResponse")]
        void EnviaEmailDataSet(System.Data.DataSet arquivo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEnviaEmail/EnviaEmailDataSet", ReplyAction="http://tempuri.org/IEnviaEmail/EnviaEmailDataSetResponse")]
        System.Threading.Tasks.Task EnviaEmailDataSetAsync(System.Data.DataSet arquivo);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IEnviaEmailChannel : PontoWeb.EnvioEmail.IEnviaEmail, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EnviaEmailClient : System.ServiceModel.ClientBase<PontoWeb.EnvioEmail.IEnviaEmail>, PontoWeb.EnvioEmail.IEnviaEmail {
        
        public EnviaEmailClient() {
        }
        
        public EnviaEmailClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EnviaEmailClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EnviaEmailClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EnviaEmailClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void EnviaEmail(string _Des_Email_Remetente, string _Des_Email_Destino, string _Des_Email_Assunto, string _Des_Email, string _Nme_Arquivo_Anexo, byte[] _Arq_Anexo, string _Des_Titulo_Remetente, string _Des_Email_Copia, string _Des_Email_Copia_Oculta, string _Des_Tipo_Anexo, string _Cod_Email_Sistema, System.Nullable<System.DateTime> _Dta_Agendamento) {
            base.Channel.EnviaEmail(_Des_Email_Remetente, _Des_Email_Destino, _Des_Email_Assunto, _Des_Email, _Nme_Arquivo_Anexo, _Arq_Anexo, _Des_Titulo_Remetente, _Des_Email_Copia, _Des_Email_Copia_Oculta, _Des_Tipo_Anexo, _Cod_Email_Sistema, _Dta_Agendamento);
        }
        
        public System.Threading.Tasks.Task EnviaEmailAsync(string _Des_Email_Remetente, string _Des_Email_Destino, string _Des_Email_Assunto, string _Des_Email, string _Nme_Arquivo_Anexo, byte[] _Arq_Anexo, string _Des_Titulo_Remetente, string _Des_Email_Copia, string _Des_Email_Copia_Oculta, string _Des_Tipo_Anexo, string _Cod_Email_Sistema, System.Nullable<System.DateTime> _Dta_Agendamento) {
            return base.Channel.EnviaEmailAsync(_Des_Email_Remetente, _Des_Email_Destino, _Des_Email_Assunto, _Des_Email, _Nme_Arquivo_Anexo, _Arq_Anexo, _Des_Titulo_Remetente, _Des_Email_Copia, _Des_Email_Copia_Oculta, _Des_Tipo_Anexo, _Cod_Email_Sistema, _Dta_Agendamento);
        }
        
        public void EnviaListEmail(PontoWeb.EnvioEmail.EmailRequest[] lstEmail) {
            base.Channel.EnviaListEmail(lstEmail);
        }
        
        public System.Threading.Tasks.Task EnviaListEmailAsync(PontoWeb.EnvioEmail.EmailRequest[] lstEmail) {
            return base.Channel.EnviaListEmailAsync(lstEmail);
        }
        
        public void EnviaEmailArquivo(byte[] arquivo) {
            base.Channel.EnviaEmailArquivo(arquivo);
        }
        
        public System.Threading.Tasks.Task EnviaEmailArquivoAsync(byte[] arquivo) {
            return base.Channel.EnviaEmailArquivoAsync(arquivo);
        }
        
        public void EnviaEmailDataSet(System.Data.DataSet arquivo) {
            base.Channel.EnviaEmailDataSet(arquivo);
        }
        
        public System.Threading.Tasks.Task EnviaEmailDataSetAsync(System.Data.DataSet arquivo) {
            return base.Channel.EnviaEmailDataSetAsync(arquivo);
        }
    }
}