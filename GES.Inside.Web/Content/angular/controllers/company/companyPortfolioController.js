"use strict";
GesInsideApp.controller("CompanyPortfolioController", ["$scope", "$filter","$q", "$timeout", "$window", "CompanyService", "ModalService", "NgTableParams", function ($scope, $filter,$q, $timeout, $window, CompanyService, ModalService, NgTableParams) {
    $scope.companyPortfolioTableParams = null;
    $scope.companyPortfolios = [];
    $scope.sortType = 'Created'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.searchPortfolio = ''; 
    $scope.filter = "";
    $scope.$on('finishInitCompany', function (e) {
        init();
    });

    function init() {
        $scope.companyPortfolios = $scope.companyDetails.CompanyPortfolios;

        if ($scope.companyPortfolios != null) {
            $scope.companyPortfolioTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 10          // count per page    
            }, {
                    total: $scope.companyPortfolios.length, // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        var sortedData = params.sorting() ?
                            $filter('orderBy')($scope.companyPortfolios, params.orderBy()) :
                            $scope.companyPortfolios;
                        var filterInfo = params.filter();                        
                        var orderedData = filterInfo ?
                            $filter('filter')(sortedData, filterInfo, null) :
                            sortedData;
                        params.total(orderedData.length); // set total for recalc pagination
                        $scope.data = $q.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                        return $scope.data;
                    }
                });
        }        

        $scope.companyPortfoliosOriginal = angular.copy($scope.companyPortfolios);
    }
}]);