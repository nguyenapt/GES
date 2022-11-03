$(function () {

    //"use strict";

    var grid = $("#tblportfolios");

    // options
    // show services
    var optionShowServicesCookieName = "options.portfolioList.showServices";
    var optionShowServices = typeof Cookies.get(optionShowServicesCookieName) != "undefined" ? Cookies.get(optionShowServicesCookieName) : false;
    optionShowServices = toBool(optionShowServices);
    $(".toggle-show-services").prop("checked", optionShowServices);
    $(".toggle-show-services").on("change", function () {
        optionShowServices = !optionShowServices;
        Cookies.set(optionShowServicesCookieName, optionShowServices);
        grid.trigger("reloadGrid");
    });

    function cboxFormatterGesStandardUniverse(cellvalue, options, rowObject) {
        return '<input type="checkbox"' + (cellvalue ? ' checked="checked"' : '') + ' id="' + GenerateGesStandardUniverseChkId(rowObject.PortfolioId) + '"/>';
    }

    var gridColNames = ["Id", "Name", "Type", "Alerts?", "InCSC?", "Std.Portfolio?", "GES Standard Universe?", "Created", "#Companies", "#Contro-Activities", "#Services", "Services"];
    var gridColModel = [
        { name: "PortfolioId", width: "35px", align: "right", hidden: true, search: false },
        {
            name: "Name",
            width: "200px",
            searchoptions: {
                searchOperators: true,
                sopt: ["cn", "ew", "en", "bw", "bn"]
            },
            formatter: function (cellvalue, options, rowObject) {
                var cellPrefix = "";
                return cellPrefix + "<a class=\"portfolio-name-link\" href=\"/Portfolio/Details?id=" + rowObject.PortfolioId + "&po_id=" + rowObject.PortfolioOrganizationId + "\">" + cellvalue + "</a>";
            }
        },
        { name: "Type", width: "130px", hidden: true },
        { name: "IncludeInAlerts", width: "90px", formatter: "checkbox" },
        { name: "ShowInCSC", hidden: true, formatter: "checkbox" },
        { name: "StandardPortfolio", width: "90px", formatter: "checkbox" },
        { name: "GESStandardUniverse", width: "90px", classes: 'cellGESStandardUniverse', formatter: cboxFormatterGesStandardUniverse },
        { name: "Created", width: "120px", formatter: dateFormatter, searchoptions: { sopt: ["eq", "lt", "gt"] } },
        { name: "Companies", width: "90px", searchoptions: { sopt: ["eq", "ne", "lt", "le", "gt", "ge"] } },
        { name: "ControversialActivities", hidden: true, searchoptions: { sopt: ["eq", "ne", "lt", "le", "gt", "ge"] } },
        { name: "GEServices", hidden: true, searchoptions: { sopt: ["eq", "ne", "lt", "le", "gt", "ge"] } },
        {
            name: "Services",
            search: false,
            sortable: false,
            width: "200px",
            formatter: function (cellvalue, options, rowObject) {
                return "<a href=\"#\" data-type=\"checklist\" data-pk=\"" + rowObject.PortfolioOrganizationId + "\" data-value=\"" + rowObject.GEServiceIds + "\" data-title=\"Select services\" class=\"portfolio-services\" target=\"_blank\">" + rowObject.GEServices + " services</a>";
            }
        }
    ];
    // only for an Organization?
    var postUrl = "/portfolio/GetDataForPortfoliosJqGrid?orgid=";
    var postUrlDelete = "/portfolio/DeletePortfolios?orgid=";
    var hideOrgNameCol = false;
    var gridCaption = "Portfolio List in jqGrid";

    if (orgId != null && orgId > 0) {
        postUrl += orgId;
        hideOrgNameCol = true;
        gridCaption = "Data";
        postUrlDelete += orgId;
    } else {
        // Portfolio List (no organization)
        gridColNames = ["Id", "Name", "Type", "InCSC?", "Std.Portfolio?", "GES Standard Universe?", "#Companies", "#Clients", "Created"];
        gridColModel = [
            { name: "PortfolioId", width: "35px", align: "right", hidden: true, search: false },
            {
                name: "Name",
                width: "200px",
                searchoptions: {
                    searchOperators: true,
                    sopt: ["cn", "ew", "en", "bw", "bn"]
                },
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    return cellPrefix + "<a class=\"portfolio-name-link\" href=\"/Portfolio/Details?id=" + rowObject.PortfolioId + "\">" + cellvalue + "</a>";
                }
            },
            { name: "Type", width: "130px", hidden: true },
            { name: "ShowInCSC", hidden: true, formatter: "checkbox" },
            { name: "StandardPortfolio", width: "90px", formatter: "checkbox" },
            { name: "GESStandardUniverse", width: "90px", classes: 'cellGESStandardUniverse', formatter: cboxFormatterGesStandardUniverse },
            { name: "Companies", width: "90px", searchoptions: { sopt: ["eq", "ne", "lt", "le", "gt", "ge"] } },
            { name: "Clients", width: "90px", searchoptions: { sopt: ["eq", "ne", "lt", "le", "gt", "ge"] } },
            { name: "Created", width: "120px", formatter: dateFormatter, searchoptions: { sopt: ["eq", "lt", "gt"] } }
        ];
    }

    var getUniqueTypes = function() {
        return types;
    }, setTypesSelect = function (columnName) {
        this.jqGrid("setColProp", columnName, {
            stype: "select",
            searchoptions: {
                value: buildSearchSelect(getUniqueTypes.call(this)),
                sopt: ["eq"]
            }
        });
    }

    // x-editable defaults
    $.fn.editable.defaults.url = "/Portfolio/UpdatePortfolioServices";

    $.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = "Bootstrap";
    grid.bind("jqGridLoadComplete", function (e, rowid, orgClickEvent) {
        // x-editable
        $(".portfolio-services").editable({
            container: "body",
            placement: "left",
            pk: 1,
            source: "/Service/GetServices",
            sourceCache: true,
            autotext: "never",
            display: function (value, sourceData) {
                //display checklist as comma-separated values
                var checked = $.fn.editableutils.itemsByValue(value, sourceData);

                if (checked.length) {
                    var tooltipHtml = "<ul class=\"servicesListUl\">";

                    $.each(checked, function(i, v) {
                        if (agreementServices.length > 0 && agreementServices.indexOf(v.value) >= 0) {
                            tooltipHtml += "<li class=\"text-green\">" + v.text + "</li>";
                        } else {
                            tooltipHtml += "<li class=\"text-orange\" title=\"'" + v.text + "' is not in Agreements\">" + v.text + "</li>";
                        }
                    });
                    
                    tooltipHtml += "</ul>";

                    if (!optionShowServices) {
                        $(this).attr("qtip-content", tooltipHtml);
                        $(this).qtip({
                            content: {
                                text: function(event, api) {
                                    return $(this).attr("qtip-content");
                                },
                                title: "Services: Click to edit"
                            },
                            position: {
                                my: "top right",
                                at: "center left",
                                adjust: {
                                    method: "shift"
                                }
                            }
                        });
                        $(this).html(checked.length + " services");
                    } else {
                        $(this).html(tooltipHtml);
                    }
                } else {
                    $(this).empty();
                }
            }
        });
        
        $(".portfolio-services").on("shown", function (e, editable) {
            //Change color for Agreement Services
            editable.input.$input.each(function (value, chk) {
                var chkValue = parseInt(chk.value);
                if (agreementServices.length > 0 && agreementServices.indexOf(chkValue) >= 0) {
                    $(chk).next("span").addClass("text-green");
                }
            });
        });

        $(window).resize();

        // odd, even row
        $("tr.jqgrow:even").addClass("jqgrid-row-even");
    });

    grid.bind("jqGridBeforeSelectRow", function (e, rowid, orgClickEvent) {
        // prepare buttons
        var records = grid.jqGrid("getGridParam", "records");
        if (records === 0) {
            $(".batch-delete-portfolios").prop("disabled", true);
        } else {
            $(".batch-delete-portfolios").prop("disabled", false);
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
        ondblClickRow: function (rowid, iRow, iCol, e) {
            // OnBegin
            var rowData = grid.jqGrid("getRowData", rowid);
            rowData["orgId"] = orgId;
            rowData["orgName"] = orgName;
            rowData["ShowInCSC"] = toBool(rowData["ShowInCSC"]);
            rowData["StandardPortfolio"] = toBool(rowData["StandardPortfolio"]);
            rowData["GESStandardUniverse"] = getCheckboxCellValueNoPrefix(GenerateGesStandardUniverseChkId(rowData["PortfolioId"]));
            rowData["Name"] = $(rowData["Name"]).text();

            if (orgId != null && orgId > 0) {
                rowData["IncludeInAlerts"] = toBool(rowData["IncludeInAlerts"]);
            }

            $.ajax({
                contentType: "application/json; charset=utf-8",
                //dataType: "json",
                type: "POST",
                url: "/Portfolio/CreateForm",
                data: JSON.stringify(rowData)
            })
            .done(function (response, textStatus, jqXHR) {
                $("#newPortfolioModalDialog").html(response);

                OpenDialog("newPortfolioModalDialog");
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                quickNotification("Error loading Edit form: <br/>" + errorThrown, "error");
            })
            .always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) {
                
            });
        },
        sortname: "Name",
        sortorder: "asc",
        multiselect: true,
        multiboxonly: false,
        beforeSelectRow: handleMultiSelect,
        loadComplete: function() {
            $(".cellGESStandardUniverse :checkbox").change(function (e) {
                var portIdMatches = this.id.match(/\d+/);
                var portId = -1;
                if (portIdMatches.length > 0) {
                    portId = portIdMatches[0];
                }

                var data = {
                    portfolioId: portId,
                    newValue: this.checked
                }
                var self = $(this);

                $.ajax({
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        type: "POST",
                        url: "/Portfolio/UpdateGESStandardUniverse",
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
        } 
    }).jqGrid("hideCol", "cb");

    setTypesSelect.call(grid, "Type");
    setBooleanSelect.call(grid, "IncludeInAlerts", "All");
    setBooleanSelect.call(grid, "ShowInCSC", "All");
    setBooleanSelect.call(grid, "StandardPortfolio", "All");
    setFilterDate.call(grid, "Created");
    setBooleanSelect.call(grid, "GESStandardUniverse", "All");
    grid.jqGrid("navGrid", "#myPager", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true }, {
        // edit options
        zIndex: 100,
        url: "/TodoList/Update",
        closeOnEscape: true,
        closeAfterEdit: true,
        recreateForm: true,
        afterComplete: function(response) {
            if (response.responseText) {
                //alert(response,responseText);
            }
        }
    }, {
        // add options
        zIndex: 10000,
        url: "/TodoList/Create",
        closeOnEscape: true,
        closeAfterEdit: true,
        recreateForm: true,
        afterComplete: function (response) {
            if (response.responseText) {
                //alert(response, responseText);
            }
        }
    }, {
        // delete options
        zIndex: 100,
        url: "/TodoList/Delete",
        closeOnEscape: true,
        closeAfterEdit: true,
        recreateForm: true,
        msg: "Are you sure?",
        afterComplete: function (response) {
            if (response.responseText) {
                //alert(response, responseText);
            }
        }
    });

    grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: true });

    $("#newPortfolioModalDialog .modal").modal({ show: false });
    
    $(document).on("shown.bs.modal", ".modal", function () {
        $("#new-portfolio-name").focus();
    });

    $(".batch-delete-portfolios").on("click", function () {
        var rowIds = grid.jqGrid("getGridParam", "selarrrow");

        var finalIds = [];
        for (var i = 0; i < rowIds.length; i++) {
            var selectedRowData = grid.jqGrid("getRowData", rowIds[i]);
            finalIds.push(selectedRowData["PortfolioId"]);
        }

        var data = {
            Ids: finalIds
        };
        bootbox.confirm("Are you sure you want to delete selected portfolio(s)?", function (result) {
            if (result) {
                $(".batch-delete-portfolios").button("loading");

                $.ajax({
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    type: "POST",
                    url: postUrlDelete,
                    data: JSON.stringify(data)
                })
                .done(function (response, textStatus, jqXHR) {
                    quickNotification("Delete selected portfolio(s) successfully", "info");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    quickNotification("Error occurred deleting portfolio(s)", "error");
                })
                .always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) {
                    $(".batch-delete-portfolios").button("reset");
                    grid.trigger("reloadGrid");
                });

            }
        });
    });
});

