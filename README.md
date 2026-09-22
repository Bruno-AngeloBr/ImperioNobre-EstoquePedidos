# Império Nobre - Estoque e Pedidos

Sistema web em ASP.NET Core para gerenciamento de estoque e pedidos de venda da confeitaria Império Nobre. Integração com Google Sheets para controle de produtos, quantidades disponíveis e criação de pedidos de venda.

Meu objetivo nesse projeto é desenvolver um sistema de gerenciamento de pedidos eficiente para minha amiga que trabalha vendendo doces, ela utiliza uma planilha do Google Sheets para gerenciar suas vendas então quero integrar esse sistema à sua planilha para facilitar seu trabalho.

Dessa forma permitindo um registro de vendas mais simples e dinâmico na sua planilha além de uma melhor visualização e controle de seu negócio.

Projeto ainda em desenvolvimento.

---

## 🚀 Funcionalidades
- [Controle de estoque](ca://s?q=Controle_de_estoque_em_sistema_web) integrado com Google Sheets.
- [Cadastro de produtos](ca://s?q=Cadastro_de_produtos_em_ASP.NET_Core).
- [Pedidos de venda](ca://s?q=Sistema_de_pedidos_de_venda) com múltiplos itens e quantidades diferentes.
- Interface responsiva com [Bootstrap](ca://s?q=Bootstrap_tabelas).

---

## 🛠️ Tecnologias utilizadas
- ASP.NET Core MVC
- Google Sheets API
- Entity Framework Core
- Bootstrap 5

---

## 📦 Instalação
1. Clone o repositório: git clone https://github.com/Bruno-AngeloBr/ImperioNobre-EstoquePedidos.git
   
2. Entre na pasta do projeto: cd ImperioNobre-EstoquePedidos

3. Configure as credenciais da Google Sheets API no appsettings.json.

4. Execute o projeto: dotnet run

📊 Estrutura da planilha
Produtos: contém ID, Nome, Sabor, Tamanho, Preço Unitário e Quantidade disponível.

Pedidos: armazena dados gerais dos pedidos.

ItensPedido: vincula cada pedido aos produtos vendidos.

