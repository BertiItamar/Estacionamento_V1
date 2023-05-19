# Estacionamento

# Descrição 

O projeto tem como objetivo desenvolver um aplicativo simples de controle de estacionamento, onde os usuários poderão registrar a entrada e saída de veículos. O sistema permitirá a configuração dos valores praticados pelo estacionamento por meio de uma tabela de preços com controle de vigência. Por exemplo, será possível definir valores válidos para um determinado período.
O objetivo final é fornecer aos usuários uma solução intuitiva e eficiente para o controle de estacionamento, permitindo um gerenciamento adequado das entradas, saídas e valores praticados.

# Pré-requisitos

Para executar o projeto, é necessário atender aos seguintes pré-requisitos:
.NET 6: O projeto é desenvolvido em .NET 6. Portanto, certifique-se de ter o SDK do .NET 6 instalado em sua máquina. Você pode fazer o download do SDK mais recente em: https://dotnet.microsoft.com/download/dotnet/6.0
SQL Server Local: O projeto utiliza o banco de dados SQL Server localmente. Certifique-se de ter o SQL Server instalado e configurado em sua máquina. Você pode fazer o download da versão Express gratuita do SQL Server em: https://www.microsoft.com/pt-br/sql-server/sql-server-downloads
Node.js 18.6: Para o frontend do projeto, é utilizado o React com a versão do Node.js 18.6. Certifique-se de ter o Node.js instalado em sua máquina. Você pode fazer o download da versão mais recente do Node.js em: https://nodejs.org
Dependências do projeto: Após ter o Node.js instalado, navegue até a pasta do projeto frontend em React e execute o comando npm install para instalar todas as dependências necessárias.

# Estrutura do Projeto 

A estrutura do projeto segue o padrão MVC (Model-View-Controller) no backend, com uma organização de diretórios que segue as boas práticas de desenvolvimento e separação de responsabilidades. A seguir, fornecerei uma visão geral das principais pastas e arquivos do projeto:

Controllers: Esta pasta contém os controladores do padrão MVC, responsáveis por receber as requisições HTTP, processá-las e retornar as respostas adequadas. Cada controlador é responsável por um conjunto de ações relacionadas a um determinado contexto do aplicativo.

Models: A pasta Models abriga as classes que representam as entidades do domínio do projeto. Essas classes podem conter propriedades, métodos e validações relacionadas a essas entidades.

Services: Nesta pasta estão os serviços do projeto, que contêm a lógica de negócio. Cada serviço encapsula um conjunto de operações relacionadas a uma determinada funcionalidade. Esses serviços implementam interfaces que definem os contratos das operações disponíveis.

Infrastructure: O diretório Infrastructure contém a infraestrutura do projeto, incluindo classes para tratamento de erros, gerenciamento de exceções e outras funcionalidades relacionadas à infraestrutura do sistema.

Data: Esta pasta abriga as classes relacionadas ao acesso e manipulação de dados do projeto. Aqui, são definidos os contextos do banco de dados usando o Entity Framework, bem como as configurações de mapeamento de entidades e as migrações do banco de dados.
Essa é apenas uma visão geral da estrutura do projeto, e podem haver outros diretórios e arquivos dependendo das necessidades específicas do projeto. No entanto, a estrutura descrita acima segue os princípios do padrão MVC e uma boa organização do código-fonte.
