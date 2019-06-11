/*
 * Localized default methods for the jQuery validation plugin.
 * Locale: PT_BR
 */
jQuery.extend(jQuery.validator.methods, {
    date: function (value, element) {
        return this.optional(element) || /^\d\d?\/\d\d?\/\d\d\d?\d?$/.test(value);
    },
    number: function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$/.test(value);
    }
});


$.validator.addMethod('date',
    function (value, element, params) {
        if (this.optional(element)) {
            return true;
        }

        var result = true;
        try {
            $.datepicker.parseDate('dd/mm/yy', value);
        } catch (e) {
            result = false;
        }

        return result;
    }
)