function resetNewPortfolioButton() {
    $(".btn-new-portfolio").button("reset");
}
function resetAddStandardPortfolioButton() {
    $(".btn-add-standard-portfolio").button("reset");
}

function resetCreateEditPortfolioButton() {
    $(".btn-create-edit-portfolio").button("reset");
}

function OpenDialog(dialogContainerId) {
    //var $DialogContainer = $("#" + dialogContainerId);
    var $DialogContainer = $(dialogContainerId);
    var $jQval = $.validator; //This is the validator
    $jQval.unobtrusive.parse($DialogContainer); // and here is where you set it up.
    //$DialogContainer.modal();
    $("#newPortfolioModalDialog .modal").modal("show");

    var $form = $("#newPortfolioModalDialog").find("form");
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
                $("#newPortfolioModalDialog .modal").modal("hide");
                //window.location.href = window.location.href;
                // reload grid
                $("#tblportfolios").trigger("reloadGrid");
                // show success notification
                if (data["editing"])
                    quickNotification("Portfolio has been updated successfully");
                else {
                    quickNotification("Created new portfolio successfully");
                    if (data["redirectUrl"] !== "")
                        window.location.href = data["redirectUrl"];
                }
            },
            function (xhr, ajaxOptions, thrownError) {
                //console.log(xhr.responseText);
                //$("body").html(xhr.responseText);
                quickNotification("Error occurred during creating/editing portfolio", "error");
            }
        );

        return false;
    });
}

