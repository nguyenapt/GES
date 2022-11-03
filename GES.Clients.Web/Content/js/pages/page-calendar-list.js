
$(function () {
    var postUrl = "/calendar/GetDataForCalendarJqGrid";
    var gridCaption = "Calendar List";

    var grid = $("#tblcalendars");
    $.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = "Bootstrap";
    grid.bind("jqGridLoadComplete", function (e, rowid, orgClickEvent) {
        $(window).resize();

        // odd, even row
        $("tr.jqgrow:even").addClass("jqgrid-row-even");
    });
    grid.jqGrid({
        url: postUrl,
        postDate:{onlyUpcomingEvents: true},
        datatype: "json",
        mtype: "post",
        colNames: ["Id","Event Title", "Company / Engagement", "Description", "Location", "Event time", "End time", "All day event", "Company event", "Description","Email me details"],
        colModel: [
            { name: "Id", width: "35px", align: "right", hidden: true, search: false },
           {
                name: "Heading",  searchoptions: {searchOperators: true, sopt: ["cn", "ew", "en", "bw", "bn"] },
               cellattr: function (rowId, tv, rawObject, cm, rdata) {
                   return 'style="white-space: normal;"';
               },
               formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    var eventTypeLogo = "";
                    
                    if (rowObject.IsGesEvent){
                        eventTypeLogo = " <img style='text-align: center; float: right; height: 40%; margin-top: 5px;' src='../Content/img/sus-logo.png' alt='Ges event' />"
                    } 
                    
                    if (cellvalue == null){
                        cellvalue = (rowObject.Description != null?(rowObject.Description.length >= 60?rowObject.Description.substring(0, 60):rowObject.Description):"") + "...";
                    } 
                    
                    return cellPrefix + "<a data-toggle=\"modal\" data-target=\"#calendar-dialog\" onclick='getSelectCalendar(" + rowObject.Id + "," + rowObject.IsCompanyEvent + ")' href=\"/Calendar/Details/" + rowObject.Id + "\">" + cellvalue + "</a>" + eventTypeLogo;
                }
            },            
            { name: "CompanyNameOrEngagementName", width: "120px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] } },
            { name: "Description", width: "180px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] } ,
                cellattr: function (rowId, tv, rawObject, cm, rdata) {
                    return 'style="white-space: normal;"';
                }},
            { name: "EventLocation", width: "100px",searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "EventDate", search: false, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d h:i A" }, align: "center", width: 60, title: false, searchoptions: { sopt: ["eq", "lt", "gt"] } },
            { name: "EventEndDate", hidden: true, search: false, formatter: 'date', formatoptions: { srcformat: "ISO8601Long", newformat: "Y-m-d h:i A" }, align: "center", width: 50, title: false, searchoptions: { sopt: ["eq", "lt", "gt"] }  },

            {
                name: "AllDayEvent", search: false, width: 40, hidden: true,
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    if (cellvalue === true){
                        return cellPrefix + "<div style='text-align: center'><i class=\"fa fa-check\" aria-hidden=\"true\"></i></div>";

                    } else{
                        return cellPrefix;
                    }
                }
            },
            {
                name: "IsCompanyEvent", search: false, width: 40, hidden: true,
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                    if (cellvalue === true ){
                        return cellPrefix + "<div style='text-align: center'><i class=\"fa fa-check\" aria-hidden=\"true\"></i></div>";

                    } else{
                        return cellPrefix;
                    }
                }
            },            
            { name: "Description", search: false,  hidden: true,  searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] } },
            {
                name: "IsCompanyEvent", width: "50px", search: false, searchoptions: {searchOperators: false, sopt: ["cn", "ew", "en", "bw", "bn"]},
                formatter: function (cellvalue, options, rowObject) {
                    var cellPrefix = "";
                   
                    if (cellvalue === true  && rowObject.EventEndDate != null) {
                        return cellPrefix + "<div style='text-align: center'><button  type='button' title='Send the calendar to me' onclick='sendCalendar(" + rowObject.Id + ")' data-tooltip-content='download-event-calendar-send-mail-hint' style='padding: 0px; background-color: transparent;'  class='btn btn-sm  tooltip-hint btn-send-email-calendar ei-event-send-mail-link-loding" + rowObject.Id + "' value='" + rowObject.Id + "' data-loading-text=\"<i class=\'fa fa-circle-o-notch fa-spin\'></i> Sending email...\" ><i class=\"fa fa-paper-plane\"></i></button></div>";
                    } else {
                        return cellPrefix;
                    }
                }
            }
        ],
        pager: $("#myPager"),
        rowNum: 50,
        rowList: [20, 50, 100],
        autowidth: true,
        shrinkToFit: true,
        toppager: true,
        //loadonce: true,
        rownumbers: false,
        //pagerpos: "left",
        gridview: true,
        //width: "auto",
        height: "auto",
        viewrecords: true,
        caption: gridCaption,

        sortname: "none",
        sortorder: "asc",
        loadError: function (jqXHR, textStatus, errorThrown) {
            alert("HTTP status code: " + jqXHR.status + "\n" +
                  "TextStatus: " + textStatus + "\n" +
                  "Error Message: " + errorThrown);
        }
    });

    grid.jqGrid("navGrid", "#myPager", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });

    grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: true });

});

