$(function () {
    var $form = $("#box-create-edit-acc").find("form");

    $form.unbind('submit');
    $form.on("submit", function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();
        var $form = $(this);

        //Function is defined later...
        submitAsyncForm($form,
            function (data) {
                if (data["success"]) {
                    if (data["editing"])
                        quickNotification("Account has been updated successfully");
                    else {
                        quickNotification("Created new account successfully");                        
                    }

                    if (data["redirectUrl"] !== "")
                        window.location.href = "/AccountMgmt/List";
                } else {
                    quickNotification(data["message"], "error");
                }
            },
            function (xhr, ajaxOptions, thrownError) {
                quickNotification("Error occurred during creating/editing account", "error");
            }
        );

        return false;
    });

    // select2 for roles
    $("#ddlRoles").select2({
        width: "100%",
        placeholder: "Select Roles..."
    });

    // select2 for roles
    $("#ddlClaims").select2({
        width: "100%",
        placeholder: "Select Claims..."
    });

    // select2 for Organization
    $("#ddlOrganizations").select2({
        
        width: "100%",
        placeholder: "Select Organization..."
    });


    $("#btn-generate-password").on("click", function () {
        $(".txt-user-password").val(randomPassword(8));
        return false;
    });

     $("#cancel-save").confirmModal({
        confirmCallback: goToAccountMgmtList
    });
     
     $("#delete-account").confirmModal({
        confirmCallback: deleteAccountConfirmed
    });

});

function deleteAccountConfirmed() {

    var accountId =  $("#Id").val();
    var deleteAccountUrl = "/accountmgmt/DeleteAccount";
    
    $.ajax({
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        url: deleteAccountUrl,
        data: JSON.stringify({accountId: accountId})
    })
        .done(function (response, textStatus, jqXHR) {            
            if (!response.success) {
                quickNotification(response.message, "error");
                return;
            }
            quickNotification("Selected account have been removed", "info");
            goToAccountMgmtList();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            quickNotification("Error occurred removing account(s)", "error");
        })
        .always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) {
        });
}
function goToAccountMgmtList() {
    window.location.href = "/AccountMgmt/List";
}

function resetCreateEditAccountButton() {
    $(".btn-acc-details-submit").button("reset");
}

function randomPassword(length) {
    var chars = "abcdefghijklmnopqrstuvwxyz!@#$%^&*ABCDEFGHIJKLMNOP1234567890";
    var pass = "";
    for (var x = 0; x < length; x++) {
        var i = Math.floor(Math.random() * chars.length);
        pass += chars.charAt(i);
    }
    return pass;
}

