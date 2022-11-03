var toBool = function (str) {
    if (typeof str === "boolean") return str;
    var strLower = str.toLowerCase();
    if (strLower === "yes" || strLower === "true")
        return true;
    return false;
}

function quickNotification(msg, type, timeOut) {
    if (typeof (type) === "undefined") type = "success"; // success, info, warning, error
    if (typeof (timeOut) === "undefined") timeOut = 4000;

    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "100",
        "hideDuration": "1000",
        "timeOut": timeOut,
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut",
        bodyOutputType: "trustedHtml"
    };
    toastr[type](msg);
}

function convertNetDate(netDate) {
    var re = /-?\d+/;
    var m = re.exec(netDate);
    var d = new Date(parseInt(m[0]));
    return d;
};

function dateFormatter(cellvalue, options, rowObject) {
    if (cellvalue) {
        return $.format.date(convertNetDate(cellvalue), "yyyy-MM-dd");
    } else {
        return "";
    }
}

function getBooleanValues() {
    return [true, false];
}

function buildSearchSelect (uniqueNames) {
    var values = ":All";
    $.each(uniqueNames, function () {
        values += ";" + this + ":" + this;
    });
    return values;
}

function setBooleanSelect(columnName, defaultvalue) {
    this.jqGrid("setColProp", columnName, {
        stype: "select",
        searchoptions: {
            value: buildSearchSelect(getBooleanValues.call(this)),
            defaultValue:defaultvalue,
            sopt: ["eq"]
        }
    });
}

// validation rules
function isNormalInteger(str) {
    var n = ~~Number(str);
    return String(n) === str && n >= 0;
}

function validatePositive(value, column) {
    if (isNaN(value) && value < 0)
        return [false, "Please enter a correct positive value."];
    else
        return [true, ""];
}

function getDatePickerConfig() {
    return {
        dateFormat: "yy-mm-dd",
        autoSize: true,
        changeYear: true,
        changeMonth: true,
        showButtonPanel: true,
        showWeek: true
    };
}

// date picker
function loadDatePicker(filters) {
    var datePickerConfig = getDatePickerConfig();
    datePickerConfig.onSelect = function () {
        var $grid, grid;
        if (typeof (this.id) === "string" && this.id.substr(0, 3) === "gs_") {
            // in case of searching toolbar  
            $grid = $(this).closest("div.ui-jqgrid-hdiv")
                .next("div.ui-jqgrid-bdiv")
                .find("table.ui-jqgrid-btable:first");
            if ($grid.length > 0) {
                grid = $grid[0];
                if ($.isFunction(grid.triggerToolbar)) {
                    setTimeout(function() {
                            grid.triggerToolbar();
                        },
                        50);
                }
            }
        } else {
            // refresh the filter in case of  
            // searching dialog   
            $(this).trigger("change");
        }
    };
    setTimeout(function () {
        $(filters).datepicker(datePickerConfig);
    }, 100);
};

function setFilterDate (column) {
    this.jqGrid("setColProp", column, {
        searchoptions: {
            dataInit: loadDatePicker
        },
        dataEvents: [{
            type: "change",
            fn: function (e) {
                console.log("change via datePicker failed - search dialog detected");
            }
        }]
    });
}

// handle multi-select
function handleMultiSelect(rowid, e) {
    var grid = $(this);
    if (!e.ctrlKey && !e.shiftKey) {
        grid.jqGrid('resetSelection');
    }
    else if (e.shiftKey) {
        var initialRowSelect = grid.jqGrid('getGridParam', 'selrow');

        grid.jqGrid('resetSelection');

        var CurrentSelectIndex = grid.jqGrid('getInd', rowid);
        var InitialSelectIndex = grid.jqGrid('getInd', initialRowSelect);
        var startID = "";
        var endID = "";
        if (CurrentSelectIndex > InitialSelectIndex) {
            startID = initialRowSelect;
            endID = rowid;
        }
        else {
            startID = rowid;
            endID = initialRowSelect;
        }
        var shouldSelectRow = false;
        $.each(grid.getDataIDs(), function (_, id) {
            if ((shouldSelectRow = id == startID || shouldSelectRow) && (id != rowid)) {
                grid.jqGrid('setSelection', id, false);
            }
            return id != endID;
        });
    }
    return true;
};