function sendCalendar(id) {
    
    var value = id;
    var data = {
        eventId: value
    };

    $(".ei-event-send-mail-link-loding" + value).button("loading");
    $.ajax({
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        url: "/Company/EventExportSendMail2",
        data: JSON.stringify(data)
    })
        .done(function (response, textStatus, jqXHR) {
            //self.button("reset");
            if (response.meta.success) {
                utils.quickNotification(response.meta.message);

            } else {
                utils.quickNotification(response.meta.message, "error");
            }
            $(".ei-event-send-mail-link-loding" + value).button("reset");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            utils.quickNotification("Failed: Error occurred sending email", "error");
        })
        .always(function () {

        });
}

function sendCalendarFromDiaglog() {
   var id =  $('#eventid').val();
    sendCalendar(id);
    
}
function getSelectCalendar(id, IsCompanyEvent) {
    var sendButton = $("#btn-submit-company-event");

    sendButton.show();
    if (!IsCompanyEvent){
        sendButton.hide();
    } 

    $.ajax({
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "GET",
        url: "/Company/GetCalendar",
        data: {eventId: id, companyEvent: IsCompanyEvent}
    })
        .done(function (response, textStatus, jqXHR) {

            if (response.meta.success) {                

                $("#eventid").val(response.data.Id);
                $("#eventtitle").val(response.data.Heading);
                
                $("#eventlocation").val(response.data.EventLocation);                

                if (response.data.EventDate != null) {
                    var startDate = new Date(response.data.EventDate.match(/\d+/)[0] * 1);
                    $("#eventdate").val(formatDate(startDate));
                }                

                if (response.data.EventEndDate != null) {
                    var endDate = new Date(response.data.EventEndDate.match(/\d+/)[0] * 1);
                    $("#eventenddate").val(formatDate(endDate));
                    
                    document.getElementById("btn-submit-company-event").visiable = false;
                } else{
                    document.getElementById("btn-submit-company-event").disabled = true;
                }               
                
                $("#alldayevent").prop('checked', response.data.AllDayEvent);                
                $("#isgesevent").prop('checked',response.data.IsGesEvent);
                
                $("#description").val(response.data.Description);
                
                var title = $("#title");
                title.text("Company event");
                
                if (!response.data.IsCompanyEvent){
                    title.text("Engagement timeline");
                } 

            } else {
                utils.quickNotification(response.meta.message, "error");
            }

        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            utils.quickNotification("Failed: Error occurred sending email", "error");
        })
        .always(function () {

        });      
    
}

function formatDate(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0'+minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return date.getFullYear() + "/" + (date.getMonth()+1) + "/" + date.getDate()  + "  " + strTime;
}
