using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Modelo.Utils
{
    public sealed class HorarioObrigatorioAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrorMessage = "O Horário de '{0}' não pode ser vazio";

        /// <summary>
        /// Atributo de validação para horários
        /// </summary>
        public HorarioObrigatorioAttribute()
            : base(_defaultErrorMessage)
        {
        }

        //Override default FormatErrorMessage Method  
        public override string FormatErrorMessage(string name)
        {
            return string.Format(_defaultErrorMessage, name);
        }

        //Override IsValid  
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string val = value.ToString();
            if (String.IsNullOrEmpty(val))
            {
                var message = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(message);
            }
            else if (val == "--:--" || val == "---:--" || val == "00:00" || val == "000:00")
            {
                var message = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(message);
            }
            //Default return - This means there were no validation error  
            return null;
        }


        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationType = "timemandatory";
            rule.ValidationParameters.Add("other", metadata.PropertyName);
            yield return rule;
        }
    }
}
