(function() {
'use strict';

    angular
        .module('starter.services')
        .value('auth', {
            done: false,
            data: null,
            id: null,
            token: null
        })
        .service('LoginService', LoginService);

    LoginService.$inject = ['api', '$http', 'auth', '$ionicPopup', '$rootScope'];
    function LoginService(api, $http, auth, $ionicPopup, $rootScope) {
        this.doLogin = doLogin;
        this.recover = recover;
        
        ////////////////

        function doLogin(user, pass, cb) {
            if (!api.on()) {
                auth.done = true;
                cb(true);
                return;
            }
            $http({
                method: 'POST',
                url: api.url() + 'logins',
                data: { email: user, senha: pass }
            }).success(function (data) {
                auth.data = data;
                auth.done = true;
                cb(true);
            }).error(function (data) {
                auth.done = false;
                console.log(data);
                $ionicPopup.alert({
                    title: 'Ops!',
                    template: data[0].errorMessage
                });
                cb(false);
            });
        }

        function recover(email) {
            
        }
    }
})();