$(function () {

    //"use strict";

    $("a.termTip").each(function () {
        $(this).qtip({
            content: {
                text: $(this).next(".tooltiptext")
            },
            hide: {
                fixed: true,
                delay: 300
            }
        });
    });

    // Sidebar
    $('#simple-search-textbox').catcomplete({
        source: function (request, response) {
            window.searchStr = request.term;
            $.ajax({
                type: "POST",

                url: "/Company/GetCompaniesForAutocomplete",

                data: {
                    //searchTags: false,
                    //searchCases: false,
                    term: request.term,
                    limit: 30
                },

                success: function (ret, textStatus, jqXhr) {
                    response($.map(ret.rows,
                        function (item) {
                            return { label: item.Id, category: item.Name, desc: item.CompanyId };
                        }));
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    console.log("Autocomplete failed.");
                }
            });
        },
        minLength: 2,
        delay: 250,

        focus: function (event, ui) {
            $('#simple-search-textbox').val(ui.item.label.split('(')[0].trim());
            $('#hdCompanyIdSelected').val(ui.item.desc);
            return false;
        },

        select: function (event, ui) {
            $('#simple-search-textbox').val(ui.item.label.split('(')[0].trim());
            $('#hdCompanyIdSelected').val(ui.item.desc);
            $('#simple-search-btn').focus().click();
            return false;
        }
    });

    $(".menu-item-search").click(function () {
        $(".sidebar-toggle").trigger('click');
        $("#simple-search-textbox").focus();

        return false;
    });

    // mouse leaves dropdowns >>> dismiss the dropdown
    $("body").on("mouseleave", ".bootstrap-select.btn-group",
        function (e) {
            //var overElem = e.toElement || e.relatedTarget;
            $("body").trigger("click");
        });

    $(".tooltip-hint").each(function (index, tooltip) {
        var title = $(tooltip).attr("data-tooltip-title");
        var content = $("#" + $(tooltip).attr("data-tooltip-content")).html();
        $(tooltip).popover(utils.getPopoverConfig(title, content));
    });
});
