namespace Gestão_financeiro
{
    using System.Net.Http.Headers;
    using Newtonsoft.Json;

    public class ClienteService
    {
        private readonly string apiUrl = "https://sandbox.asaas.com/api/v3/customers";
        private readonly string user_agent = "Loja Filipe/v1.0";
        private readonly string accessToken;

        // Construtor que recebe o accessToken da configuração
        public ClienteService(string accessToken)
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

        //***************************Método para Criar um Cliente***************************
        public async Task CriarClienteAsync()
        {
            using (var client = new HttpClient())
            {
                // Solicitando informações ao usuário
                Console.Write("Digite o nome do cliente: ");
                string nome = Console.ReadLine();

                Console.Write("Digite o CPF/CNPJ do cliente: ");
                string documento = Console.ReadLine();

                Console.Write("Digite o email do cliente: ");
                string Email = Console.ReadLine();

                Console.Write("Digite o telefone móvel do cliente: ");
                string telefone = Console.ReadLine();

                Console.Write("Digite o endereço do cliente: ");
                string endereco = Console.ReadLine();

                Console.Write("Digite o número do endereço: ");
                string numero_endereco = Console.ReadLine();

                Console.Write("Digite o complemento do endereço (ou pressione Enter se não houver): ");
                string complemento = Console.ReadLine();

                Console.Write("Digite a província/bairro do cliente: ");
                string estado = Console.ReadLine();

                Console.Write("Digite o código postal (CEP) do cliente: ");
                string CEP = Console.ReadLine();

                // Criação dinâmica do objeto clienteJson com base na entrada do usuário
                var clienteJson = new
                {
                    name = nome,
                    cpfCnpj = documento,
                    email = Email,
                    mobilePhone = telefone,
                    address = endereco,
                    addressNumber = numero_endereco,
                    complement = complemento,
                    province = estado,
                    postalCode = CEP
                };
                // Convertendo o objeto clienteJson em uma string JSON
                var jsonContent = JsonConvert.SerializeObject(clienteJson);

                // Criando o StringContent com a string JSON e o cabeçalho Content-Type
                var clienteContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                clienteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Configurando a requisição
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(apiUrl),
                    Headers =
                    {
                        { "accept", "application/json" },
                        { "access_token", accessToken},
                        { "User-Agent", user_agent}
                    },
                    Content = clienteContent
                };

                // Enviando a requisição
                if (await Resposta_api(client, request))
                {
                    Console.WriteLine($"O cliente foi cadastrado com sucesso!!\nDeseja criar outro cliente?");
                    string resposta_usuario = Console.ReadLine().ToUpper();
                    if (resposta_usuario == "SIM")
                    {
                        await CriarClienteAsync();
                    }
                    else
                    {
                        Console.WriteLine("Retornando ao menu anterior!!!!!");
                        await Program.CobrancasMenu(new CobrancaService(accessToken));
                    }
                }
                else
                {
                    Console.WriteLine($"Aconteceu um erro ao cadastrar o cliente!!\nRetornando ao menu anterior!!");
                    await Program.ClientesMenu(new ClienteService(accessToken));
                }
            }
        }

