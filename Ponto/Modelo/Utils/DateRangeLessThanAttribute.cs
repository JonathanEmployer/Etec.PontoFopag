using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Modelo.Utils
{
    public sealed class DateRangeLessThanAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrorMessage = "A diferença em dias entre '{0}' e '{1}' deve ser menor que '{2}' dias.";
        private string _otherPropertyName;

        public string OtherPropertyName
        {
            get { return _otherPropertyName; }
            set { _otherPropertyName = value; }
        }

        private string _ClientSideDisplayPropertyName;

        public string ClientSideDisplayPropertyName
        {
            get { return _ClientSideDisplayPropertyName; }
            set { _ClientSideDisplayPropertyName = value; }
        }

        private int _MaxRangeInDays;

        public int MaxRangeInDays
        {
            get { return _MaxRangeInDays; }
            set { _MaxRangeInDays = value; }
        }



        /// <summary>
        /// Atributo de validação para intervalo de datas com mensagens em português
        /// </summary>
        /// <param name="maxRangeInDays">Valor máximo que de diferença que pode existir entre as datas comparadas</param>
        /// <param name="basePropertyName">Nome da variável do objeto com a qual esta propriedade será comparada</param>
        /// <param name="clientSideDisplayPropertyName">String do nome da propriedade comparada que aparecerá no ValidationMessageFor para o Cliente</param>
        public DateRangeLessThanAttribute(int maxRangeInDays, string basePropertyName, string clientSideDisplayPropertyName)
            : base(_defaultErrorMessage)
        {
            OtherPropertyName = basePropertyName;
            ClientSideDisplayPropertyName = clientSideDisplayPropertyName;
            MaxRangeInDays = maxRangeInDays;
        }

        //Override default FormatErrorMessage Method  
        public override string FormatErrorMessage(string name)
        {
            return string.Format(_defaultErrorMessage, name, ClientSideDisplayPropertyName, MaxRangeInDays);
        }

        //Override IsValid  
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //Get PropertyInfo Object  
            var basePropertyInfo = validationContext.ObjectType.GetProperty(OtherPropertyName);

            //Get Value of the property  
            var startDate = (DateTime)basePropertyInfo.GetValue(validationContext.ObjectInstance, null);


            var thisDate = value != null ? (DateTime)value : DateTime.Now;

            //Actual comparision  
            if (thisDate <= startDate)
            {
                TimeSpan ts = startDate - thisDate;
                if (ts.Days > MaxRangeInDays)
                {
                    var message = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(message);
                }
                
            }
            else
            {
                TimeSpan ts = thisDate - startDate;
                if (ts.Days > MaxRangeInDays)
                {
                    var message = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(message);
                }
            }

            return null;
        }


        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationType = "maxdiffrange";
            rule.ValidationParameters.Add("other", OtherPropertyName);
            rule.ValidationParameters.Add("range", MaxRangeInDays);
            yield return rule;
        }
    }
}
