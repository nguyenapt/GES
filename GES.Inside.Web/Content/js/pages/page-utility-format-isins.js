$(function () {

    $(".btn-format-isins").on("click", function() {
        var lines = $("#isin-list-textarea").val().split("\n");

        var formattedLines = [];
        $.each(lines, function (index, value) {
            formattedLines.push(formatIsin(value));
        });

        var outputStr = formattedLines.join("\n");
        $("#isin-list-textarea").val(outputStr);
    });
});

function formatIsin(input) {
    // cleanup
    input = input.trim();

    // only format if needed
    if (isPartlyISIN(input)) {
        return input + calcISINCheck(input);
    } else { // no need to do anything >>> return input
        return input;
    }
}
