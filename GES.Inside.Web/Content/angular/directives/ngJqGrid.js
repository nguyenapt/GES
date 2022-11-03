GesInsideApp.directive('ngJqGrid', ["$timeout", "$compile", function ($timeout, $compile) {
    return {
        restrict: 'E',
        scope: {
            config: '='
        },
        link: function (scope, element, attrs) {
            scope.$watch('config', function (newValue) {
                element.children().empty();
                var table = angular.element('<table id="' + newValue.id + '"></table>');

                element.append($compile(table)(scope));

                angular.extend(newValue, {
                    jsonReader: {
                        root: 'rows',
                        page: 'page',
                        total: 'total',
                        records: 'records',
                        repeatitems: false
                    },
                    viewrecords: true,
                    rowNum: 50,
                    loadComplete: function () {
                        $compile(angular.element('#' + newValue.id))(scope);
                    },
                    rowList: [20, 50, 100]                    
                });

                if (newValue.pager) {
                    var pager = angular.element('<div id="' + newValue.pager + '"></table>');
                    element.append($compile(pager)(scope));
                    angular.extend(newValue, {
                        pager: '#' + newValue.pager,
                        pagerpos: "center",
                        recordpos: "right",
                        pgbuttons: true,
                        pginput: true
                    });
                }

                angular.element(table).jqGrid(newValue);
                angular.element(table).jqGrid("filterToolbar", { stringResult: true, searchOnEnter: true, defaultSearch: "cn", searchOperators: false });        

            });
        }
    };
}]);