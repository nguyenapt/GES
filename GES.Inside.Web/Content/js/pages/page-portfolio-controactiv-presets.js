$(function () {

    //"use strict";

    var grid = $("#tblcontroactivpresets");

    var gridColNames = ["Id", "Name", "Created", "#Contro-Activities"];
    var gridColModel = [
        { name: "ControActivPresetId", width: "35px", align: "right", hidden: true, search: false },
        {
            name: "ControActivPresetName",
            width: "200px",
            searchoptions: {
                searchOperators: true,
                sopt: ["cn", "ew", "en", "bw", "bn"]
            }
        },
        { name: "Created", width: "120px", formatter: dateFormatter, searchoptions: { sopt: ["eq", "lt", "gt"] } },
        { name: "ControversialActivities", searchoptions: { sopt: ["eq", "ne", "lt", "le", "gt", "ge"] } }
    ];

    var postUrl = "/portfolio/GetDataForControActivPresets";
    var postUrlDelete = "/portfolio/DeleteControActivPresets";
    var gridCaption = "ControActivPreset List in jqGrid";

    $.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = "Bootstrap";
    grid.bind("jqGridLoadComplete", function (e, rowid, orgClickEvent) {
        $(window).resize();

        // odd, even row
        $("tr.jqgrow:even").addClass("jqgrid-row-even");
    });

    grid.bind("jqGridBeforeSelectRow", function (e, rowid, orgClickEvent) {
        // prepare buttons
        var records = grid.jqGrid("getGridParam", "records");
        if (records === 0) {
            $(".batch-delete-controactiv-presets").prop("disabled", true);
        } else {
            $(".batch-delete-controactiv-presets").prop("disabled", false);
        }
    });

    grid.jqGrid({
        url: postUrl,
        datatype: "json",
        mtype: "post",
        colNames: gridColNames,
        colModel: gridColModel,
        pager: $("#myPager"),
        rowNum: 50,
        rowList: [50, 100],
        autowidth: true,
        shrinkToFit: true,
        toppager: true,
        //loadonce: true,
        rownumbers: false,
        //pagerpos: "left",
        gridview: true,
        //width: "auto",
        height: "auto",
        viewrecords: true,
        caption: gridCaption,
        sortname: "Created",
        sortorder: "desc",
        multiselect: true,
        multiboxonly: false,
        beforeSelectRow: handleMultiSelect,
        ondblClickRow: function (rowid, iRow, iCol, e) {
            // OnBegin
            var rowData = grid.jqGrid("getRowData", rowid);
           
            $.ajax({
                contentType: "application/json; charset=utf-8",
                type: "POST",
                url: "/Portfolio/CreateForm_ControActivPreset",
                data: JSON.stringify(rowData)
            })
            .done(function (response, textStatus, jqXHR) {
                $("#newControActivPresetModalDialog").html(response);
                    OpenNewControActivPresetDialog();
                })
            .fail(function (jqXHR, textStatus, errorThrown) {
                quickNotification("Error loading Edit form: <br/>" + errorThrown, "error");
            })
            .always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) {

            });
        },
    }).jqGrid("hideCol", "cb");

    setFilterDate.call(grid, "Created");

    grid.jqGrid("navGrid", "#myPager", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });

    grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: true });

    $("#newControActivPresetModalDialog .modal").modal({ show: false });

    $(document).on("shown.bs.modal", ".modal", function () {
        $("#new-controactiv-preset-name").focus();
    });

    var batchDeleteBtnClass = "batch-delete-controactiv-presets";
    $("." + batchDeleteBtnClass).on("click", function () {
        var rowIds = grid.jqGrid("getGridParam", "selarrrow");

        var finalIds = [];
        for (var i = 0; i < rowIds.length; i++) {
            var selectedControActivRowData = grid.jqGrid("getRowData", rowIds[i]);
            finalIds.push(selectedControActivRowData["ControActivPresetId"]);
        }

        var data = {
            Ids: finalIds
        };

        bootbox.confirm("Are you sure you want to delete selected preset(s)?", function (result) {
            if (result) {
                $("." + batchDeleteBtnClass).button("loading");

                $.ajax({
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: "POST",
                    url: postUrlDelete,
                    data: JSON.stringify(data)
                })
                .done(function (response, textStatus, jqXHR) {
                    quickNotification("Selected preset(s) have been deleted successfully", "info");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    quickNotification("Error occurred deleting preset(s)", "error");
                })
                .always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) {
                    $("." + batchDeleteBtnClass).button("reset");
                    grid.trigger("reloadGrid");
                });

            }
        });
    });
});

