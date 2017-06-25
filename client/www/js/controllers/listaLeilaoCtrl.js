(function() {
'use strict';

    angular
        .module('starter.controllers')
        .controller('listaController', listaController);

    listaController.$inject = ['$scope', '$http', 'api', 'auth', '$ionicPopup', '$state'];
    function listaController($scope, $http, api, auth, $ionicPopup, $state) {
        var vm = this;

        vm.Dados = {};
        vm.tipoUsuario = 0;

        activate();

        ////////////////

        function activate() {
            console.log($state);
            switch ($state.current.name) {
                case 'app.solicitacoes':
                    
                    break;
                case 'app.acompanha':

                    break;
                case 'app.leiloei':

                    break;
                case 'app.andamento':

                    break;
                case 'app.destaque':

                    break;
                default:
                    break;
            }

            if (!auth.done) {
                window.location.hash = '#/';
                return;
            }
            carregarDados();
        }

        function carregarDados() {
            if (!api.on()) {
                vm.Dados.leiloes = [];

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
        }
    }
})();