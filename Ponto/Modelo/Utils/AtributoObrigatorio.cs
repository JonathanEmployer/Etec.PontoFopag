using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Modelo.Utils
{
    public class Obrigatorio : System.ComponentModel.DataAnnotations.RequiredAttribute
    {
        private String displayName;

        public Obrigatorio()
        {
            this.ErrorMessage = "O campo {0} é obrigatório!";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            object attributes;
            try
            {
                attributes = validationContext.ObjectType.GetProperty(validationContext.MemberName).GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault();
            }
            catch (Exception)
            {
                attributes = null;
            }
            if (attributes != null)
                this.displayName = (attributes as DisplayNameAttribute).DisplayName;
            else
                this.displayName = validationContext.DisplayName;

            return base.IsValid(value, validationContext);
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(this.ErrorMessageString, displayName);
        }
 
    }
}