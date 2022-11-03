$(function () {

    //"use strict";

    // disable links with disabled-action class
    $("a.apply-preset-btn").bind("click", function (e) {
        if ($(e.target).hasClass("action-disabled")) {
            e.preventDefault();
            e.stopImmediatePropagation();
            return false;
        }
        return true;
    });

    // Apply Preset modal > bind action for Overwrite checkbox change
    $("body").on("change", "#OverwriteExistingValues", function() {
        renderControActivPreview();
    });
  

    // check file input > enable/disable submit button
    $("#excelfileinput").on("change", function() {
        if ($("#excelfileinput").val()) {
            $("#btn-submit-import-via-excel").prop("disabled", false);
        } else {
            $("#btn-submit-import-via-excel").prop("disabled", true);
        }
    });

    var origPendingTblData;
    var origControTblData;
    window.filteredControActivData = [];
    window.selectedPresetData = [];
    var gridCaption = "Data";
    var postUrl = "/portfolio/GetDataForPortfolioDetails?id=";
    var postUrl_Pending = "/portfolio/GetPendingCompanies?id=";
    var postUrl_Contro = "/portfolio/GetControversialActivitiesGridData?po_id=";
    var loadedControActiv = false;

    var editingPendingList = false;
    var editingControList = false;
    
    if (portfolioId != null && portfolioId > 0) {
        postUrl += portfolioId;
        postUrl_Pending += portfolioId;
    } else {
        return; // exit if no Portfolio Id specified
    }

    if (portfolioOrgId != null && portfolioOrgId > 0) {
        postUrl_Contro += portfolioOrgId;
    }

    if (invalidIds !== "") {
        bootbox.dialog({
            message: "Company with ID(s) " + invalidIds + " do(es) not exist in the sytem!",
            title: "Import from Excel",
            buttons: {
                success: {
                    label: "Ok",
                    className: "btn-success",
                    callback: function () {
                        window.location.href = "/Portfolio/Details?id=" + portfolioId;
                        return true;
                    }
                }
                
            },
            onEscape: function() {
                window.location.href = "/Portfolio/Details?id=" + portfolioId;
            }
        });
    }
    
    
    function cboxFormatterCompanyId(cellvalue, options, rowObject) {
        return '<input type="checkbox"' + (cellvalue ? ' checked="checked"' : '') + ' id="' + GenerateCompanyChkId(rowObject.CompanyId) + '"/>';
    }

    function chkboxFormatterShowInClient(cellvalue, options, rowObject) {
        return '<input type="checkbox"' + (cellvalue ? ' checked="checked"' : '') + ' id="' + GenerateShowClientChkId(rowObject.CompanyId) + '"/>';
    }

    var grid = $("#tblportfoliodetails");
    $.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = "Bootstrap";
    grid.bind("jqGridLoadComplete", function (e, rowid, orgClickEvent) {
        $(window).resize();

        // odd, even row
        $("tr.jqgrow:even").addClass("jqgrid-row-even");
    });
    grid.bind("jqGridAfterLoadComplete", function () {
        // prepare buttons
        var records = grid.jqGrid("getGridParam", "records");
        if (records === 0) {
            $(".batch-edit-clear-portfolios").prop("disabled", true);
        } else {
            $(".batch-edit-clear-portfolios").prop("disabled", false);
        }
    });
    grid.jqGrid({
        url: postUrl,
        datatype: "json",
        mtype: "post",
        colNames: ["Id", "Name", "IsMasterCompany?", "ISIN","SustainalyticsID", "SEDOL", "IsInThisPortfolio?", "ShowInClient"],
        colModel: [
            { name: "CompanyId", width: "35px", align: "right", hidden: true, search: false },
            {
                name: "CompanyName", searchoptions: {
                    searchOperators: true,
                    sopt: ["cn", "ew", "en", "bw", "bn"]
                }
            },
            { name: "IsMasterCompany", formatter: "checkbox" },
            { name: "Isin", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] } },
            { name: "SustainalyticsID", searchoptions: { sopt: ["eq", "ne", "lt", "le", "gt", "ge"] } },
            { name: "Sedol", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] } },
            { name: "IsInThisPortfolio", classes: 'cellIsInThisPortfolio', formatter: cboxFormatterCompanyId },
            { name: "ShowInClient", classes: 'cellShowInClient', formatter: chkboxFormatterShowInClient }
        ],
        pager: $("#myPager"),
        rowNum: 10,
        rowList: [10, 30, 50, 100],
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
        search: true,
        postData: { filters: "{\"groupOp\":\"AND\",\"rules\":[{\"field\":\"IsInThisPortfolio\",\"op\":\"eq\",\"data\":\"true\"}]}"},
        sortname: "CompanyId",
        sortorder: "desc",
        cellsubmit: "clientArray",
        loadComplete: function() {
            $(".cellIsInThisPortfolio :checkbox").change(function (e) {
                var portIdMatches = this.id.match(/\d+/);
                var portId = -1;
                if (portIdMatches.length > 0) {
                    portId = portIdMatches[0];
                }

                var data = {
                    portfolioId: portfolioId,
                    newValue: this.checked,
                    companyId: portId
                }
                var self = $(this);

                $.ajax({
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        type: "POST",
                        url: "/Portfolio/UpdateCompanies",
                        data: JSON.stringify(data)
                    })
                    .done(function (response, textStatus, jqXHR) {
                        if (response.meta.success) {
                            quickNotification("Successfully updated portfolio");
                        } else {
                            quickNotification(response.meta.error, "error");
                        }
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        // revert action
                        self.prop("checked", self.val());

                        quickNotification("Failed: Error occurred updating portfolio", "error");
                        grid.trigger("reloadGrid");
                    });
            });

            $(".cellShowInClient :checkbox").change(function (e) {
                var portIdMatches = this.id.match(/\d+/);
                var portId = -1;
                if (portIdMatches.length > 0) {
                    portId = portIdMatches[0];
                }

                var data = {
                    newValue: this.checked,
                    companyId: portId
                }
                var self = $(this);

                $.ajax({
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: "POST",
                    url: "/Portfolio/UpdateShowInClient",
                    data: JSON.stringify(data)
                })
                    .done(function (response, textStatus, jqXHR) {
                        if (response.meta.success) {
                            quickNotification("Successfully updated company");
                        } else {
                            quickNotification(response.meta.error, "error");
                        }
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        // revert action
                        self.prop("checked", self.val());

                        quickNotification("Failed: Error occurred updating portfolio", "error");
                        grid.trigger("reloadGrid");
                    });
            });
        } 
    });
    
    setBooleanSelect.call(grid, "IsInThisPortfolio", true);
    setBooleanSelect.call(grid, "IsMasterCompany", "All");
    setBooleanSelect.call(grid, "ShowInClient", "All");
    
    grid.jqGrid("navGrid", "#myPager", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });

    grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: true });


    // Pending Companies
    /////////////////////

    // var gridPending = $("#tblportfoliodetails-pending");
    // var gridPendingClass = "tblportfoliodetails-pending";
    // var gridPendingRowNum = 50,
    //    gridPendingRowList = [50, 100];
    // gridPending.bind("jqGridLoadComplete", function (e, rowid, orgClickEvent) {
    //    var postData = gridPending.jqGrid("getGridParam", "postData");
    //    $(window).resize();
    //
    //    // odd, even row
    //    $("tr.jqgrow:even").addClass("jqgrid-row-even");
    // });
    // gridPending.bind("jqGridAfterLoadComplete", function () {
    //    // prepare buttons
    //    var records = gridPending.jqGrid("getGridParam", "records");
    //    if (records === 0) {
    //        $(".batch-edit-start").prop("disabled", true);
    //        $(".batch-edit-clear-pending").prop("disabled", true);
    //        $(".export-pending-list-btn").prop("disabled", true);
    //    } else {
    //        $(".batch-edit-start").prop("disabled", false);
    //        $(".batch-edit-clear-pending").prop("disabled", false);
    //        $(".export-pending-list-btn").prop("disabled", false);
    //    }
    // });
    // gridPending.jqGrid({
    //    url: postUrl_Pending,
    //    datatype: "json",
    //    mtype: "post",
    //    colNames: ["Id", "ISIN", "Name", "SEDOL", "MasterCompanyId", "Not Screened?", "Add?", "Delete?"],
    //    colModel: [
    //        {
    //            name: "Id", width: "35px", align: "right", hidden: true, search: false
    //        },
    //        {
    //            name: "Isin", editable: true, searchoptions: {
    //                searchOperators: true,
    //                sopt: ["cn", "ew", "en", "bw", "bn"]
    //            },
    //            editoptions: {
    //                maxlength: 12,
    //                dataInit: function (el) {
    //                    $(el).autocomplete({
    //                        source: function (request, response) {
    //                            $.ajax({
    //
    //                                type: "POST",
    //
    //                                url: "/Client/GetCompaniesWSubCompaniesForAutocomplete",
    //
    //                                data: {
    //                                    term: request.term,
    //                                    limit: 10
    //                                },
    //
    //                                success: function (ret, textStatus, jqXhr) {
    //                                    response($.map(ret.rows, function (item) {
    //                                        return { label: item.Value, Value: item.Key };
    //                                    }));
    //                                },
    //                                error: function (jqXhr, textStatus, errorThrown) {
    //                                    console.log("Autocomplete failed.");
    //                                }
    //                            });
    //                        },
    //                        minLength: 3,
    //                        delay: 200,
    //
    //                        select: function (event, ui) {
    //
    //                            // Substitute the words in the edit control himself .
    //                            $(el).val(ui.item.Value);
    //
    //                            // Stops further processing of events .
    //                            return false;
    //                        },
    //
    //                        open: function (event, ui) {
    //                            $(".ui-autocomplete").css("zIndex", 10000);
    //                        }
    //                    });
    //
    //                    $(el).focus(function (e) {
    //                        //set selection for current row
    //                        var tr = $(e.target).closest("tr");
    //                        gridPending.jqGrid("setSelection", tr[0].id);
    //                    });
    //                }
    //            }
    //        },
    //        {
    //            name: "Name", editable: true, searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] },
    //            editoptions: {
    //                dataInit: function (element) {
    //                    $(element).focus(function (e) {
    //                        //set selection for current row
    //                        var tr = $(e.target).closest("tr");
    //                        gridPending.jqGrid("setSelection", tr[0].id);
    //                    });
    //                }
    //            }
    //            
    //        },
    //        {
    //            name: "Sedol", editable: true, searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] },
    //            editoptions: {
    //                dataInit: function (element) {
    //                    $(element).focus(function (e) {
    //                        //set selection for current row
    //                        var tr = $(e.target).closest("tr");
    //                        gridPending.jqGrid("setSelection", tr[0].id);
    //                    });
    //                }
    //            }
    //        },
    //        {
    //            name: "MasterCompanyId", editable: true, search: false,
    //            editrules: {
    //                //custom rules
    //                custom_func: validatePositive,
    //                custom: true,
    //                required: false
    //            },
    //            editoptions: {
    //                maxlength: 25,
    //                dataInit: function (el) {
    //                    $(el).autocomplete({
    //                        source: function (request, response) {
    //                            $.ajax({
    //
    //                                type: "POST",
    //
    //                                url: "/Client/GetCompaniesForAutocomplete",
    //
    //                                data: {
    //                                    term: request.term,
    //                                    limit: 10
    //                                },
    //
    //                                success: function (ret, textStatus, jqXhr) {
    //                                    response($.map(ret.rows, function (item) {
    //                                        return { label: item.Name, Value: item.Id };
    //                                    }));
    //                                },
    //                                error: function (jqXhr, textStatus, errorThrown) {
    //                                    console.log("Autocomplete failed.");
    //                                }
    //                            });
    //                        },
    //                        minLength: 3,
    //                        delay: 200,
    //
    //                        select: function (event, ui) {
    //                            $(el).val(ui.item.Value);
    //
    //                            // Stops further processing of events .
    //                            return false;
    //                        },
    //
    //                        open: function (event, ui) {
    //                            $(".ui-autocomplete").css("zIndex", 10000);
    //                        }
    //                    });
    //
    //                    $(el).focus(function (e) {
    //                        //set selection for current row
    //                        var tr = $(e.target).closest("tr");
    //                        gridPending.jqGrid("setSelection", tr[0].id);
    //                    });
    //                }
    //            }
    //        },
    //        {
    //            name: "Screened", editable: true, search: true, formatter: "checkbox", edittype: "checkbox", editoptions: {
    //                value: "Yes:No",
    //                dataInit: function (element) {
    //                    $(element).focus(function (e) {
    //                        //set selection for current row
    //                        var tr = $(e.target).closest("tr");
    //                        gridPending.jqGrid("setSelection", tr[0].id);
    //                    });
    //                }
    //            }
    //        },
    //        { name: "SelectedToAdd", editable: true, search: true, formatter: "checkbox", edittype: "checkbox", editoptions: {
    //            value: "Yes:No",
    //            dataInit: function (element) {
    //                $(element).focus(function (e) {
    //                    //set selection for current row
    //                    var tr = $(e.target).closest("tr");
    //                    gridPending.jqGrid("setSelection", tr[0].id);
    //                });
    //            }
    //        } },
    //        { name: "SelectedToDelete", editable: true, search: true, formatter: "checkbox", edittype: "checkbox", editoptions: {
    //            value: "Yes:No",
    //            dataInit: function (element) {
    //                $(element).focus(function (e) {
    //                    //set selection for current row
    //                    var tr = $(e.target).closest("tr");
    //                    gridPending.jqGrid("setSelection", tr[0].id);
    //                });
    //            }
    //        } }
    //    ],
    //    pager: $("#myPager-pending"),
    //    rowNum: gridPendingRowNum,
    //    rowList: gridPendingRowList,
    //    autowidth: true,
    //    shrinkToFit: true,
    //    toppager: true,
    //    //loadonce: true,
    //    rownumbers: false,
    //    //pagerpos: "left",
    //    gridview: true,
    //    //width: "auto",
    //    height: "auto",
    //    viewrecords: true,
    //    caption: gridCaption,
    //    editurl:"clientArray",
    //    //loadui: "block",
    //    //loadtext: "Processing...",
    //
    //    sortname: "Id",
    //    sortorder: "desc"
    // });
    //
    // setBooleanSelect.call(gridPending, "Screened", "All");
    // setBooleanSelect.call(gridPending, "SelectedToAdd", "All");
    // setBooleanSelect.call(gridPending, "SelectedToDelete", "All");
    //
    // gridPending.jqGrid("navGrid", "#myPager-pending", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });
    //
    // gridPending.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: true });


    // Batch Edit functions
    function toggleEditMode() {
        editingPendingList = !editingPendingList;

        if (!editingPendingList) {
            //$(".batch-edit-start").prop("disabled", false);
            $(".batch-edit-start").val("Edit");
            $(".batch-edit-save").prop("disabled", true);

            // filter toolbar
            $(".pending-companies-box .ui-search-toolbar").css("display", "table-row");
            // pager
            $("#tblportfoliodetails-pending_toppager").css("display", "block");
            $("#myPager-pending").css("display", "block");
        } else {
            //$(".batch-edit-start").prop("disabled", true);
            $(".batch-edit-start").val("Exit Edit Mode");
            $(".batch-edit-save").prop("disabled", false);

            $(".pending-companies-box .ui-search-toolbar").css("display", "none");
            $("#tblportfoliodetails-pending_toppager").css("display", "none");
            $("#myPager-pending").css("display", "none");
        }
    }

    function startEdit() {
        toggleEditMode();
        var ids = gridPending.jqGrid("getDataIDs");

        if (!editingPendingList) {
            //gridPending.jqGrid("resetSelection");
            $("input[type='checkbox'][name='Screened']").prop("checked", false);
            $("input[type='checkbox'][name='SelectedToAdd']").prop("checked", false);
            $("input[type='checkbox'][name='SelectedToDelete']").prop("checked", false);

            // exit Edit mode > call dummy function in editUrl (it does nothing)
            for (var i = 0; i < ids.length; i++) {
                gridPending.jqGrid("saveRow", ids[i]);
            }
        } else {
            // store orig data to compare later
            origPendingTblData = gridPending.jqGrid("getRowData");

            for (var i = ids.length - 1; i >= 0; i--) {
                gridPending.jqGrid("editRow", ids[i], {
                    keys: false,
                    aftersavefunc: function(rowid, response) { console.log("after save"); },
                    errorfunc: function(rowid, response) { console.log("...we have a problem"); }
                });
            }
        }
    };

    function saveRows() {
        var ids = gridPending.jqGrid("getDataIDs");

        // validate MasterCompanyId column > positive integers
        for (var i = 1; i <= ids.length; i++) {
            var rCellAdd = getCheckboxCellValue(i, "SelectedToAdd");
            if(rCellAdd === false) continue;

            var rCell = getCellValue(i, "MasterCompanyId").trim();
            
            if (isNormalInteger(rCell) === false && rCell !== "") {
                console.log(isNormalInteger(rCell) + " - " + rCell);
                quickNotification("Validation failed: please make sure all MasterCompanyIds (of rows with Add? = true) are positive integers, or empty strings.", "warning");
                return false;
            }
        }

        // validate ISIN
        var countWs = 0;
        var countNotValidatedIsin = 0;
        var listIsinInvalid = "";
        for (var i = 1; i <= ids.length; i++) {
            var rCellAdd = getCheckboxCellValue(i, "SelectedToAdd");
            if (rCellAdd === false) continue;

            var rCellIsin = getCellValue(i, "Isin").trim();

            if (rCellIsin === "") {
                countWs++;
            } else if (!isValidISIN(rCellIsin)) {
                countNotValidatedIsin++;
                listIsinInvalid += rCellIsin + "; ";
            }
        }

        // before showing bootbox dialog, create func
        var execSaving = function(isGenerateNewIsin) {
            // get edited row
            var editedRows = [];
            for (i = 1; i <= ids.length; i++) {
                var dat = origPendingTblData[i - 1];
                var tempIsin = getCellValue(i, "Isin").trim();
                var tempSedol = getCellValue(i, "Sedol").trim();
                var tempName = getCellValue(i, "Name").trim();
                var tempMasterCompanyId = getCellValue(i, "MasterCompanyId").trim();
                var tempScreened = getCheckboxCellValue(i, "Screened");

                if (dat["Name"] !== tempName ||
                    dat["Isin"] !== tempIsin ||
                    dat["Sedol"] !== tempSedol ||
                    dat["MasterCompanyId"] !== tempMasterCompanyId ||
                    toBool(dat["Screened"]) !== tempScreened) {
                    editedRows.push(i);
                }
            }
            //console.log("edited rows:" + editedRows);
            //console.log(editedRows);

            // exit Edit mode > call dummy function in editUrl (it does nothing)
            for (var i = 0; i < ids.length; i++) {
                gridPending.jqGrid("saveRow", ids[i]);
            }

            // get data and send to real endpoint
            // get all data
            //var allPendingData = gridPending.jqGrid("getRowData");

            // get selected data
            var selectedPendingData = [];
            for (i = 1; i <= ids.length; i++) {
                var rowData = gridPending.jqGrid("getRowData", i);
                rowData["SelectedToAdd"] = toBool(rowData["SelectedToAdd"]);
                rowData["SelectedToDelete"] = toBool(rowData["SelectedToDelete"]);
                rowData["Screened"] = toBool(rowData["Screened"]);
                rowData["Isin"] = rowData["Isin"].trim();
                rowData["Name"] = rowData["Name"].trim();

                if (rowData["SelectedToAdd"] === true || rowData["SelectedToDelete"] === true) {
                    if (rowData["SelectedToAdd"] === true) {
                        if (!isValidISIN(rowData["Isin"]) && isGenerateNewIsin) {
                            rowData["Isin"] = "";
                        }
                    }

                    selectedPendingData.push(rowData);
                } else {
                    if ($.inArray(i, editedRows) !== -1) {
                        selectedPendingData.push(rowData);
                    }
                }
            }
            //console.log("Edited Pending Companies Data:");
            //console.log(selectedPendingData);
            var data = {
                id: portfolioId,
                records: selectedPendingData
            }

            if (selectedPendingData.length == 0) {
                // notification
                // enable UI elements
                toggleEditMode();
                $("#load_" + gridPendingClass).hide();
                // return
                return false;
            }

            // disable UI elements
            $(".batch-edit-btn").prop("disabled", true);
            //$("#lui_" + gridPendingClass).show();
            $("#load_" + gridPendingClass).text("Processing...").show();
            $.ajax({
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                type: "POST",
                url: "/Portfolio/EditPendingCompanyRecords",
                data: JSON.stringify(data)
            })
                .done(function (response, textStatus, jqXHR) {
                    quickNotification("Added: " + response.data.AddedItems + " items"
                        + "<br/> Edited: " + response.data.EditedItems + " items"
                        + "<br/> Deleted: " + response.data.DeletedItems + " items"
                        + "<br/> Duplicated: " + response.data.DuplicatedItems + " items");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    quickNotification("Failed: Unknown error", "error");
                })
                .always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) {
                    gridPending.trigger("reloadGrid");
                    grid.trigger("reloadGrid");
                    // enable UI elements
                    toggleEditMode();
                    $("#load_" + gridPendingClass).hide();
                });
            return true;
        }

        if (countWs !== 0 || countNotValidatedIsin !== 0) {
            var strMsg = "";

            if (countNotValidatedIsin > 0 && listIsinInvalid.length > 2) {

                listIsinInvalid = listIsinInvalid.substring(0, listIsinInvalid.length - 2);

                strMsg = "Among rows with Add?=true, there are " + countWs + " blank ISIN, and " + countNotValidatedIsin + " bad ISIN (" + listIsinInvalid + "). Please choose one option below.";

                bootbox.dialog({
                    message: strMsg,
                    buttons: {
                        cancel: {
                            label: "Cancel",
                            className: "btn-default",
                            callback: function () {
                                return true;
                            }
                        },
                        success: {
                            label: "Keep original ISIN(s)",
                            className: "btn-success",
                            callback: function () {
                                execSaving(false);
                            }
                        },
                        main: {
                            label: "Generate Custom ISIN(s)",
                            className: "btn-primary",
                            callback: function () {
                                execSaving(true);
                            }
                        }
                    }
                });
            } else { //generate new isin for blank isin
                strMsg = "Among rows with Add?=true, there are " + countWs + " blank ISIN. Do you want those ISIN codes to be auto-generated?";

                bootbox.confirm(strMsg, function(result) {
                    if (result) {
                        execSaving(true);
                    }
                });
            }
            
        } else {
            return execSaving(false);
        }
        return true;
    }

    // allow tick only Add? or Delete? or Screened?
    $(document).on("change", "input.inline-edit-cell[type='checkbox']", function () {
        $(this).closest("tr").find("input[type='checkbox']").not(this).prop("checked", false);
    });

    $(".batch-edit-start").on("click", function () {
        startEdit();
    });
    $(".batch-edit-save").on("click", function () {
        saveRows();
    });
    $(".batch-edit-clear-pending").on("click", function () {
        var data = {
            portfolioId: portfolioId
        };
        bootbox.confirm("Are you sure you want to clear all pending companies?", function (result) {
            if (result) {
                $(".batch-edit-clear-pending").button("loading");

                $.ajax({
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: "POST",
                    url: "/Portfolio/ClearPendingCompanies",
                    data: JSON.stringify(data)
                })
                .done(function (response, textStatus, jqXHR) {
                    quickNotification("Cleared all pending companies", "info");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    quickNotification("Error occurred clearing pending companies", "error");
                })
                .always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) {
                    $(".batch-edit-clear-pending").button("reset");
                    gridPending.trigger("reloadGrid");
                });
                
            }
        });
    });

    $(".export-pending-list-btn").on("click", function (e) {

        bootbox.confirm("Export all pending securities?", function (result) {
            if (result) {
                window.location.href = "/Portfolio/ExportPendingListToExcel?id=" + portfolioId;
                return true;
            }
        });
    });

    // CLEAR ALL button
    $(".batch-edit-portfolio-clear-all").on("click", function () {
        var data = {
            portfolioId: portfolioId
        };
        bootbox.confirm("<span class='important-warning'>WARNING: THIS ACTION WOULD CLEAR ALL SECURITIES & PENDING SECURITIES FROM THIS PORTFOLIO.</span><br />Are you sure you want to CLEAR ALL?", function (result) {
            if (result) {
                $(".batch-edit-portfolio-clear-all").button("loading");

                $.ajax({
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: "POST",
                    url: "/Portfolio/ClearAll",
                    data: JSON.stringify(data)
                })
                .done(function (response, textStatus, jqXHR) {
                    quickNotification("Cleared all securities", "info");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    quickNotification("Error occurred clearing securities", "error");
                })
                .always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) {
                    $(".batch-edit-portfolio-clear-all").button("reset");
                    gridPending.trigger("reloadGrid");
                    grid.trigger("reloadGrid");
                });

            }
        });
    });

    $(".batch-edit-clear-portfolios").on("click", function () {
        var data = {
            portfolioId: portfolioId
        };
        bootbox.confirm("Are you sure you want to remove all companies from this portfolio?", function (result) {
            if (result) {
                $(".batch-edit-clear-portfolios").button("loading");

                $.ajax({
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: "POST",
                    url: "/Portfolio/ClearCompanies",
                    data: JSON.stringify(data)
                })
                .done(function (response, textStatus, jqXHR) {
                    quickNotification("Cleared all companies successfully", "info");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    quickNotification("Error occurred clearning companies from portfolio", "error");
                })
                .always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) {
                    $(".batch-edit-clear-portfolios").button("reset");
                    grid.trigger("reloadGrid");
                });

            }
        });
    });


    // Controversial Activities Threshold
    //////////////////////////////////////

    var gridControActivId = "tblportfoliodetails-contro";
    var gridControActivPagerId = "myPager-contro";
    var gridControActiv = $("#" + gridControActivId);
    gridControActiv.bind("jqGridLoadComplete", function (e, rowid, orgClickEvent) {
        var postData = gridControActiv.jqGrid("getGridParam", "postData");
        $(window).resize();

        // odd, even row
        $("tr.jqgrow:even").addClass("jqgrid-row-even");
    });
    gridControActiv.bind("jqGridAfterLoadComplete", function () {
        loadedControActiv = true;

        // prepare buttons
        var records = gridControActiv.jqGrid("getGridParam", "records");
        if (records === 0) {
            $(".batch-edit-contro-start").prop("disabled", true);
            $(".batch-edit-contro-clear").prop("disabled", true);
        } else {
            $(".batch-edit-contro-start").prop("disabled", false);
            $(".batch-edit-contro-clear").prop("disabled", false);
        }

        // data
        var data = gridControActiv.jqGrid("getRowData");
        //data = $.grep(data, function (n, i) {
        //    return n.Threshold !== "";
        //});
        data = $.map(data, function (item) {
            return { id: parseInt(item.ControActivId), name: item.ControActivName, threshold: parseInt(item.Threshold) };
        });
        window.filteredControActivData = data;
        //console.log(window.filteredControActivData);
    });

    var loadControActiv = function() {
        gridControActiv.jqGrid({
            url: postUrl_Contro,
            datatype: "json",
            mtype: "post",
            colNames: ["ControActivId", "Name", "Threshold"],
            colModel: [
                { name: "ControActivId", width: "35px", align: "right", hidden: true, search: false },
                { name: "ControActivName", width: "400px", editable: false, search: false },
                {
                    name: "Threshold", width: "100px", editable: true, search: false,
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
            pager: $("#" + gridControActivPagerId),
            rowNum: 9999,
            rowList: [],        // disable page size dropdown
            pgbuttons: false,     // disable page control like next, back button
            pgtext: null,         // disable pager text like 'Page 0 of 10'
            
            searchable: false,
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
            editurl:"clientArray", 
            sortname: "Threshold",
            sortorder: "desc"
        });
        gridControActiv.jqGrid("navGrid", "#" + gridControActivPagerId, { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });
        //gridControActiv.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: true });
    }

    // ControActiv Batch Edit functions
    function toggleEditMode_ControActiv() {
        editingControList = !editingControList;

        if (!editingControList) {
            $(".batch-edit-contro-start").val("Edit");
            $(".batch-edit-contro-save").prop("disabled", true);

            $(".apply-preset-btn").attr("disabled", false);
            $(".apply-preset-btn").prop("disabled", false);
            $(".apply-preset-btn").removeClass("action-disabled");

            $(".batch-edit-contro-clear").attr("disabled", false);
            $(".batch-edit-contro-clear").prop("disabled", false);
            $(".batch-edit-contro-clear").removeClass("action-disabled");
        } else {
            $(".batch-edit-contro-start").val("Exit Edit Mode");
            $(".batch-edit-contro-save").prop("disabled", false);

            $(".apply-preset-btn").attr("disabled", true);
            $(".apply-preset-btn").prop("disabled", true);
            $(".apply-preset-btn").addClass("action-disabled");

            $(".batch-edit-contro-clear").attr("disabled", true);
            $(".batch-edit-contro-clear").prop("disabled", true);
            $(".batch-edit-contro-clear").addClass("action-disabled");

        }
    }

    function startEdit_ControActiv() {
        toggleEditMode_ControActiv();
        var ids = gridControActiv.jqGrid("getDataIDs");

        if (!editingControList) {
            // exit Edit mode > call dummy function in editUrl (it does nothing)
            for (var i = 0; i < ids.length; i++) {
                gridControActiv.jqGrid("saveRow", ids[i]);
            }
        } else {
            // store orig data to compare later
            origControTblData = gridControActiv.jqGrid("getRowData");

            for (var i = ids.length -1; i >= 0; i--) {
                gridControActiv.jqGrid("editRow", ids[i], {
                    keys: false,
                    aftersavefunc: function(rowid, response) { console.log("after save"); },
                    errorfunc: function(rowid, response) { console.log("...we have a problem"); }
                });
            }
        }
    };

    function saveRows_ControActiv() {
        var ids = gridControActiv.jqGrid("getDataIDs");

        // validate Threshold column: integers >= 0
        for (var i = 1; i <= ids.length; i++) {

            var rCell = getCellValue(i, "Threshold").trim();
            
            if (isNormalInteger(rCell) === false && rCell !== "") {
                console.log(isNormalInteger(rCell) + " - " + rCell);
                quickNotification("Validation failed: please make sure all Threshold are positive integers, or empty strings.", "warning");
                return false;
            }
        }

        // before showing bootbox dialog, create func
        var execSaving = function() {
            // get edited row
            var editedRows = [];
            for (i = 1; i <= ids.length; i++) {
                var dat = origControTblData[i - 1];
                var tempThreshold = getCellValue(i, "Threshold").trim();

                if (dat["Threshold"] !== tempThreshold) {
                    editedRows.push(i);
                }
            }
            console.log("edited controActiv rows:" + editedRows);

            // exit Edit mode > call dummy function in editUrl (it does nothing)
            for (var i = 0; i < ids.length; i++) {
                gridControActiv.jqGrid("saveRow", ids[i]);
            }

            // get controActiv data
            var finalControActivData = [];
            for (i = 1; i <= ids.length; i++) {
                if ($.inArray(i, editedRows) !== -1) {
                    var rowData = gridControActiv.jqGrid("getRowData", i);
                    rowData["Threshold"] = rowData["Threshold"].trim();
                    finalControActivData.push(rowData);
                }
            }
            //console.log("Edited ControActiv Data:");
            //console.log(finalControActivData);
            var data = {
                poId: portfolioOrgId,
                records: finalControActivData
            }

            if (finalControActivData.length === 0) {
                // notification
                // enable UI elements
                toggleEditMode_ControActiv();
                $("#load_" + gridControActivId).hide();
                return false;
            }

            // disable UI elements
            $(".batch-edit-contro-btn").prop("disabled", true);
            //$("#lui_" + gridControActivId).show();
            $("#load_" + gridControActivId).text("Processing...").show();
            $.ajax({
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                type: "POST",
                url: "/Portfolio/EditControActivRecords",
                data: JSON.stringify(data)
            })
                .done(function (response, textStatus, jqXHR) {
                    if (response.meta.success) {
                        quickNotification("Added: " + response.data.AddedItems + " items"
                            + "<br/> Edited: " + response.data.EditedItems + " items"
                            + "<br/> Deleted: " + response.data.DeletedItems + " items");
                    } else {
                        quickNotification(response.meta.message, "error");
                    }
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    quickNotification("Failed: Unknown error", "error");
                })
                .always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) {
                    gridControActiv.trigger("reloadGrid");
                    // enable UI elements
                    toggleEditMode_ControActiv();
                    $("#load_" + gridControActivId).hide();
                });
            return true;
        }
        
        return execSaving();
    }

    $(".batch-edit-contro-start").on("click", function () {
        startEdit_ControActiv();
    });
    $(".batch-edit-contro-save").on("click", function () {
        saveRows_ControActiv();
    });
    $(".batch-edit-contro-clear").on("click", function () {
        var data = {
            portfolioOrgId: portfolioOrgId
        };
        bootbox.confirm("Are you sure you want to clear all controversial activities thresholds?", function (result) {
            if (result) {
                $(".batch-edit-contro-clear").button("loading");

                $.ajax({
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: "POST",
                    url: "/Portfolio/ClearControActivThresholds",
                    data: JSON.stringify(data)
                })
                .done(function (response, textStatus, jqXHR) {
                    if (response.meta.success) {
                        quickNotification("Cleared all thresholds", "info");
                    } else {
                        quickNotification(response.meta.error, "error");
                    }
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    quickNotification("Error occurred clearing thresholds", "error");
                })
                .always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) {
                    $(".batch-edit-contro-clear").button("reset");
                    gridControActiv.trigger("reloadGrid");
                });
            }
        });
    });

    $(document).on("shown.bs.modal", ".modal", function () {
        $("#input-search-preset").focus();
    });

    // Tab Management
    var activeTab = null;
    $("a[data-toggle=\"tab\"]").on("shown.bs.tab", function (e) {
        activeTab = e.target.hash;
        if (activeTab === "#tab_controactivities" && !loadedControActiv) {
            loadControActiv();
        }

        $(window).resize();
    });

    // Presets dropdown
    $("body").on("change", "#presetsDropdown", function(e) {
        var selectedId = $(this).val();
        if (selectedId === "") {
            window.selectedPresetData = [];
        } else {
            var newArr = $.grep(window.presetsData, function(n, i) {
                return n.Id === parseInt(selectedId);
            });

            if(newArr.length === 1)
            window.selectedPresetData = newArr[0].Items;
        }
        
        // render preview
        renderControActivPreview();
    });
});

