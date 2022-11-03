
$(function () {
    $("#_form_edi_upload").submit(function (event) {
        // disable button
        $("#btn-submit-edi-sync").button("loading");

        event.preventDefault();
        EdiUploadSubmit();
    });

    $("#_form_market_cap_upload").submit(function (event) {
        // disable button
        $("#btn-submit-marketcap-sync").button("loading");

        event.preventDefault();
        MarketCapUploadSubmit();
    });
});

function EdiUploadSubmit() {
    var form = $("#_form_edi_upload");
    var formdata = new FormData();
    
    var files = $("#txtfileinput").get(0).files;
    if (files.length > 0) {
        formdata.append("txtfileinput", files[0]);
    }

    var formAction = form.attr('action');
    $.ajax({
        url: '/Data/EdiSync',
        data: formdata,
        contentType: false,
        processData: false,
        type: 'POST'
    })
    .done(function (response, textStatus, jqXHR) {
        if (response.success) {
            quickNotification(response.message);
        } else {
            quickNotification(response.message, "error");
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        // revert action
        self.prop("checked", self.val());

        quickNotification("Failed: Error occurred", "error");
    })
    .always(function (jqXHR, textStatus, errorThrown) {
        // reset button
        $("#btn-submit-edi-sync").button("reset");
    });
}


function MarketCapUploadSubmit() {
    var form = $("#_form_market_cap_upload");
    var formdata = new FormData();

    var files = $("#marketCapFileInput").get(0).files;
    if (files.length > 0) {
        formdata.append("marketCapFileInput", files[0]);
    }

    var formAction = form.attr('action');
    $.ajax({
        url: '/Data/MarketCap',
        data: formdata,
        contentType: false,
        processData: false,
        type: 'POST'
    })
    .done(function (response, textStatus, jqXHR) {
        if (response.success) {
            quickNotification(response.message);
        } else {
            quickNotification(response.message, "error");
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        // revert action
        self.prop("checked", self.val());

        quickNotification("Failed: Error occurred", "error");
    })
    .always(function (jqXHR, textStatus, errorThrown) {
        // reset button
        $("#btn-submit-marketcap-sync").button("reset");
    });
}
