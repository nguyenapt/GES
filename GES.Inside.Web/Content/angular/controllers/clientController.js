"use strict";
GesInsideApp.controller("ClientController", ["$scope", "$timeout", "$window", "ClientService", "NgTableParams", function ($scope, $timeout, $window, clientService, NgTableParams) {
    $scope.isFormValid = false;
    $scope.message = null;
    $scope.clientDetails = null;
    $scope.editingAgreement= null;
    $scope.organizationServices = null;
    $scope.countries = null;
    $scope.clientTypes = null;
    $scope.services = null;
    $scope.allServices = null;
    $scope.isNewClient = false;
    $scope.isSaving = false;
    $scope.isAddNew = false;
    $scope.isAddNewService = true;
    $scope.isExistedService = false;

    init();


    function init() {
        initSelect2();
        var urlPath = $window.location.href;
        var urlPathSplit = String(urlPath).split("/");             
        
        var clientId = 0;

        if (urlPathSplit !== null && urlPathSplit.length > 0) {
            var i = urlPathSplit[urlPathSplit.length - 1].indexOf('#');
            var s = urlPathSplit[urlPathSplit.length - 1];
            if (i !== -1) {
                s  = urlPathSplit[urlPathSplit.length - 1].substring(0, i); 
            }            
            
            if (s!== 'Add') {
                clientId = s;
            }
        }

        $scope.isNewCaseProfile = urlPath.includes("/Add");

        if (clientId !== 0) {
            clientService.GetClientDetailsById(clientId).then(
                function (d) {
                    $scope.clientDetails = d.data;

                    $scope.organizationServices = $scope.clientDetails.OrganizationsServicesViewModels;
                    $scope.agreementsTableParams = new NgTableParams({
                        page: 1,            // show first page
                        count: 10          // count per page    
                    }, {
                        total: $scope.organizationServices.length, // length of data
                        counts: [10, 20, 30, 40],
                        getData: function(params) {
                            params.total($scope.organizationServices.length);
                            $scope.data = $scope.organizationServices.slice((params.page() - 1) * params.count(), params.page() * params.count());
                            return $scope.data;
                        }
                    });

                    $timeout(function () {
                        $("#country-select").val($scope.clientDetails.G_Countries_Id).trigger("change");
                        $("#billing-country-select").val($scope.clientDetails.BillingG_Countries_Id).trigger("change");
                        $("#client-type-select").val($scope.clientDetails.G_Industries_Id).trigger("change");
                        $scope.clientForm.$setPristine();

                        $scope.edited = true;                       
                        

                    }, 2500); //Add timeout to fix initialize value in minified code                     

                    initCancelSaveConfirmationBox();

                },
                function () {
                    alert("Failed");
                }
            );            
        }

        clientService.GetCountries().then(
            function (d) {
                $scope.countries = d.data;
            },
            function () {
                alert("Failed");
            }
        );

        clientService.GetClientTypes().then(
            function (d) {
                $scope.clientTypes = d.data;
            },
            function () {
                alert("Failed");
            }
        );

        clientService.GetServices().then(
            function (d) {
                $scope.services = d.data;
                $scope.allServices = d.data;

            },
            function () {
                alert("Failed");
            }
        );       

        $scope.openAgreementPopup = function (agreement) {
            $(window).resize();
            var selectedServiceId = "";
            var temp = $scope.allServices;
            $scope.services = [];
            
            if (agreement) {

                $scope.editingAgreement = agreement;
                
                for (var i = 0; i < temp.length; i++) {
                    if(temp[i].G_Services_Id === $scope.editingAgreement.ServicesId) {
                        $scope.services.push(temp[i]);
                        break;
                    }
                }    
                selectedServiceId = $scope.editingAgreement.ServicesId; 
                $scope.isAddNewService = false;
                
            } else {
                $scope.editingAgreement = {
                    OrganizationsServicesId: keyId(),
                    OrganizationsId: "",
                    ServicesId: "",
                    ServicesName: "",
                    ManagedDocumentsId: "",
                    Created: "",
                    Modified: "",
                    ModifiedByUsersId: "",
                    DemoEnd: "",
                    ServiceStatesId: "",
                    TermsAccepted: "",
                    TermsAcceptedByIp: "",
                    SuperFilter: "",
                    Price: "",
                    Reporting: "",
                    Comment: ""
                };
            }          
            
            for (var i = 0; i < temp.length; i++) {
                var isExisted = false;
                for (var j = 0; j < $scope.organizationServices.length; j++) {
                    if(temp[i].G_Services_Id.toString() === $scope.organizationServices[j].ServicesId.toString()) {
                        isExisted = true;
                        break;
                    }                    
                }
                if (!isExisted){
                    $scope.services.push(temp[i]);
                }
                
            }

            $timeout(function () {
                $("#service-select").val(selectedServiceId).trigger("change");

                $scope.edited = true;

            }, 100); //Add timeout to fix initialize value in minified code  

        };
    }

    $scope.deleteAgreement = function (agreement) {
        if (agreement) {

            for (var i in $scope.organizationServices) {
                if ($scope.organizationServices[i].OrganizationsServicesId === agreement.OrganizationsServicesId) {
                    $scope.organizationServices.splice(i, 1);
                }
            }

            if ($scope.agreementsTableParams.data.length === 1 && $scope.agreementsTableParams.page() !== 1) {
                $scope.agreementsTableParams.page($scope.agreementsTableParams.page() - 1);
            }
        }

        $scope.agreementsTableParams.reload();

    };

    $scope.agreementSelect = function () {
        
        if ($scope.editingAgreement != null) {
            
            for (var j = 0; j < $scope.services.length; j++){
                if ($scope.editingAgreement.ServicesId === $scope.services[j].G_Services_Id.toString()) {
                    $scope.editingAgreement.ServicesName = $scope.services[j].FullName;
                }
            } 
            
            if (!$scope.isAddNewService) {
                for (var i = 0; i < $scope.organizationServices.length; i++) {
                    if ($scope.organizationServices[i].OrganizationsServicesId === $scope.editingAgreement.OrganizationsServicesId) {
                        $scope.organizationServices[i].ServicesId = $scope.editingAgreement.ServicesId;
                        $scope.organizationServices[i].ServicesName = $scope.editingAgreement.ServicesName;

                        $scope.organizationServices[i].Price = $scope.editingAgreement.Price;
                        $scope.organizationServices[i].Reporting = $scope.editingAgreement.Reporting;
                        $scope.organizationServices[i].Comment = $scope.editingAgreement.Comment;
                        break;
                    }
                }
            } else{
                $scope.organizationServices.push($scope.editingAgreement);
            }                       
        }
 
        $scope.agreementsTableParams.reload();
        var lastPage = Math.ceil($scope.agreementsTableParams.total()/$scope.agreementsTableParams.count());
        
        $scope.agreementsTableParams.page(lastPage);

    };

    $scope.$watch(function(){
        return $scope.editingAgreement != null && $scope.editingAgreement.Price;
    }, function(newvalue, oldvalue){
        
        if (!newvalue && !oldvalue) {
            return;
        }
        
        if ($scope.editingAgreement !== null && $scope.editingAgreement.Price !== null && $scope.editingAgreement.Price.match(/^\d+$/) === null) {
            $scope.editingAgreement.Price = oldvalue;
        }

        if (newvalue != null && newvalue.toString() === ""){
            
            $scope.editingAgreement.Price = "";
        } 

    },true);

    function keyId(){
        var number = Math.random();
        number.toString(36);
        return number.toString(36).substr(2, 9);
    }

    $scope.UpdateClient = function () {
        $scope.isSaving = true;

        $scope.clientDetails.OrganizationsServicesViewModels = $scope.organizationServices;
        
        clientService.UpdateClientData($scope.clientDetails).then(function () {
            goToClientList();
        });

    };  

    function initSelect2() {
        $("#country-select").select2({
            templateResult: countryFormatState,
            templateSelection: countryFormatState
        });        
        $("#billing-country-select").select2({
            templateResult: countryFormatState,
            templateSelection: countryFormatState
        }); 
        
        $("#client-type-select").select2();
        $("#service-select").select2();
    }

    function countryFormatState(state) {
        if (!state.id) {
            return state.text;
        }

        var flagIcon = "";
        var countryCode = state.element.attributes["data-country-code"];
        if (countryCode !== undefined && countryCode !== null)
            flagIcon = '<span class="flag-icon flag-icon-' + countryCode.value.toLowerCase() + '"></span>';
        return $(
            '<div class="status-value">' + flagIcon + state.text + "</div>"
        );
    }

    function goToClientList() {
        $window.location.href = "/Client/List";
    }

    String.isNullOrEmpty = function(value) {
        return !(typeof value === "undefined" || (typeof value === "string" && value.length > 0));
    };

    function initCancelSaveConfirmationBox() {
        $("#cancel-save").confirmModal({
            confirmCallback: goToClientList
        });
    }

    
}]);
