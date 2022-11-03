"use strict";
GesInsideApp.controller("CompanyEventsController", ["$scope", "$filter","$q", "$timeout", "$window", "CompanyService", "ModalService", "NgTableParams", function ($scope, $filter,$q, $timeout, $window, CompanyService, ModalService, NgTableParams) {
    $scope.companyEventsTableParams = null;
    $scope.companyEvents = [];
    $scope.companyEventsOrigin = [];
    $scope.companyId = companyId;

    $scope.editingCompanyEvent = null;      
    
    $scope.sortType = 'Created'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.searchPortfolio = ''; 
    $scope.filter = "";
    $scope.currentStartTime;
    $scope.currentEndTime;
    $scope.Attendees = [];
    $scope.ModalName = "";
    $scope.formValid = false;
    
    $scope.errorMessages = [];

    $scope.$on('finishInitCompany', function (e) {
        init();
        tableInit();
    });

    function init() {

        var timepickerStartTime = $('#timepickerStartTime');
        timepickerStartTime.timepicker({
            showMeridian: false
        });
        timepickerStartTime.on('show.timepicker', function () {
            $('.ngdialog').removeAttr('tabindex');
        });

        var timepickerEndTime = $('#timepickerEndTime');
        timepickerEndTime.timepicker({
            showMeridian: false
        });
        timepickerEndTime.on('show.timepicker', function () {
            $('.ngdialog').removeAttr('tabindex');
        });


        GetCompanyEvents( $scope.companyId);
        
    }
    
    function tableInit() {
        $scope.companyEventsTableParams = new NgTableParams({
            page: 1,            // show first page
            count: 10          // count per page    
        }, {
            total: $scope.companyEvents != null?$scope.companyEvents.length:0, // length of data
            counts: [5, 25, 50, 100],
            getData: function (params) {
                params.total($scope.companyEvents != null ? $scope.companyEvents.length : 0);
                if ($scope.companyEvents != null) {
                    $scope.data = $scope.companyEvents.slice((params.page() - 1) * params.count(), params.page() * params.count());
                }

                return $scope.data;
            }
        });

    }

    var GetCompanyEvents = function (companyId) {

        $scope.companyEvents = [];
        CompanyService.GetCompanyEvents(companyId).then(
            function (response) {
                $scope.companyEvents  = response.data;

                if ($scope.companyEvents.length > 0) {
                    for (var i = 0; i < $scope.companyEvents.length; i++) {
                        if ($scope.companyEvents[i].EventDate != null) {
                            var startDate = new Date(
                                $scope.convertDate($scope.companyEvents[i].EventDate, 'yyyy/MM/dd'));
                            var monthValue = (startDate.getMonth() + 1);
                            var dayValue = startDate.getDate();
                            $scope.companyEvents[i].EventDate = startDate.getFullYear() + "-" + (monthValue < 10? 0 + monthValue.toString(): monthValue) + "-" + (dayValue < 10? 0 + dayValue.toString(): dayValue);

                        }
                        if ($scope.companyEvents[i].EventEndDate != null) {
                            var endDate = new Date(
                                $scope.convertDate($scope.companyEvents[i].EventEndDate, 'yyyy/MM/dd'));

                            var endEventmonthValue = (endDate.getMonth() + 1);
                            var endEventdayValue = endDate.getDate();
                            $scope.companyEvents[i].EventEndDate = endDate.getFullYear() + "-" + (endEventmonthValue < 10? 0 + endEventmonthValue.toString(): endEventmonthValue) + "-" + (endEventdayValue < 10? 0 + endEventdayValue.toString(): endEventdayValue);

                        }
                        var timeMeridiem = "AM";
                        if ($scope.companyEvents[i].StartTime != null ){
                            if ($scope.companyEvents[i].StartTime.Hours > 12 ) {
                               // $scope.companyEvents[i].StartTime.Hours = $scope.companyEvents[i].StartTime.Hours - 12;
                               // timeMeridiem = "PM";
                            }
                            $scope.companyEvents[i].StartTime = $scope.companyEvents[i].StartTime.Hours + ":" + ($scope.companyEvents[i].StartTime.Minutes === 0?"00": $scope.companyEvents[i].StartTime.Minutes);// + " " + timeMeridiem;
                        }

                        if ($scope.companyEvents[i].EndTime != null ){
                            if ($scope.companyEvents[i].EndTime.Hours > 12 ) {
                               // $scope.companyEvents[i].EndTime.Hours = $scope.companyEvents[i].EndTime.Hours - 12;
                               // timeMeridiem = "PM";
                            }
                            $scope.companyEvents[i].EndTime = $scope.companyEvents[i].EndTime.Hours + ":" + ($scope.companyEvents[i].EndTime.Minutes === 0?"00": $scope.companyEvents[i].EndTime.Minutes);// + " " + timeMeridiem;
                        }                       
                        
                    }

                }
                $scope.companyEventsOrigin = angular.copy($scope.companyEvents);
                
                $scope.companyEventsTableParams.reload();
             
            },
            function (reason) {
                quickNotification("Error occurred during loading company events, caused: " + reason, "error");
            }
        );

    };

    $scope.AddOrUpdateEvent = function (event) {
        var $startdatepicker = $('#eventdate');
        $startdatepicker.datepicker({
            format: 'yyyy-mm-dd'
        });        
        
        var $enddatepicker = $('#eventenddate');
        $enddatepicker.datepicker({
            format: 'yyyy-mm-dd'
        });     

        $scope.editingCompanyEvent = {
            Id: 0,
            CompanyId: $scope.companyId,
            Created: "",
            Description: "",
            EndTime: new Date().toLocaleTimeString(navigator.language, {hour: '2-digit', minute:'2-digit', hour12: false }),
            EngagementTypeId: "",
            EventDate: new Date(),
            EventDateString: "",
            EventEndDate: new Date(),
            AllDayEvent: "",
            EventLocation: "",
            EventTitle: "",
            Heading: "",
            IsGesEvent: false,
            Location: "",
            StartTime: new Date().toLocaleTimeString(navigator.language, {hour: '2-digit', minute:'2-digit', hour12: false})
        };

        $scope.isAddNew = true;
        if (event != null) {
            $scope.editingCompanyEvent = event;
            $scope.isAddNew = false;
            
            $scope.currentStartTime = $scope.editingCompanyEvent.StartTime;
            
            $scope.currentEndTime = $scope.editingCompanyEvent.EndTime;
        }

        $startdatepicker.datepicker('setDate', $scope.editingCompanyEvent.EventDate);
        $enddatepicker.datepicker('setDate', $scope.editingCompanyEvent.EventEndDate);
    };

    $scope.GesEventAttendees = function (selectedEvent) {
        
        $scope.Attendees  = angular.copy(selectedEvent.Attendees);        

        if ( $scope.Attendees !== null) {
            for (var i = 0; i < $scope.Attendees.length; i++) {
                if ($scope.Attendees[i].SendDate != null) {
                    
                    var dateValue = $scope.convertDate($scope.Attendees[i].SendDate, 'yyyy-MM-dd hh:mm a');
                    $scope.Attendees[i].SendDate = dateValue;

                }
            }
        }
        
    };

    $scope.CancelUpdateEvent = function (companyEvent) {
        $scope.companyEvents = [];
        $scope.companyEvents = angular.copy($scope.companyEventsOrigin);
        $scope.companyEventsTableParams.reload();
    };

    $scope.SaveCompanyEvent = function (companyEvent) {
        $scope.formValid = Validate(companyEvent);
        
        if (!$scope.formValid){            
            return;
        }
        $scope.ModalName = "modal";   
       
        CompanyService.SaveCompanyEvent(companyEvent, saveCompanyEventCallback);
        
        $('#company-event-dialog').modal('hide');
    };

    /**
     * @return {boolean}
     */
    function Validate(companyEvent){
        $scope.errorMessages = [];
        
        var startDate = new Date(companyEvent.EventDate);
        var endDate = new Date(companyEvent.EventEndDate);
        
        if (startDate.getTime() > endDate.getTime()){
            $scope.errorMessages.push({"text":"The End date must be greater than or equal to the Start date"});
            
            return false;
        }

        var startTime = companyEvent.StartTime;
        var endTime = companyEvent.EndTime;
        
        if (startDate.getTime() === endDate.getTime() && startTime > endTime) {
            $scope.errorMessages.push({"text":"The End time must be greater than or equal to the Start time"});

            return false;
        }
        
        return true;
    }
    
    $scope.AllDayEventChange = function(event){
        $scope.editingCompanyEvent = event;
        SetStartupTime($scope.editingCompanyEvent);
    };
    $scope.TimeChange = function () {
        // var companyEvent = $scope.editingCompanyEvent;
        // if (companyEvent != null && companyEvent.StartTime != null && companyEvent.EndTime != null) {
        //     companyEvent.AllDayEvent = companyEvent.StartTime === "00:00" && companyEvent.EndTime === "00:00";
        // }
    };
    
    function SetStartupTime(event){        
        
        if (event != null) {
            
            var currentStartTime = $scope.currentStartTime;
            var currentEndTime = $scope.currentEndTime;
            
            if (event.AllDayEvent){

                event.StartTime = "00:00";
                event.EndTime = "00:00";

            } else{              
                
                if (!$scope.isAddNew){
                    event.StartTime = currentStartTime;
                    event.EndTime = currentEndTime;
                } 
            }
        }
    }

    $scope.DeleteCompanyEvent = function (event) {
        ModalService.openConfirm("Are you sure to delete this event calendar?", function (result) {
            if (result) {                
                
                CompanyService.DeleteCompanyEvent(event, deleteCompanyEventCallback);

            }
        });
    };  

    function deleteCompanyEventCallback() {
        GetCompanyEvents( $scope.companyId);
        $('#company-event-dialog').modal('hide');
    }

    function saveCompanyEventCallback() {
        GetCompanyEvents( $scope.companyId);
    }

    $scope.convertDate = function (value, format) {
        if (value != null && !value.isNullOrEmpty) {
            return $filter("date")(new Date(parseInt(value.substr(6))), format);
        }

        return null;
    };
    
    String.isNullOrEmpty = function(value) {
        return !(typeof value === "undefined" || (typeof value === "string" && value.length > 0));
    };

}]);