function OpenAddStandardPortfolioDialog(dialogContainerId) {
    var $DialogContainer = $(dialogContainerId);
    var $jQval = $.validator; //This is the validator
    $jQval.unobtrusive.parse($DialogContainer); // and here is where you set it up.
    $("#addStandardPortfolioModalDialog .modal").modal("show");

    var $form = $("#addStandardPortfolioModalDialog").find("form");
    $.validator.unobtrusive.parse($form);

    $form.on("submit", function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();

        var $form = $(this);

        //Function is defined later...
        submitAsyncForm($form,
            function (data) {
                $("#addStandardPortfolioModalDialog .modal").modal("hide");
                // reload grid
                $("#tblportfolios").trigger("reloadGrid");
                // show success notification
                // TO-DO: ...
                if (data["success"] === true)
                    quickNotification("Added a standard portfolio successfully");
                else
                    quickNotification("Exception occurred: " + data["message"], "warning");
            },
            function (xhr, ajaxOptions, thrownError) {
                quickNotification("Error occurred during adding standard portfolio", "error");
            }
        );

        return false;
    });

    // select2 for standard portfolios dropdown
    $("#standardPortfoliosDropdown").select2({ width: "100%" });
}

$(".export-standard-universe-list-btn").on("click", function (e) {

    bootbox.dialog({
        message: "Which one do you want? Full list (including sub-securities) or Default list?",
        title: "Export GES Standard Universe",
        buttons: {
            success: {
                label: "Full List",
                className: "btn-success",
                callback: function () {
                    window.location.href = "/Portfolio/ExportGESStandardUniverseToExcel?fullList=true";
                    return true;
                }
            },
            main: {
                label: "DEFAULT List",
                className: "btn-primary",
                callback: function () {
                    window.location.href = "/Portfolio/ExportGESStandardUniverseToExcel";
                    return true;
                }
            }
        }
    });
});

function GenerateGesStandardUniverseChkId(portId) {
    return "GESStandardUniverse_" + portId;
}
