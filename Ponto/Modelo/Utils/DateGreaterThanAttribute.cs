using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Modelo.Utils
{
    public sealed class DateGreaterThanAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrorMessage = "A data de '{0}' deve ser maior que a data de '{1}'";
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


        /// <summary>
        /// Atributo de validação para intervalo de datas com mensagens em português
        /// </summary>
        /// <param name="basePropertyName">Nome da variável do objeto com a qual esta propriedade será comparada</param>
        /// <param name="clientSideDisplayPropertyName">String do nome da propriedade comparada que aparecerá no ValidationMessageFor para o Cliente</param>
        public DateGreaterThanAttribute(string basePropertyName, string clientSideDisplayPropertyName)
            : base(_defaultErrorMessage)
        {
            OtherPropertyName = basePropertyName;
            ClientSideDisplayPropertyName = clientSideDisplayPropertyName;
        }

        //Override default FormatErrorMessage Method  
        public override string FormatErrorMessage(string name)
        {
            return string.Format(_defaultErrorMessage, name, ClientSideDisplayPropertyName);
        }

        //Override IsValid  
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //Get PropertyInfo Object  
            var basePropertyInfo = validationContext.ObjectType.GetProperty(OtherPropertyName);

            //Get Value of the property  
            var startDate = basePropertyInfo.GetValue(validationContext.ObjectInstance, null);
            if (startDate == null)
            {
                return new ValidationResult("O campo " + ClientSideDisplayPropertyName + " não pode ser vazio");
            }
            if (value == null)
            {
                return new ValidationResult("O campo " + validationContext.DisplayName + " não pode ser vazio");
            }


            var thisDate = (DateTime)value;

            //Actual comparision  
            if (thisDate < (DateTime)startDate)
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
            rule.ValidationType = "greater";
            rule.ValidationParameters.Add("other", OtherPropertyName);
            yield return rule;
        }
    }
}
