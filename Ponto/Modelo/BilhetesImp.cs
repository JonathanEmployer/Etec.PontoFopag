using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Modelo.Utils;

namespace Modelo
{
    public class BilhetesImp : Modelo.ModeloBase
    {
        public BilhetesImp()
        {
            Mapper.CreateMap<BilhetesImp, BilhetesImp>();

            Ordem = String.Empty;
            Hora = String.Empty;
            Func = String.Empty;
            Relogio = String.Empty;
            Mar_data = new DateTime();
            Mar_hora = String.Empty;
            Mar_relogio = String.Empty;
            Ent_sai = String.Empty;
            Chave = String.Empty;
            DsCodigo = String.Empty;
            Motivo = String.Empty;
            Incdata = new DateTime();
            Inchora = new DateTime();
            Incusuario = String.Empty;
            Altdata = new DateTime();
            Althora = new DateTime();
            Altusuario = String.Empty;
        }

        public BilhetesImp Clone(BilhetesImp objClone)
        {
            return Mapper.Map<BilhetesImp, BilhetesImp>(objClone);
        }

        public List<BilhetesImp> Clone(List<BilhetesImp> listClone)
        {
            List<BilhetesImp> clonados = new List<BilhetesImp>();
            listClone.ForEach(f => clonados.Add(f.Clone(f)));
            return clonados;
        }

        /// <summary>
        /// Ordem do Bilhete
        /// </summary>
        public string Ordem { get; set; }
        /// <summary>
        /// Data Importada
        /// </summary>
        public DateTime Data { get; set; }
        /// <summary>
        /// Hora Importada
        /// </summary>
        public string Hora { get; set; }
        /// <summary>
        /// Codigo do Funcionário
        /// </summary>
        public string Func { get; set; }
        /// <summary>
        /// Número do Relógio
        /// </summary>
        [Display(Name = "Relógio")]
        public string Relogio { get; set; }
        /// <summary>
        /// Flag que Verifica se o Bilhete foi gravado na tblMarcação
        /// </summary>
        public Int16 Importado { get; set; }
        
        /// <summary>
        /// Data da Marcação
        /// </summary>
        [Display(Name="Data")]
        public DateTime? Mar_data
        {
            get;
            set;
        }
        /// <summary>
        /// Hora da Marcação
        /// </summary>
        [Display(Name = "Hora")]
        public string Mar_hora { get; set; }
        /// <summary>
        /// Número do Relógio da Marcação
        /// </summary>
        [Display(Name = "Relógio")]
        public string Mar_relogio { get; set; }
        /// <summary>
        /// Posição Entrada e Saida da Marcação
        /// </summary>
        [Display(Name = "Posição")]
        public int Posicao { get; set; }
        /// <summary>
        /// Tratamento da Ordem do Bilhete
        /// </summary>
        public string Ent_sai { get; set; }

        /// <summary>
        /// Chave para validar se os dados do bilhete não foram manipulados
        /// </summary>
        public string Chave { get; set; }

        /// <summary>
        /// DsCodigo da Marcação
        /// </summary>
        public string DsCodigo { get; set; }

        /// <summary>
        /// Ocorrência da Marcação
        /// </summary>
        [Display(Name = "Ocorrência")]
        public char Ocorrencia { get; set; }
        /// <summary>
        /// Motivo da Ocorrência
        /// </summary>
        public string Motivo { get; set; }

        /// <summary>
        /// Identificação da Justificativa
        /// </summary>
        public int Idjustificativa { get; set; }
        /// <summary>
        /// Identificação da Justificativa para o ponto web, composto por codigo | descrição
        /// </summary>
        [Display(Name = "Justificativa")]
        public string DescJustificativa { get; set; }

        [Display(Name="Manutenção")]
        public int tipoManutencao { get; set; }

        public int idMarcacao { get; set; }

        /// <summary>
        /// Gera o Código da Chave
        /// </summary>
        /// <returns></returns>
        public string ToMD5()
        {
            string chave = "cwork" + Ordem + String.Format("{0:dd/MM/yyyy}", Data) + Hora + Func + "sistemas" + Relogio + Importado + String.Format("{0:dd/MM/yyyy}", Mar_data) + Mar_hora + "ltda" + Mar_relogio + Posicao + Ent_sai + "me";
            return MD5HashGenerator.GenerateKey(chave);
        }

        /// <summary>
        /// Verifica o Código da Chave
        /// </summary>
        /// <returns></returns>
        public bool BilheteOK()
        {
            try
            {
                string aux = this.ToMD5();
                return (aux == this.Chave);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int Nsr { get; set; }
        public int? IdLancamentoLoteFuncionario { get; set; }
        public int IdFuncionario { get; set; }
        public string PIS { get; set; }

        /// <summary>
        /// Campo utilizado para order entrada e saida + posicao como numero para ordenação de registros. Se tiver desconsiderada joga para o final da ordenação.
        /// Ex: Tipo | Posicao | Fica
        ///        E | 1       | 11
        ///        S | 1       | 12
        ///        E | 2       | 21
        ///        S | 2       | 22
        /// </summary>
        private int ordenacaoPosicaoEnt_Sai;
        public int OrdenacaoPosicaoEnt_Sai
        {
            get
            {
                int entsainum = Ent_sai == "E" ? 1 : 2;
                int desconsiderada = Ocorrencia == 'D' ? 1000 : 10;
                return (Posicao * desconsiderada) + entsainum;
            }
            set
            {
                ordenacaoPosicaoEnt_Sai = value;
            }
        }

        public int? IdRegistroPonto { get; set; }

        [IsComparable(false)]
        public virtual RegistroPonto RegistroPonto { get; set; }

        [IsComparable(false)]
        public LocalizacaoRegistroPonto localizacaoRegistroPonto { get; set; }

        public bool BilheteIsEqual(BilhetesImp old)
        {
            if (
                this.Data != old.Data ||
                this.DsCodigo != old.DsCodigo ||
                this.Ent_sai != old.Ent_sai ||
                this.Func != old.Func ||
                this.Hora != old.Hora ||
                this.idMarcacao != old.idMarcacao ||
                this.Mar_data != old.Mar_data ||
                this.Mar_hora != old.Mar_hora ||
                this.Mar_relogio != old.Mar_relogio ||
                this.Ordem != old.Ordem ||
                this.Posicao != old.Posicao ||
                this.Relogio != old.Relogio ||
                this.Importado != old.Importado
            )
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
