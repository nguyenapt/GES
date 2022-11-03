"use strict";
GesInsideApp.controller("EngagementTypeController", ["$scope", "$filter", "$timeout", "$window", "EngagementTypeService", "NgTableParams",
    function($scope, $filter, $timeout, $window, engagementTypeService, NgTableParams) {
        $scope.isFormValid = false;
        $scope.message = null;
        $scope.engagementTypeDetails = null;
        $scope.engagementTypeCategories = null;
        $scope.engagementTypeContacts = null;
        $scope.documentTypes = null;
        $scope.SecurityTypes = null; 
        $scope.IsShowIndustryGICSLevel3 = false;
        $scope.isSaving = false;
        $scope.IsinLink = null;
        $scope.isAddNew = false;
        $scope.allowDelete = false;
        $scope.files = [];
        $scope.newsList = [];
        $scope.timelines = [];
        $scope.kpis = [];
        $scope.selectedContact = 0;
        $scope.themeBanerImage = "";
        $scope.themeBanerImagePath = "";
        $scope.hasThemeBanerImage = false;
        $scope.activeOrDeactiveButtonLabel = "Close Theme";
        var self = this;     
        
        init();

        function init() {
            initSelect2();
            $scope.timelines =[];

            $scope.timelinesTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5          // count per page    
            }, {
                total: $scope.timelines.length, // length of data
                counts: [5, 25, 50, 100],
                getData: function(params) {
                    params.total($scope.timelines.length);
                    $scope.data = $scope.timelines.slice((params.page() - 1) * params.count(), params.page() * params.count());
                    return $scope.data;
                }
            });

            $scope.kpis =[];
            $scope.kpisTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 2          // count per page    
            }, {
                total: $scope.kpis.length, // length of data
                counts: [2, 4, 6, 8, 10, 12, 20],
                getData: function(params) {
                    params.total($scope.kpis.length);
                    $scope.data = $scope.kpis.slice((params.page() - 1) * params.count(), params.page() * params.count());
                    return $scope.data;
                }
            });

            $scope.newsList=[];
            $scope.newsTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5          // count per page    
            }, {
                total: $scope.newsList.length, // length of data
                counts: [5, 10, 12, 20],
                getData: function(params) {
                    params.total($scope.newsList.length);
                    $scope.data = $scope.newsList.slice((params.page() - 1) * params.count(), params.page() * params.count());
                    return $scope.data;
                }
            });

            $scope.files = [];
            $scope.fileTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5          // count per page    
            }, {
                total: $scope.files.length, // length of data
                counts: [5, 10, 12, 20],
                getData: function(params) {
                    params.total($scope.files.length);
                    $scope.data = $scope.files.slice((params.page() - 1) * params.count(), params.page() * params.count());
                    return $scope.data;
                }
            });
            
  
            $('.button-bottom').fadeOut();
            $(document).scroll(function() {
                var y = $(this).scrollTop();
                if (y > 600) {
                    $('.button-bottom').fadeIn();
                } else {
                    $('.button-bottom').fadeOut();
                }
            });

            var urlPath = $window.location.href;
            var urlPathSplit = String(urlPath).split("/");
            var engagementTypeId = 0;

            if (urlPathSplit !== null && urlPathSplit.length > 0) {
                if (urlPathSplit[urlPathSplit.length - 1] !== 'Add') {
                    engagementTypeId = urlPathSplit[urlPathSplit.length - 1];
                } else{
                    $scope.isAddNew = true;
                }
            }

            //Get engagement type deatils
            if (engagementTypeId !== 0) {

                engagementTypeService.GetEngagementTypeDetailsById(engagementTypeId).then(
                   function (d) {
                       $scope.engagementTypeDetails = d.data;
                       if (($scope.engagementTypeDetails.Created != null)) {
                           $scope.engagementTypeDetails.Created = $scope.convertDate($scope.engagementTypeDetails.Created, 'yyyy/MM/dd HH:mm:ss a');
                       }                       
                       $scope.selectedContact = $scope.engagementTypeDetails.ContactG_Users_Id;

                       var i;

                       $scope.files = $scope.engagementTypeDetails.EngagementTypeDocuments;
                       if ($scope.files.length > 0) {
                           for (i = 0; i < $scope.files.length; i++) {
                               if ($scope.files[i].Created != null) {
                                   $scope.files[i].Created =
                                       $scope.convertDate($scope.files[i].Created, 'yyyy/MM/dd HH:mm:ss a');
                               }
                           }
                       }

                       $scope.fileTableParams.reload();

                       $scope.timelines = $scope.engagementTypeDetails.TimeLine;
                       if ($scope.timelines.length > 0) {
                           for (i = 0; i < $scope.timelines.length; i++) {

                               if (($scope.timelines[i].EventDate != null)) {
                                   $scope.timelines[i].EventDate =
                                       new Date($scope.convertDate($scope.timelines[i].EventDate, 'yyyy/MM/dd'));
                               }

                               if (($scope.timelines[i].Created != null)) {
                                   $scope.timelines[i].Created =
                                       $scope.convertDate($scope.timelines[i].Created, 'yyyy/MM/dd HH:mm:ss a');
                               }
                           }
                       }
                       
                       $scope.timelinesTableParams.reload();                       

                       $scope.kpis = $scope.engagementTypeDetails.KPIs;
                       if ($scope.kpis.length > 0) {
                           for (i = 0; i < $scope.kpis.length; i++) {
                               if (($scope.kpis[i].Created != null)) {
                                   $scope.kpis[i].Created =
                                       $scope.convertDate($scope.kpis[i].Created, 'yyyy/MM/dd HH:mm:ss a');
                               }                              

                           }
                       }

                       $scope.kpisTableParams.reload();                     
                       
                       
                       $scope.newsList = $scope.engagementTypeDetails.EngagementTypeNews;
                       if ($scope.newsList.length > 0) {
                           for (i = 0; i < $scope.newsList.length; i++) {
                               $scope.newsList[i].Created =
                                       new Date($scope.convertDate($scope.newsList[i].Created, 'yyyy/MM/dd'));
                           }
                       }

                       $scope.newsTableParams.reload();                       
                       

                       $scope.themeBanerImage = $scope.engagementTypeDetails.ThemeImage;
                       
                       if ($scope.themeBanerImage != null) {
                           $scope.hasThemeBanerImage = true;                           
                       }

                       $scope.themeBanerImagePath = $scope.engagementTypeDetails.ThemeImagePath;
                       
                       if($scope.engagementTypeDetails.Deactive !== null && $scope.engagementTypeDetails.Deactive){
                           $scope.activeOrDeactiveButtonLabel = "Open Theme"
                       };

                       if (!$scope.isAddNew) {
                           $timeout(function () {
                               $("#engagementtypecategory-select")
                                   .val($scope.engagementTypeDetails.I_EngagementTypeCategories_Id).trigger("change");
                               $("#engagementTypeContact-select").val($scope.engagementTypeDetails.ContactG_Users_Id)
                                   .trigger("change");
                               $('.has-clear input[type="text"]').on('input propertychange',
                                   function () {
                                       var $this = $(this);
                                       var visible = Boolean($this.val());
                                       $this.siblings('.form-control-clear').toggleClass('hidden', !visible);
                                   }).trigger('propertychange');
                           },
                               2500);
                       }

                       
                       initClearContent();
                       initDeleteConfirmationBox();
                       initActiveOrDeactiveConfirmationBox();

                       $scope.allowDelete = true;

                   },
                   function () {
                       alert("Failed");
                   }
               );
            }

            //Get all Engagement Type Categories
            engagementTypeService.GetAllEngagementTypeCategories().then(
                function(d) {
                    $scope.engagementTypeCategories = d.data;
                },
                function() {
                    alert("Failed");
                }
            ); 
            
            engagementTypeService.GetDocumentTypes().then(
                function(d) {
                    $scope.documentTypes = d.data;
                },
                function() {
                    alert("Failed");
                }
            );

            initCancelSaveConfirmationBox();
        }

        //* News methods
        $scope.addNews = function () {
            var temp= $scope.newsList;
            $scope.newsList =[];

            var news = {
                EngagementTypeNewsId: keyId(), EngagementTypeNewsDescription: "", Created: "", disableEdit: true
            };
            $scope.newsList.push(news);

            for (var i = 0; i < temp.length; i++) {
                $scope.newsList.push(temp[i]);
            }

            $scope.newsTableParams.reload();
        };

        $scope.deleteNews = function (news) {
            if (news) {

                for (var i in $scope.newsList) {
                    if ($scope.newsList[i].EngagementTypeNewsId === news.EngagementTypeNewsId) {
                        $scope.newsList.splice(i, 1);
                    }
                }

                if ($scope.newsTableParams.data.length === 1 && $scope.newsTableParams.page() !== 1) {
                    $scope.newsTableParams.page($scope.newsTableParams.page() - 1);
                }
            }

            $scope.newsTableParams.reload();
            
        };
        //End news

        //* timeline methods
        $scope.addTimeline = function() {
            var temp= $scope.timelines;
            $scope.timelines =[];          

            var news = {
                CompanyId: "",
                Created: "",
                Description: "",
                EngagementTypeId: "",
                EventDate: "",
                Heading: "",
                Id: keyId(),
                IsGesEvent: false,
                Location: "",
                disableEdit: true
            };
            $scope.timelines.push(news);

            for (var i = 0; i < temp.length; i++) {
                $scope.timelines.push(temp[i]);
            }           
            
            $scope.timelinesTableParams.reload();            
         
        };

        $scope.deleteTimeline = function (timeline) {
            if (timeline) {

                for (var i in $scope.timelines) {
                    if ($scope.timelines[i].Id === timeline.Id) {
                        $scope.timelines.splice(i, 1);
                    }
                }

                if ($scope.timelinesTableParams.data.length === 1 && $scope.timelinesTableParams.page() !== 1) {
                    $scope.timelinesTableParams.page($scope.timelinesTableParams.page() - 1);
                }
            }            
    
            $scope.timelinesTableParams.reload();
            
        };
        //End timeline

        //* Kpi methods
        $scope.addKpi = function () {
            var temp= $scope.kpis;
            $scope.kpis = [];
            
            var kpi = {
                KpiId: keyId(), KpiName: "", KpiDescription: "", KpiPerformance: "", Created: "", disableEdit: true
            };
            $scope.kpis.push(kpi);
            
            for (var i = 0; i < temp.length; i++) {
                $scope.kpis.push(temp[i]);
            }
            $scope.kpisTableParams.reload();
        };

        $scope.deleteKpi = function (kpi) {

            if (kpi) {

                for (var i in $scope.kpis) {
                    if ($scope.kpis[i].KpiId === kpi.KpiId) {
                        $scope.kpis.splice(i, 1);
                    }
                }

                if ($scope.kpisTableParams.data.length === 1 && $scope.kpisTableParams.page() !== 1) {
                    $scope.kpisTableParams.page($scope.kpisTableParams.page() - 1);
                }
            }

            $scope.kpisTableParams.reload();
            
        };
        //End Kpi

        function initSelect2() {
            //Have a bug with ui-select2 directive in form
            //So, use manually initialize instead of directive
            $("#engagementtypecategory-select").select2();
            $("#engagementTypeContact-select").select2();

        }        

        $scope.convertDate = function (value, format) {
            if (value != null && !value.isNullOrEmpty) {
                return $filter("date")(new Date(parseInt(value.substr(6))), format);
            }

            return null;
        };

        $scope.UpdateEngagementTypeData = function() {
            $scope.isSaving = true;

            $scope.engagementTypeDetails.EngagementTypeDocuments = $scope.files;
            $scope.engagementTypeDetails.KPIs = $scope.kpis;
            $scope.engagementTypeDetails.TimeLine = $scope.timelines;
            $scope.engagementTypeDetails.EngagementTypeNews = $scope.newsList;
            $scope.engagementTypeDetails.ContactG_Users_Id = $scope.selectedContact;
            $scope.engagementTypeDetails.ThemeImagePath = $scope.themeBanerImagePath;            
            var a = $scope.data;

            engagementTypeService.UpdateEngagementTypeData($scope.engagementTypeDetails).then(function() {
                goToEngagementTypeList();
            });
        };

        $scope.uploadFile = function (files) {

            var allowedExtensions = /(\.xls|\.xlsx|\.pdf|\.doc|\.docx|\.ppt|\.pptx)$/i;
            if (fileValidation(files, allowedExtensions, "Please upload file having extensions .xls\\.xlsx\\.pdf\\.doc\\.docx\\.ppt\\.pptx only")) {
                engagementTypeService.UploadFile(files).then(
                    function (d) {
                        var newId = $scope.files.length + 1;
                        
                        var temp = $scope.files;
                        $scope.files =[];
                        $scope.files.push({
                            'Name': "",
                            'FileName': files[0].name
                        });

                        for (var i = 0; i < temp.length; i++) {
                            $scope.files.push(temp[i]);
                        }

                        $scope.fileTableParams.reload();
                    },
                    function () {
                        alert("Failed");
                    }
                );
            }
        };       

        $scope.uploadImageFile = function (filesImage) {
            var allowedExtensions = /(\.jpg|\.png)$/i;
            if (fileValidation(filesImage, allowedExtensions, "Please upload file having extensions .jpg\\.png only")) {

                if (filesImage && filesImage[0]) {                    

                    engagementTypeService.UploadFile(filesImage).then(
                        function (d) {
                            var reader = new FileReader();
                            reader.onload = function (e) {

                                //Sets the Old Image to new New Image
                                $('#photo-id').attr('src', e.target.result);

                                //Create a canvas and draw image on Client Side to get the byte[] equivalent
                                var canvas = document.createElement("canvas");
                                var imageElement = document.createElement("img");

                                imageElement.setAttribute('src', e.target.result);
                                canvas.width = imageElement.width;
                                canvas.height = imageElement.height;
                                var context = canvas.getContext("2d");
                                context.drawImage(imageElement, 0, 0);
                                var base64Image = canvas.toDataURL("image/jpeg");

                                //Removes the Data Type Prefix 
                                //And set the view model to the new value
                                $scope.themeBanerImage = base64Image.replace(/data:image\/jpeg;base64,/g, '');
                            };

                            //Renders Image on Page
                            reader.readAsDataURL(filesImage[0]);
                            
                            $scope.themeBanerImagePath = filesImage[0].name;
                            
                        },
                        function () {
                            alert("Failed");
                        }
                    );

                    $scope.hasThemeBanerImage = true;
                }

            }
        };

        $scope.contactSelect = function() {

            var grid = $("#tblcontact");
            var rowKey = grid.jqGrid('getGridParam', "selrow");
            if ($scope.engagementTypeDetails == null){
                $scope.engagementTypeDetails =  {};
            } 

            $scope.engagementTypeDetails.ContactFullName = grid.jqGrid('getCell', rowKey, 'FirstName') +
                " " +
                grid.jqGrid('getCell', rowKey, 'LastName');
            $scope.selectedContact = grid.jqGrid('getCell', rowKey, 'UserId');
        };

        $scope.setValuesForPopup = function(d) {
            $(window).resize();
            selectRow(d);
        };

        $scope.deleteFile = function (index) {
            $scope.files.splice(index, 1);

            if ($scope.fileTableParams.data.length === 1 && $scope.fileTableParams.page() !== 1) {
                $scope.fileTableParams.page($scope.fileTableParams.page() - 1);
            }

            $scope.fileTableParams.reload();
        };

        $scope.deleteImageFile = function (e) {
            $scope.engagementTypeDetails.ThemeImage = "";
            var fileInput = document.getElementById('imagefile');
            fileInput.value="";
            $scope.themeBanerImagePath = "";
            $scope.hasThemeBanerImage = false;
        };        

        
        function fileValidation(file, allowedExtensions, alertMessage) {
            var filePath = file[0].name;
            
            if (!allowedExtensions.exec(filePath)) {
                alert(alertMessage);
                return false;
            }

            return true;
        }

        function keyId(){
            var number = Math.random();
            number.toString(36);
            return number.toString(36).substr(2, 9);
        }

        function goToEngagementTypeList() {
            $window.location.href = "/EngagementType/List";
        }

        String.isNullOrEmpty = function(value) {
            return !(typeof value === "undefined" || (typeof value === "string" && value.length > 0));
        };

        function initCancelSaveConfirmationBox() {
            $("#cancel-save").confirmModal({
                confirmCallback: goToEngagementTypeList
            });   
            $("#cancel-save-bottom").confirmModal({
                confirmCallback: goToEngagementTypeList
            });
        }

        function initDeleteConfirmationBox() {
            $("#delete").confirmModal({
                confirmCallback: deleteEngagementTypeData
            });
            $("#delete-bottom").confirmModal({
                confirmCallback: deleteEngagementTypeData
            });
        }

        //Delete engagement Type
        function deleteEngagementTypeData() {

            $scope.engagementTypeDetails.EngagementTypeDocuments = $scope.files;
            $scope.engagementTypeDetails.KPIs = $scope.kpis;
            $scope.engagementTypeDetails.TimeLine = $scope.timelines;
            $scope.engagementTypeDetails.EngagementTypeNews = $scope.newsList;
            $scope.engagementTypeDetails.ContactG_Users_Id = $scope.selectedContact;

            engagementTypeService.DeleteEngagementTypeData($scope.engagementTypeDetails).then(function () {
                goToEngagementTypeList();
            });
        }

        function initActiveOrDeactiveConfirmationBox() {
            $("#deactive").confirmModal({
                confirmCallback: EngagementTypeActiveOrDeactiveAction
            });
            $("#deactive-bottom").confirmModal({
                confirmCallback: EngagementTypeActiveOrDeactiveAction
            });
        }

        //Delete engagement Type
        function EngagementTypeActiveOrDeactiveAction() {

            if( $scope.engagementTypeDetails.Deactive) {
                $scope.engagementTypeDetails.Deactive = false;
            } else if ($scope.engagementTypeDetails.Deactive === null || !$scope.engagementTypeDetails.Deactive){
                $scope.engagementTypeDetails.Deactive = true;
            }        

            engagementTypeService.ActiveOrDeactiveEngagementType($scope.engagementTypeDetails).then(function () {
                goToEngagementTypeList();
            });
        }


        function initClearContent() {
            $('.has-clear input[type="text"]').on('input propertychange',
                function() {
                    var $this = $(this);
                    var visible = Boolean($this.val());
                    $this.siblings('.form-control-clear').toggleClass('hidden', !visible);
                }).trigger('propertychange');

            $('.form-control-clear').click(function() {
                $(this).siblings('input[type="text"]').val('')
                    .trigger('propertychange').focus();
            });
        }
        
        $(function() {
            var postUrl = "/EngagementType/GetEngagementTypeContactsJqGrid";
            var gridCaption = "";

            var grid = $("#tblcontact");
            $.jgrid.defaults.responsive = true;
            grid.bind("jqGridLoadComplete",
                function(e, rowid, orgClickEvent) {

                    $(window).resize();
                });
            grid.jqGrid({
                url: postUrl,
                datatype: "json",
                postData: { },
                mtype: "post",
                colNames: ["UserId", "First Name", "Last Name", "Job Title", "Email"],
                colModel: [
                    { name: "UserId", width: "35px", align: "right", hidden: false, search: false, key: true },
                    { name: "FirstName" },
                    { name: "LastName" },
                    { name: "JobTitle" },
                    { name: "Email" }
                ],
                pager: $("#myPager"),
                rowNum: 50,
                rowList: [20, 50, 100],
                autowidth: true,
                shrinkToFit: true,

                toppager: true,
                // rownumbers: true,
                gridview: true,
                height: "400",
                viewrecords: true,
                caption: gridCaption,
                scrollrows: true,
                sortname: "Id",
                sortorder: "desc",
                ondblClickRow: function() {
                    $('#btn-contact-select').click();
                }
            });
            setBooleanSelect.call(grid, "LockoutEnabled", "All");
            grid.jqGrid("navGrid",
                "#myPager",
                { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });

            grid.jqGrid("filterToolbar",
                { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: false });

        });

        function selectRow(id) {
            var $grid = $("#tblcontact");
            $grid.jqGrid("setSelection", id);
        }


    }]);