//This is the function that will submit the form using ajax and check for validation errors before that.
function submitAsyncForm($formToSubmit, fnSuccess, fnError) {
    if (!$formToSubmit.valid())
        return false;

    $.ajax({
        type: $formToSubmit.attr("method"),
        url: $formToSubmit.attr("action"),
        data: $formToSubmit.serialize(),

        success: fnSuccess,
        error: fnError

    });
    return true;
}

// get values from jqGrid cells in Edit mode
function getCellValue(rowId, cellId) {
    var cell = jQuery("#" + rowId + "_" + cellId);
    return cell.val();
}

function getCheckboxCellValue(rowId, cellId) {
    var cell = jQuery("#" + rowId + "_" + cellId);
    return cell.prop("checked");
}

function getCheckboxCellValueNoPrefix(id) {
    var cell = jQuery("#" + id);
    return cell.prop("checked");
}

/**
 * @author sellistonball
 */

/**
 * Calculates a check digit for an isin
 * @param {String} code an ISIN code with country code, but without check digit
 * @return {Integer} The check digit for this code
 */
function calcISINCheck(code) {
    var conv = '';
    var digits = '';
    var sd = 0;
    // convert letters
    for(var i =0; i < code.length; i++) {
        var c = code.charCodeAt(i);
        conv += (c > 57) ? (c - 55).toString() : code[i]
    }
    // group by odd and even, multiply digits from group containing rightmost character by 2
    for (var i = 0 ; i < conv.length; i++) {
        digits += (parseInt(conv[i])*((i % 2)==(conv.length % 2 != 0 ? 0 : 1) ? 2 : 1)).toString();
    }
    // sum all digits
    for (var i = 0; i < digits.length; i++) {
        sd += parseInt(digits[i]);
    }
    // subtract mod 10 of the sum from 10, return mod 10 of result 
    return (10 - (sd % 10)) % 10;
}

function isValidISIN(isin){
    // basic pattern
    var regex = /^([a-zA-Z_-_]{2})([0-9A-Za-z]{9})([0-9])$/;
    var match = regex.exec(isin);
    if (match === null) return false;
    if (match.length !== 4) return false;

    // if start with C_ >>> no checksum
    if (isin.indexOf("C_") === 0)
        return true;

    // validate the check digit
    var result = match[3] == calcISINCheck(match[1] + match[2]);
    return result;
}

function isPartlyISIN(str) {
    // basic pattern
    var regex = /^([a-zA-Z_-_]{2})([0-9A-Za-z]{9})$/;
    var match = regex.exec(str);
    if (typeof match === "undefined") {
        return false;
    }
    if (match === null) return false;
    if (match.length !== 3) return false;

    return true;
}

$(".tooltip-hint").each(function (index, tooltip) {
    var title = $(tooltip).attr("data-tooltip-title");
    var content = $("#" + $(tooltip).attr("data-tooltip-content")).html();
    $(tooltip).popover(getPopoverConfig(title, content));
});

 function getPopoverConfig(title, content) {
    return {
        html: true,
        animation: true,
        title: title,
        content: content,
        container: "body",
        placement: function (context, source) {
            var offset = $(source).offset();
            if (offset.left > 300) {
                return "left";
            }
            return "right";
        },
        trigger: "hover"
    }
}

function setUpContentBlockAnimation()
{
    $('.ges-content-block > .header > .title').removeClass("fadeIn animated").addClass("fadeIn animated").one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
        $(this).removeClass("fadeIn animated");
    });

    $('.ges-content-block > .ges-content').removeClass("fadeIn animated").addClass("fadeIn animated").one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
        $(this).removeClass("fadeIn animated");
    });

    $('.ges-content-read-more').removeClass("bounceInLeft animated").addClass("bounceInLeft animated").one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () {
        $(this).removeClass("bounceInLeft animated");
    });
 }

$('.nav-tabs-custom li a').click(function (e) {
    e.preventDefault();
    e.stopImmediatePropagation();
    $(this).tab('show');
});