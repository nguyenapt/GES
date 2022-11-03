"use strict";
GesInsideApp.controller("CaseProfileTemplateController", ["$scope", "$timeout", "$window", "CaseProfileService", "NgTableParams", function ($scope, $timeout, $window, CaseProfileService, NgTableParams) {
    $scope.isFormValid = false;
    $scope.message = null;
    $scope.engagementTypes = null;
    $scope.engagementTypeId= null;
    $scope.recommendations = null;
    $scope.recommendationId = null;
    $scope.companyDetails = null;
    $scope.countries =[];
    $scope.isAllowChangeCompanyValue = true;

    init();

    function init() {
        initSelect2();

        CaseProfileService.GetAllEngagementTypes().then(
            function (d) {
                $scope.engagementTypes = d.data;
            },
            function () {
                quickNotification("Error occurred during loading engagement type data", "error");
            }
        );

        CaseProfileService.GetAllRecommendations().then(
            function (d) {
                $scope.recommendations = d.data;
            },
            function () {
                quickNotification("Error occurred during loading recommendations data", "error");
            }
        );

        CaseProfileService.GetAllCountries().then(
            function (d) {
                $scope.countries = d.data;
            },
            function () {
                quickNotification("Error occurred during loading countries data", "error");
            }
        );


        initData(companyId);
        
    }
    $scope.nextStep = function () {
        goToCaseProfile();
    };

    $scope.companySelect = function() {

        var grid = $("#tblcompaniesList");
        var rowKey = grid.jqGrid('getGridParam', "selrow");

        $scope.CompanyName = grid.jqGrid('getCell', rowKey, 'Name');
        $scope.selectedCompanyId = grid.jqGrid('getCell', rowKey, 'Id');
    };

    $scope.init = function(companyId) {
        initData(companyId);
    };
    
    function initData(companyId) {
        if (companyId != null && companyId > 0) {
            $scope.isAllowChangeCompanyValue = false;

            CaseProfileService.GetCompanyDetailsById(companyId).then(
                function (d) {
                    $scope.companyDetails = d.data;

                    if ($scope.companyDetails != null){
                        $scope.CompanyName = $scope.companyDetails.CompanyName;
                    }
                },
                function () {
                    quickNotification("Error occurred during loading company details data", "error");
                }
            )

        }
    }

    function initSelect2() {

    }

    function goToCaseProfile() {
        
        var companyIdPram;
        
        if (companyId == null || companyId < 0) {
            companyIdPram = $scope.selectedCompanyId;
        } else {
            companyIdPram = companyId;
        }
        
        $window.location.href = "/CaseProfile/AddWithTemplate?companyId=" + companyIdPram + '&engagementTypeId=' + $scope.engagementTypeId + '&recommendationId=' + $scope.recommendationId;
    }

    $(function() {
        if ((typeof $scope === "undefined") || !$scope.isAllowChangeCompanyValue ) return; 
        
        var postUrl = "/company/GetDataForCompaniesAllowCreateCaseProfileJqGrid";
        var gridCaption = "Company List";

        var grid = $("#tblcompaniesList");
        $.jgrid.defaults.responsive = true;
        //$.jgrid.defaults.styleUI = "Bootstrap";
        grid.bind("jqGridLoadComplete", function (e, rowid, orgClickEvent) {
            $(window).resize();

            // odd, even row
            $("tr.jqgrow:even").addClass("jqgrid-row-even");
        });
        grid.jqGrid({
            url: postUrl,
            datatype: "json",
            mtype: "post",
            colNames: ["Id", "Name", "ISIN", "SEDOL", "Location", "WebsiteUrl", "Modified"],
            colModel: [
                { name: "Id", width: "35px", align: "right", hidden: true, key: true},
                {
                    name: "Name", searchoptions: { searchOperators: true, sopt: ["cn", "ew", "en", "bw", "bn"] },
                    formatter: function (cellvalue, options, rowObject) {
                        return rowObject.Name;
                    }
                },
                { name: "Isin", searchoptions: { searchOperators: true, sopt: ["cn", "ew", "en", "bw", "bn"] } },
                { name: "Sedol", width: "100px", searchoptions: { searchOperators: true, sopt: ["cn", "ew", "en", "bw", "bn"] } },
                { name: "Location", searchoptions: { searchOperators: true, sopt: ["cn", "ew", "en", "bw", "bn"] } },
                { name: "WebsiteUrl", hidden: true, searchoptions: { searchOperators: true, sopt: ["cn", "ew", "en", "bw", "bn"] } },
                { name: "Modified", hidden: true, width: "85px", formatter: dateFormatter, searchoptions: { sopt: ["eq", "lt", "gt", "nu"] } }
            ],
            // pager: $("#myPager"),
            rowNum: 13,
            rowList: [20, 50, 100],
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

            sortname: "Id",
            sortorder: "asc",
            cmTemplate: { sortable: false },
            loadError: function (jqXHR, textStatus, errorThrown) {
                alert("HTTP status code: " + jqXHR.status + "\n" +
                    "TextStatus: " + textStatus + "\n" +
                    "Error Message: " + errorThrown);
            },
            ondblClickRow: function() {
                $('#btn-company-select').click();
            }

        });
        setFilterDate.call(grid, "Modified");

        grid.jqGrid("navGrid", "#myPager", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });

        grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: true });
    });

    function selectRow(id) {
        var $grid = $("#tblcompaniesList");
        $grid.jqGrid("setSelection", id);
    }
    
}]);
