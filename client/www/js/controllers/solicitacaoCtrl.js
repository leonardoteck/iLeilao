(function() {
'use strict';

    angular
        .module('starter.controllers')
        .controller('solicitacaoController', solicitacaoController);

    solicitacaoController.$inject = ['$scope', '$http', 'api', 'auth', '$ionicPopup', '$state', '$timeout'];
    function solicitacaoController($scope, $http, api, auth, $ionicPopup, $state, $timeout) {
        var vm = this;

        vm.dados = {
            lote: {
                produtos: [
                    {}
                ]
            }
        };

        vm.salvarDados = salvarDados;
        vm.addProduto = addProduto;
        vm.removeProduto = removeProduto;
        vm.base64 = base64;

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
                vm.dados = {
                    "id": 1,
                    "tempoLimiteLance": "03:00:25",
                    "loteId": 1,
                    "diasDuracao": 20,
                    "incrementoMinimo": 30,
                    "status": 0,
                    "usuarioId": "1ff83f28-8d42-4fb0-a976-c13344d70917",
                    "lote": {
                        "id": 1,
                        "produtos": [
                            {
                                "id": 1,
                                "nome": "Moto G",
                                "descricao": "2nd Generation",
                                "quantidade": 4,
                                "imagem": "http://www.comprasparaguai.com.br/media/fotos/modelos/celular_motorola_moto_g_xt_1068_dual_chip_8gb_26740_550x550.png",
                                "loteId": 1
                            }
                        ],
                        "valorMinimo": 30,
                        "vendedorId": "1ff83f28-8d42-4fb0-a976-c13344d70917",
                        "vendedor": null
                    },
                    "usuario": null
                }
                formataDados(vm.dados);
                return;
            }

            if ($state.params.id)
                $http({
                    method: 'GET',
                    url: api.url() + 'solicitacoes/' + $state.params.id,
                    headers: { 'Authorization': 'Bearer ' + auth.token }
                }).success(function (data) {
                    vm.dados = data;
                    formataDados(data);
                }).error(function (data) {
                    console.log(data);
                    $ionicPopup.alert({
                        title: 'Ops!',
                        template: data[0].errorMessage
                    });
                });
        }

        function formataDados(data) {
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

        function base64(entidade, elemento) {
            var reader = new FileReader();
            reader.onloadend = function() {
                $scope.$eval(entidade + '=' + '"' + reader.result + '"')
                // entidade.imagem = reader.result;
                console.log(reader.result);
                $timeout(function () {});
            }
            reader.readAsDataURL(elemento.files[0]);
        }
    }
})();