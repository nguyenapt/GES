"use strict";

GesInsideApp.factory("ModalService", ['ngDialog', function (ngDialog) {    
    var scope = null;    
    var openModal = function (template, data, classname, controller) {        
        var dialog = ngDialog.open({
            template: template,
            controller: controller,
            closeByDocument: false,
            closeByEscape: false,
            showClose: false,
            data: data,
            scope: data,
            className: 'ngdialog-theme-default ' + classname
        });
        return dialog.id;
    };

    var openConfirm = function (message,callback) {        
            ngDialog.openConfirm({
                template: message + 
            '<div class="ngdialog-buttons">\
                <button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(0)">No</button>\
                <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Yes</button>\
            </div>',
                plain: true
            }).then(function (confirm) {                
                callback(true);
            }, function (reject) {                
                callback(false);
            });
    };

    var closeModal = function (id,callback) {
        ngDialog.close(id);
        if (callback !== null && callback !== undefined) {
            callback();
        }
    };

    return {
        openModal: openModal,
        openConfirm: openConfirm,
        closeModal: closeModal,    
        scope:scope
    }
}]);