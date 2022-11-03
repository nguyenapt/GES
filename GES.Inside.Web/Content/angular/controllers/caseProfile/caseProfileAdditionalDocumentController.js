"use strict";
GesInsideApp.controller("CaseProfileAdditionalDocumentController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, ModalService, NgTableParams) {
    $scope.caseProfileAdditionalDocumentTableParams = null;
    $scope.caseProfileAdditionalDocuments = [];
    $scope.caseProfileAdditionalDocumentsOriginal = [];
    $scope.editingCaseProfileAdditionalDocument = null;
    $scope.template = '/Content/angular/templates/caseProfile/caseProfileAdditionalDocumentDialog.html';

    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });

    $scope.addCaseProfileAdditionalDocument = function () {
        var temp = $scope.caseProfileAdditionalDocuments;
        $scope.caseProfileAdditionalDocuments = [];

        var caseProfileAdditionalDocument = {
            I_Companies_Id: 0,
            Comment: "",
            caseProfileName: "",
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
            I_GesCaseReports_Id: CaseProfileService.caseProfile.I_GesCaseReports_Id,
            I_GescaseProfileDialogues_Id: 0,
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

        $scope.editingCaseProfileAdditionalDocument = caseProfileAdditionalDocument;
        $scope.caseProfileAdditionalDocuments.push(caseProfileAdditionalDocument);

        if (temp != null) {
            for (var i = 0; i < temp.length; i++) {
                $scope.caseProfileAdditionalDocuments.push(temp[i]);
            }
        }

        $scope.caseProfileAdditionalDocumentTableParams.reload();

        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileAdditionalDocumentController');

    };

    $scope.deleteCaseProfileAdditionalDocument = function (caseProfileAdditionalDocument, frommodal) {
        ModalService.openConfirm("Are you sure to delete this document?", function (result) {
            if (result) {
                $scope.editingCaseProfileAdditionalDocument = caseProfileAdditionalDocument;
                if (frommodal) {
                    CaseProfileService.DeleteCaseProfileAdditionalDocument(caseProfileAdditionalDocument, deleteCaseProfileAdditionalDocumentFromModalCallback);
                }
                else {
                    CaseProfileService.DeleteCaseProfileAdditionalDocument(caseProfileAdditionalDocument, deleteCaseProfileAdditionalDocumentCallback);
                }

                ModalService.closeModal();

            }
        });
    };

    function deleteCaseProfileAdditionalDocumentCallback() {
        if ($scope.editingCaseProfileAdditionalDocument != null) {
            for (var i in $scope.caseProfileAdditionalDocuments) {
                if ($scope.caseProfileAdditionalDocuments[i].G_ManagedDocuments_Id === $scope.editingCaseProfileAdditionalDocument.G_ManagedDocuments_Id) {
                    $scope.caseProfileAdditionalDocuments.splice(i, 1);
                }
            }
            if ($scope.caseProfileAdditionalDocumentTableParams.data.length === 1 && $scope.caseProfileAdditionalDocumentTableParams.page() !== 1) {
                $scope.caseProfileAdditionalDocumentTableParams.page($scope.caseProfileAdditionalDocumentTableParams.page() - 1);
            }
        }

        $scope.caseProfileAdditionalDocumentsOriginal = $scope.caseProfileAdditionalDocuments;
        $scope.caseProfileAdditionalDocumentTableParams.reload();
    }

    function deleteCaseProfileAdditionalDocumentFromModalCallback() {
        if ($scope.$parent.editingCaseProfileAdditionalDocument != null) {
            for (var i in $scope.$parent.caseProfileAdditionalDocuments) {
                if ($scope.$parent.caseProfileAdditionalDocuments[i].G_ManagedDocuments_Id === $scope.$parent.editingCaseProfileAdditionalDocument.G_ManagedDocuments_Id) {
                    $scope.$parent.caseProfileAdditionalDocuments.splice(i, 1);
                }
            }
            if ($scope.$parent.caseProfileAdditionalDocumentTableParams.data.length === 1 && $scope.$parent.caseProfileAdditionalDocumentTableParams.page() !== 1) {
                $scope.$parent.caseProfileAdditionalDocumentTableParams.page($scope.$parent.caseProfileAdditionalDocumentTableParams.page() - 1);
            }
        }

        $scope.$parent.caseProfileAdditionalDocumentsOriginal = $scope.$parent.caseProfileAdditionalDocuments;
        $scope.$parent.caseProfileAdditionalDocumentTableParams.reload();
    }

    $scope.editCaseProfileAdditionalDocument = function (caseProfileAdditionalDocument) {
        if (caseProfileAdditionalDocument) {
            $scope.editingCaseProfileAdditionalDocument = caseProfileAdditionalDocument;
        }
        $scope.caseProfileAdditionalDocumentTableParams.reload();
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileAdditionalDocumentController');
    };

    $scope.cancelCaseProfileAdditionalDocument = function (caseProfileAdditionalDocument) {
        if (caseProfileAdditionalDocument.I_CompaniesI_ManagementSystems_Id === 0) {
            $scope.$parent.caseProfileAdditionalDocuments.splice(0, 1);
        }
        else {
            $scope.$parent.caseProfileAdditionalDocuments = angular.copy($scope.$parent.caseProfileAdditionalDocumentsOriginal);
        }

        $scope.$parent.caseProfileAdditionalDocumentTableParams.reload();


        ModalService.closeModal();

    };

    $scope.saveCaseProfileAdditionalDocument = function (caseProfileAdditionalDocument, isAddNew) {

        var file = document.getElementById('file').files[0];

        var allowedExtensions = /(\.xls|\.xlsx|\.pdf|\.doc|\.docx|\.ppt|\.pptx)$/i;
        if (file !== undefined && fileValidation(file, allowedExtensions, "Please upload file having extensions .xls\\.xlsx\\.pdf\\.doc\\.docx\\.ppt\\.pptx only")) {
            caseProfileAdditionalDocument.FileName =  file.Name;
            
        }        
                
        if (isAddNew) {
            CaseProfileService.SavecaseProfileAdditionalDocument(caseProfileAdditionalDocument, file,  saveAndAddNewCaseProfileAdditionalDocumentCallback);
        }
        else {
            CaseProfileService.SavecaseProfileAdditionalDocument(caseProfileAdditionalDocument, file, saveCaseProfileAdditionalDocumentCallback);
        }


        ModalService.closeModal();
    };    
    
    $scope.downloadDocument = function (documentId) {
            CaseProfileService.DownloadDocument(documentId);
    };

    function fileValidation(file, allowedExtensions, alertMessage) {
        var filePath = file.name;

        if (!allowedExtensions.exec(filePath)) {
            alert(alertMessage);
            return false;
        }

        return true;
    }
    
    function saveCaseProfileAdditionalDocumentCallback(data) {

        if ($scope.ngDialogData.editingCaseProfileAdditionalDocument.G_ManagedDocuments_Id === 0) {
            $scope.ngDialogData.editingCaseProfileAdditionalDocument.G_ManagedDocuments_Id = data.docId;
            $scope.ngDialogData.editingCaseProfileAdditionalDocument.DownloadUrl = data.filePath;
            $scope.ngDialogData.editingCaseProfileAdditionalDocument.FileName = data.fileName
        }
        
        $scope.ngDialogData.caseProfileAdditionalDocumentsOriginal = angular.copy($scope.ngDialogData.caseProfileAdditionalDocuments);
        $scope.ngDialogData.caseProfileAdditionalDocumentTableParams.reload();
    }

    function saveAndAddNewCaseProfileAdditionalDocumentCallback(Id) {

        $scope.ngDialogData.caseProfileAdditionalDocumentsOriginal = angular.copy($scope.ngDialogData.caseProfileAdditionalDocuments);
        $scope.ngDialogData.caseProfileAdditionalDocumentTableParams.reload();

        $scope.ngDialogData.addcaseProfileAdditionalDocument();
    }

    function init() {
        $scope.caseProfileAdditionalDocuments = CaseProfileService.caseProfile.Documents;

        if ($scope.caseProfileAdditionalDocuments != null && $scope.caseProfileAdditionalDocuments.length > 0) {
            for (var i = 0; i < $scope.caseProfileAdditionalDocuments.length; i++) {

                if (($scope.caseProfileAdditionalDocuments[i].Created != null)) {
                    $scope.caseProfileAdditionalDocuments[i].Created = new Date(convertDate($scope.caseProfileAdditionalDocuments[i].Created, 'yyyy/MM/dd'));
                }
            }
        }
        if ($scope.caseProfileAdditionalDocuments != null) {
            $scope.caseProfileAdditionalDocumentTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5          // count per page    
            }, {
                    total: $scope.caseProfileAdditionalDocuments.length, // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        params.total($scope.caseProfileAdditionalDocuments != null ? $scope.caseProfileAdditionalDocuments.length : 0);
                        if ($scope.caseProfileAdditionalDocuments != null) {
                            $scope.data = $scope.caseProfileAdditionalDocuments.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        }

                        return $scope.data;
                    }
                });
        }

        $scope.caseProfileAdditionalDocumentsOriginal = angular.copy($scope.caseProfileAdditionalDocuments);
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

    function getCaseProfileId() {
        var caseProfileid = 0;
        if (CaseProfileService.caseProfileDetails.caseProfileId != 0) {
            caseProfileid = CaseProfileService.caseProfileDetails.caseProfileId;
        }
        else {
            caseProfileid = CaseProfileService.GetCaseProfileId()
        }
        return caseProfileid;
    }

    function getCompanyId() {
        var companyid = 0;
        if (CaseProfileService.companyDetails.CompanyId != 0) {
            companyid = CompanyService.companyDetails.CompanyId;
        }
        else {
            companyid = CompanyService.GetCompanyId()
        }
        return companyid;
    }
}]);