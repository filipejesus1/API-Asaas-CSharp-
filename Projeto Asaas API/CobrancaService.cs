namespace Gestão_financeiro
{
    using Newtonsoft.Json;

    public class CobrancaService
    {
        private readonly string apiUrl = "https://sandbox.asaas.com/api/v3/payments";
        private readonly string user_agent = "Loja Filipe/v1.0";
        private readonly string accessToken;

        public CobrancaService(string accessToken)
        {
            this.accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        }
        //***************************Método para Requisitar resposta da API***************************
        public async Task<bool> Resposta_api(HttpClient cliente, HttpRequestMessage resposta)
        {
            using (var resposta_api = await cliente.SendAsync(resposta))
            {
                var detalhes = await resposta_api.Content.ReadAsByteArrayAsync();
                if (!resposta_api.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro: {resposta_api.StatusCode}");
                    Console.WriteLine($"Detalhes: {detalhes}");
                    return false;
                }
                else
                {
                    Console.WriteLine(detalhes);
                    return true;
                }
            }
        }

        //***************************Método para Criação de uma cobrança***************************
        public async Task Criar_Cobranca()
        {
            // Solicitando os dados do usuário
            Console.WriteLine("Digite o ID do cliente: ");
            string id_cliente = Console.ReadLine();

            Console.WriteLine("Digite o método de pagamento: BOLETO/PIX:");
            string metodo_pagamento = Console.ReadLine();

            Console.WriteLine("Digite o valor da cobrança: ");
            string valor = Console.ReadLine();

            Console.WriteLine("Digite a data de vencimento: ");
            string data = Console.ReadLine();

            //Objeto em formato Json
            var cobracaJson = new
            {
                customer = id_cliente,
                billingType = metodo_pagamento,
                value = valor,
                dueDate = data
            };
            // Convertendo o objeto clienteJson em uma string JSON
            var jsonContent = JsonConvert.SerializeObject(cobracaJson);

            // Criando o StringContent com a string JSON e o cabeçalho Content-Type
            var clienteContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var client = new HttpClient(); ;
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{apiUrl}"),
                Headers =
                {
                    {"accept","application/Json"},
                    {"acess_token", accessToken},
                    { "User-Agent", user_agent}
                },
                Content = clienteContent
            };

            if (await Resposta_api(client, request))
            {
                Console.WriteLine($"A cobrança foi criada com sucesso!!\nDeseja criar outra cobrança?");
                string resposta_usuario = Console.ReadLine().ToUpper();
                if (resposta_usuario == "SIM")
                {
                    await Criar_Cobranca();
                }
                else
                {
                    Console.WriteLine("Retornando ao menu anterior!!!!!");
                    await Program.CobrancasMenu(new CobrancaService(accessToken));
                }
            }
            else
            {
                Console.WriteLine($"Aconteceu um erro ao criar uma cobrança!!\nRetornando ao menu anterior!!");
                await Program.CobrancasMenu(new CobrancaService(accessToken));
            }
        }

        //***************************Método para Exclusão de uma cobrança***************************
        public async Task Excluir_Cobranca()
        {
            Console.WriteLine("Digite o ID da Cobrança que deseja Excluir: ");
            string id_cobranca = Console.ReadLine();
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(apiUrl + $"/{id_cobranca}"),
                Headers =
                {
                    {"accept","application/json"},
                    {"access_token", accessToken},
                    {"User-Agent", user_agent}
                },
            };

            if (await Resposta_api(client, request))
            {
                Console.WriteLine($"A cobrança: {id_cobranca} foi excluida com sucesso!!\nDeseja excluir outra cobrança?");
                string resposta_usuario = Console.ReadLine().ToUpper();
                if (resposta_usuario == "SIM")
                {
                    await Excluir_Cobranca();
                }
                else
                {
                    Console.WriteLine("Retornando ao menu anterior!!!!!");
                    await Program.CobrancasMenu(new CobrancaService(accessToken));
                }
            }
            else
            {
                Console.WriteLine($"Aconteceu um erro ao excluir a cobrança: {id_cobranca}!!\nRetornando ao menu anterior!!");
                await Program.CobrancasMenu(new CobrancaService(accessToken));
            }
        }

        //***************************Método para Listagem de Cobrança***************************
        public async Task Listagem_Unica_Cobrancas()
        {
            Console.WriteLine("Digite o ID da Cobrança que deseja consultar: ");
            string id_cobranca = Console.ReadLine();
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://sandbox.asaas.com/api/v3/payments?externalReference={id_cobranca}"),
                Headers =
            {
                { "accept", "application/json" },
                { "access_token","$aact_YTU5YTE0M2M2N2I4MTliNzk0YTI5N2U5MzdjNWZmNDQ6OjAwMDAwMDAwMDAwMDAwNDA5OTE6OiRhYWNoXzBmYzNkODhlLTI2ZGMtNDk1OC04NTJjLWEyZTBjYjQxYzg2YQ==" },
                { "User-Agent", user_agent}
            },
            };

            if (await Resposta_api(client, request))
            {
                Console.WriteLine($"A cobrança: {id_cobranca} foi listada com sucesso!!\nDeseja consultar outra cobrança?");
                string resposta_usuario = Console.ReadLine().ToUpper();
                if (resposta_usuario == "SIM")
                {
                    await Listagem_Unica_Cobrancas();
                }
                else
                {
                    Console.WriteLine("Retornando ao menu anterior!!!!!");
                    await Program.CobrancasMenu(new CobrancaService(accessToken));
                }
            }
            else
            {
                Console.WriteLine($"Aconteceu um erro ao consultar a cobrança:{id_cobranca}, voltando ao menu anterior\n");
                await Program.CobrancasMenu(new CobrancaService(accessToken));
            }
        }

        //***************************Método para Listagem por filtros***************************
        public async Task Listagem_Todas_Cobrancas()
        {
            Console.WriteLine("Digite o cliente que deseja filtrar: ");
            string customer = Console.ReadLine();

            Console.WriteLine("Digite o método de pagamento: BOLETO/PIX:");
            string billingType = Console.ReadLine();

            Console.WriteLine("Digite o valor da cobrança: ");
            string valor = Console.ReadLine();

            Console.WriteLine("Digite a data de pagamento: ");
            string paymentDate = Console.ReadLine();

            Console.WriteLine("Digite o status da cobrança: ");
            string status = Console.ReadLine();

            Console.WriteLine("Digite o e-mail cadastrado: ");
            string user = Console.ReadLine();

            Console.WriteLine("Digite o Elemento inicial da lista: ");
            int offset = int.Parse(Console.ReadLine());

            Console.WriteLine("Digite o número de elementos da lista: ");
            int limit = int.Parse(Console.ReadLine());

            //Montando o URLBASE
            var Pesquisa_Parametros = new List<String>();

            if (!string.IsNullOrEmpty(customer))
                Pesquisa_Parametros.Add($"customer={customer}");

            if (!string.IsNullOrEmpty(billingType))
                Pesquisa_Parametros.Add($"billingType={billingType}");

            if (!string.IsNullOrEmpty(status))
                Pesquisa_Parametros.Add($"status={status}");

            if (!string.IsNullOrEmpty(paymentDate))
                Pesquisa_Parametros.Add($"paymentDate={paymentDate}");

            if (!string.IsNullOrEmpty(user))
                Pesquisa_Parametros.Add($"user={user}");

            Pesquisa_Parametros.Add($"offset={offset}");

            Pesquisa_Parametros.Add($"limit={limit}");

            //Criando a url final com base nas opções
            string Pesquisa_String = string.Join("&", Pesquisa_Parametros);
            string url = apiUrl + $"?{Pesquisa_String}";

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
            {
                {"accept","aaplication/json"},
                {"access_token",accessToken},
                {"User-Agent",user_agent}
            }
            };

            if (await Resposta_api(client, request))
            {
                Console.WriteLine("A lista foi gerada com sucesso!!\nDeseja consultar outros parametros?");
                string resposta_usuario = Console.ReadLine().ToUpper();
                if (resposta_usuario == "SIM")
                {
                    await Listagem_Todas_Cobrancas();
                }
                else
                {
                    Console.WriteLine("Retornando ao menu anterior!!!!!");
                    await Program.CobrancasMenu(new CobrancaService(accessToken));
                }
            }
            else
            {
                Console.WriteLine($"Aconteceu um erro ao consultar, retornando ao menu anterior\n");
                await Program.CobrancasMenu(new CobrancaService(accessToken));
            }
        }

    }
}

