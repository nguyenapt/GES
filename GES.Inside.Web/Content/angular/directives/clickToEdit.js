GesInsideApp.directive("clickToEdit", ["$timeout", function ($timeout) {
    return {
        restrict: "A",
        require: "ngModel",
        scope: {
            model: "=ngModel",
            required: "=ngRequired",
            placeholder: "@",
            isEditState: "=",
            isDisabled: "=ngDisabled"
        },
        replace: true,
        transclude: false,
        templateUrl: "/Content/angular/templates/clickToEditTemplate.html",
        link: function (scope, element) {
            scope.editState = !!scope.isEditState;
            scope.localModel = scope.model || "";
            scope.localRequired = scope.required;
            scope.setDisabled = scope.isDisabled;
            
            scope.save = function () {
                scope.localModel = scope.model;
                scope.toggle();
            };

            scope.cancel = function () {
                scope.model = scope.localModel;
                scope.toggle();
            }

            var boxWidth = $(".box-body").width();

            var input = $(element[0]).find(".inputText")[0];
            
            $(input).width(boxWidth);

            scope.toggle = function () {
                if (scope.localModel === "") {
                    scope.localModel = scope.model;
                }
                scope.editState = !scope.editState;
                scope.setDisabled = scope.isDisabled;

                if (scope.localRequired && (scope.localModel == null || scope.localModel === "")){
                    scope.editState = true;
                }


                $(input).width(boxWidth);
                
                $timeout(function () {
                    scope.editState ? input.focus() : input.blur();
                    
                    
                }, 0);
            }
        }
    }
}]);