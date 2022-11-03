$(function() {
    var $createGlosarryForm = $("#form-create-glossary");
    $.validator.unobtrusive.parse($createGlosarryForm);

    var dialogContainerId = "newGlossaryModalDialog";
    $createGlosarryForm.on("submit",
        function(event) {
            event.preventDefault();
            event.stopImmediatePropagation();

            var $createGlosarryForm = $(this);

            submitAsyncForm($createGlosarryForm,
                function(data) {
                    if (data["success"]) {                        
                        $("#" + dialogContainerId + " .modal").modal("hide");
                        var message = data["editing"] ? "Updated successfully" : "New term/category has been created successfully";
                        quickNotification(message);
                        var grid = $("#tblGlossaryList");
                        grid.trigger('reloadGrid');
                    } else {
                        quickNotification(data["error"], "error");                        
                    }
                },
                function(xhr, ajaxOptions, thrownError) {
                    quickNotification("Error occurred.", "error");
                }
            );

            return false;
        });
});
