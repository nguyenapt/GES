"use strict";
GesInsideApp.controller("CaseProfileNewsController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, ModalService, NgTableParams) {
    $scope.newsTableParams = null;
    $scope.newsList = [];
    $scope.newsListOriginal = [];
    $scope.editingNews = null;
    $scope.isAddNewNews = true;
    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();
    $scope.template = '/Content/angular/templates/caseprofile/LatestNewsDialog.html';

    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });

    $scope.addNews = function () {
        var temp = $scope.newsList;
        $scope.newsList = [];

        var news = {
            I_GesLatestNews_Id: 0,
            I_GesCaseReports_Id: CaseProfileService.caseProfile.I_GesCaseReports_Id,
            Description: "",
            LatestNewsModified: "",
            LatestNewsModifiedString: "",
            Created: "",
            CreatedString: ""
        };

        $scope.editingNews = news;
        $scope.newsList.push(news);

        if (temp != null) {
            for (var i = 0; i < temp.length; i++) {
                $scope.newsList.push(temp[i]);
            }
        }

        CaseProfileService.caseProfile.GesLatestNewsViewModels = $scope.newsList;
        $scope.newsTableParams.reload();
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileNewsController');
    };

    $scope.deleteNews = function (news, frommodal) {
        ModalService.openConfirm("Are you sure to delete this news?", function (result) {
            if (result) {
                $scope.editingNews = news;
                if (frommodal) {
                    CaseProfileService.DeleteNews(news, deleteNewsFromModalCallback);
                }
                else {
                    CaseProfileService.DeleteNews(news, deleteNewsCallback);
                }
                ModalService.closeModal();
            }
        });
    };

    function deleteNewsCallback() {
        if ($scope.editingNews) {
            for (var i in $scope.newsList) {
                if ($scope.newsList[i].I_GesLatestNews_Id === $scope.editingNews.I_GesLatestNews_Id) {
                    $scope.newsList.splice(i, 1);
                }
            }
            if ($scope.newsTableParams.data.length === 1 && $scope.newsTableParams.page() !== 1) {
                $scope.newsTableParams.page($scope.newsTableParams.page() - 1);
            }
        }

        CaseProfileService.caseProfile.GesLatestNewsViewModels = $scope.newsList;
        $scope.newsListOriginal = $scope.newsList;
        $scope.newsTableParams.reload();
    }

    function deleteNewsFromModalCallback() {
        if ($scope.$parent.editingNews) {
            for (var i in $scope.$parent.newsList) {
                if ($scope.$parent.newsList[i].I_GesLatestNews_Id === $scope.$parent.editingNews.I_GesLatestNews_Id) {
                    $scope.$parent.newsList.splice(i, 1);
                }
            }
            if ($scope.$parent.newsTableParams.data.length === 1 && $scope.$parent.newsTableParams.page() !== 1) {
                $scope.$parent.newsTableParams.page($scope.$parent.newsTableParams.page() - 1);
            }
        }

        CaseProfileService.caseProfile.GesLatestNewsViewModels = $scope.$parent.newsList;
        $scope.$parent.newsListOriginal = $scope.$parent.newsList;
        $scope.$parent.newsTableParams.reload();
    }

    $scope.editNews = function (news) {
        if (news) {
            $scope.editingNews = news;
        }
        $scope.newsTableParams.reload();
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileNewsController');
    };

    $scope.cancelNews = function (news) {
        if (news.I_GesLatestNews_Id === 0) {
            $scope.$parent.newsList.splice(0, 1);
        }
        else {
            $scope.$parent.newsList = angular.copy($scope.$parent.newsListOriginal);
        }

        $scope.$parent.newsTableParams.reload();
        ModalService.closeModal();
    }

    $scope.saveNews = function (news, isAddNew) {
        if (isAddNew) {
            CaseProfileService.SaveNews(news, saveAndAddNewNewsCallback);
        }
        else {
            CaseProfileService.SaveNews(news, saveNewsCallback);
        }
       // news.LatestNewsModifiedString = new Date(convertDate($scope.ngDialogData.editingNews.LatestNewsModified, 'yyyy/MM/dd'));
        ModalService.closeModal();
    }

    function saveNewsCallback(Id) {
        if ($scope.ngDialogData.editingNews.I_GesLatestNews_Id === 0) {
            $scope.ngDialogData.editingNews.I_GesLatestNews_Id = Id;
        }

        $scope.ngDialogData.editingNews.LatestNewsModifiedString = ConvertDate2($scope.ngDialogData.editingNews.LatestNewsModified.toString());
        
        $scope.ngDialogData.newsListOriginal = angular.copy($scope.ngDialogData.newsList);
        $scope.ngDialogData.newsTableParams.reload();
    }

    function saveAndAddNewNewsCallback(Id) {
        if ($scope.ngDialogData.editingNews.I_GesLatestNews_Id === 0) {
            $scope.ngDialogData.editingNews.I_GesLatestNews_Id = Id;
        }
        $scope.ngDialogData.editingNews.LatestNewsModifiedString = ConvertDate2($scope.ngDialogData.editingNews.LatestNewsModified.toString());
        $scope.ngDialogData.newsListOriginal = angular.copy($scope.ngDialogData.newsList);
        $scope.ngDialogData.newsTableParams.reload();
        $scope.ngDialogData.addNews();
    }

    function init() {
        $scope.newsList = $scope.caseProfile.GesLatestNewsViewModels;

        if ($scope.newsList != null && $scope.newsList.length > 0) {
            for (var i = 0; i < $scope.newsList.length; i++) {

                if (($scope.newsList[i].Created != null)) {
                    $scope.newsList[i].Created = new Date(convertDate($scope.newsList[i].Created, 'yyyy/MM/dd'));
                }
                if (($scope.newsList[i].LatestNewsModified != null)) {
                    $scope.newsList[i].LatestNewsModified = new Date(convertDate($scope.newsList[i].LatestNewsModified, 'yyyy/MM/dd'));
                }
            }
        }
        if ($scope.newsList != null) {
            $scope.newsTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5          // count per page    
            }, {
                    total: $scope.newsList.length, // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        params.total($scope.newsList != null ? $scope.newsList.length : 0);
                        if ($scope.newsList != null) {
                            $scope.data = $scope.newsList.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        }

                        return $scope.data;
                    }
                });
        }

        $scope.newsListOriginal = angular.copy($scope.newsList);
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

    /**
     * @return {string}
     */
    function ConvertDate2(value) {
        var date = new Date(value);
        var monthValue = date.getMonth() + 1;
        
        if ((date.getMonth() + 1) < 10){
            monthValue = "0" + (date.getMonth() + 1).toString()
        }
     
       return date.getFullYear() + '-' + monthValue + '-' + date.getDate();
    }

    
}]);