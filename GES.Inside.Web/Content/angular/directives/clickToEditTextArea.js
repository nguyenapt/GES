GesInsideApp.directive("clickToEditTextArea", ["$timeout", function ($timeout) {
    return {
        restrict: "A",
        require: "ngModel",
        scope: {
            model: "=ngModel",
            required: "=ngRequired",
            placeholder: "@",
            isEditState: "=",
            isDisabled: "=ngDisabled",
            maxLengthValue: "=maxlength"
        },
        replace: true,
        transclude: false,
        templateUrl: "/Content/angular/templates/clickToEditTextAreaTemplate.html",
        link: function (scope, element) {
            scope.editState = !!scope.isEditState;
            scope.localModel = scope.model || "";
            scope.setDisabled = scope.isDisabled;

            var maxlength = scope.maxLengthValue;

            var input = $(element[0]).find(".inputText")[0];

            if (maxlength!== undefined) {
                scope.inputValue = '(' + input.value.length + "/" + maxlength + ')';
            }

            $(input).height($(element).height() + 50);
            
            scope.save = function () {
                scope.localModel = scope.model;
                scope.toggle();
            };

            scope.cancel = function () {
                scope.model = scope.localModel;
                scope.toggle();
            }
            
            scope.change = function(val){
                var maxlength = scope.maxLengthValue;
                var input = $(element[0]).find(".inputText")[0];

                var currentTextCount;

                if (val === undefined){
                    currentTextCount = input.value.length;
                }
                else{
                    currentTextCount= val.length
                } 
                
                if (maxlength !== undefined) {
                    scope.inputValue = '(' + currentTextCount + "/" + maxlength + ')';
                    scope.model = input.value;
                }               
                
            }
            
            scope.toggle = function () {
                scope.localModel = scope.model;
                scope.editState = !scope.editState;
                scope.setDisabled = scope.isDisabled;
                var maxlength = scope.maxLengthValue;
                
                var input = $(element[0]).find(".inputText")[0];

                if (maxlength!== undefined) {
                    scope.inputValue = '(' + input.value.length + "/" + maxlength + ')';
                }
                               
                $(input).height($(element).height() + 50);

                $timeout(function () {
                    scope.editState ? input.focus() : input.blur();
                }, 0);
            }
        }
    }
}]);