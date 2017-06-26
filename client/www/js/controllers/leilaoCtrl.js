(function() {
'use strict';

    angular
        .module('starter.controllers')
        .controller('leilaoController', leilaoController);

    leilaoController.$inject = ['$scope', '$http', 'api', 'auth', '$ionicPopup', '$state', '$interval'];
    function leilaoController($scope, $http, api, auth, $ionicPopup, $state, $interval) {
        var vm = this;

        vm.dados = {};

        vm.lanceMinimo = lanceMinimo;
        vm.lance = lance;
        vm.encerrar = encerrar;

        activate();

        ////////////////

        function activate() {
            if (!auth.done) {
                window.location.hash = '#/';
                return;
            }
            carregarDados();

            $interval(function () {
                $http({
                    method: 'GET',
                    url: api.url() + 'leiloes/' + $state.params.id,
                    headers: { 'Authorization': 'Bearer ' + auth.token }
                }).success(function (data) {
                    if (!vm.dados.maiorLance && !data.maiorLance)
                        return;
                    if (!vm.dados.maiorLance && data.maiorLance) {
                        vm.dados.maiorLance = data.maiorLance;
                        return;
                    }
                    if (data.maiorLance.id != vm.dados.maiorLance.id)
                        vm.dados.maiorLance = data.maiorLance;
                    if (data.status != 0) {
                        $ionicPopup.alert({ title: 'Vendido!', template: 'Desculpe, mas o leilão foi encerrado.' });
                        history.back();
                    }
                }).error(function (data) {
                    console.log(data);
                    $ionicPopup.alert({ title: 'Ops!', template: data[0].errorMessage });
                });
            }, 30000);
        }

        function carregarDados() {
            if (!api.on()) {
                vm.dados = {};
                return;
            }

            $http({
                method: 'GET',
                url: api.url() + 'leiloes/' + $state.params.id,
                headers: { 'Authorization': 'Bearer ' + auth.token }
            }).success(function (data) {
                vm.dados = data;
            }).error(function (data) {
                console.log(data);
                $ionicPopup.alert({ title: 'Ops!', template: data[0].errorMessage });
            });
        }

        function lanceMinimo() {
            lance(vm.dados.maiorLance.valor + vm.dados.lote.valorMinimo);
        }

        function lance(valor) {
            $http({
                method: 'POST',
                url: api.url() + 'leiloes/lance/' + $state.params.id,
                data: {
                    valor: valor,
                    usuarioId: auth.id,
                    leilaoId: $state.params.id
                },
                headers: { 'Authorization': 'Bearer ' + auth.token }
            }).success(function (data) {
                vm.dados.maiorLance = data.maiorLance;
                $ionicPopup.alert({ title: 'Sucesso!', template: 'Lance efetuado com sucesso' });
            }).error(function (data) {
                console.log(data);
                $ionicPopup.alert({ title: 'Ops!', template: data[0].errorMessage });
            });
        }

        function encerrar() {
            $http({
                method: 'POST',
                url: api.url() + 'leiloes/encerrar/' + $state.params.id,
                headers: { 'Authorization': 'Bearer ' + auth.token }
            }).success(function (data) {
                history.back();
                $ionicPopup.alert({ title: 'Sucesso!', template: 'Leilão encerrado com sucesso' });
            }).error(function (data) {
                console.log(data);
                $ionicPopup.alert({ title: 'Ops!', template: data[0].errorMessage });
            });
        }
    }
})();