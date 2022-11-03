"use strict";
GesInsideApp.controller("GesContactController", ["$scope", "$filter", "$timeout", "$q", "ClientService", "GesContactService", "GesOrganizationService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout,$q, clientService, GesContactService, GesOrganizationService, ModalService, NgTableParams) {
    $scope.editingGesContact = null;
    $scope.editingOrganization = null;
    $scope.templateContact = '/Content/angular/templates/config/GesContactDialog.html';
    $scope.templateOrganization = '/Content/angular/templates/config/GesOrganizationDialog.html';
    $scope.templateOrganizationSelect = '/Content/angular/templates/caseprofile/OrganizationListDialog.html';
    $scope.templateDialogue = '/Content/angular/templates/config/DialogueListDialog.html';

    $scope.addEditContactDialogId = "";
    $scope.addEditOrganizationDialogId = "";

    $scope.contactSelectDialogId = "";
    $scope.organizationSelectDialogId = "";

    $scope.dialogueSelectDialogId = "";

    $scope.countries = [];
    $scope.dialogues = [];
    $scope.currentType = "";
    GesContactService.GetAllCountries().then(
        function (d) {
            $scope.countries = d.data;
        },
        function () {
            quickNotification("Error occurred during loading countries data", "error");
        }
    )

    init();

    //Ges contact
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
        $scope.addEditContactDialogId = ModalService.openModal($scope.templateContact, $scope, "ng-dialog-medium", 'GesContactController');

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
        ModalService.closeModal($scope.$parent.addEditContactDialogId, null);
    }

    function saveAndAddNewGesContactCallback() {
        reloadGesContactGrid();
        ModalService.closeModal($scope.$parent.addEditContactDialogId, null);
        $scope.ngDialogData.addGesContact();
    }

    $scope.cancelGesContact = function () {
        ModalService.closeModal($scope.addEditContactDialogId, null);
    }

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

        $scope.addEditOrganizationDialogId = ModalService.openModal($scope.templateOrganization, $scope, "ng-dialog-medium", 'GesContactController');

    }

    $scope.openOrganizationList = function (d) {
        $scope.organizationSelectDialogId = ModalService.openModal($scope.templateOrganizationSelect, $scope, "ng-dialog-large", 'GesContactController');
        selectRow(d);
    };

    function selectRow(id) {
        var $grid = $("#tblOrganization");
        $grid.jqGrid("setSelection", id);

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
        reloadGesContactGrid();
        ModalService.closeModal($scope.$parent.addEditOrganizationDialogId, null);
    }

    function saveAndAddNewGesOrganizationCallback() {
        reloadGesContactGrid();
        ModalService.closeModal($scope.$parent.addEditOrganizationDialogId, null);
        $scope.ngDialogData.addGesOrganization();
    }

    $scope.cancelGesOrganization = function () {
        ModalService.closeModal($scope.$parent.addEditOrganizationDialogId, null);
    }

    $scope.cancelDialogueDialog = function () {
        ModalService.closeModal($scope.$parent.dialogueSelectDialogId, null);
    }

    $scope.organizationSelect = function (contact) {
        var grid = $("#tblOrganization");
        var rowKey = grid.jqGrid('getGridParam', "selrow");

        $scope.$parent.ngDialogData.editingGesContact.OrganizationName = grid.jqGrid('getCell', rowKey, 'Name');

        $scope.$parent.ngDialogData.editingGesContact.OrganizationId = grid.jqGrid('getCell', rowKey, 'Id');

        ModalService.closeModal($scope.$parent.organizationSelectDialogId, null);
    };

    $scope.cancelOrganizationSelect = function () {
        ModalService.closeModal($scope.$parent.organizationSelectDialogId, null);
    }

    function init() {
        //loadAnnouncements();
    }

    function convertDate(value) {
        if (value !== null && !value.isNullOrEmpty) {
            return moment.utc(value).toDate();
        }

        return null;
    };

    function reloadGesContactGrid() {
        $("#tblcontact").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
        $("#tblOrganization").setGridParam({ rowNum: 50, datatype: "json" }).trigger('reloadGrid');
    }

    $scope.config = {
        id: 'tblcontact',

        // And this is where I'd like ng-click to bind to.
        editGesContact: function (userId, organizationId, firstName, lastName, JobTitle, phone, email, comment, organizationName, customer) {
            if (!customer) {
                var contact = {
                    UserId: userId,
                    FirstName: ((firstName != 'null') ? firstName : ''),
                    LastName: ((lastName != 'null') ? lastName : ''),
                    Email: ((email != 'null') ? email : ''),
                    JobTitle: ((JobTitle != 'null') ? JobTitle : ''),
                    OrganizationId: organizationId,
                    OrganizationName: ((organizationName != 'null') ? organizationName : ''),
                    Phone: ((phone != 'null') ? phone : ''),
                    Comment: ((comment != 'null') ? comment : '')
                };

                $scope.editingGesContact = contact;
                $scope.addEditContactDialogId = ModalService.openModal($scope.templateContact, $scope, "ng-dialog-medium", 'GesContactController');
            }
        },

        editOrganization: function (id, name, address, postalCode, city, countryId, phone, website, comment, customer) {
            if (!customer) {
                var organization = {
                    Id: id,
                    Name: ((name != 'null') ? name : ''),
                    Address: ((address != 'null') ? address : ''),
                    PostalCode: ((postalCode != 'null') ? postalCode : ''),
                    City: ((city != 'null') ? city : ''),
                    CountryId: countryId,
                    Phone: ((phone != 'null') ? phone : ''),
                    Website: ((website != 'null') ? website : ''),
                    Comment: ((comment != 'null') ? comment : '')
                };

                $scope.editGesOrganization = organization;
                $scope.addEditOrganizationDialogId = ModalService.openModal($scope.templateOrganization, $scope, "ng-dialog-medium", 'GesContactController');
            }
        },
        showDialogues: function (id, type) {
            GesContactService.GetDialogueByIndividual(id, type).then(
                function (d) {
                    $scope.dialogues = d.data;
                    if ($scope.dialogues != null && $scope.dialogues.length > 0) {
                        for (var i = 0; i < $scope.dialogues.length; i++) {

                            if (($scope.dialogues[i].ContactDate != null)) {
                                $scope.dialogues[i].ContactDate =
                                    new Date(convertDate($scope.dialogues[i].ContactDate, 'yyyy/MM/dd'));
                            }
                        }
                    }
                    if ($scope.dialogues != null) {
                        $scope.dialogueTableParams = new NgTableParams({
                            page: 1,            // show first page
                            count: 10          // count per page    
                        }, {
                                total: $scope.dialogues.length, // length of data
                                counts: [5, 25, 50, 100],
                                getData: function (params) {
                                    // use build-in angular filter
                                    var sortedData = params.sorting() ?
                                        $filter('orderBy')($scope.dialogues, params.orderBy()) :
                                        $scope.dialogues;
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
                    $scope.currentType = type;
                    $scope.dialogueSelectDialogId = ModalService.openModal($scope.templateDialogue, $scope, "ng-dialog-large", 'GesContactController');
                },
                function () {
                    quickNotification("Error occurred during loading dialogues data", "error");
                }
            )
        },

        datatype: "json",
        url: "/CaseProfile/GetContactsJqGrid",
        postData: {},
        mtype: "post",
        colModel: [
            { name: "UserId", width: "50px", align: "right", hidden: false, search: false, key: true, label: 'Individual Id' },
            { name: "FirstName", width: "100px", label: 'First Name', search: true },
            { name: "LastName", width: "100px", label: 'Last Name', search: true },
            { name: "JobTitle", label: 'Job Title', search: true },
            {
                name: "Email",
                label: "Email",
                search: true,
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    if (!rowObject.Organization_Customer) {
                        return cellPrefix + "<div class=\"click-link\" ng-click=\"config.editGesContact('" + rowObject.UserId + "','" + rowObject.OrganizationId + "','" + rowObject.FirstName + "','" + rowObject.LastName + "','" + rowObject.JobTitle + "','" + rowObject.Phone + "','" + rowObject.Email + "','" + rowObject.Comment + "','" + rowObject.OrganizationName + "'," + rowObject.Organization_Customer + ")\">" + rowObject.Email + "</div>";
                    }
                    else {
                        return cellPrefix + "<div>" + rowObject.Email + "</div>";
                    }
                }
            },
            {
                name: "OrganizationName", searchoptions: {
                    search: true
                },
                label: "Organization",
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    if (!rowObject.Organization_Customer) {
                        return cellPrefix + "<div class=\"click-link\" ng-click=\"config.editOrganization('" + rowObject.OrganizationId + "','" + rowObject.OrganizationName + "','" + rowObject.Organization_Address + "','" + rowObject.Organization_PostalCode + "','" + rowObject.Organization_City + "'," + rowObject.Organization_G_Countries_Id + ",'" + rowObject.Organization_Phone + "','" + rowObject.Organization_WebsiteUrl + "','" + rowObject.Organization_Comment + "'," + rowObject.Organization_Customer + ")\">" + rowObject.OrganizationName + "</div>";
                    }
                    else {
                        return cellPrefix + "<div>" + rowObject.OrganizationName + "</div>";
                    }

                }
            },
            {
                name: "NumberCompanyDialogue", searchoptions: {
                    search: true
                },
                label: "Company Dialogues",
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    return cellPrefix + "<div class=\"click-link\" ng-click=\"config.showDialogues('" + rowObject.UserId + "','Company')\">" + rowObject.NumberCompanyDialogue + " company dialogues</div>";
                }
            },
            {
                name: "NumberSourceDialogue", searchoptions: {
                    search: true
                },
                label: "Source Dialogues",
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    return cellPrefix + "<div class=\"click-link\" ng-click=\"config.showDialogues('" + rowObject.UserId + "','Source')\">" + rowObject.NumberSourceDialogue + " source dialogues</div>";
                }
            }
        ],
        pager: "myPager",
        rowNum: 50,
        rowList: [20, 50, 100],
        autowidth: true,
        shrinkToFit: true,
        toppager: true,
        gridview: true,
        height: "auto",
        viewrecords: true,
        caption: "",
        scrollrows: true,
        sortname: "Id",
        sortorder: "desc"
    };
}]);