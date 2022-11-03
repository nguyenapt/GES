"use strict";
GesInsideApp.controller("CompanyController", ["$scope", "$filter", "$timeout", "$q", "$window", "CompanyService", "NgTableParams", function ($scope, $filter, $timeout, $q, $window, CompanyService, NgTableParams) {
    $scope.isFormValid = false;
    $scope.message = null;
    $scope.companyDetails = null;
    $scope.SecurityTypes = null;
    $scope.IsShowIndustryGICSLevel3 = false;
    $scope.isSaving = false;
    $scope.IsinLink = null;
    $scope.isAddNew = false;
    $scope.managementSystems = null;
    $scope.caseProfiles = null;
    $scope.subPeerGroups = null;
    $scope.caseProfilesTableParams = null;
    $scope.IsGSSParked = false;
    init();


    function init() {
        $timeout(function () {
            $('[data-toggle="popover"]')
                .popover({
                    html: true,
                    animation: true
                });

            StartLoad()
                .then(GetAllCountries)
                .then(GetAllSubPeerGroups)
                .then(GetAllManagementSystems)
                .then(GetCompanyDetail)
                .then(GetAllCaseProfiles)
                .then(CompanyPopulateData)
                .then(function () {
                    setUpContentBlockAnimation();
                    $(".animation-loading").hide();
                    quickNotification("Data loading completed", "success");
                },
                function (failReason) {
                    setUpContentBlockAnimation();
                    $(".animation-loading").hide();
                    quickNotification("Error occurred during loading data " + failReason, "error");
                });
        });


    }

    $scope.UpdateCompanyData = function () {
        $scope.isSaving = true;
        CompanyService.UpdateCompanyData($scope.companyDetails).then(function () {
            goToCompanyList();
        });
    };

    var StartLoad = function () {
        var deferred = $q.defer();
        deferred.resolve('Starting');
        return deferred.promise;
    };

    var GetAllCountries = function () {
        var deferred = $q.defer();

        CompanyService.GetAllCountries().then(
            function (d) {
                $scope.countries = d.data;
                deferred.resolve('Get all Countries');
            },
            function () {
                quickNotification("Error occurred during loading countries data", "error");
            }
        );

        return deferred.promise;
    };

    var GetAllSubPeerGroups = function () {
        var deferred = $q.defer();

        CompanyService.GetAllSubPeerGroups().then(
            function (d) {
                $scope.subPeerGroups = d.data;
                deferred.resolve('Get all subPeerGroups');
            },
            function () {
                quickNotification("Error occurred during loading subPeerGroups data", "error");
            }
        );

        return deferred.promise;
    };

    var GetAllManagementSystems = function () {
        var deferred = $q.defer();

        CompanyService.GetAllManagementSystems().then(
            function (d) {
                $scope.managementSystems = d.data;
                deferred.resolve('Get all management systems');
            },
            function () {
                quickNotification("Error occurred during loading management systems data", "error");
            }
        );

        return deferred.promise;
    };

    var GetAllCaseProfiles = function () {
        var deferred = $q.defer();
        var companyId = $scope.companyDetails.CompanyId;
        CompanyService.GetAllCaseProfiles(companyId).then(
            function (d) {
                $scope.caseProfiles = d.data;
                deferred.resolve('Get case profiles');
            },
            function () {
                quickNotification("Error occurred during loading case profiles data", "error");
            }
        );

        return deferred.promise;
    };

    var GetCompanyDetail = function () {
        var deferred = $q.defer();

        CompanyService.GetCompanyDetails().then(
            function (d) {
                $scope.companyDetails = d.data;
                if (!String.isNullOrEmpty($scope.companyDetails.Grade) ||
                    !String.isNullOrEmpty($scope.companyDetails.PrimeThreshold)) {
                    $scope.IsShowIndustryGICSLevel3 = true;
                }

                if (($scope.companyDetails.IsParkedForGssResearchSince != null)) {
                    $scope.companyDetails.IsParkedForGssResearchSince = new Date(convertDate($scope.companyDetails.IsParkedForGssResearchSince, 'yyyy/MM/dd'));
                    $scope.IsGSSParked = true;
                }

                $scope.IsinLink = $window.oldClientsSiteUrl + "I_Companies_Id=" +
                    $scope.companyDetails.CompanyId +
                    "&I_GesCompanies_Id=" +
                    $scope.companyDetails.GesCompanyId +
                    "&s_Sedol=" +
                    $scope.companyDetails.Sedol +
                    "&s_Isin=" +
                    $scope.companyDetails.Isin;

                initCancelSaveConfirmationBox();

                CompanyService.companyDetails = $scope.companyDetails;
                $scope.$broadcast('finishInitCompany');
                deferred.resolve('Get Company');
            },
            function () {
                alert("Failed");
            }
        );

        return deferred.promise;
    };

    function CompanyPopulateData() {
        if ($scope.caseProfiles != null && $scope.caseProfiles.length > 0) {
            for (var i = 0; i < $scope.caseProfiles.length; i++) {

                if (($scope.caseProfiles[i].EntryDate != null)) {
                    $scope.caseProfiles[i].EntryDate = new Date(convertDate($scope.caseProfiles[i].EntryDate, 'yyyy/MM/dd'));
                }
            }
        }

        $scope.caseProfilesTableParams = new NgTableParams({
            page: 1,            // show first page
            count: 5          // count per page    
        }, {
                total: ($scope.caseProfiles != null ? $scope.caseProfiles.length : 0), // length of data
                counts: [5, 25, 50, 100],
                getData: function (params) {
                    params.total($scope.caseProfiles != null ? $scope.caseProfiles.length : 0);
                    $scope.data = $scope.caseProfiles.slice((params.page() - 1) * params.count(), params.page() * params.count());
                    return $scope.data;
                }
            });
    }

    function goToCompanyList() {
        $window.location.href = "/Company/List";
    }

    String.isNullOrEmpty = function (value) {
        return !(typeof value === "undefined" || (typeof value === "string" && value.length > 0));
    };

    function initCancelSaveConfirmationBox() {
        $("#cancel-save").confirmModal({
            confirmCallback: goToCompanyList
        });
    }

    $scope.getManagementSystemName = function (id) {
        var obj = $scope.managementSystems.filter(function (ms) {
            return ms.I_ManagementSystems_Id == id;
        });

        if (obj != null && obj.length > 0)
            return obj[0].Name;
        return "";
    }

    String.isNullOrEmpty = function (value) {
        return !(typeof value === "undefined" || (typeof value === "string" && value.length > 0));
    };

    function convertDate(value, format) {
        if (value != null && !value.isNullOrEmpty) {
            return $filter("date")(new Date(parseInt(value.substr(6))), format);
        }

        return null;
    };

    $scope.companySelect = function () {
        var grid = $("#tblcompanies");
        var rowKey = grid.jqGrid('getGridParam', "selrow");

        $scope.companyDetails.MasterCompanyName = grid.jqGrid('getCell', rowKey, 'Name');

        $scope.companyDetails.MasterCompanyId = grid.jqGrid('getCell', rowKey, 'Id');
    };

    $scope.mastercompanyClear = function () {
        $scope.companyDetails.MasterCompanyName = "";
        $scope.companyDetails.MasterCompanyId = "";
    };

    $scope.setValuesForPopup = function (d) {
        $(window).resize();
        selectRow(d);
    };

    function selectRow(id) {
        var $grid = $("#tblcompanies");
        $grid.jqGrid("setSelection", id);
    }

    $(function () {
        var postUrl = "/company/GetDataForMasterCompaniesJqGrid";
        var gridCaption = "";

        var getCountries = function () {
            return countries;
        }
        var setCountriesSelect = function (columnName) {
            this.jqGrid("setColProp", columnName, {
                stype: "select",
                searchoptions: {
                    value: buildSearchSelect(getCountries.call(this)),
                    sopt: ["eq"]
                }
            });
        };

        var grid = $("#tblcompanies");
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
            colNames: ["Id", "GES Company ID", "Name", "ISIN", "SEDOL", "Location", "Website", "Modified", "Number Of Cases"],
            colModel: [                
                { name: "Id", align: "center", hidden: true, key: true },
                { name: "MasterCompanyId", width: "100px", align: "center", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] }},
                { name: "Name", width: "300px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] } },                
                { name: "Isin", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] } },
                { name: "Sedol", width: "100px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] } },
                { name: "Location", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
                { name: "Website", hidden: true, searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] } },
                { name: "Modified", hidden: true, width: "85px", formatter: dateFormatter, searchoptions: { sopt: ["eq", "lt", "gt", "nu"] } },
                {
                    name: "NumberOfCases",
                    width: "85px",
                    search: false,
                    sortable: false,
                    align: "center",
                    hidden: true,
                    formatter: function (cellvalue, options, rowObject) {
                        var cellPrefix = "";
                        if (rowObject.IsMasterCompany === true) {
                            return cellPrefix + "<a href=\"/CaseProfile/List?companyId=" + rowObject.Id + "\">" + rowObject.NumberOfCases + " case profiles" + "</a>";
                        }
                        return "";
                    }
                }
            ],
            pager: $("#myPager"),
            rowNum: 50,
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
                $('#btn-contact-select').click();
            }
        });
        setCountriesSelect.call(grid, "Location");
        setFilterDate.call(grid, "Modified");

        grid.jqGrid("navGrid", "#myPager", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });

        grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: true });

    });

}]);