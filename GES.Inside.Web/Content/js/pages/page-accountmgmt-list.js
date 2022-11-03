$(function () {
    var postUrl = "/accountmgmt/GetDataForGesUsersJqGrid";
    var deleteAccountUrl = "/accountmgmt/DeleteAccount";
    var gridCaption = "";

    var grid = $("#tblgesusers");
    $.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = "Bootstrap";
    grid.bind("jqGridLoadComplete", function (e, rowid, orgClickEvent) {
        $(window).resize();
    });

    grid.bind("jqGridSelectRow", function (e, rowid, orgClickEvent) {
        manageDeleteButton();
    });
    grid.jqGrid({
        url: postUrl,
        datatype: "json",
        mtype: "post",
        colNames: ["Id", "User Name", "CS User Name", "Email", "First Name", "Last Name", "Organization", "Roles", "Status", "Claims"],
        colModel: [
            { name: "Id", width: "35px", align: "right", hidden: true, search: false,key: true },
            {
                name: "UserName", searchoptions: {
                    searchOperators: true
                },
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    return cellPrefix + "<a href=\"/AccountMgmt/Details?id=" + rowObject.Id + "\">" + cellvalue + "</a>";
                }
            },
            { name: "OldUserName" },
            { name: "Email"},
            { name: "FirstName" },
            { name: "LastName" },
            { name: "OrgName" },
            {
                name: "RoleList", search: false,
                formatter: function (cellvalue, options, rowObject) {
                    return roleIdsToRoleNames(cellvalue);
                }
            },
            { name: "Status", width: "60px", search: false, align: "center" },
            { name: "ClaimsString"}
        ],
        pager: $("#myPager"),
        rowNum: 50,
        rowList: [20, 50, 100],
        autowidth: true,
        shrinkToFit: true,
        toppager: true,
        rownumbers: true,
        gridview: true,
        height: "auto",
        viewrecords: true,
        caption: gridCaption,
        sortname: "RoleList",
        sortorder: "desc"
    });
    setBooleanSelect.call(grid, "LockoutEnabled", "All");
    grid.jqGrid("navGrid", "#myPager", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });

    grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: false });

    $("#btn-sync-users").on("click", function (e) {
        // not allow to click if disabled
        if ($(this).prop("disabled")) {
            return false;
        }

        $(this).val("Processing...");

        // disable this button temporarily
        // until all ajax components have been loaded, or failed
        $(this).prop("disabled", "disabled");

        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "GET",
            url: "/AccountMgmt/SyncUsers"
        })
                    .done(function (response, textStatus, jqXHR) {
                        //if (response.success) {
                        //    quickNotification("Successfully updated users");
                        //} else {
                        //    quickNotification(response.message, "error");
                        //}
                        quickNotification("Added acounts: " + response.AddedItems + " "
                        + "<br/> Existed email: " + response.ExistedEmail + " "
                        + "<br/> Duplicated email: " + response.DuplicatedEmail + " "
                        + "<br/> Password synchronized : " + response.PasswordSync + " ");
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        quickNotification("Failed: Error occurred updating users", "error");
                    })
                    .always(function () {
                        $("#btn-sync-users").prop("disabled", "");
                        $("#btn-sync-users").val("Sync Users");
                    });
        return false;
    });


    $(".delete-account").on("click", function () {
        var deletedId = this.id;
        $(".ui-state-highlight").each(function () { deletedId = this.id });
        
        bootbox.confirm("Are you sure you want to delete selected account?",
            function (result) {
                if (result) {
                    $.ajax({
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        type: "POST",
                        url: deleteAccountUrl,
                        data: JSON.stringify({accountId: deletedId})
                    })
                        .done(function (response, textStatus, jqXHR) {
                            $(".batch-delete-glossary").button("reset");
                            if (!response.success) {
                                quickNotification(response.message, "error");
                                return;
                            }
                            quickNotification("Selected account have been removed", "info");
                            grid.trigger("reloadGrid");
                            $(".delete-account").prop("disabled", true);
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            quickNotification("Error occurred removing account(s)", "error");
                        })
                        .always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) {
                        });
                }
            });
    });

});

function manageDeleteButton() {
    if ($(".ui-state-highlight").length > 0) {
        $(".delete-account").prop("disabled", false);
        return;
    }
    $(".delete-account").prop("disabled", true);
}

function resetNewUserButton() {
    $(".btn-new-user").button("reset");
}

function OpenDialog(dialogContainerId) {
    //var $DialogContainer = $("#" + dialogContainerId);
    var $DialogContainer = $(dialogContainerId);
    var $jQval = $.validator; //This is the validator
    $jQval.unobtrusive.parse($DialogContainer); // and here is where you set it up.
    //$DialogContainer.modal();
    $("#newUserModalDialog .modal").modal("show");

    var $form = $("#newUserModalDialog").find("form");
    $.validator.unobtrusive.parse($form);

    // focus into first text input field
    $("#new-portfolio-name").focus();

    $form.on("submit", function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();

        var $form = $(this);

        //Function is defined later...
        submitAsyncForm($form,
            function (data) {
                $("#newUserModalDialog .modal").modal("hide");
                //window.location.href = window.location.href;
                // reload grid
                $("#tblportfolios").trigger("reloadGrid");
                // show success notification
                if (data["editing"])
                    quickNotification("User has been updated successfully");
                else {
                    quickNotification("Created new user successfully");
                    if (data["redirectUrl"] !== "")
                        window.location.href = data["redirectUrl"];
                }
            },
            function (xhr, ajaxOptions, thrownError) {
                //console.log(xhr.responseText);
                //$("body").html(xhr.responseText);
                quickNotification("Error occurred during creating/editing user", "error");
            }
        );

        return false;
    });
}

function roleIdToRoleName(id) {
    var roleNames = $.grep(window.userRoles, function (n, i) {
        return n.Key === id;
    });

    if (roleNames.length === 1)
        return roleNames[0].Value;
    return false;
}

function roleIdsToRoleNames(ids) {
    var results = [];
    var temp = "";
    $.each(ids, function(index, value) {
        temp = roleIdToRoleName(value);
        if(temp !== false)
            results.push(temp);
    });

    return results.length > 0 ? results.join(", ") : "";
}

