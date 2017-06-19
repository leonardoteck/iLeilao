(function() {
'use strict';

    angular
        .module('starter.controllers')
        .controller('principalController', principalController);

    principalController.$inject = ['$scope', '$http', 'api', 'auth', '$ionicPopup', '$state'];
    function principalController($scope, $http, api, auth, $ionicPopup, $state) {
        var vm = this;

        vm.Dados = {};
        vm.Dados.leiloes = {};
        vm.tipoUsuario = 0;

        activate();

        ////////////////

        function activate() {
            if (!auth.done) {
                window.location.hash = '#/';
                return;
            }
            carregarDados();
        }

        function carregarDados() {
            if (!api.on()) {
                vm.Dados.leiloes.todos = [];
                vm.Dados.leiloes.usuario = [];

                return;
            }

            // $http({
            //     method: 'GET',
            //     url: api.url() + 'movimentacoes/usuario/' + auth.data.config.usuario.Id,
            //     headers: { 'Authorization': 'Bearer ' + auth.data.tokenUsuario.tokenUsuario }
            // }).success(function (data) {
            //     vm.Dados.Movimentacoes = data;
            //     filtrarMovs(vm.Dados.Movimentacoes);
            // }).error(function (data) {
            //     console.log(data);
            //     $ionicPopup.alert({
            //         title: 'Ops!',
            //         template: data[0].errorMessage
            //     });
            // });

            // $http({
            //     method: 'GET',
            //     url: api.url() + 'contascontabeis/usuario/' + auth.config.usuario.id,
            //     headers: { 'Authorization': 'Bearer ' + auth.data.tokenUsuario.tokenUsuario }
            // }).success(function (data) {
            //     vm.Dados.Contas = data;
            // }).error(function (data) {
            //     console.log(data);
            //     $ionicPopup.alert({
            //         title: 'Ops!',
            //         template: data[0].errorMessage
            //     });
            // });
        }
    }
})();