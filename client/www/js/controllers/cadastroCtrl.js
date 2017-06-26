(function() {
'use strict';

    angular
        .module('starter.controllers')
        .controller('cadastroController', cadastroController);

    cadastroController.$inject = ['$http', 'api', 'auth', '$ionicPopup', '$state'];
    function cadastroController($http, api, auth, $ionicPopup, $state) {
        var vm = this;

        vm.dados = {};
        vm.tipos = [
            {
                id: 0,
                nome: 'Participante'
            },
            {
                id: 1,
                nome: 'Leiloeiro'
            }
        ]
        
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
                data: vm.dados
            }).success(function (data) {
                $ionicPopup.alert({
                    title: 'Olá!',
                    template: 'Seja bem-vindo ao iLeilão!'
                });
                $state.go('login');
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