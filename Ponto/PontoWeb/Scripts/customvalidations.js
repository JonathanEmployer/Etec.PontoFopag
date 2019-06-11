/// <reference path="jquery-2.1.0-vsdoc.js" />
/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="moment.js" />

jQuery.validator.addMethod("minimumdate", function (value, element, param) {
    var dataPos = moment(value, "DD/MM/YYYY");
    var strDtAnt = param.substring(1, param.length);
    var dataAnt = moment(strDtAnt, "DD/MM/YYYY");
    return dataAnt.toDate() < dataPos.toDate();
});

jQuery.validator.addMethod("greater", function (value, element, param) {
    var dataPos = moment(value, "DD/MM/YYYY");
    var dataAnt = moment($(param).val(), "DD/MM/YYYY");
    return dataAnt.toDate() <= dataPos.toDate();
});

jQuery.validator.addMethod("timemandatory", function (value, element, param) {
    return (!(value === "---:--" || value === "--:--" || value === "000:00" || value === "00:00" || value === "" || value === null || value == undefined));
});

jQuery.validator.addMethod("maxdiffrange", function (value, element, param) {
    var dataPos = moment.utc(value, "DD/MM/YYYY");
    var dataAnt = moment.utc($(param).val(), "DD/MM/YYYY");
    var intervalo = Number(element.dataset.valMaxdiffrangeRange);
    var diferenca = dataPos.diff(dataAnt, 'days');
    return diferenca <= intervalo;
});

jQuery.validator.addMethod("requiredif", function (value, element, param) {
    var valorProp = $(param).val();
    var valorEsperado = element.dataset.valRequiredifReqvalue;

    if (valorProp != undefined) {
        if (value == null || value == undefined || value == '' || value == '0') {

            if (valorProp == valorEsperado) {
                return false;
            }
            else {
                return true;
            }
        }
    }
    else { //tipo radiobutton
        var parm = param.substring(1);
        var radios = $('[name=' + parm + ']');
        radios = radios.toArray().filter(function (item, index, array) {
            if ($(item).is(':checked')) {
                return item;
            }
        });
        if (radios.length > 0) {
            if (value == null || value == undefined || value == '' || value == '0') {
                valorProp = $(radios[0]).val();
                if (valorProp == valorEsperado) {
                    return false;
                }
                else {
                    return true;
                }
            }
        }
        else {
            return true;
        }
    }
    return true;
});

jQuery.validator.unobtrusive.adapters.add("minimumdate", ["other"], function (options) {
    options.rules["minimumdate"] = "#" + options.params.other;
    options.messages["minimumdate"] = options.message;
});

jQuery.validator.unobtrusive.adapters.add("greater", ["other"], function (options) {
    options.rules["greater"] = "#" + options.params.other;
    options.messages["greater"] = options.message;
});

jQuery.validator.unobtrusive.adapters.add("maxdiffrange", ["other"], function (options) {
    options.rules["maxdiffrange"] = "#" + options.params.other;
    options.messages["maxdiffrange"] = options.message;
});

jQuery.validator.unobtrusive.adapters.add("timemandatory", ["other"], function (options) {
    options.rules["timemandatory"] = "#" + options.params.other;
    options.messages["timemandatory"] = options.message;
});

jQuery.validator.unobtrusive.adapters.add("requiredif", ["other"], function (options) {
    options.rules["requiredif"] = "#" + options.params.other;
    options.messages["requiredif"] = options.message;
});