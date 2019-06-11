using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Modelo.Utils
{
    public sealed class MinDateAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrorMessage = "A data do campo '{0}' deve ser maior que '{1}'";
        private string _strMaxDate;

        public string MinimumDateString
        {
            get { return _strMaxDate; }
            set { _strMaxDate = value; }
        }

        /// <summary>
        /// Atributo de validação para intervalo de datas com mensagens em português
        /// </summary>
        /// <param name="minimumDateString">String contendo o valor mínimo para o valor de data</param>
        public MinDateAttribute(string minimumDateString)
            : base(_defaultErrorMessage)
        {
            MinimumDateString = minimumDateString;
        }

        //Override default FormatErrorMessage Method  
        public override string FormatErrorMessage(string name)
        {
            return string.Format(_defaultErrorMessage, name, MinimumDateString);
        }

        //Override IsValid  
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime data;
            if (!DateTime.TryParse(MinimumDateString, out data))
            {
                var message = "A data limite informada está incorreta";
                return new ValidationResult(message);
            }
            else if (value == null)
            {
                var message = "O campo " + validationContext.DisplayName + " não pode ser vazio.";
                return new ValidationResult(message);
            }
            else if (data > (DateTime)value)
            {
                var message = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(message);
            }
            else
            {
                return null;
            }
        }


        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationType = "minimumdate";
            rule.ValidationParameters.Add("other", MinimumDateString);
            yield return rule;
        }
    }
}