function applyPresetBegin() {
    $(".apply-preset-btn").button("loading");
}

function resetApplyPresetButton() {
    $(".apply-preset-btn").button("reset");
}

function renderControActivPreview() {
    var selectedPresetId = $("#presetsDropdown").val();
    //if (!selectedPresetId) return false;
    if (selectedPresetId === "" || !selectedPresetId) {
        $(".preview-values .editor-field").html("<p>N/A. Select a Preset first.</p>");
        return false;
    }

    var overwrittenOrNot = $("#OverwriteExistingValues").prop("checked");
    var presetItems = [];
    $.each(window.selectedPresetData, function (index, value) {
        presetItems[value.Id] = value.Threshold;
    });
    var previewHtml = "<ul class=\"controActivPreview\">";
    $.each(window.filteredControActivData, function (index, value) {
        if ($.isNumeric(value.threshold)) {
            if (value.id in presetItems && presetItems[value.id] !== value.threshold) { // overwrite or not
                if (overwrittenOrNot) { // overwrite
                    previewHtml += "<li title='value will be overwritten' class=\"text-orange\">" + value.name + ": " + presetItems[value.id] + "</li>";
                } else {
                    previewHtml += "<li title='not to overwrite existing value'>" + value.name + ": " + value.threshold + "</li>";
                }
            } else if (value.id in presetItems && presetItems[value.id] === value.threshold) { // overwrite but values are the same
                previewHtml += "<li title='unchanged value'>" + value.name + ": " + presetItems[value.id] + "</li>";
            } else { // nothing changes
                previewHtml += "<li title='unchanged value'>" + value.name + ": " + value.threshold + "</li>";
            }
        } else {
            if (value.id in presetItems) { // add new
                previewHtml += "<li title='new item' class=\"text-green\">" + value.name + ": " + presetItems[value.id] + "</li>";
            }
        }
    });
    previewHtml += "</ul>";
    $(".preview-values .editor-field").html(previewHtml);
}

function OpenApplyControActivPresetDialog() {
    var dialogContainerId = "applyControActivPresetModalDialog";
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

        //Function is defined later...
        submitAsyncForm($form,
            function (response) {
                $("#" + dialogContainerId + " .modal").modal("hide");
                // reload grid
                $("#tblportfoliodetails-contro").trigger("reloadGrid");
                // show success notification
                // TO-DO: ...
                if (response.meta.success === true) {
                    quickNotification("Applied controversial activities preset successfully.<br/>Added: " + response.data.AddedItems + " items"
                        + "<br/> Overwritten: " + response.data.OverwrittenItems + " items");
                } else
                    quickNotification("Exception occurred: " + data.meta.message, "warning");
            },
            function (xhr, ajaxOptions, thrownError) {
                quickNotification("Error occurred during applying preset", "error");
            }
        );

        return false;
    });

    // select2 for presets dropdown
    $("#presetsDropdown").select2({ width: "100%" });
}

function GenerateCompanyChkId(companyId) {
    return "CompanyId_" + companyId;
}

function GenerateShowClientChkId(companyId) {
    return "CompId_" + companyId;
}
