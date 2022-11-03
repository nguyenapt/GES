$(function () {
    var grid = $("#tblGlossaryList");

    var postUrl = "/glossary/GetGlossariesByCategoryId";
    var sortGlosaryUrl = "/glossary/SortGlossaries";
    var deleteGlosaryUrl = "/glossary/DeleteGlossaries";
    var colNames = ["Id", "Title", "Slug"];

    var gridColModel = [
        { name: "Id", hidden: true, sortable: false, key: true },
        { name: "Title", search: false, sortable: false },
        { name: "Slug", search: false, sortable: false }
    ];

    $.jgrid.defaults.responsive = true;
    grid.bind("jqGridLoadComplete",
        function(e, items, orgClickEvent) {
            $(window).resize();

            items.forEach(function(item, index, array) {
                if (item.ChildNums <= 0) {
                    $("#" + item.Id + " td.ui-sgcollapsed").text("");
                    $("#" + item.Id + " td.ui-sgcollapsed").unbind("click").html("");
                }
            });

            // odd, even row
            $("#tblGlossaryList tbody>tr.jqgrow:even").addClass("jqgrid-row-even");
            $("#tblGlossaryList tbody>tr.jqgrow:odd").removeClass("jqgrid-row-even");
        });

    grid.bind("jqGridSelectRow", function (e, rowid, orgClickEvent) {
        manageDeleteButton();
    });

    grid.jqGrid({
            url: postUrl,
            postData: { categoryId: "00000000-0000-0000-0000-000000000000" },
            height: "100%",
            datatype: "json",
            mtype: "post",
            colNames: colNames,
            colModel: gridColModel,
            subGrid: true,
            subGridOptions: {
                "plusicon": "ui-icon-triangle-1-e",
                "minusicon": "ui-icon-triangle-1-s",
                "openicon": "ui-icon-arrowreturn-1-e",
                "reloadOnExpand": false,
                "selectOnExpand": false
            },
            ondblClickRow: function (rowid, iRow, iCol, e) {
                var rowData = grid.jqGrid("getRowData", rowid);
                $.ajax({
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    url: "/Glossary/CreateForm_Glossary",
                    data: JSON.stringify({glossaryId: rowData.Id})
                })
                .done(function (response, textStatus, jqXHR) {
                    $("#newGlossaryModalDialog").html(response);
                    openNewGlossaryDialog();

                    var dialogContainerId = "newGlossaryModalDialog";
                    $("#" + dialogContainerId + " .modal").modal("show");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    quickNotification("Error loading Edit form: <br/>" + errorThrown, "error");
                })
                .always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) {

                });
            },
            multiselect: true,
            beforeSelectRow: handleMultiSelect,
            autowidth: true,
            shrinkToFit: true,

            loadComplete: function () {
                $(window).resize();
                var total = $("#tblGlossaryList").jqGrid('getGridParam', 'records');
                if (total === 0) {
                    $("#tblGlossaryList tbody").html("<tr class='jqgfirstrow' role='row'>" +
                        "<td role='gridcell' style='height: 0px; width: 35px; display: none;'></td>" +
                        "<td role='gridcell' style='height:0px;width:25px;'></td>" +
                        "<td role='gridcell' style='height:0px;width:150px;display:none;'></td>" +
                        "<td role='gridcell' style='height:0px;width:527px;'></td>" +
                        "<td role='gridcell' style='height:0px;width:527px;'></td>" +
                        "<td role='gridcell' style='height: 0px; width: 526px;'></td>" +
                        "</tr>" +
                        "<tr><td colspan='5'><div class='jqgrid-no-row-style'>No term found</div></td></tr>");
                }           
            },
            subGridRowExpanded: function(subgrid_id, row_id) {
                var subgrid_table_id = subgrid_id + "_t";
                var rowData = grid.getRowData(row_id);
                $("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table>");
                var subGrid = jQuery("#" + subgrid_table_id);
                subGrid.bind("jqGridSelectRow",
                    function(e, rowid, orgClickEvent) {
                        manageDeleteButton();
                    });

                // prep subgrid loaded function
                subGrid.bind("jqGridLoadComplete",
                    function (e, items, orgClickEvent) {
                        // fix issue: column shrinked while dragging
                        $(window).resize();

                        // subgrid: odd, even row
                        $("#" + subgrid_table_id + " tbody>tr.jqgrow:even").addClass("jqgrid-row-even");
                        $("#" + subgrid_table_id + " tbody>tr.jqgrow:odd").removeClass("jqgrid-row-even");
                    });

                subGrid.jqGrid({
                        url: postUrl,
                        postData: { categoryId: rowData.Id },
                        height: "100%",
                        datatype: 'json',
                        mtype: 'POST',
                        colNames: colNames,
                        colModel: gridColModel,
                        toppager: false,
                        multiselect: true,
                        beforeSelectRow: handleMultiSelect,
                        ondblClickRow: function(rowid, iRow, iCol, e) {
                            var subgrid_table_id = subgrid_id + "_t";
                            var subGird = $("#" + subgrid_table_id);
                            var rowData = subGird.jqGrid("getRowData", rowid);
                            $.ajax({
                                contentType: "application/json; charset=utf-8",
                                type: "POST",
                                url: "/Glossary/CreateForm_Glossary",
                                data: JSON.stringify({ glossaryId: rowData.Id })
                            })
                            .done(function (response, textStatus, jqXHR) {
                                $("#newGlossaryModalDialog").html(response);
                                openNewGlossaryDialog();

                                var dialogContainerId = "newGlossaryModalDialog";
                                $("#" + dialogContainerId + " .modal").modal("show");
                            })
                            .fail(function (jqXHR, textStatus, errorThrown) {
                                quickNotification("Error loading Edit form: <br/>" + errorThrown, "error");
                            })
                            .always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) {

                            });

                            e.stopPropagation();
                        }
                    })
                    .jqGrid('sortableRows',
                    {
                        update: function(ev, ui) {
                            var sortItem = ui.item[0];
                            var parentTable = sortItem.closest("table");
                            var glossaryIds = [];
                            $(parentTable).find("tr.jqgrow").each(function() { glossaryIds.push(this.id) });

                            $.ajax({
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    type: "POST",
                                    url: sortGlosaryUrl,
                                    data: JSON.stringify(glossaryIds)
                                })
                                .done(function(response, textStatus, jqXHR) {
                                    quickNotification("Ordering has been updated", "info");
                                })
                                .fail(function(jqXHR, textStatus, errorThrown) {
                                    quickNotification("Failed updating terms ordering", "error");
                                })
                                .always(function(jqXHROrData, textStatus, jqXHROrErrorThrown) {
                                });

                            console.log($(parentTable).attr("id"));
                            //subGrid: update odd/even row glossary table
                            $("#" + $(parentTable).attr("id") + " tbody>tr:even").addClass("jqgrid-row-even");
                            $("#" + $(parentTable).attr("id") + " tbody>tr:odd").removeClass("jqgrid-row-even");
                        }
                    })
                    .jqGrid('hideCol', 'cb');

                //remove unused td tags
                var element = $(".ui-subgrid.ui-sg-expanded td[colspan=1]");
                element.remove();
            }
        })
        .jqGrid('sortableRows',
        {
            update: function(ev, ui) {
                //fix ui bug 
                var sortItem = ui.item[0];
                var expandCols = $("#tblGlossaryList_" + sortItem.id + "_expandedContent");
                expandCols.detach().insertAfter($("#" + sortItem.id));

                //update to server
                var glossaryIds = [];
                $("#tblGlossaryList tr.jqgrow").each(function() { glossaryIds.push(this.id) });

                $.ajax({
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        type: "POST",
                        url: sortGlosaryUrl,
                        data: JSON.stringify(glossaryIds)
                    })
                    .done(function(response, textStatus, jqXHR) {
                        quickNotification("Ordering has been updated", "info");
                    })
                    .fail(function(jqXHR, textStatus, errorThrown) {
                        quickNotification("Failed updating terms ordering", "error");
                    })
                    .always(function(jqXHROrData, textStatus, jqXHROrErrorThrown) {
                    });

                //update odd/even row glossary table
                $("#tblGlossaryList tbody>tr:even").addClass("jqgrid-row-even");
                $("#tblGlossaryList tbody>tr:odd").removeClass("jqgrid-row-even");
            }
        })
        .jqGrid('hideCol', 'cb');

    $(".batch-delete-glossary").on("click", function() {
        var deletedIds = [];
        $(".ui-state-highlight").each(function() { deletedIds.push(this.id) });
        bootbox.confirm("Are you sure you want to delete selected term(s)?",
            function(result) {
                if (result) {
                    $.ajax({
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            type: "POST",
                            url: deleteGlosaryUrl,
                            data: JSON.stringify(deletedIds)
                        })
                        .done(function (response, textStatus, jqXHR) {
                            $(".batch-delete-glossary").button("reset");
                            if (!response.success) {
                                quickNotification(response.message, "error");                                                                
                                return;
                            }
                            quickNotification("Selected terms have been removed", "info");
                            grid.trigger("reloadGrid");                            
                            $(".batch-delete-glossary").prop("disabled", true);
                        })
                        .fail(function(jqXHR, textStatus, errorThrown) {
                            quickNotification("Error occurred removing term(s)", "error");
                        })
                        .always(function(jqXHROrData, textStatus, jqXHROrErrorThrown) {                                       
                        });
                }
            });
    });
});

function resetNewGlossaryButton() {
    $(".btn-new-glossary").button("reset");
}

function openNewGlossaryDialog() {
    var dialogContainerId = "newGlossaryModalDialog";
    $("#" + dialogContainerId + " .modal").modal("show");

    // on change "Title" > update "Slug"
    $("#new-glossary-slug").slugify("#new-glossary-title");
}

function manageDeleteButton() {
    if ($(".ui-state-highlight").length > 0) {
        $(".batch-delete-glossary").prop("disabled", false);
        return;
    }
    $(".batch-delete-glossary").prop("disabled", true);
}
