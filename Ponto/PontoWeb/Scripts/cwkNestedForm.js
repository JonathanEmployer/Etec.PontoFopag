function removeNestedForm(element, container, deleteElement) {
    $container = $(element).parents(container);
    $container.find(deleteElement).val('True');
    $container.hide();
}

function addNestedForm(container, counter, ticks, content, optionalIndex) {
    var nextIndex = $(counter).length;
    var pattern = new RegExp(ticks, "gi");
    content = content.replace(pattern, nextIndex);
    $(container).append(content);
    corrigeDatePicker();
    $(container).find('input:visible:last').focus();
}

function addNestedFormV2(botaoAdd, container, counter, ticks, content, divContainerElementMaster, ticksMaster) {
    var nextIndex = 0;
    if (divContainerElementMaster != null && divContainerElementMaster != '' && divContainerElementMaster != undefined) {
        var elementoPai = $(botaoAdd).parents(divContainerElementMaster + ':first');
        container = elementoPai.find(container);
        if (ticksMaster > 0) {
            var patternMaster = new RegExp(ticksMaster, "gi");
        }

        content = content.replace(patternMaster, elementoPai.attr('idGrupo'));
        nextIndex = elementoPai.find(counter).length;
    }
    else { nextIndex = $(counter).length; }

    var pattern = new RegExp(ticks, "gi");
    content = content.replace(pattern, nextIndex);
    $(container).append(content);
    corrigeDatePicker();
    $(container).find('input:visible:last').focus();
}