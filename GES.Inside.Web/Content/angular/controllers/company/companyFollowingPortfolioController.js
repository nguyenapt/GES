"use strict";
GesInsideApp.controller("CompanyFollowingPortfolioController", ["$scope", "$filter", "$timeout", "$q", "$window", "CompanyService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $q, $window, CompanyService, ModalService, NgTableParams) {
    $scope.companyFollowingPortfolioStandardTableParams = null;
    $scope.companyFollowingPortfolioOtherTableParams = null;
    $scope.companyFollowingPortfolioCustomerTableParams = null;
    $scope.companyFollowingStandardPortfolios = [];
    $scope.companyFollowingOtherPortfolios = [];
    $scope.companyFollowingCustomerPortfolios = [];
    $scope.sortType = 'Created'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.searchPortfolio = '';
    $scope.orderedData = [];
    $scope.$on('finishInitCompany', function (e) {
        init();
    });

    function init() {
        $scope.companyFollowingStandardPortfolios = $scope.companyDetails.CompanyFollowingStandardPortfolios;
        $scope.companyFollowingOtherPortfolios = $scope.companyDetails.CompanyFollowingOtherPortfolios;
        $scope.companyFollowingCustomerPortfolios = $scope.companyDetails.CompanyFollowingCustomerPortfolios;
        $scope.orderedData = angular.copy($scope.companyFollowingCustomerPortfolios);
        if ($scope.companyFollowingStandardPortfolios != null && $scope.companyFollowingStandardPortfolios.length > 0) {
            for (var i = 0; i < $scope.companyFollowingStandardPortfolios.length; i++) {

                if (($scope.companyFollowingStandardPortfolios[i].Created != null)) {
                    $scope.companyFollowingStandardPortfolios[i].Created = new Date(convertDate($scope.companyFollowingStandardPortfolios[i].Created, 'yyyy/MM/dd'));
                }
            }
        }

        if ($scope.companyFollowingOtherPortfolios != null && $scope.companyFollowingOtherPortfolios.length > 0) {
            for (var i = 0; i < $scope.companyFollowingOtherPortfolios.length; i++) {

                if (($scope.companyFollowingOtherPortfolios[i].Created != null)) {
                    $scope.companyFollowingOtherPortfolios[i].Created = new Date(convertDate($scope.companyFollowingOtherPortfolios[i].Created, 'yyyy/MM/dd'));
                }
            }
        }

        if ($scope.companyFollowingCustomerPortfolios != null && $scope.companyFollowingCustomerPortfolios.length > 0) {
            for (var i = 0; i < $scope.companyFollowingCustomerPortfolios.length; i++) {

                if (($scope.companyFollowingCustomerPortfolios[i].Created != null)) {
                    $scope.companyFollowingCustomerPortfolios[i].Created = new Date(convertDate($scope.companyFollowingCustomerPortfolios[i].Created, 'yyyy/MM/dd'));
                }
            }
        }


        if ($scope.companyFollowingStandardPortfolios != null) {
            $scope.companyFollowingPortfolioStandardTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 10          // count per page    
            }, {
                    total: $scope.companyFollowingStandardPortfolios.length, // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        // use build-in angular filter
                        var sortedData = params.sorting() ?
                            $filter('orderBy')($scope.companyFollowingStandardPortfolios, params.orderBy()) :
                            $scope.companyFollowingStandardPortfolios;
                        var filterInfo = params.filter();
                        var comparer = (filterInfo && filterInfo['0']) ? dateComparer : undefined;
                        var orderedData = filterInfo ?
                            $filter('filter')(sortedData, filterInfo, comparer) :
                            sortedData;
                        params.total(orderedData.length); // set total for recalc pagination
                        $scope.data = $q.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                        return $scope.data;
                    }
                });
        }

        if ($scope.companyFollowingOtherPortfolios != null) {
            $scope.companyFollowingPortfolioOtherTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 10          // count per page    
            }, {
                    total: $scope.companyFollowingOtherPortfolios.length, // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        // use build-in angular filter
                        var sortedData = params.sorting() ?
                            $filter('orderBy')($scope.companyFollowingOtherPortfolios, params.orderBy()) :
                            $scope.companyFollowingOtherPortfolios;
                        var filterInfo = params.filter();
                        var comparer = (filterInfo && filterInfo['0']) ? dateComparer : undefined;
                        var orderedData = filterInfo ?
                            $filter('filter')(sortedData, filterInfo, comparer) :
                            sortedData;
                        params.total(orderedData.length); // set total for recalc pagination
                        $scope.data = $q.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                        return $scope.data;
                    }
                });
        }

        $scope.companyFollowingPortfolioCustomerTableParams = new NgTableParams(
            {
                page: 1, // show first page  
                count: 5, // count per page  
                sorting:
                    {
                        Created: 'desc' // initial sorting  
                    }
            },
            {
                total: $scope.orderedData.length,
                counts: [5, 25, 50, 100],
                getData: function (params) {
                    // use build-in angular filter
                    var sortedData = params.sorting() ?
                        $filter('orderBy')($scope.companyFollowingCustomerPortfolios, params.orderBy()) :
                        $scope.companyFollowingCustomerPortfolios;
                    var filterInfo = params.filter();
                    var comparer = (filterInfo && filterInfo['0']) ? dateComparer : undefined;                    
                    var orderedData = filterInfo ?
                        $filter('filter')(sortedData, filterInfo, comparer) :
                        sortedData;
                    params.total(orderedData.length); // set total for recalc pagination
                    $scope.data = $q.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                    return $scope.data;
                }
            });


        //if ($scope.companyFollowingCustomerPortfolios != null) {
        //    $scope.companyFollowingPortfolioCustomerTableParams = new NgTableParams({
        //        page: 1,            // show first page
        //        count: 10          // count per page    
        //    }, {
        //            total: $scope.companyFollowingCustomerPortfolios.length, // length of data
        //            counts: [5, 25, 50, 100],
        //            getData: function (params) {
        //                params.total($scope.companyFollowingCustomerPortfolios != null ? $scope.companyFollowingCustomerPortfolios.length : 0);
        //                if ($scope.companyFollowingCustomerPortfolios != null) {
        //                    $scope.data = $scope.companyFollowingCustomerPortfolios.slice((params.page() - 1) * params.count(), params.page() * params.count());
        //                }

        //                return $scope.data;
        //            }
        //        });
        //}

    }
    function dateComperator(obj, text) {
        var valueAsText = obj + '';
        if (valueAsText.length == 13) { // must be milliseconds.
            valueAsText = $filter('date')(obj, 'd MMM yyyy HH:mm');
        }
        return !text || (obj && valueAsText.toLowerCase().indexOf(text.toLowerCase()) > -1);
    };

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