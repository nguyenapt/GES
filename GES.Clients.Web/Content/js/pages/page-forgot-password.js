$(function () {
    var $forgotPasswordForm = $("#form-forgot-password");
    $.validator.unobtrusive.parse($forgotPasswordForm);

    $forgotPasswordForm.on("submit", function(event) {
            event.preventDefault();
            event.stopImmediatePropagation();

            var $theForgotPasswordForm = $(this);

            utils.submitAsyncForm($theForgotPasswordForm,
                function(data) {
                    // show success notification
                    if (data["success"]) {
                        window.location.href= "ForgotPasswordConfirm";
                    } else {
                        utils.quickNotification(data["error"], "error");
                    }
                },
                function(xhr, ajaxOptions, thrownError) {
                    utils.quickNotification("Unknown error occurred. Please try again.", "error");
                }
            );

            return false;
        });

});


function resetForgotPasswordBtn() {
    $("#btn-forgot-password").button("reset");
}