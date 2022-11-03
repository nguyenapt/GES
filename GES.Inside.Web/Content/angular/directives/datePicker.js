GesInsideApp.directive("datePicker", function () {
    return {
        restrict: "A",
        require: "ngModel",
        link: function (scope, element, attrs, ngModelCtrl) {
            $(function () {
                var datePickerConfig = getDatePickerConfig();
                datePickerConfig.defaultDate = ngModelCtrl.$viewValue;
                datePickerConfig.onSelect = function(date) {
                    scope.$apply(function() {
                        ngModelCtrl.$setViewValue(date);
                    });
                };
                element.datepicker(datePickerConfig);
            });
        }
    }
});