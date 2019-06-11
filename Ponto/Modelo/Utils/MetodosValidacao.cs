using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Utils
{
    public class MetodosValidacao
    {
        public static ValidationResult ValidaMaiorOuIgualAZero(decimal value, ValidationContext context)
        {
            bool isValid = true;

            if (value < decimal.Zero)
            {
                isValid = false;
            }

            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(
                    string.Format("O campo {0} deve possuir valor maior ou igual a 0.", context.MemberName),
                    new List<string>() { context.MemberName });
            }
        }

        public static ValidationResult ValidaMaiorQueZero(decimal value, ValidationContext context)
        {
            bool isValid = true;

            if (value < decimal.One)
            {
                isValid = false;
            }

            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(
                    string.Format("O campo {0} deve possuir valor maior ou igual a 1.", context.MemberName),
                    new List<string>() { context.MemberName });
            }
        }
    }
}
