namespace Gestão_financeiro
{
    using Microsoft.Extensions.Configuration;

    class Program
    {

        //***************************DECLARAÇÃO DA FUNÇÃO PRINCIPAL DO PROGRAMA***************************
        static async Task Main(string[] args)
        {
            // //***************************Carrega a configuração a partir do arquivo appsettings.json//***************************
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string accessToken = configuration["Asaas:AccessToken"];


            if (string.IsNullOrEmpty(accessToken))
            {
                Console.WriteLine("Access Token não configurado. Encerrando o programa.");
                Environment.Exit(1);
            }
            else
            {
                Console.WriteLine("Access Token configurado com sucesso.");
            }
            //***************************Variavel de Controle de loop***************************
            bool continuar = true;

            //***************************Instância da classe ClienteService***************************
            var clienteService = new ClienteService(accessToken);
            var cobrancaService = new CobrancaService(accessToken);

            while (continuar)
            {
                if (!Console.IsOutputRedirected)
                {
                    Console.Clear();
                }
                Console.WriteLine("----- MENU DE GESTÃO FINANCEIRA -----");
                Console.WriteLine("1. Gerenciamento de Clientes");
                Console.WriteLine("2. Gerenciamento de Cobranças");
                Console.WriteLine("3. Sair");
                Console.Write("Escolha uma opção: ");
                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1"://Chama a função que organiza o menu de clientes e passa a instância do clienteService
                        await ClientesMenu(clienteService);
                        break;
                    case "2"://Chama a função que  é responsavel pela exibição do menu de cobranças com instância da classe
                        await CobrancasMenu(cobrancaService);
                        break;
                    case "3":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
        }

        //***************************Declaração da função de menu de clientes***************************
        internal static async Task ClientesMenu(ClienteService clienteService)
        {
            bool continuar = true;

            Console.Clear();
            while (continuar)
            {
                Console.WriteLine("----- Menu de Clientes -----");
                Console.WriteLine("1. Cadastrar novo Cliente");
                Console.WriteLine("2. Excluir Cliente");
                Console.WriteLine("3. Lista Clientes");
                Console.WriteLine("4. Atualizar Cliente");
                Console.WriteLine("5. Voltar ao menu anterior");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1"://Chamada do método para cadastrar um novo cliente
                        await clienteService.CriarClienteAsync();
                        break;
                    case "2"://Chamada do método para Excluir um Cliente
                        await clienteService.ExcluirClienteAsync();
                        break;
                    case "3"://Chamada do método para Listar os clientes
                        await clienteService.ListarClientesAsync();
                        break;
                    case "4"://Chamada do método para atualizar um Cliente
                        await clienteService.AtualizarClientesAsync();
                        break;
                    case "5"://Retorna ao menu Anterior
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
        }

        //***************************Declaração da função de menu de cobranças***************************
        internal static async Task CobrancasMenu(CobrancaService cobrancaService)
        {
            //Variavel de controle de loop
            bool continuar = true;

            //Loop condicional para exibição de Menu
            while (continuar)
            {
                Console.Clear();
                Console.WriteLine("----- Menu de Cobranças -----");
                Console.WriteLine("1. Criar uma cobrança");
                Console.WriteLine("2. Excluir uma cobrança");
                Console.WriteLine("3. Listar uma unica cobrança");
                Console.WriteLine("4. Listar todas cobranças");
                Console.WriteLine("5. Sair");
                Console.Write("Escolha uma opção: ");
                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1"://Função para criar uma cobrança nova
                        await cobrancaService.Criar_Cobranca();
                        break;
                    case "2"://Função para Excluir uma cobrança
                        await cobrancaService.Excluir_Cobranca();
                        break;
                    case "3"://Função para listar uma unica cobrança
                        await cobrancaService.Listagem_Unica_Cobrancas();
                        break;
                    case "4"://Função para trazer uma lista de cobranças
                        await cobrancaService.Listagem_Todas_Cobrancas();
                        break;
                    case "5"://Retorna ao menu Anterior
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }

        }


    }
}

