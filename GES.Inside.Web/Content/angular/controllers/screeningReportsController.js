"use strict";
GesInsideApp.controller("ScreeningReportsController", ["$scope", "$timeout", "$window", "ScreeningReportsService", "NgTableParams", function ($scope, $timeout, $window, screeningReportsService, NgTableParams) {
    $scope.isFormValid = false;
    $scope.message = null;
    $scope.isExporting = false;

    $scope.clients = [];
    $scope.portfolios = [];
    $scope.screeningNormThemes = [];    
    $scope.portfoliosTemp = [];
    $scope.screeningNormThemesTemp = [];

    $scope.selectedScreeningReports = {
        clientId: "",
        portfolioId:"",
        FromDate: "",
        ToDate: "",
        NormTheme:""
    };

   
    init();

    $scope.example13model = []; 
    $scope.example13data = [ {id: 1, label: "David"}, {id: 2, label: "Jhon"}, {id: 3, label: "Lisa"}, {id: 4, label: "Nicole"}, {id: 5, label: "Danny"} ];
    $scope.example13settings = { smartButtonMaxItems: 3, smartButtonTextConverter: function(itemText, originalItem) { if (itemText === 'Jhon') { return 'Jhonny!'; } return itemText; } };
    
    function init() {
        initSelect2();
        var urlPath = $window.location.href;
        var urlPathSplit = String(urlPath).split("/");



        screeningReportsService.GetClients().then(
            function (d) {
                $scope.clients = d.data;            
                $timeout(function () {
                        $("#client-select").val(3485).trigger("change"); //Default Ges International
                    },
                    2500);
            },
            function () {
                alert("Failed");
            }
        );

        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!
        var yyyy = today.getFullYear();

        if (dd < 10) {
            dd = '0' + dd;
        }

        if (mm < 10) {
            mm = '0' + mm;
        }

        today = mm + '/' + dd + '/' + yyyy;

        $scope.selectedScreeningReports.ToDate = today;
    }
    initTheme();
    $scope.clientSelect = function (clientId){
        screeningReportsService.GetPortfolioIndex(clientId).then(
            function (d) {
                $scope.portfolios = d.data;
                $scope.portfoliosTemp = angular.copy($scope.portfolios);

            },
            function () {
                alert("Failed");
            }
        );
    };
    
    function initTheme(){
        $scope.screeningNormThemes = [
            { 	icon: "", 	value: "1", 	text: "Global Ethical Standard", 	ticked: true 	},
            { 	icon: "", 	value: "2", 	text: "Global Standards", 	ticked: true 	},
            { 	icon: "", 	value: "3", 	text: "Corporate Governance", 	ticked: false 	},
            { 	icon: "", 	value: "4", 	text: "Stewardship & Risk", 	ticked: false 	}
        ];

        $scope.screeningNormThemesTemp = angular.copy($scope.screeningNormThemes);
    }

    $scope.Reset = function () {

        $timeout(function () {
                $("#client-select").val(3485).trigger("change"); //Default Ges International
            },
            2500);
        $scope.screeningNormThemes= $scope.screeningNormThemesTemp;
        $scope.portfolios = $scope.portfoliosTemp;
        $scope.selectedScreeningReports.fromDate = "";
        
    };

    $(".export-btn").on("click", function (e) {
        var themeIds = "";
        var portfolioIds = "";
        $scope.isExporting = true;
        if (($scope.selectedScreeningNormThemes != null)) {
            var selectedThemeLength = $scope.selectedScreeningNormThemes.length;
            for (var i = 0; i < selectedThemeLength; i++) {
                themeIds += $scope.selectedScreeningNormThemes[i].value;
                
                if (i < selectedThemeLength - 1) {
                    themeIds += ",";
                }
            }
        }

        if (($scope.selectedportfolios != null)) {
            var selectedPortfolioLength = $scope.selectedportfolios.length;
            for (var j = 0; j < selectedPortfolioLength; j++) {
                portfolioIds += $scope.selectedportfolios[j].PortfolioId;

                if (j < selectedPortfolioLength - 1) {
                    portfolioIds += ",";
                }
            }
        }
        
        
        window.location.href = "/Export/ScreeningReportExport?clientId=" + $scope.selectedScreeningReports.clientId 
            + "&portfolioIdsString=" + portfolioIds
            + "&fromDate=" + $scope.selectedScreeningReports.fromDate 
            + "&toDate=" + $scope.selectedScreeningReports.ToDate
            + "&normThemeIdsString=" + themeIds ;
        $scope.isExporting = false;
        return true;
    });

    function initSelect2() {        
        $("#client-select").select2();
    }


    String.isNullOrEmpty = function(value) {
        return !(typeof value === "undefined" || (typeof value === "string" && value.length > 0));
    };


    
}]);