function resetNewControActivPresetButton() {
    $(".btn-new-controactiv-preset").button("reset");
}

function OpenNewControActivPresetDialog() {
    var dialogContainerId = "newControActivPresetModalDialog";
    var $DialogContainer = $(dialogContainerId);
    var $jQval = $.validator; //This is the validator
    $jQval.unobtrusive.parse($DialogContainer); // and here is where you set it up.
    $("#" + dialogContainerId + " .modal").modal("show");

    var $form = $("#" + dialogContainerId).find("form");
    $.validator.unobtrusive.parse($form);

    $form.on("submit", function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();

        var $form = $(this);

        var gridControActiv = $("#tblcontroactivpresetsdetail");

        var strValues = "";
        var rows = gridControActiv.getDataIDs();
        for (var a = 1; a <= rows.length; a++) {
            var row = gridControActiv.getRowData(rows[a-1]);
            var rCellThreshold = getCellValue(a, "Threshold").trim();

            if (rCellThreshold.length > 0) {
                strValues += row.ControActivId + ":" + rCellThreshold + ";";
            };
        }
        if (strValues.length === 0) {
            quickNotification("You need to input threshold for at least one Controversial Activitity", "warning");
            return false;
        }

        $(".hiden-controversial-settings").val(strValues);

        //Function is defined later...
        submitAsyncForm($form,
            function (data) {
                $("#newControActivPresetModalDialog .modal").modal("hide");
                // reload grid
                $("#tblcontroactivpresets").trigger("reloadGrid");
                // show success notification
                if (data["success"]) {
                    if (data["editing"])
                        quickNotification("Preset has been updated successfully");
                    else
                        quickNotification("New preset has been created successfully");
                } else {
                    quickNotification(data["message"], "error");
                }
            },
            function (xhr, ajaxOptions, thrownError) {
                quickNotification("Error occurred during creating new preset", "error");
            }
        );

        return false;
    });

    $("#" + dialogContainerId + " .modal").on('shown.bs.modal', function (e) {
        var gridControActivId = "tblcontroactivpresetsdetail";
        var gridControActiv = $("#" + gridControActivId);
        gridControActiv.bind("jqGridLoadComplete", function (e, rowid, orgClickEvent) {
            $(window).resize();

            // odd, even row
            $("tr.jqgrow:even").addClass("jqgrid-row-even");

            $(".modal-dialog .ui-jqgrid-bdiv").perfectScrollbar();

            // all rows > edit mode
            var ids = gridControActiv.jqGrid("getDataIDs");
            for (var i = ids.length -1; i >=0; i--) {
                gridControActiv.jqGrid("editRow", ids[i], {
                    keys: false
                });
            }
        });

        var postUrl_Contro = "/portfolio/GetControversialPresetGridData?presetId=" + $(".hiden-controversial-id").val();
        gridControActiv.jqGrid({
            url: postUrl_Contro,
            datatype: "json",
            mtype: "post",
            colNames: ["ControActivId", "Name", "Threshold"],
            colModel: [
                { name: "ControActivId", hidden: true, search: false },
                { name: "ControActivName", editable: false, sortable: false, search: false },
                {
                    name: "Threshold", width: 100, editable: true, sortable:false,
                    editoptions: {
                        maxlength: 4,
                        dataInit: function(element) {
                            $(element).keypress(function (e) {
                                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                                    return false;
                                }
                                return true;
                            });

                            $(element).focus(function (e) {
                                //set selection for current row
                                var tr = $(e.target).closest('tr');
                                gridControActiv.jqGrid('setSelection', tr[0].id);
                            });
                        }
                    }
                }
            ],
            rowNum: 1000,
            //autowidth: true,
            shrinkToFit: true,
            forceFit: true, 
            rownumbers: true,
            height: "347",
            caption: "Preset details",
            editurl: "clientArray",
            rownumWidth: 20,
            sortable: false
            });
        
        gridControActiv.bind("jqGridLoadComplete", function (e, rowid, orgClickEvent) {
            $(window).resize();
        });
    });
}

