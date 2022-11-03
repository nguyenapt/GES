$(function () {
    var $basicInfoForm = $("#generalInformation").find("form");
    var $changePassForm = $("#changePassword").find("form");
    $.validator.unobtrusive.parse($basicInfoForm);
    $.validator.unobtrusive.parse($changePassForm);

    $basicInfoForm.on("submit", function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();

        var $theBasicForm = $(this);

        utils.submitAsyncForm($theBasicForm,
            function (data) {
                // show success notification
                if (data["success"]) {
                    utils.quickNotification(data["message"]);
                } else {
                    utils.quickNotification(data["error"], "error");
                }
            },
            function (xhr, ajaxOptions, thrownError) {
                utils.quickNotification("Error occurred.", "error");
            }
        );

        return false;
    });

    $changePassForm.on("submit", function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();

        var $thePasswordForm = $(this);

        utils.submitAsyncForm($thePasswordForm,
            function (data) {
                // show success notification
                if (data["success"]) {
                    utils.quickNotification(data["message"]);
                } else {
                    utils.quickNotification(data["error"], "error");
                }
            },
            function (xhr, ajaxOptions, thrownError) {
                utils.quickNotification("Error occurred.", "error");
            }
        );

        return false;
    });
});

function resetSubmitUserProfileButton() {
    $(".btn-submit-user-profile").button("reset");
}

function resetChangePasswordBtn() {
    $(".btn-submit-change-password").button("reset");
}
