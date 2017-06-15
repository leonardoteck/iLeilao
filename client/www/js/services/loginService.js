(function() {
'use strict';

    angular
        .module('starter.services')
        .value('auth', { done: false })
        .service('LoginService', LoginService);

    LoginService.$inject = ['api', '$http', 'auth', '$ionicPopup'];
    function LoginService(api, $http, auth, $ionicPopup) {
        this.doLogin = doLogin;
        
        ////////////////

        function doLogin(user, pass, cb) {
            if (!api.on()) {
                auth.done = true;
                cb();
                return;
            }
            $http({
                method: 'POST',
                url: api.url() + 'logins',
                data: { email: user, senha: pass }
            }).success(function (data) {
                auth.data = data;
                auth.done = true;
                cb();
            }).error(function (data) {
                auth.done = false;
                console.log(data);
                $ionicPopup.alert({
                    title: 'Ops!',
                    template: data[0].errorMessage
                });
            });
        }
    }
})();