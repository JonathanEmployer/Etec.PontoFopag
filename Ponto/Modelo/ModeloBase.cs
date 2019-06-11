using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Modelo.Attributes;

namespace Modelo
{
    public class ModeloBase
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        [DataTableAttribute()]
        [LoggingPrimaryKey]
        public int Id { get; set; }
        [Display(Name = "Código")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [RegularExpression(@"^\d+$", ErrorMessage = "{0} deve conter apenas números!")]
        [DataTableAttribute()]
        public int Codigo { get; set; }
        [DataTableAttribute()]
        public DateTime? Incdata { get; set; }
        [DataTableAttribute()]
        public DateTime? Inchora { get; set; }
        [DataTableAttribute()]
        public string Incusuario { get; set; }
        [DataTableAttribute()]
        public DateTime? Altdata { get; set; }
        [DataTableAttribute()]
        public DateTime? Althora { get; set; }
        [DataTableAttribute()]
        public string Altusuario { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            System.Reflection.PropertyInfo[] campos = GetType().GetProperties();

            for (int i = 0; i < campos.Length; i++)
            {
                bool comparavel = true;
                Object[] AttArray =
campos[i].GetCustomAttributes(typeof(IsComparable),
false);
                if (AttArray.Length > 0)
                {
                    IsComparable objIsCompar =
(IsComparable)AttArray[0];
                    comparavel = objIsCompar.COMPARAVEL;
                }
                if (comparavel)
                {
                    if (campos[i].GetValue(this, null) == null)
                    {
                        if (campos[i].GetValue(obj, null) != null)
                            return false;
                    }
                    var ant = campos[i].GetValue(obj, null);
                    var dep = campos[i].GetValue(this, null);
                    if (!(ant == null && dep == null))
                    {
                        if (!campos[i].GetValue(this, null).Equals(campos[i].GetValue(obj, null)))
                            return false;
                    }
                }
            }

            return true;
        }

        public bool ForcarNovoCodigo { get; set; }

        public bool NaoValidaCodigo { get; set; }

        public List<ChangeLog> GetChanges(object oldEntry)
        {
            object newEntry = this;
            List<ChangeLog> logs = new List<ChangeLog>();

            var oldType = oldEntry.GetType();
            var newType = newEntry.GetType();
            if (oldType != newType)
            {
                return logs;
            }

            var oldProperties = oldType.GetProperties();
            var newProperties = newType.GetProperties();

            var dateChanged = DateTime.Now;
            var primaryKey = (int)oldProperties.Where(x => Attribute.IsDefined(x, typeof(LoggingPrimaryKeyAttribute))).First().GetValue(oldEntry, null);
            var className = oldEntry.GetType().Name;

            foreach (var oldProperty in oldProperties)
            {
                try
                {
                    var matchingProperty = newProperties.Where(x => !Attribute.IsDefined(x, typeof(IgnoreLoggingAttribute))
                                                                            && x.Name == oldProperty.Name
                                                                            && x.PropertyType == oldProperty.PropertyType)
                                                                .FirstOrDefault();
                    if (matchingProperty == null)
                    {
                        continue;
                    }
                    object oldValue = oldProperty.GetValue(oldEntry, null);
                    object newValue = matchingProperty.GetValue(newEntry, null);
                    if (matchingProperty != null && (!(oldValue == null && newValue == null) && ((oldValue == null && newValue != null) ||
                                                                                                 (oldValue != null && newValue == null) ||
                                                                                                 !oldValue.Equals(newValue))))
                    {
                        logs.Add(new ChangeLog()
                        {
                            PrimaryKey = primaryKey,
                            DateChanged = dateChanged,
                            ClassName = className,
                            PropertyName = matchingProperty.Name,
                            OldValue = Convert.ToString(oldValue),
                            NewValue = Convert.ToString(newValue)
                        });
                    }
                }
                catch (Exception e)
                {

                    throw e;
                }
            }

            return logs;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Acao Acao { get; set; }

        public string AcaoDescricao {
            get {
                switch (this.Acao)
                {
                    case Modelo.Acao.Incluir:
                        return "inclusão";
                    case Modelo.Acao.Alterar:
                        return "alteração";
                    case Modelo.Acao.Excluir:
                        return "exclusão";
                    default:
                        return "acão desconhecia";
                }
            }
        }
    }
}
