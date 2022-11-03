"use strict";
GesInsideApp.controller("CompanyAdditionalDocumentController", ["$scope", "$filter", "$timeout", "$window", "CompanyService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CompanyService, ModalService, NgTableParams) {
    $scope.companyAdditionalDocumentTableParams = null;
    $scope.companyAdditionalDocuments = [];
    $scope.companyAdditionalDocumentsOriginal = [];
    $scope.editingCompanyAdditionalDocument = null;
    $scope.template = '/Content/angular/templates/company/CompanyAdditionalDocumentDialog.html';

    $scope.$on('finishInitCompany', function (e) {
        init();
    });

    $scope.addCompanyAdditionalDocument = function () {
        var temp = $scope.companyAdditionalDocuments;
        $scope.companyAdditionalDocuments = [];

        var companyAdditionalDocument = {
            I_Companies_Id: getCompanyId(),
            Comment: "",
            CompanyName: "",
            Created: null,
            DateText: "",
            DownloadUrl: "",
            FileExtension: "",
            FileName: "",
            G_DocumentManagementTaxonomies_Id: 0,
            G_ManagedDocumentApprovalStatuses_Id: 0,
            G_ManagedDocumentRiskLevels_Id: 0,
            G_ManagedDocumentServices_Id: 0,
            G_ManagedDocuments_Id: 0,
            G_Uploads_Id: 0,
            I_Ftse_Id: 0,
            I_GesCaseReports_Id: 0,
            I_GesCompanyDialogues_Id: 0,
            I_Msci_Id: 0,
            Keywords: "",
            Modified: null,
            ModifiedByG_Users_Id: 0,
            Name: "",
            NecI_Companies_Id: 0,
            ReportIncident: "",
            ServiceName: "",
            Source: ""
        };

        $scope.editingCompanyAdditionalDocument = companyAdditionalDocument;
        $scope.companyAdditionalDocuments.push(companyAdditionalDocument);

        if (temp != null) {
            for (var i = 0; i < temp.length; i++) {
                $scope.companyAdditionalDocuments.push(temp[i]);
            }
        }

        $scope.companyAdditionalDocumentTableParams.reload();

        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CompanyAdditionalDocumentController');

    };

    $scope.deleteCompanyAdditionalDocument = function (companyAdditionalDocument, frommodal) {
        ModalService.openConfirm("Are you sure to delete this document?", function (result) {
            if (result) {
                $scope.editingCompanyAdditionalDocument = companyAdditionalDocument;
                if (frommodal) {
                    CompanyService.DeleteCompanyAdditionalDocument(companyAdditionalDocument, deleteCompanyAdditionalDocumentFromModalCallback);
                }
                else {
                    CompanyService.DeleteCompanyAdditionalDocument(companyAdditionalDocument, deleteCompanyAdditionalDocumentCallback);
                }

                ModalService.closeModal();

            }
        });
    };

    function deleteCompanyAdditionalDocumentCallback() {
        if ($scope.editingCompanyAdditionalDocument != null) {
            for (var i in $scope.companyAdditionalDocuments) {
                if ($scope.companyAdditionalDocuments[i].G_ManagedDocuments_Id === $scope.editingCompanyAdditionalDocument.G_ManagedDocuments_Id) {
                    $scope.companyAdditionalDocuments.splice(i, 1);
                }
            }
            if ($scope.companyAdditionalDocumentTableParams.data.length === 1 && $scope.companyAdditionalDocumentTableParams.page() !== 1) {
                $scope.companyAdditionalDocumentTableParams.page($scope.companyAdditionalDocumentTableParams.page() - 1);
            }
        }

        $scope.companyAdditionalDocumentsOriginal = $scope.companyAdditionalDocuments;
        $scope.companyAdditionalDocumentTableParams.reload();
    }

    function deleteCompanyAdditionalDocumentFromModalCallback() {
        if ($scope.$parent.editingCompanyAdditionalDocument != null) {
            for (var i in $scope.$parent.companyAdditionalDocuments) {
                if ($scope.$parent.companyAdditionalDocuments[i].G_ManagedDocuments_Id === $scope.$parent.editingCompanyAdditionalDocument.G_ManagedDocuments_Id) {
                    $scope.$parent.companyAdditionalDocuments.splice(i, 1);
                }
            }
            if ($scope.$parent.companyAdditionalDocumentTableParams.data.length === 1 && $scope.$parent.companyAdditionalDocumentTableParams.page() !== 1) {
                $scope.$parent.companyAdditionalDocumentTableParams.page($scope.$parent.companyAdditionalDocumentTableParams.page() - 1);
            }
        }

        $scope.$parent.companyAdditionalDocumentsOriginal = $scope.$parent.companyAdditionalDocuments;
        $scope.$parent.companyAdditionalDocumentTableParams.reload();
    }

    $scope.editCompanyAdditionalDocument = function (companyAdditionalDocument) {
        if (companyAdditionalDocument) {
            $scope.editingCompanyAdditionalDocument = companyAdditionalDocument;
        }
        $scope.companyAdditionalDocumentTableParams.reload();
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CompanyAdditionalDocumentController');
    };

    $scope.cancelCompanyAdditionalDocument = function (companyAdditionalDocument) {
        if (companyAdditionalDocument.I_CompaniesI_ManagementSystems_Id === 0) {
            $scope.$parent.companyAdditionalDocuments.splice(0, 1);
        }
        else {
            $scope.$parent.companyAdditionalDocuments = angular.copy($scope.$parent.companyAdditionalDocumentsOriginal);
        }

        $scope.$parent.companyAdditionalDocumentTableParams.reload();


        ModalService.closeModal();

    };

    $scope.saveCompanyAdditionalDocument = function (companyAdditionalDocument, isAddNew) {

        var file = document.getElementById('file').files[0];

        var allowedExtensions = /(\.xls|\.xlsx|\.pdf|\.doc|\.docx|\.ppt|\.pptx)$/i;
        if (file !== undefined && fileValidation(file, allowedExtensions, "Please upload file having extensions .xls\\.xlsx\\.pdf\\.doc\\.docx\\.ppt\\.pptx only")) {
            companyAdditionalDocument.FileName =  file.Name;
            
        }        
                
        if (isAddNew) {
            CompanyService.SaveCompanyAdditionalDocument(companyAdditionalDocument, file,  saveAndAddNewCompanyAdditionalDocumentCallback);
        }
        else {
            CompanyService.SaveCompanyAdditionalDocument(companyAdditionalDocument, file, saveCompanyAdditionalDocumentCallback);
        }

        ModalService.closeModal();
    };    
    
    $scope.downloadDocument = function (documentId) {
            CompanyService.DownloadDocument(documentId);
    };

    function fileValidation(file, allowedExtensions, alertMessage) {
        var filePath = file.name;

        if (!allowedExtensions.exec(filePath)) {
            alert(alertMessage);
            return false;
        }

        return true;
    }
    
    function saveCompanyAdditionalDocumentCallback(data) {

        if ($scope.ngDialogData.editingCompanyAdditionalDocument.G_ManagedDocuments_Id === 0) {
            $scope.ngDialogData.editingCompanyAdditionalDocument.G_ManagedDocuments_Id = data.docId;
            $scope.ngDialogData.editingCompanyAdditionalDocument.DownloadUrl = data.filePath;
            $scope.ngDialogData.editingCompanyAdditionalDocument.FileName = data.fileName
        }
        
        $scope.ngDialogData.companyAdditionalDocumentsOriginal = angular.copy($scope.ngDialogData.companyAdditionalDocuments);
        $scope.ngDialogData.companyAdditionalDocumentTableParams.reload();
    }

    function saveAndAddNewCompanyAdditionalDocumentCallback(Id) {

        $scope.ngDialogData.companyAdditionalDocumentsOriginal = angular.copy($scope.ngDialogData.companyAdditionalDocuments);
        $scope.ngDialogData.companyAdditionalDocumentTableParams.reload();

        $scope.ngDialogData.addCompanyAdditionalDocument();
    }

    function init() {
        $scope.companyAdditionalDocuments = $scope.companyDetails.Documents;

        if ($scope.companyAdditionalDocuments != null && $scope.companyAdditionalDocuments.length > 0) {
            for (var i = 0; i < $scope.companyAdditionalDocuments.length; i++) {

                if (($scope.companyAdditionalDocuments[i].Created != null)) {
                    $scope.companyAdditionalDocuments[i].Created = new Date(convertDate($scope.companyAdditionalDocuments[i].Created, 'yyyy/MM/dd'));
                }
            }
        }
        if ($scope.companyAdditionalDocuments != null) {
            $scope.companyAdditionalDocumentTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5          // count per page    
            }, {
                    total: $scope.companyAdditionalDocuments.length, // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        params.total($scope.companyAdditionalDocuments != null ? $scope.companyAdditionalDocuments.length : 0);
                        if ($scope.companyAdditionalDocuments != null) {
                            $scope.data = $scope.companyAdditionalDocuments.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        }

                        return $scope.data;
                    }
                });
        }

        $scope.companyAdditionalDocumentsOriginal = angular.copy($scope.companyAdditionalDocuments);
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

    function getCompanyId() {
        var companyid = 0;
        if (CompanyService.companyDetails.CompanyId != 0) {
            companyid = CompanyService.companyDetails.CompanyId;
        }
        else {
            companyid = CompanyService.GetCompanyId()
        }
        return companyid;
    }
}]);