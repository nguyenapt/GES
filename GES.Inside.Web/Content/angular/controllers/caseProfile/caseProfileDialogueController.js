"use strict";
GesInsideApp.controller("CaseProfileDialogueController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "GesContactService", "GesOrganizationService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, GesContactService, GesOrganizationService, ModalService, NgTableParams) {
    $scope.editingGesContact = null;

    $scope.editingDialogue = null;
    $scope.editingDocument = null;
    $scope.file = null;
    $scope.dialogType = null;
    $scope.companyDialogueLogs = [];
    $scope.sourceDialogueLogs = [];
    $scope.companyDialogueLogsOriginal = [];
    $scope.sourceDialogueLogsOriginal = [];

    $scope.contactDirections = [];
    $scope.contactTypes = [];
    $scope.template = '/Content/angular/templates/caseprofile/CompanyDialogueDialog.html';
    $scope.sourceDialogueTemplate = '/Content/angular/templates/caseprofile/SourceDialogueDialog.html';    
    $scope.templateContactSelect = '/Content/angular/templates/caseprofile/ContactListDialog.html';
    $scope.templateOrganizationSelect = '/Content/angular/templates/caseprofile/OrganizationListDialog.html';

    $scope.templateContact = '/Content/angular/templates/config/GesContactDialog.html';
    $scope.templateOrganization = '/Content/angular/templates/config/GesOrganizationDialog.html';

    $scope.DocumentManagementTemplate = '/Content/angular/templates/caseprofile/DocumentManagementDialog.html';

    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();
    $scope.addEditDialogueDialogId = "";
    $scope.contactSelectDialogId = "";
    $scope.organizationSelectDialogId = "";
    $scope.addEditContactOrganizationDialogId = "";
    $scope.documentManagementDialogId = "";
    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });

    //* Company Dialog methods
    $scope.addCompanyDialogue = function () {
        var temp = $scope.companyDialogueLogs;
        $scope.companyDialogueLogs = [];

        var companyDialogueLog = {
            I_GesCompanySourceDialogues_Id: 0,
            I_GesCaseReports_Id: CaseProfileService.caseProfile.I_GesCaseReports_Id,
            ContactDate: "",
            ContactDirectionId: 0,
            ContactTypeId: 0,
            ContactTypeName: "",
            JobTitle: "",
            LastName: "",
            FirstName: "",
            FileName: "",
            ClassA: false,
            DialogueType: "Company",
            G_ManagedDocuments_Id: null
        };

        $scope.editingDialogue = companyDialogueLog;

        $scope.companyDialogueLogs.push(companyDialogueLog);

        if (temp != null) {
            for (var i = 0; i < temp.length; i++) {
                $scope.companyDialogueLogs.push(temp[i]);
            }
        }

        CaseProfileService.caseProfile.CompanyDialogueLogs = $scope.companyDialogueLogs;

        $scope.companyDialogueTableParams.reload();

        $scope.addEditDialogueDialogId = ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileDialogueController');
    };

    $scope.deleteCompanyDialogue = function (companyDialogue, frommodal) {
        ModalService.openConfirm("Are you sure to delete this company dialogue?", function (result) {
            if (result) {
                $scope.editingDialogue = companyDialogue;
                if (frommodal) {
                    CaseProfileService.DeleteCompanyDialogue(companyDialogue, deleteCompanyDialogueFromModalCallback);
                }
                else {
                    CaseProfileService.DeleteCompanyDialogue(companyDialogue, deleteCompanyDialogueCallback);
                }

                ModalService.closeModal($scope.addEditDialogueDialogId,null);
            }
        });
    };

    function deleteCompanyDialogueCallback() {
        if ($scope.editingDialogue) {
            for (var i in $scope.companyDialogueLogs) {
                if ($scope.companyDialogueLogs[i].I_GesCompanySourceDialogues_Id === $scope.editingDialogue.I_GesCompanySourceDialogues_Id) {
                    $scope.companyDialogueLogs.splice(i, 1);
                }
            }
            if ($scope.companyDialogueTableParams.data.length === 1 && $scope.companyDialogueTableParams.page() !== 1) {
                $scope.companyDialogueTableParams.page($scope.companyDialogueTableParams.page() - 1);
            }
        }
        CaseProfileService.caseProfile.CompanyDialogueLogs = $scope.companyDialogueLogs;
        $scope.companyDialogueLogsOriginal = $scope.companyDialogueLogs;
        $scope.companyDialogueTableParams.reload();
    }

    function deleteCompanyDialogueFromModalCallback() {
        if ($scope.$parent.editingDialogue) {
            for (var i in $scope.$parent.companyDialogueLogs) {
                if ($scope.$parent.companyDialogueLogs[i].I_GesCompanySourceDialogues_Id === $scope.$parent.editingDialogue.I_GesCompanySourceDialogues_Id) {
                    $scope.$parent.companyDialogueLogs.splice(i, 1);
                }
            }
            if ($scope.$parent.companyDialogueTableParams.data.length === 1 && $scope.$parent.companyDialogueTableParams.page() !== 1) {
                $scope.$parent.companyDialogueTableParams.page($scope.$parent.companyDialogueTableParams.page() - 1);
            }
        }

        CaseProfileService.caseProfile.CompanyDialogueLogs = $scope.$parent.companyDialogueLogs;
        $scope.$parent.companyDialogueLogsOriginal = $scope.$parent.companyDialogueLogs;
        $scope.$parent.companyDialogueTableParams.reload();
    }

    $scope.editCompanyDialogue = function (companyDialogue) {
        $scope.editingDocument = null;
        $scope.file = null;

        if (companyDialogue) {
            $scope.editingDialogue = companyDialogue;
            //$scope.GetDocumentMngByDocumentId(companyDialogue.I_GesCompanySourceDialogues_Id,
            //    companyDialogue.G_ManagedDocuments_Id);
        }
        $scope.dialogType = "Company";
        $scope.companyDialogueTableParams.reload();
        $scope.addEditDialogueDialogId = ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileDialogueController');
    };

    //* Source Dialog methods
    $scope.addSourceDialogue = function () {
        var temp = $scope.sourceDialogueLogs;
        $scope.sourceDialogueLogs = [];

        var sourceDialogueLog = {
            I_GesCompanySourceDialogues_Id: 0,
            I_GesCaseReports_Id: CaseProfileService.caseProfile.I_GesCaseReports_Id,
            ContactDate: "",
            ContactDirectionId: 0,
            ContactTypeId: 0,
            ContactTypeName: "",
            JobTitle: "",
            LastName: "",
            FirstName: "",
            FileName: "",
            ClassA: false,
            DialogueType: "Source",
            G_ManagedDocuments_Id: null
        };

        $scope.editingDialogue = sourceDialogueLog;

        $scope.sourceDialogueLogs.push(sourceDialogueLog);

        if (temp != null) {
            for (var i = 0; i < temp.length; i++) {
                $scope.sourceDialogueLogs.push(temp[i]);
            }
        }

        CaseProfileService.caseProfile.SourceDialogueLogs = $scope.sourceDialogueLogs;

        $scope.sourceDialogueTableParams.reload();

        $scope.addEditDialogueDialogId = ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileDialogueController');
            ModalService.openModal($scope.sourceDialogueTemplate, $scope, "ng-dialog-medium", 'CaseProfileDialogueController');
    };

    $scope.deleteSourceDialogue = function (sourceDialogue, frommodal) {
        ModalService.openConfirm("Are you sure to delete this source dialogue?", function (result) {
            if (result) {
                $scope.editingDialogue = sourceDialogue;
                if (frommodal) {
                    CaseProfileService.DeleteSourceDialogue(sourceDialogue, deleteSourceDialogueFromModalCallback);
                }
                else {
                    CaseProfileService.DeleteSourceDialogue(sourceDialogue, deleteSourceDialogueCallback);
                }


                ModalService.closeModal($scope.addEditDialogueDialogId,null);

            }
        });
    };

    function deleteSourceDialogueCallback() {
        if ($scope.editingDialogue) {

            for (var i in $scope.sourceDialogueLogs) {
                if ($scope.sourceDialogueLogs[i].I_GesCompanySourceDialogues_Id === $scope.editingDialogue.I_GesCompanySourceDialogues_Id) {
                    $scope.sourceDialogueLogs.splice(i, 1);
                }
            }

            if ($scope.sourceDialogueTableParams.data.length === 1 && $scope.companyDialogueTableParams.page() !== 1) {
                $scope.sourceDialogueTableParams.page($scope.sourceDialogueTableParams.page() - 1);
            }
        }

        CaseProfileService.caseProfile.SourceDialogueLogs = $scope.sourceDialogueLogs;
        $scope.sourceDialogueLogsOriginal = $scope.sourceDialogueLogs;
        $scope.sourceDialogueTableParams.reload();
    }

    function deleteSourceDialogueFromModalCallback() {
        if ($scope.$parent.editingDialogue) {

            for (var i in $scope.$parent.sourceDialogueLogs) {
                if ($scope.$parent.sourceDialogueLogs[i].I_GesCompanySourceDialogues_Id === $scope.$parent.editingDialogue.I_GesCompanySourceDialogues_Id) {
                    $scope.$parent.sourceDialogueLogs.splice(i, 1);
                }
            }

            if ($scope.$parent.sourceDialogueTableParams.data.length === 1 && $scope.$parent.companyDialogueTableParams.page() !== 1) {
                $scope.$parent.sourceDialogueTableParams.page($scope.$parent.sourceDialogueTableParams.page() - 1);
            }
        }

        CaseProfileService.caseProfile.SourceDialogueLogs = $scope.$parent.sourceDialogueLogs;
        $scope.$parent.sourceDialogueLogsOriginal = $scope.$parent.sourceDialogueLogs;
        $scope.$parent.sourceDialogueTableParams.reload();
    }

    $scope.editSourceDialogue = function (sourceDialogue) {
        $scope.editingDocument = null;
        $scope.file = null;

        if (sourceDialogue) {
            $scope.editingDialogue = sourceDialogue;
            //$scope.GetDocumentMngByDocumentId(sourceDialogue.I_GesCompanySourceDialogues_Id,
            //    sourceDialogue.G_ManagedDocuments_Id);

        }
        $scope.dialogType = "Source";
        $scope.sourceDialogueTableParams.reload();

        $scope.addEditDialogueDialogId = ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileDialogueController');
    };


    $scope.cancelDialogue = function (dialogue) {
        if (dialogue.I_GesCompanySourceDialogues_Id === 0) {
            if (dialogue.DialogueType === "Company") {
                $scope.$parent.companyDialogueLogs.splice(0, 1);
                $scope.$parent.companyDialogueTableParams.reload();
            }
            else {
                $scope.$parent.sourceDialogueLogs.splice(0, 1);
                $scope.$parent.sourceDialogueTableParams.reload();
            }
        }
        else {
            $scope.$parent.companyDialogueLogs = angular.copy($scope.$parent.companyDialogueLogsOriginal);
            $scope.$parent.sourceDialogueLogs = angular.copy($scope.$parent.sourceDialogueLogsOriginal);
        }

        $scope.companyDialogueTableParams.reload();
        $scope.sourceDialogueTableParams.reload();


        ModalService.closeModal($scope.$parent.addEditDialogueDialogId,null);

    }

    $scope.saveDialogue = function (dialogue, isAddNew) {
        if (dialogue.DialogueType === "Company") {
            if (isAddNew) {
                CaseProfileService.SaveCompanyDialogue(dialogue, saveAndAddNewDialogueCallback);
            }
            else {
                CaseProfileService.SaveCompanyDialogue(dialogue, saveDialogueCallback);
            }
        }
        else if (dialogue.DialogueType === "Source") {
            if (isAddNew) {
                CaseProfileService.SaveSourceDialogue(dialogue, saveAndAddNewDialogueCallback);
            }
            else {
                CaseProfileService.SaveSourceDialogue(dialogue, saveDialogueCallback);
            }
        }

        ModalService.closeModal($scope.addEditDialogueDialogId, null);
        
    }


    $scope.deleteDialogue = function (dialogue, frommodal) {
        if (dialogue.DialogueType === "Company") {
            $scope.deleteCompanyDialogue(dialogue, frommodal);
        }
        else if (dialogue.DialogueType === "Source") {
            $scope.deleteSourceDialogue(dialogue, frommodal);
        }
    };

    function saveDialogueCallback(Id) {
        if ($scope.ngDialogData.editingDialogue.I_GesCompanySourceDialogues_Id === 0) {
            $scope.ngDialogData.editingDialogue.I_GesCompanySourceDialogues_Id = Id;
        }
        $scope.ngDialogData.companyDialogueLogsOriginal = angular.copy($scope.ngDialogData.companyDialogueLogs);
        $scope.ngDialogData.sourceDialogueLogsOriginal = angular.copy($scope.ngDialogData.sourceDialogueLogs);

        $scope.ngDialogData.companyDialogueTableParams.reload();
        $scope.ngDialogData.sourceDialogueTableParams.reload();
    }

    function saveAndAddNewDialogueCallback(Id, isAddNew) {
        if ($scope.ngDialogData.editingDialogue.I_GesCompanySourceDialogues_Id === 0) {
            $scope.ngDialogData.editingDialogue.I_GesCompanySourceDialogues_Id = Id;
        }

        $scope.ngDialogData.companyDialogueLogsOriginal = angular.copy($scope.ngDialogData.companyDialogueLogs);
        $scope.ngDialogData.sourceDialogueLogsOriginal = angular.copy($scope.ngDialogData.sourceDialogueLogs);

        $scope.ngDialogData.companyDialogueTableParams.reload();
        $scope.ngDialogData.sourceDialogueTableParams.reload();

        if ($scope.ngDialogData.editingDialogue.DialogueType === "Company") {
            $scope.ngDialogData.addCompanyDialogue();
        }
        else {
            $scope.ngDialogData.addSourceDialogue();
        }
    }

    $scope.contactSelect = function (dialogue) {
        var grid = $("#tblcontact");
        var rowKey = grid.jqGrid('getGridParam', "selrow");

        $scope.$parent.ngDialogData.editingDialogue.FirstName = grid.jqGrid('getCell', rowKey, 'FirstName');
        $scope.$parent.ngDialogData.editingDialogue.LastName = grid.jqGrid('getCell', rowKey, 'LastName');
        $scope.$parent.ngDialogData.editingDialogue.ContactFullName = grid.jqGrid('getCell', rowKey, 'FirstName') +
            " " +
            grid.jqGrid('getCell', rowKey, 'LastName');

        $scope.$parent.ngDialogData.editingDialogue.G_Individuals_Id = grid.jqGrid('getCell', rowKey, 'UserId');
        ModalService.closeModal($scope.$parent.contactSelectDialogId, null);
    };

    $scope.organizationSelect = function (contact) {        
        var grid = $("#tblOrganization");
        var rowKey = grid.jqGrid('getGridParam', "selrow");

        $scope.$parent.ngDialogData.editingGesContact.OrganizationName = grid.jqGrid('getCell', rowKey, 'Name');
        $scope.$parent.ngDialogData.editingGesContact.OrganizationId = grid.jqGrid('getCell', rowKey, 'Id');

        
        ModalService.closeModal($scope.$parent.organizationSelectDialogId, null);
    }

    $scope.cancelOrganizationSelect = function () {
        ModalService.closeModal($scope.$parent.organizationSelectDialogId, null);
    }

    // add Contact
    $scope.addGesContact = function () {
        var contact = {
            UserId: 0,
            FirstName: "",
            LastName: "",
            Email: "",
            JobTitle: "",
            OrganizationId: 0,
            OrganizationName: "",
            Phone: "",
            Comment: ""
        };

        $scope.editingGesContact = contact;

        $scope.addEditContactOrganizationDialogId = ModalService.openModal($scope.templateContact, $scope, "ng-dialog-medium", 'CaseProfileDialogueController');
    }

    $scope.saveGesContact = function (contact, isAddNew) {
        if (isAddNew) {
            GesContactService.SaveGesContact(contact, saveAndAddNewGesContactCallback);
        }
        else {
            GesContactService.SaveGesContact(contact, saveGesContactCallback);
        }
    }

    function saveGesContactCallback() {
        reloadGesContactGrid();
    }

    function reloadGesContactGrid() {
        filterAndReloadGrid();
        $("#tblOrganization").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
        ModalService.closeModal($scope.$parent.addEditContactOrganizationDialogId, null);        
    }

    function filterAndReloadGrid() {

        var fields = "";
        var firstName = $scope.ngDialogData.editingGesContact.FirstName;
        var lastName = $scope.ngDialogData.editingGesContact.LastName;

        if (firstName != "") fields += (fields.length == 0 ? "" : ",") + createField("FirstName", "cn", firstName);
        if (lastName != "") fields += (fields.length == 0 ? "" : ",") + createField("LastName", "cn", lastName);


        var filters = '{\"groupOp\":\"AND\",\"rules\":[' + fields + ']}';

        $("#tblcontact").jqGrid('setGridParam', { search: true, postData: { "filters": (fields.length == 0) ? "" : filters } }).trigger("reloadGrid");

        $("#gs_FirstName").val(firstName);
        $("#gs_LastName").val(lastName);
    }

    function createField(name, op, data) {
        var field = '{\"field\":\"' + name + '\",\"op\":\"' + op + '\",\"data\":\"' + data + '\"}';
        return field;
    } 

    function saveAndAddNewGesContactCallback() {
        reloadGesContactGrid();
        $scope.ngDialogData.addGesContact();
    }

    $scope.cancelGesContact = function () {        
        ModalService.closeModal($scope.$parent.addEditContactOrganizationDialogId,null);        
    }

    $scope.cancelContactSelect = function () {
        ModalService.closeModal($scope.$parent.contactSelectDialogId,null);
    }

    $scope.openContactList = function (d) {
        $scope.contactSelectDialogId = ModalService.openModal($scope.templateContactSelect, $scope, "ng-dialog-large", 'CaseProfileDialogueController');
        selectRow(d);
    };

    $scope.openDocumentManagement = function (dialog, companysourceDialogId, dialogType) {
        $scope.editingDialogue = dialog;
        $scope.GetDocumentMngByCompanySourceDialogId(companysourceDialogId, dialogType);
    };

    $scope.cancelDocumentManagement = function () {
        ModalService.closeModal($scope.$parent.documentManagementDialogId, null);
    }

    $scope.GetDocumentMngByCompanySourceDialogId = function (companysourceDialogId, dialogType) {
        CaseProfileService.GetManagedDocumentByCompanySourceDialogId(companysourceDialogId, dialogType).then(
            function (response) {
                $scope.editingDocument = response.data;
                if ($scope.editingDocument.Created != null) {
                    $scope.editingDocument.Created = new Date(convertDate($scope.editingDocument.Created, 'yyyy/MM/dd'));
                }

                $scope.documentManagementDialogId = ModalService.openModal($scope.DocumentManagementTemplate, $scope, "ng-dialog-medium", 'CaseProfileDialogueController');
            },
            function () {
                quickNotification("Error occurred during loading attached file", "error");
            }
        );
    };

    $scope.saveManagedDocument = function (document) {
        CaseProfileService.SaveCompanyDialogAttachmentFile(document, $scope.file, $scope.$parent.$parent.dialogType, saveDocumentCallback);
    };

    $scope.deleteDocument = function (documentId) {
        ModalService.openConfirm("Are you sure to delete this document?", function (result) {
            if (result) {
                CaseProfileService.DeleteManagedDocument(documentId, deleteDocumentCallback);
            }
        });
    };  

    $scope.changeFile = function (file) {
        if (file) {
            $scope.$parent.editingDocument.DownloadUrl = "";
            $scope.$parent.editingDocument.FileName = file.name;

            if (typeof $scope.ngDialogData.$parent.editingDialogue !== "undefined") {
                $scope.ngDialogData.$parent.editingDialogue.FileName = file.name;
            }
            else if (typeof $scope.ngDialogData.editingDialogue !== "undefined"){
                $scope.ngDialogData.editingDialogue.FileName = file.name; 
            }
        }
    };

    $scope.uploadFile = function (files) {
        var allowedExtensions = /(\.pdf)$/i;
        if (fileValidation(files, allowedExtensions, "Please upload file having extensions .pdf only")) {
            $scope.file = files;
        } else {
            $scope.file = null;
        }
    };

    function fileValidation(file, allowedExtensions, alertMessage) {
        var filePath = file[0].name;

        if (!allowedExtensions.exec(filePath)) {
            alert(alertMessage);
            return false;
        }

        return true;
    }

    function saveDocumentCallback(Id) {
        if (typeof $scope.ngDialogData.$parent.editingDialogue !== "undefined") {
            $scope.ngDialogData.$parent.editingDialogue.G_ManagedDocuments_Id = Id;
            $scope.ngDialogData.$parent.companyDialogueTableParams.reload();
            $scope.ngDialogData.$parent.sourceDialogueTableParams.reload();
        } else {
            $scope.ngDialogData.editingDialogue.G_ManagedDocuments_Id = Id;
            $scope.ngDialogData.companyDialogueTableParams.reload();
            $scope.ngDialogData.sourceDialogueTableParams.reload();
        }
        
        ModalService.closeModal($scope.$parent.documentManagementDialogId, null);
    }

    function deleteDocumentCallback() {
        $scope.$parent.file = null;
        $scope.$parent.editingDocument = null;

        if (typeof $scope.ngDialogData.$parent.editingDialogue !== "undefined") {
            $scope.ngDialogData.$parent.editingDialogue.G_ManagedDocuments_Id = null;
        } else {
            $scope.ngDialogData.editingDialogue.G_ManagedDocuments_Id = null;
        }
        ModalService.closeModal($scope.$parent.documentManagementDialogId, null);
    }

    $scope.openOrganizationList = function (d) {
        $scope.organizationSelectDialogId = ModalService.openModal($scope.templateOrganizationSelect, $scope, "ng-dialog-large", 'CaseProfileDialogueController');
        selectRow(d);
    };


    //Ges organization
    $scope.addGesOrganization = function () {
        var organization = {
            Id: 0,
            Name: "",
            Address: "",
            PostalCode: "",
            City: "",
            CountryId: 0,
            CountryName: "",
            Phone: "",
            Website: "",
            Comment: ""
        };

        $scope.editGesOrganization = organization;
        
        $scope.addEditContactOrganizationDialogId = ModalService.openModal($scope.templateOrganization, $scope, "ng-dialog-medium", 'CaseProfileDialogueController');
        
    }

    $scope.saveGesOrganization = function (organization, isAddNew) {
        if (isAddNew) {
            GesOrganizationService.SaveGesOrganization(organization, saveAndAddNewGesOrganizationCallback);
        }
        else {
            GesOrganizationService.SaveGesOrganization(organization, saveGesOrganizationCallback);
        }

    }

    function saveGesOrganizationCallback() {
        //reloadGesContactGrid();
        ModalService.closeModal($scope.$parent.addEditContactOrganizationDialogId, null);
    }

    function saveAndAddNewGesOrganizationCallback() {
        //reloadGesContactGrid();
        ModalService.closeModal($scope.$parent.addEditContactOrganizationDialogId, null);        
        $scope.ngDialogData.addGesOrganization();
    }

    $scope.cancelGesOrganization = function () {        
        ModalService.closeModal($scope.$parent.addEditContactOrganizationDialogId, null);        
    }

    function init() {
        $scope.companyDialogueLogs = CaseProfileService.caseProfile.CompanyDialogueLogs;
        $scope.sourceDialogueLogs = CaseProfileService.caseProfile.SourceDialogueLogs;
        

        if ($scope.companyDialogueLogs != null && $scope.companyDialogueLogs.length > 0) {
            for (var i = 0; i < $scope.companyDialogueLogs.length; i++) {

                if (($scope.companyDialogueLogs[i].ContactDate != null)) {
                    $scope.companyDialogueLogs[i].ContactDate = new Date(convertDate($scope.companyDialogueLogs[i].ContactDate, 'yyyy-MM-dd'));
                }
            }
        }

        if ($scope.sourceDialogueLogs != null && $scope.sourceDialogueLogs.length > 0) {
            for (var i = 0; i < $scope.sourceDialogueLogs.length; i++) {

                if (($scope.sourceDialogueLogs[i].ContactDate != null)) {
                    $scope.sourceDialogueLogs[i].ContactDate = new Date(convertDate($scope.sourceDialogueLogs[i].ContactDate, 'yyyy-MM-dd'));
                }
            }
        }

        if ($scope.companyDialogueLogs != null) {
            $scope.companyDialogueTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5          // count per page    
            }, {
                    total: $scope.companyDialogueLogs.length, // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        params.total($scope.companyDialogueLogs.length);
                        $scope.data = $scope.companyDialogueLogs.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        return $scope.data;
                    }
                });
        }
        if ($scope.sourceDialogueLogs != null) {
            $scope.sourceDialogueTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5          // count per page    
            }, {
                    total: $scope.sourceDialogueLogs.length, // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        params.total($scope.sourceDialogueLogs.length);
                        $scope.data = $scope.sourceDialogueLogs.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        return $scope.data;
                    }
                });
        }

        $scope.companyDialogueLogsOriginal = angular.copy($scope.companyDialogueLogs);
        $scope.sourceDialogueLogsOriginal = angular.copy($scope.sourceDialogueLogs);
    }


    CaseProfileService.GetContactDirections().then(
        function (d) {
            $scope.contactDirections = d.data;
        },
        function () {
            alert("Failed");
        }
    );

    CaseProfileService.GetContactTypes().then(
        function (d) {
            $scope.contactTypes = d.data;
        },
        function () {
            alert("Failed");
        }
    );

    $scope.getContactTypeName = function (contactTypeId) {
        switch (contactTypeId) {
            case 1:
                return "Email"
                break;
            case 2:
                return "Postal Mail"
                break;
            case 3:
                return "Telephone"
                break;
            case 4:
                return "Meeting"
                break;
            case 5:
                return "Fax"
                break;
            case 6:
                return "Conference Call"
                break;
            case 7:
                return "Archived Dialogue"
                break;
            default:
                return "Select contact type";
        }
    }

    $scope.getContactDirectionName = function (contactDirectionId) {
        switch (contactDirectionId) {
            case 1:
                return "Incoming"
                break;
            case 2:
                return "Outgoing"
                break;
            case 4:
                return "Meeting"
                break;
            default:
                return "Select contact direction";
        }
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

    function selectRow(id) {
        var $grid = $("#tblcontact");
        $grid.jqGrid("setSelection", id);
    }
}]);