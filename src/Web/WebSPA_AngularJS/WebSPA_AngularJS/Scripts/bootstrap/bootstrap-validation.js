//<div class="input-group">
//    <span class="input-group-addon glyphicon glyphicon-user"> </span>
//    <input class="form-control input-validation-error" data-val="true" data-val-required="El campo Nombre es obligatorio." id="Usuario_Nombre" name="Usuario.Nombre" placeholder="Introduzca su nombre" type="text" value="">
//    <span class="field-validation-error" data-valmsg-for="Usuario.Nombre" data-valmsg-replace="true"><span for="Usuario_Nombre" class="">El campo Nombre es obligatorio.</span></span>
//</div>

function bootstrapValidation() {
    
    $('form').each(function () {
        var $form = $(this);
        var validator = $form.data('validator');

        validator.settings.errorPlacement = $.proxy(onError, $form);
        validator.settings.success = $.proxy(onSuccess, $form)

        function escapeAttributeValue(value) {
            // As mentioned on http://api.jquery.com/category/selectors/
            return value.replace(/([!"#$%&'()*+,./:;<=>?@\[\\\]^`{|}~])/g, "\\$1");
        }

        function onError(error, inputElement) {
            //Original code of the onError function inside jquery.validate.unobtrusive
            //------------------------------------------------------------------------
            var container = $(this).find("[data-valmsg-for='" + escapeAttributeValue(inputElement[0].name) + "']"),
            replaceAttrValue = container.attr("data-valmsg-replace"),
            replace = replaceAttrValue ? $.parseJSON(replaceAttrValue) !== false : null;

            container.removeClass("field-validation-valid").addClass("field-validation-error");
            error.data("unobtrusiveContainer", container);

            var popover = $(inputElement);
            if(popover.attr('popovercontainer') == 'true')
            {
                popover = $(inputElement).closest('.input-group');
            }
            error.data("unobtrusiveContainerPopover", popover);

            if (replace && error.html() != "" && $(inputElement).is(":visible")) {
                container.empty();
                
                $(popover).attr('data-container', 'body');
                $(popover).attr('data-toggle', 'popover');
                $(popover).attr('data-placement', 'right');
                $(popover).attr('data-content', error.html());
                $(popover).attr('data-original-title', 'Error');
                $(popover).popover({trigger: "manual", html: true});
                $(popover)
                    .data('bs.popover')
                    .tip()
                    .addClass('alert alert-danger');
                $(popover).popover("show");

                $('button[data-loading-text]').each(function () {
                    var btn = $(this);
                    $(btn).button('reset')
                }); 

                $(inputElement).closest('.modal').on('shown.bs.modal', function (e) {
                    $(inputElement).valid();
                })

                $(inputElement).closest('.modal').on('hide.bs.modal', function (e) {
                    $(popover).popover("destroy");
                })
            }
            else {
                container.empty();
            }

        }

        function onSuccess(error) {  // 'this' is the form element
            var container = error.data("unobtrusiveContainer"),
                popover = error.data("unobtrusiveContainerPopover"),
                replaceAttrValue = container.attr("data-valmsg-replace"),
                replace = replaceAttrValue ? $.parseJSON(replaceAttrValue) : null;

            if (container) {
                container.addClass("field-validation-valid").removeClass("field-validation-error");
                error.removeData("unobtrusiveContainer");

                if (replace) {
                    $(popover).popover("destroy");
                    container.empty();
                }
            }
        }
    });

}

function handleOk(message)
{
    handleMessage(message, "success");
}

function handleInfo(message)
{
    handleMessage(message, "info");
}

function handleWarning(message)
{
    handleMessage(message, "warning");
}

function handleError(message)
{
    handleMessage(message, "danger");
}

function handleMessage(message, type)
{
    var target = $("#ErrorPlaceHolder");
    if($(".ErrorModalPlaceHolder").is(":visible") == true)
    {
        target = $(".ErrorModalPlaceHolder:visible")[0];
    }

    if(message)
    {
        $(target).html("");
        $('<div class="alert alert-' + type + ' alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>'
            + message + '</div>').appendTo($(target));
    }
}

$(function () {
    
    $(document).ajaxError(
        function(e,request) {
            handleError(request.responseText);
        }
    );

    $("form").removeData("validator");
    $("form").removeData("unobtrusiveValidation");
    $("form").removeData("unobtrusiveContainerPopover");
    $.validator.unobtrusive.parse("form");

    bootstrapValidation();

});
