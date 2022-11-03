"use strict";
GesInsideApp.controller("CaseProfileKPIController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, ModalService, NgTableParams) {
    $scope.Kpis = [];
    $scope.editingKpis = [];
    $scope.editingKpi = null;
    $scope.editingKpisOriginal = [];
    $scope.KpiPerformances = [];
    $scope.template = '/Content/angular/templates/caseprofile/KpiDialog.html';
    //$scope.isNotAllowAddKpi = true;

    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();

    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });

    //KPIs
    $scope.GetCaseReportKPIsByCaseReportId = function (gesCaseReportId) {

        if (gesCaseReportId === 0)
            return;

        CaseProfileService.GetCaseReportKPIsByCaseReportId(gesCaseReportId).then(
            function (response) {
                $scope.editingKpisOriginal = response.data;
                var i;
                if ($scope.editingKpisOriginal.length > 0) {
                    for (i = 0; i < $scope.editingKpisOriginal.length; i++) {
                        $scope.editingKpisOriginal[i].Created =
                            $scope.convertDate($scope.editingKpisOriginal[i].Created, 'yyyy/MM/dd HH:mm:ss a');
                    }
                }
                $scope.editingKpis = angular.copy($scope.editingKpisOriginal);
            },
            function (reason) {
                quickNotification("Error occurred during loading KPIs, caused: " + reason, "error");
            }
        );
    };

    $scope.GetKpisByEngagementTypeId = function (engagementTypeId) {

        if (engagementTypeId === 0 || engagementTypeId === null)
            return;

        CaseProfileService.GetKpisByEngagementTypeId(engagementTypeId).then(
            function (response) {
                $scope.Kpis = response.data;
                var i;
                if ($scope.Kpis.length > 0) {
                    for (i = 0; i < $scope.Kpis.length; i++) {
                        $scope.Kpis[i].Created =
                            $scope.convertDate($scope.Kpis[i].Created, 'yyyy/MM/dd HH:mm:ss a');
                    }
                }
            },
            function (reason) {
                quickNotification("Error occurred during loading Kpi base on engagement, caused: " + reason, "error");
            }
        );
    };

    $scope.GetKpibyId = function (id) {
        var obj = $scope.editingKpis.filter(function (kpi) {
            return kpi.I_GesCaseReportsI_Kpis_Id == id;
        });

        if (obj != null && obj.length > 0)
            return obj[0];
        return null
    }

    $scope.GetKpiNamebyId = function (id) {
        var obj = $scope.Kpis.filter(function (kpi) {
            return kpi.I_Kpis_Id == id;
        });

        if (obj != null && obj.length > 0)
            return obj[0].Name;
        return "";
    }

    $scope.GetKpiPerformanceNamebyId = function (id) {
        var obj = $scope.KpiPerformances.filter(function (performance) {
            return performance.I_KpiPerformance_Id == id;
        });

        if (obj != null && obj.length > 0)
            return obj[0].Name;
        return "";
    }

    $scope.GetKpiPerformances = function () {

        CaseProfileService.GetKpiPerformances().then(
            function (response) {
                $scope.KpiPerformances = response.data;
                var i;
                if ($scope.KpiPerformances.length > 0) {
                    for (i = 0; i < $scope.KpiPerformances.length; i++) {
                        $scope.KpiPerformances[i].Created =
                            $scope.convertDate($scope.KpiPerformances[i].Created, 'yyyy/MM/dd HH:mm:ss a');
                        $scope.KpiPerformances[i].image = '<i class="fa fa-gear"></i>';
                    }
                    CaseProfileService.caseProfile.CaseReportKpiViewModels = $scope.KpiPerformances;
                }
            },
            function (reason) {
                quickNotification("Error occurred during loading Kpi performance, caused: " + reason, "error");
            }
        );
    };

    //End Kpis      

    $scope.editKpi = function (kpi) {
        if (kpi) {
            $scope.editingKpi = kpi;
        }
        ModalService.openModal($scope.template, $scope, "ng-dialog-small", 'CaseProfileKPIController');
    };

    $scope.cancelKpi = function (kpi) {
        if (kpi.I_GesCaseReportsI_Kpis_Id === 0) {
            $scope.$parent.editingKpis.splice(0, 1);
        }
        else {
            $scope.$parent.editingKpis = angular.copy($scope.$parent.editingKpisOriginal);
        }
        ModalService.closeModal();
    }

    $scope.saveKpi = function (kpi) {
        CaseProfileService.SaveKpi(kpi, saveKpiCallback);
    }

    $scope.saveAddNewKpi = function (kpi) {
        CaseProfileService.SaveKpi(kpi, saveAndAddNewKpiCallback);

    }

    function saveKpiCallback(Id) {
        if ($scope.$parent.editingKpi.I_GesCaseReportsI_Kpis_Id === 0) {
            $scope.$parent.editingKpi.I_GesCaseReportsI_Kpis_Id = Id;
        }
        $scope.$parent.editingKpisOriginal = angular.copy($scope.$parent.editingKpis);
        ModalService.closeModal();
    }

    function saveAndAddNewKpiCallback(Id) {
        if ($scope.$parent.editingKpi.I_GesCaseReportsI_Kpis_Id === 0) {
            $scope.$parent.editingKpi.I_GesCaseReportsI_Kpis_Id = Id;
        }
        $scope.$parent.editingKpisOriginal = angular.copy($scope.$parent.editingKpis);
        $scope.$parent.addKpi();
    }

    $scope.addKpi = function () {
        var temp = $scope.editingKpis;
        $scope.editingKpis = [];

        var kpi = {
            I_GesCaseReportsI_Kpis_Id: 0,
            I_GesCaseReports_Id: CaseProfileService.caseProfile.I_GesCaseReports_Id,
            I_Kpis_Id: "",
            I_KpiPerformance_Id: "",
            Created: "",
            disableEdit: true
        };

        $scope.editingKpi = kpi;

        $scope.editingKpis.push(kpi);

        if (temp != null) {
            for (var i = 0; i < temp.length; i++) {
                $scope.editingKpis.push(temp[i]);
            }
        }
        CaseProfileService.caseProfile.CaseReportKpiViewModels = $scope.editingKpis;

        ModalService.openModal($scope.template, $scope, "ng-dialog-small", 'CaseProfileKPIController');
    };

    $scope.deleteKpi = function (kpi, frommodal) {
        ModalService.openConfirm("Are you sure to delete this kpi?", function (result) {
            if (result) {
                $scope.editingKpi = kpi;
                if (frommodal) {
                    CaseProfileService.DeleteKpi(kpi, deleteKpiFromModalCallback);
                }
                else {
                    CaseProfileService.DeleteKpi(kpi, deleteKpiCallback);
                }

                ModalService.closeModal();

            }
        });
    };

    function deleteKpiCallback() {
        if ($scope.editingKpi) {

            for (var i in $scope.editingKpis) {
                if ($scope.editingKpis[i].I_GesCaseReportsI_Kpis_Id === $scope.editingKpi.I_GesCaseReportsI_Kpis_Id) {
                    $scope.editingKpis.splice(i, 1);
                }
            }
        }
        $scope.editingKpisOriginal = $scope.editingKpis;
        CaseProfileService.caseProfile.CaseReportKpiViewModels = $scope.editingKpis;
    }

    function deleteKpiFromModalCallback() {
        if ($scope.$parent.editingKpi) {

            for (var i in $scope.$parent.editingKpis) {
                if ($scope.$parent.editingKpis[i].I_GesCaseReportsI_Kpis_Id === $scope.$parent.editingKpi.I_GesCaseReportsI_Kpis_Id) {
                    $scope.$parent.editingKpis.splice(i, 1);
                }
            }
        }
        $scope.$parent.editingKpisOriginal = $scope.$parent.editingKpis;
        CaseProfileService.caseProfile.CaseReportKpiViewModels = $scope.editingKpis;
    }

    function init() {
        $scope.editingKpis = $scope.caseProfile.CaseReportKpiViewModels;
        $scope.GetCaseReportKPIsByCaseReportId($scope.caseProfile.I_GesCaseReports_Id);
        $scope.GetKpisByEngagementTypeId($scope.caseProfile.I_Engagement_Type_Id);
        $scope.GetKpiPerformances();
    }

    function kpiPerformanceFormatState(state) {
        var baseUrl = "/Content/img/icons/progress_";
        return formatState(state, baseUrl);
    }

    function formatState(state, baseUrl) {
        if (!state.id) {
            return state.text;
        }
        return $(
            '<div class="status-value response-status" style="background-image: url(\'' + baseUrl + state.text.toLowerCase() + '.png\')">' + state.text + "</div>"
        );
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
}]);