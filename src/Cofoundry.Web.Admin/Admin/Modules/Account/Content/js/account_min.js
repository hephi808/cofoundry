/*! UberCMS 2017-07-17 */
angular.module("cms.account",["ngRoute","cms.shared"]).constant("_",window._).constant("account.modulePath","/Admin/Modules/Account/Js/"),angular.module("cms.account").config(["$routeProvider","shared.routingUtilities","account.modulePath",function(a,b,c){a.when("/change-password",b.mapOptions(c,"ChangePassword")).otherwise(b.mapOptions(c,"AccountDetails"))}]),angular.module("cms.account").factory("account.accountService",["$http","shared.serviceBase",function(a,b){var c={},d=b+"account";return c.getAccountDetails=function(){return a.get(d)},c.update=function(b){return a.patch(d,b)},c.updatePassword=function(b){return a.put(d+"/password",b)},c}]),angular.module("cms.account").controller("AccountDetailsController",["shared.LoadState","shared.modalDialogService","account.accountService","account.modulePath",function(a,b,c,d){function e(){n.edit=f,n.save=g,n.cancel=h,n.editMode=!1,n.globalLoadState=new a,n.saveLoadState=new a,n.formLoadState=new a(!0),j().then(m.bind(null,n.formLoadState))}function f(){n.editMode=!0,n.mainForm.formStatus.clear()}function g(){l(n.saveLoadState),c.update(n.command).then(i.bind(null,"Changes were saved successfully"))["finally"](m.bind(null,n.saveLoadState))}function h(){n.editMode=!1,n.command=k(n.user),n.mainForm.formStatus.clear()}function i(a){return j().then(n.mainForm.formStatus.success.bind(null,a))}function j(){function a(a){n.user=a,n.command=k(a),n.editMode=!1}return c.getAccountDetails().then(a)}function k(a){return _.pick(a,"firstName","lastName","email")}function l(a){n.globalLoadState.on(),a&&_.isFunction(a.on)&&a.on()}function m(a){n.globalLoadState.off(),a&&_.isFunction(a.off)&&a.off()}var n=this;e()}]),angular.module("cms.account").controller("ChangePasswordController",["$location","shared.LoadState","shared.modalDialogService","account.accountService",function(a,b,c,d){function e(){k.save=f,k.cancel=g,k.doesPasswordMatch=h,k.globalLoadState=new b,k.formLoadState=new b(!0),i().then(k.formLoadState.off)}function f(){k.globalLoadState.on(),d.updatePassword(k.command).then(j)["finally"](k.globalLoadState.off)}function g(){j()}function h(a){return k.command?k.command.newPassword===a:!1}function i(){function a(a){k.user=a,k.command={}}return d.getAccountDetails().then(a)}function j(){a.path("/")}var k=this;e()}]);