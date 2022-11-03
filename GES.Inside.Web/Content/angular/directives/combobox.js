GesInsideApp.directive('combobox', ["$timeout","$compile", function ($timeout,$compile) {
    return {
        restrict: 'E',
        scope: {
            data: '=',
            selected: '=',
            ddlClass: '='
        },
        template:
            "<div class='select' ng-click='openTree()'><p>{{selected.Name}}</p></div>"
            + "<div class='list' ng-show='isOpen'></div>",
        controller: ['$controller', '$scope', '$element', function ($controller, $scope, $element) {
            $scope.openTree = function () {
                $scope.isOpen = $scope.isOpen ? false : true;
            }

            $scope.childClick = function (obj) {
                setSelected($scope, obj);
                $scope.isOpen = false;
                $scope.$apply();
            }
        }],
        controllerAs: 'combobox',
        link: function (scope, element, attrs, ngModel) {
            var list = angular.element(element[0].querySelector('.list'));

            scope.$watchGroup(['data', 'selected'], function (newValues, oldValues, scope) {
                list.html('');

                if (!scope.selected) {
                    setSelected(scope, null);
                }
                var options = getOptions(scope, scope.data, 0);
                list.append($compile(options)(scope));
            });

            // Close on click outside the dropdown...            
            angular.element(document).bind('click', function (event) {
                if (element !== event.target && !element[0].contains(event.target)) {
                    scope.$apply(function () {
                        scope.isOpen = false;
                    })
                }
            });            
        }
    };
    function getOptions(scope, data, level) {

        var optionUL = angular.element("<ul></ul>");

        angular.forEach(data, function (obj) {
            var optionLI = angular.element("<li></li>");
            var optionA = angular.element("<p ng-class='{selected:selected.Id==" + obj.Id + "}' class='level-" + level + "'>" + obj.Name + "</p>");
            optionLI.append(optionA);

            // Set selected option if selected id or object exist..
            if (scope.selected == obj) {
                setSelected(scope, obj);
            }

            optionA.bind("click", function () {
                scope.childClick(obj);
            })

            if (obj.children) {
                optionLI.append(getOptions(scope, obj.children, level + 1));
            }
            optionUL.append(optionLI);
        })

        return optionUL;
    }

    function setSelected(scope, obj) {
        if (obj) {
            scope.selected = obj;
        } else {
            scope.selected = null;
        }
    }
}]);
