(function() {
'use strict';

    angular
        .module('starter.controllers')
        .controller('cadastroController', cadastroController);

    cadastroController.$inject = ['$http', 'api', 'auth', '$ionicPopup', '$state'];
    function cadastroController($http, api, auth, $ionicPopup, $state) {
        var vm = this;

        vm.Dados = {};
        
        vm.cadastrar = cadastrar;

        activate();

        ////////////////

        function activate() { }

        function cadastrar() {
            if (!api.on()) {
                auth.done = true;
                $ionicPopup.alert({
                    title: 'Olá!',
                    template: 'Seja bem-vindo ao iLeilão!'
                });
                $state.go('app.principal');
                return;
            }
            $http({
                method: 'POST',
                url: api.url() + 'usuarios/',
                data: vm.Dados
            }).success(function (data) {
                auth.done = true;
                auth.data = data;
                $ionicPopup.alert({
                    title: 'Olá!',
                    template: 'Seja bem-vindo ao iLeilão!'
                });
                $state.go('app.');
            }).error(function (data) {
                console.log(data);
                $ionicPopup.alert({
                    title: 'Ops!',
                    template: data[0].errorMessage
                });
            });
        }
    }
})();