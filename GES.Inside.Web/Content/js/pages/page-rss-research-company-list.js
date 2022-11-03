$(function () {
    var postUrl = "/GSSResearch/GetDataForRssResearchCompaniesJqGrid";
    var gridCaption = "Company List";

    var getAssessments = function () {
        return assessments;
    };
        
    var getWorkflowStatues = function () {
        return workflowStatues;
    };

    var getflags = function () {
        return flags;
    };       
    
    var getGssocReview = function () {
        return gssocReviews;
    };    
    
    var setAssessmentSelect = function (columnName) {
        this.jqGrid("setColProp", columnName, {
            stype: "select",
            searchoptions: {
                value: buildSearchSelect(getAssessments.call(this)),
                sopt: ["eq"]
            }
        });
    }, 
        setWorkflowStatuesSelect = function (columnName) {
        this.jqGrid("setColProp", columnName, {
            stype: "select",
            searchoptions: {
                value: buildSearchSelect(getWorkflowStatues.call(this)),
                sopt: ["eq"]
            }
        });
    }, 
        setFlagsSelect = function (columnName) {
        this.jqGrid("setColProp", columnName, {
            stype: "select",
            width: "200px",
            searchoptions: {
                value: buildSearchSelect(getflags.call(this)),
                sopt: ["eq"]
            }
        });
    } ,
        setGssocReviewsSelect = function (columnName) {
        this.jqGrid("setColProp", columnName, {
            stype: "select",
            width: "200px",
            searchoptions: {
                value: buildSearchSelect(getGssocReview.call(this)),
                sopt: ["eq"]
            }
        });
    };
    
    
    
    
    var grid = $("#tblGssCompanyList");
    $.jgrid.defaults.responsive = true;
    //$.jgrid.defaults.styleUI = "Bootstrap";
    grid.bind("jqGridLoadComplete", function (e, rowid, orgClickEvent) {
        $(window).resize();

        // odd, even row
        $("tr.jqgrow:even").addClass("jqgrid-row-even");
    });
    grid.jqGrid({
        url: postUrl,
        datatype: "json",
        mtype: "post",
        colNames: ["Id", "Name", "Workflow status", "Assessment", "Analyst", "Reviewer", "Flagged", "GSSOC Review"],
        colModel: [
            { name: "Id", width: "35px", align: "right", hidden: true, search: false },
            {
                name: "Name", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] },
                formatter: function (cellvalue, options, rowObject) {
                    var parkedCompany;
                    
                    if(rowObject.IsParked){
                        return  "<span class='parked-glyphicon'><b>" + rowObject.Name + "</b></span>";
                    }                    
                    
                    return "<b><a class='cell-company-name' href=\"/GSSResearch/CompanyDetails/" + rowObject.Id + "\">" + rowObject.Name + "</a></b>";
                }
            },
            { name: "WorkflowStatus", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] } },
            { name: "Assessment", width: "100px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "Analyst", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "Reviewer", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } },
            { name: "Flags", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn"] } },
            { name: "GssocReview", width: "100px", searchoptions: { sopt: ["cn", "ew", "en", "bw", "bn", "nu"] } }
        ],
        pager: $("#myPager"),
        rowNum: 20,
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

        sortname: "Name",
        sortorder: "asc",
        cmTemplate: { sortable: true },
        loadError: function (jqXHR, textStatus, errorThrown) {
            alert("HTTP status code: " + jqXHR.status + "\n" +
                  "TextStatus: " + textStatus + "\n" +
                  "Error Message: " + errorThrown);
        }
    });
    setAssessmentSelect.call(grid, "Assessment");
    setWorkflowStatuesSelect.call(grid, "WorkflowStatus");
    setFlagsSelect.call(grid, "Flags");
    setGssocReviewsSelect.call(grid, "GssocReview");

    grid.jqGrid("navGrid", "#myPager", { edit: false, add: false, del: false, search: false, refresh: true, 'cloneToTop': true });

    grid.jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: true });

});