        //***************************Método para Excluir um Cliente***************************
        public async Task ExcluirClienteAsync()
        {

            //Recebendo o ID do cliente que deseja excluir
            Console.Write("Digite o ID do cliente que deseja excluir: ");
            string id_cliente = Console.ReadLine();


            //Url para usar no Request
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{apiUrl}/{id_cliente}"),
                Headers =
            {
                { "accept", "application/json" },
                { "access_token", accessToken},
                { "User-Agent", user_agent }
            },
            };
            // Enviando a requisição
            if (await Resposta_api(client, request))
            {
                Console.WriteLine($"O cliente com id {id_cliente} foi excluido com sucesso!!\nDeseja excluir outro cliente?");
                string resposta_usuario = Console.ReadLine().ToUpper();
                if (resposta_usuario == "SIM")
                {
                    await ExcluirClienteAsync();
                }
                else
                {
                    Console.WriteLine("Retornando ao menu anterior!!!!!");
                    await Program.CobrancasMenu(new CobrancaService(accessToken));
                }
            }
            else
            {
                Console.WriteLine($"Aconteceu um erro ao cadastrar o cliente!!\nRetornando ao menu anterior!!");
                await Program.ClientesMenu(new ClienteService(accessToken));
            }

        }

        //***************************Método para Listar os Clientes***************************
        public async Task ListarClientesAsync()
        {
            Console.WriteLine("Digite o tipo de filtragem que deseja realizar:");
            string opcao = Console.ReadLine();

            if (opcao == "nome")
            {
                //Configuração da conexão com escolha do nome
                Console.WriteLine("Digite o nome que deseja usar como filtro!!!");
                string cliente_nome = Console.ReadLine();
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(apiUrl),
                    Headers =
                    {
                    { "accept", "application/json" },
                    { "access_token", accessToken},
                    { "User-Agent", "Loja do Filipe/v1.0" }
                    },
                };

                //Recebendo retorno da API
                if (await Resposta_api(client, request))
                {
                    Console.WriteLine($"A lista foi gerada com sucesso!!\nDeseja criar outra lista de clientes?");
                    string resposta_usuario = Console.ReadLine().ToUpper();
                    if (resposta_usuario == "SIM")
                    {
                        await ListarClientesAsync();
                    }
                    else
                    {
                        Console.WriteLine("Retornando ao menu anterior!!!!!");
                        await Program.CobrancasMenu(new CobrancaService(accessToken));
                    }
                }
                else
                {
                    Console.WriteLine($"Aconteceu um erro ao cadastrar o cliente!!\nRetornando ao menu anterior!!");
                    await Program.ClientesMenu(new ClienteService(accessToken));
                }

            }
            else if (opcao == "cpf" || opcao == "cnpj")
            {
                //Configração da conexão com escolha de cpf
                Console.WriteLine("Digite o CPF que deseja consultar: ");
                string cliente_cpf_cnpj = Console.ReadLine();
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{apiUrl}?cpfCnpj={cliente_cpf_cnpj}"),
                    Headers =
                    {
                    { "accept", "application/json" },
                    { "access_token",accessToken},
                    { "User-Agent", "Loja do Filipe/v1.0" }
                    }
                };
                if (await Resposta_api(client, request))
                {
                    Console.WriteLine($"A lista foi gerada com sucesso!!\nDeseja criar outra lista de clientes?");
                    string resposta_usuario = Console.ReadLine().ToUpper();
                    if (resposta_usuario == "SIM")
                    {
                        await ListarClientesAsync();
                    }
                    else
                    {
                        Console.WriteLine("Retornando ao menu anterior!!!!!");
                        await Program.CobrancasMenu(new CobrancaService(accessToken));
                    }

                }
                else
                {
                    Console.WriteLine($"Aconteceu um erro ao cadastrar o cliente!!\nRetornando ao menu anterior!!");
                    await Program.ClientesMenu(new ClienteService(accessToken));
                }
            }
            else
            {
                Console.WriteLine("Digite o tipo de filtragem que deseja realizar:");
                await ListarClientesAsync();
            }

        }

        //***************************Método para Atualizar um Cliente existente***************************
        public async Task AtualizarClientesAsync()
        {

            System.Console.WriteLine("Digite o id do cliente que deseja atualizar:");
            string id_cliente = Console.ReadLine();
            Console.WriteLine("Digite o novo nome do cliente:");

            using (var client = new HttpClient())
            {
                // Solicitando informações ao usuário
                Console.Write("Digite o nome do cliente: ");
                string nome = Console.ReadLine();
                Console.Write("Digite o CPF/CNPJ do cliente: ");
                string documento = Console.ReadLine();
                Console.Write("Digite o email do cliente (ou pressione Enter se não houver): ");
                string Email = Console.ReadLine();
                Console.Write("Digite o telefone móvel do cliente (ou pressione Enter se não houver): ");
                string telefone = Console.ReadLine();
                Console.Write("Digite o endereço do cliente (ou pressione Enter se não houver): ");
                string endereco = Console.ReadLine();
                Console.Write("Digite o número do endereço (ou pressione Enter se não houver): ");
                string numero_endereco = Console.ReadLine();
                Console.Write("Digite o complemento do endereço (ou pressione Enter se não houver): ");
                string complemento = Console.ReadLine();
                Console.Write("Digite a província/bairro do cliente (ou pressione Enter se não houver): ");
                string estado = Console.ReadLine();
                Console.Write("Digite o código postal (CEP) do cliente (ou pressione Enter se não houver): ");
                string CEP = Console.ReadLine();

                // Criação dinâmica do objeto clienteJson com base na entrada do usuário
                var clienteJson = new
                {
                    name = nome,
                    cpfCnpj = documento,
                    email = Email,
                    mobilePhone = telefone,
                    address = endereco,
                    addressNumber = numero_endereco,
                    complement = complemento,
                    province = estado,
                    postalCode = CEP
                };
                // Convertendo o objeto clienteJson em uma string JSON
                var jsonContent = JsonConvert.SerializeObject(clienteJson);

                // Criando o StringContent com a string JSON e o cabeçalho Content-Type
                var clienteContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                clienteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"{apiUrl}/{id_cliente}"),
                    Headers =
                    {
                        { "accept", "application/json" },
                        { "access_token",accessToken},
                        { "User-Agent", user_agent },
                    },
                    Content = clienteContent
                };

                //Resposta da requisição 
                if (await Resposta_api(client, request))
                {
                    Console.WriteLine($"O cliente: {id_cliente} foi atualizado com sucesso!!\nDeseja atualizar outro cliente?");
                    string resposta_usuario = Console.ReadLine().ToUpper();
                    if (resposta_usuario == "SIM")
                    {
                        await AtualizarClientesAsync();
                    }
                    else
                    {
                        Console.WriteLine("Retornando ao menu anterior!!!!!");
                        await Program.CobrancasMenu(new CobrancaService(accessToken));
                    }
                }
                else
                {
                    Console.WriteLine($"Aconteceu um erro ao Atualizar o cliente: {id_cliente}!!!\nRetornando ao menu anterior!!");
                    await Program.ClientesMenu(new ClienteService(accessToken));
                }
            }
        }

    }
}