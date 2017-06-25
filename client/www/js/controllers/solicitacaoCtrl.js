(function() {
'use strict';

    angular
        .module('starter.controllers')
        .controller('listaController', listaController);

    listaController.$inject = ['$scope', '$http', 'api', 'auth', '$ionicPopup', '$state'];
    function listaController($scope, $http, api, auth, $ionicPopup, $state) {
        var vm = this;

        vm.dados = {
            lote: {
                produtos: [
                    {}
                ]
            }
        };
        vm.tipoUsuario = 0;

        vm.cadastrar = cadastrar;

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

                return;
            }

            if ($state.params.id)
                $http({
                    method: 'GET',
                    url: api.url() + 'solicitacoes/' + $state.params.id,
                    headers: { 'Authorization': 'Bearer ' + auth.token }
                }).success(function (data) {
                    vm.dados = data;
                    if (data.lote.produtos.length == 0)
                        data.lote.produtos.push({});

                    var tempo = data.tempoLimiteLance.split(':');
                    var tempo2 = tempo[0].split('.');
                    if (tempo2.length == 2) {
                        data.tempoLimiteDias = tempo2[0];
                        data.tempoLimiteHoras = tempo2[1];
                    } else
                        data.tempoLimiteHoras = tempo[0];
                    data.tempoLimiteMinutos = tempo[1];
                    data.tempoLimiteSegundos = tempo[2];
                }).error(function (data) {
                    console.log(data);
                    $ionicPopup.alert({
                        title: 'Ops!',
                        template: data[0].errorMessage
                    });
                });
        }

        function addProduto() {
            vm.dados.lote.produtos.push({});
        }

        function removeProduto(index) {
            vm.dados.produtos.splice(index, 1);
        }

        function salvarDados() {
            var dados = JSON.parse(JSON.stringify(vm.dados));

            // Validação
                var dataAtual = new Date();
                if (dataAtual.valueOf() > vm.dados.DataFinal.valueOf()) {
                    $ionicPopup.alert({ title: 'Atenção!', template: 'A data final deve ser após a data atual' });
                }
            // Formatando alguns dados para a API entender
                dados.tempoLimiteLance = vm.dados.tempoLimiteDias + '.' +
                    vm.dados.tempoLimiteHoras + ':' +
                    vm.dados.tempoLimiteMinutos + ':' +
                    vm.dados.tempoLimiteSegundos;
                delete dados.tempoLimiteDias;
                delete dados.tempoLimiteHoras;
                delete dados.tempoLimiteMinutos;
                delete dados.tempoLimiteSegundos;

                dados.UsuarioId = auth.id;
                dados.status = 0

                for (var i = 0; i < dados.lote.produtos.length; i++) {
                    delete dados.lote.produtos[i].imagem;
                    if (!dados.lote.produtos[i].nome) {
                        dados.lote.produtos.splice(i, 1);
                        i--;
                    }
                }

            var request = {
                method: 'POST',
                data: dados,
                url: api.url() + 'Solicitacoes',
                headers: { 'Authorization': 'Bearer ' + auth.token }
            };
            var msg = 'Solicitação cadastrada com sucesso!';

            if (vm.dados.Id) {
                request.method = 'PUT';
                request.url += vm.dados.Id;
                var msg = 'Solicitação alterada com sucesso!';
            }

            $http(request)
            .success(function (data) {
                $ionicPopup.alert({ title: 'Sucesso!', template: msg });
                console.log(data);
            })
            .error(function (data) {
                console.log(data);
                $ionicPopup.alert({ title: 'Ops!', template: data[0].errorMessage });
            });
        }
    }
})();