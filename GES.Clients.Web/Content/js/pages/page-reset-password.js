$(function () {
    var $resetPasswordForm = $("#form-reset-password");
    $.validator.unobtrusive.parse($resetPasswordForm);

    $resetPasswordForm.on("submit", function(event) {
            event.preventDefault();
            event.stopImmediatePropagation();

            var $theResetPasswordForm = $(this);

            utils.submitAsyncForm($theResetPasswordForm,
                function(data) {
                    // show success notification
                    if (data["success"]) {
                        window.location.href= "ResetPasswordConfirm";
                    } else {
                        utils.quickNotification(data["error"] + " Please try again!", "error");
                    }
                },
                function(xhr, ajaxOptions, thrownError) {
                    utils.quickNotification("Error occurred.", "error");
                }
            );

            return false;
        });

});


function resetResetPasswordBtn() {
    $("#btn-reset-password").button("reset");
}