$(function () {
    var $updateEndorsementForm = $("#update-endorsement-form");    
    $.validator.unobtrusive.parse($updateEndorsementForm);

    $updateEndorsementForm.on("submit",
        function(event) {
            event.preventDefault();
            event.stopImmediatePropagation();

            var $updateEndorsementForm = $(this);            

            utils.submitAsyncForm($updateEndorsementForm,
                function (data) {
                    $(".btn-save-endorsement").button("reset");

                    if (data["success"]) {
                        $("#myEndorsementModal").modal("hide");

                        utils.quickNotification("Update activity form successfuly");

                        var grid = $("#" + window.currentSubGridId);
                        grid.trigger('reloadGrid');
                    } else {
                        utils.quickNotification(data["error"], "error");
                    }
                   
                });

            return false;
        });

    $("#textbox-NumberofShareDate").datepicker({
        autoclose: true
    });
});

