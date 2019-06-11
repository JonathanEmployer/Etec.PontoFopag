using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Modelo.Utils;


/// <summary>
/// Annotation para obrigatoriedade condicional de uma determinada property, baseada em uma Função booleana à ser passada como parâmetro.
/// Caso a Função retorne true, o resultado é válido.
/// </summary>
public class RequiredIfAttribute : ValidationAttribute, IClientValidatable
{
    public string DependentProperty { get; set; }
    public object TargetValue { get; set; }
    public string ClientSideDisplayPropertyName { get; set; }

    public string TextoOpcional { get; set; }

    public RequiredIfAttribute(string dependentProperty, object targetValue, string clientSideDisplayPropertyName, string textoOpcional)
    {
        this.DependentProperty = dependentProperty;
        this.TargetValue = targetValue;
        this.ClientSideDisplayPropertyName = clientSideDisplayPropertyName;
        this.TextoOpcional = textoOpcional;
    }

    public override string FormatErrorMessage(string name)
    {
        if (String.IsNullOrEmpty(TextoOpcional))
        {
            return string.Format("O valor de '{0}' não pode ser vazio se o campo '{1}' for igual à '{2}'", name, ClientSideDisplayPropertyName, TargetValue.ToString());
        }
        else
        {
            return string.Format("O valor de '{0}' não pode ser vazio se o campo '{1}' for igual à '{2}'", name, ClientSideDisplayPropertyName, TextoOpcional);
        }
        
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var field = validationContext.ObjectType.GetProperty(DependentProperty);

        // get the value of the dependent property
        var val = field.GetValue(validationContext.ObjectInstance, null);

        if (value == null || String.IsNullOrEmpty(value.ToString()) || value.ToString() == "0")
        {
            if (val == null && TargetValue == null)
            {
                return null;
            }
            else if (!val.Equals(TargetValue))
            {
                return null;
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

        }
        return null;
    }

    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    {
        var rule = new ModelClientValidationRule();
        rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
        rule.ValidationType = "requiredif";
        rule.ValidationParameters.Add("other", DependentProperty);
        rule.ValidationParameters.Add("reqvalue", TargetValue);
        yield return rule;
    }
}
