$(function () {
    var postUrl = "/documentmgmt/GetDataForCompanyDocumentJqGrid";
    var gridCaption = "";
    var deleteDocumentUrl = "/documentmgmt/DeleteCompanyDocuments";

    var grid = $("#tblcompanydocuments");
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
        colNames: ["Id", "Name", "Company", "CompanyId", "Issue/Case", "Case Report Id", "Service", "Comment", "Created"],
        colModel: [
            { name: "G_ManagedDocuments_Id", width: "35px", align: "right", hidden: true, search: false, key: true },
            {
                name: "Name"
            },
            {
                name: "CompanyName"
            },
            {
                name: "I_Companies_Id", hidden: true
            },           
            {
                name: "ReportIncident"
            },
            {
                name: "I_GesCaseReports_Id", hidden: true
            },
            {
                name: "ServiceName"
            },
            { name: "Comment" },
            { name: "Created", width: "60px", align: "center", formatter: dateFormatter, searchoptions: { sopt: ["eq"]}  }
        ],
        pager: $("#myPager"),
        rowNum: 50,
        rowList: [20, 50, 100],
        autowidth: true,
        shrinkToFit: true,
        toppager: true,
        rownumbers: false,
        gridview: true,
        ondblClickRow: function (rowid, iRow, iCol, e) {
            var rowData = grid.jqGrid("getRowData", rowid);
            $.ajax({
                contentType: "application/json; charset=utf-8",
                type: "POST",
                url: "/documentmgmt/CreateForm_CompanyDocument",
                data: JSON.stringify({ documentId: rowData.G_ManagedDocuments_Id })
            })
            .done(function (response, textStatus, jqXHR) {
                $("#newGesCompanyDocumentModalDialog").html(response);

                var dialogContainerId = "newGesCompanyDocumentModalDialog";
                $("#" + dialogContainerId + " .modal").modal("show");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                quickNotification("Error loading Edit form: <br/>" + errorThrown, "error");
            })
            .always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) {
                
            });
        },
        height: "auto",
        viewrecords: true,
        caption: gridCaption,
        sortname: "Created",
        sortorder: "desc"
    });
    setFilterDate.call(grid, "Created");
    grid.jqGrid("navGrid", "#myPager", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });

    //grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: false });
    grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: false });

    $(".batch-delete-document").on("click", function () {
        var deletedIds = [];
        $(".ui-state-highlight").each(function () { deletedIds.push(this.id) });
        bootbox.confirm("Are you sure you want to delete selected document(s)?",
            function (result) {
                if (result) {
                    $.ajax({
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        type: "POST",
                        url: deleteDocumentUrl,
                        data: JSON.stringify(deletedIds)
                    })
                        .done(function (response, textStatus, jqXHR) {
                            $(".batch-delete-glossary").button("reset");
                            if (!response.success) {
                                quickNotification(response.message, "error");
                                return;
                            }
                            quickNotification("Selected documents have been removed", "info");
                            grid.trigger("reloadGrid");
                            $(".batch-delete-document").prop("disabled", true);
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            quickNotification("Error occurred removing document(s)", "error");
                        })
                        .always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) {
                        });
                }
            });
    });

});

function resetNewGesDocumentButton() {
    $(".btn-create-edit-document").button("reset");
}

function OpenDialog(dialogContainerId) {
    //var $DialogContainer = $("#" + dialogContainerId);
    var $DialogContainer = $(dialogContainerId);
    var $jQval = $.validator; //This is the validator
    $jQval.unobtrusive.parse($DialogContainer); // and here is where you set it up.
    //$DialogContainer.modal();
    $("#newGesCompanyDocumentModalDialog .modal").modal("show");

    var $form = $("#newGesCompanyDocumentModalDialog").find("form");
    $.validator.unobtrusive.parse($form);

    // focus into first text input field
    $("#new-gesdocument-name").focus();

    $form.on("submit", function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();
        
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
                var message = data["editing"] ? "Updated successfully" : "New document has been created successfully";
                quickNotification(message);
                var grid = $("#tblcompanydocuments");
                grid.trigger('reloadGrid');
            },
            error: function(xhr, status, error) {
                quickNotification(data["error"], "error");
            }
        });

        return false;
    });
}


function manageDeleteButton() {
    if ($(".ui-state-highlight").length > 0) {
        $(".batch-delete-document").prop("disabled", false);
        return;
    }
    $(".batch-delete-document").prop("disabled", true);
}



