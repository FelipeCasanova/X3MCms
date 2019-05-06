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
