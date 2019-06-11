using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Modelo.Utils
{
    public sealed class DateGreaterThanAttributeNull : ValidationAttribute, IClientValidatable
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
        public DateGreaterThanAttributeNull(string basePropertyName, string clientSideDisplayPropertyName)
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
        protected override ValidationResult IsValid
        (object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            try
            {
                // Using reflection we can get a reference to the other date property, in this example the project start date
                var containerType = validationContext.ObjectInstance.GetType();
                var field = containerType.GetProperty(this.OtherPropertyName);
                var extensionValue = field.GetValue(validationContext.ObjectInstance, null);
                if (value != null && extensionValue != null)
                {
                    var datatype = extensionValue.GetType();

                    //var otherPropertyInfo = validationContext.ObjectInstance.GetType().GetProperty(this.otherPropertyName);
                    if (field == null)
                        return new ValidationResult(String.Format("Unknown property: {0}.", OtherPropertyName));
                    // Let's check that otherProperty is of type DateTime as we expect it to be
                    if ((field.PropertyType == typeof(DateTime) ||
                         (field.PropertyType.IsGenericType && field.PropertyType == typeof(Nullable<DateTime>))))
                    {
                        DateTime toValidate = (DateTime)value;
                        DateTime referenceProperty = (DateTime)field.GetValue(validationContext.ObjectInstance, null);
                        // if the end date is lower than the start date, than the validationResult will be set to false and return
                        // a properly formatted error message
                        if (toValidate.CompareTo(referenceProperty) < 1)
                        {
                            var message = FormatErrorMessage(validationContext.DisplayName);
                            return new ValidationResult(message);
                        }
                    }
                    else
                    {
                        validationResult = new ValidationResult("Ocorreu um erro ao validar a propriedade. a propriedade utilizada para comparação não é do tipo Data");
                    }
                }
            }
            catch (Exception ex)
            {
                // Do stuff, i.e. log the exception
                // Let it go through the upper levels, something bad happened
                throw ex;
            }

            return validationResult;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules
            (ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "isgreater",
            };
            rule.ValidationParameters.Add("otherproperty", OtherPropertyName);
            yield return rule;
        }
    }
}
