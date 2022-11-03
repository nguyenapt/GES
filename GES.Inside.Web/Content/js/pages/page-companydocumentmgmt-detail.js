$(function() {
    var $form = $("#newGesCompanyDocumentModalDialog").find("form");
    $.validator.unobtrusive.parse($form);

    // focus into first text input field
    $("#new-gesdocument-name").focus();

    $form.on("submit", function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();

        if ($(".btn-create-edit-document").prop("disabled")) {
            return false;
        }

        $(".btn-create-edit-document").val("Processing...");
        $(".btn-create-edit-document").prop("disabled", "disabled");
        
        var form = $(this);

        var formdata = false;
        if (window.FormData) {
            formdata = new FormData(form[0]);
        }
        var dialogContainerId = "newGesCompanyDocumentModalDialog";

        $.ajax({
            url: '/documentmgmt/CreateUpdateCompanyDocument',
            data: formdata ? formdata : form.serialize(),
            cache: false,
            contentType: false,
            processData: false,
            type: 'POST',
            success: function (data, textStatus, jqXHR) {
                // Callback code
                $("#" + dialogContainerId + " .modal").modal("hide");
                if (data["success"]) {
                    var message = data["editing"]
                        ? "Updated successfully"
                        : "New document has been created successfully";
                    quickNotification(message);
                    var grid = $("#tblcompanydocuments");
                    grid.trigger('reloadGrid');
                } else {
                    quickNotification(data["error"], "error");
                }
            },
            error: function(xhr, status, error) {
                quickNotification(data["error"], "error");
            }
            ,
            always: function ()
            {
                $(".btn-create-edit-document").prop("disabled", "");
                $(".btn-create-edit-document").val(submitBtnText);
            }
    });

        return false;
    });


    // select2 for roles
    $("#ddlOrganizations").select2({
        width: "100%",
        placeholder: "Select Organization..."
    });

});
