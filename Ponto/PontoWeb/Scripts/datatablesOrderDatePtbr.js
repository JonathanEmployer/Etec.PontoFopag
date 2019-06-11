// Metodo auxiliar para ajudar a ordenar a data no padrão pt
// type detection - UK date
jQuery.fn.dataTableExt.aTypes.unshift(
function (sData) {
    if (sData !== null) {
        var entry = sData.toString();
        if (entry.match(/^(0[1-9]|[12][0-9]|3[01])[\/-](0[1-9]|1[012])[\/-](19|20|21)\d\d$/)) {
            return 'uk_date';
        }
    }
    return null;
}
);

// sorting - UK date
jQuery.fn.dataTableExt.oSort['uk_date-asc'] = function (a, b) {

    var ukDatea = a.split(/[\/-]/);
    var ukDateb = b.split(/[\/-]/);

    var x = (ukDatea[2] + ukDatea[1] + ukDatea[0]) * 1;
    var y = (ukDateb[2] + ukDateb[1] + ukDateb[0]) * 1;

    if (isNaN(x)) { x = 0; }
    if (isNaN(y)) { y = 0; }

    return ((x < y) ? -1 : ((x > y) ? 1 : 0));
};

jQuery.fn.dataTableExt.oSort['uk_date-desc'] = function (a, b) {
    var ukDatea = a.split(/[\/-]/);
    var ukDateb = b.split(/[\/-]/);

    var x = (ukDatea[2] + ukDatea[1] + ukDatea[0]) * 1;
    var y = (ukDateb[2] + ukDateb[1] + ukDateb[0]) * 1;

    if (isNaN(x)) { x = 0; }
    if (isNaN(y)) { y = 0; }

    return ((x < y) ? 1 : ((x > y) ? -1 : 0));
};

$.extend(jQuery.fn.dataTableExt.oSort, {
    "date-uk-pre": function (a) {
        var x;
        try {
            var dateA = a.replace(/ /g, '').split("/");
            var day = parseInt(dateA[0], 10);
            var month = parseInt(dateA[1], 10);
            var year = parseInt(dateA[2], 10);
            var date = new Date(year, month - 1, day)
            x = date.getTime();
        }
        catch (err) {
            x = new Date().getTime();
        }

        return x;
    },

    "date-uk-asc": function (a, b) {
        return a - b;
    },

    "date-uk-desc": function (a, b) {
        return b - a;
    }
});