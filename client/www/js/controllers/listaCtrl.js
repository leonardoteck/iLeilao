(function() {
'use strict';

    angular
        .module('starter.controllers')
        .controller('listaController', listaController);

    listaController.$inject = ['$scope', '$http', 'api', 'auth', '$ionicPopup', '$state'];
    function listaController($scope, $http, api, auth, $ionicPopup, $state) {
        var vm = this;

        vm.dados = {};

        activate();

        ////////////////

        function activate() {
            var url = '';
            switch ($state.current.name) {
                case 'app.solicitacoes':
                    if (auth.participante)
                        url = 'solicitacoes/usuario/' + auth.id;
                    else
                        url = 'solicitacoes/pendentes'
                    break;
                case 'app.acompanha':
                    url = 'leiloes/usuario/participando/' + auth.id;
                    return;
                case 'app.leiloei':
                    url = 'leiloes/usuario/' + auth.id;
                    return;
                case 'app.andamento':
                    url = 'leiloes'
                    return;
                case 'app.destaque':
                    url = 'leiloes/populares'
                    return;
                default:
                    url = 'solicitacoes'
                    break;
            }

            if (!auth.done) {
                window.location.hash = '#/';
                return;
            }
            carregarDados(url);
        }

        function carregarDados(url) {
            if (!api.on()) {
                
                return;
            }

            $http({
                method: 'GET',
                url: api.url() + url,
                headers: { 'Authorization': 'Bearer ' + auth.token }
            }).success(function (data) {
                vm.dados = data;
            }).error(function (data) {
                console.log(data);
                $ionicPopup.alert({ title: 'Ops!', template: data[0].errorMessage });
            });
        }
    }
})();