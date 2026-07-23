# 🎮 CatalogAPI — FCG FIAP Cloud Games

Microsserviço responsável pelo **catálogo de jogos e biblioteca do usuário** da plataforma FIAP Cloud Games. Gerencia o CRUD de jogos e inicia o fluxo de compra via eventos RabbitMQ.

---



## 🧱 Tecnologias

- .NET 9
- SQL Server (dados relacionais)
- MongoDB (avaliações de jogos + logs via Serilog)
- Elasticsearch (busca avançada — fuzzy search e relevância)
- RabbitMQ (publicação e consumo de eventos)
- JWT Bearer Authentication

---

## 📡 Endpoints

### 🎮 Games — `/api/Game`

| Método | Rota | Descrição | Auth |
|---|---|---|---|
| `POST` | `/api/Game/Cadastrar/` | Cadastra novo jogo | ✅ usuario |
| `GET` | `/api/Game/BuscarPorId/{id}` | Busca jogo por ID | ✅ usuario |
| `GET` | `/api/Game/ListarGames` | Lista jogos paginado | ✅ usuario |
| `PUT` | `/api/Game/AtualizarGame/{id}` | Atualiza jogo | ✅ administrador |

**GET /ListarGames**
Lista jogos com paginação.

```json
// Response 200
{
  "items": [...],
  "totalItems": 0,
  "pagina": 1,
  "tamanhoPagina": 10
}
```

---

### 📚 Biblioteca — `/api/usuarios/{usuarioId}/biblioteca`

| Método | Rota | Descrição | Auth |
|---|---|---|---|
| `GET` | `/BuscarPorUsuarioId/` | Lista biblioteca do usuário | ✅ usuario |
| `POST` | `/Comprar/` | Inicia compra de um jogo | ✅ usuario |
| `PUT` | `/Atualizar/{id}` | Atualiza item da biblioteca | ✅ usuario |
| `DELETE` | `/Deletar/{id}` | Remove jogo da biblioteca | ✅ usuario |

> ⚠️ **Atenção:** ao revisar este README, encontramos duas divergências entre a documentação anterior e o comportamento real do código: `POST /Game/Cadastrar/` exige apenas a role `usuario` (não `administrador`), e `POST /biblioteca/Comprar/` exige autenticação (não é mais público). Este README já reflete o código atual — mas vale o time confirmar se esse é o comportamento pretendido ou se é um bug a corrigir.

**POST /Comprar/**
Inicia o fluxo de compra de um jogo. Publica um evento `OrderPlacedEvent` no RabbitMQ para a PaymentsAPI processar.

```json
// Request
{
  "gameId": "guid"
}

// Response 201 - Compra iniciada com sucesso
// Response 400 - Jogo não encontrado ou já adquirido
```

---

### ⭐ Avaliações — `/api/Game/{gameId}/ratings`

| Método | Rota | Descrição | Auth |
|---|---|---|---|
| `POST` | `/` | Registra avaliação de um jogo | ✅ usuario |
| `GET` | `/` | Lista avaliações de um jogo | ✅ usuario |

> 🗃️ Persistido em **MongoDB** (não em SQL Server) — implementação da persistência poliglota NoSQL da Fase 4, usando o cenário de "sistema de avaliações" citado no enunciado.

---

### 🔍 Busca Avançada — `/api/Search`

| Método | Rota | Descrição | Auth |
|---|---|---|---|
| `GET` | `/api/Search?termo={termo}&take={take}` | Busca jogos por termo | ✅ usuario |

Implementada via **Elasticsearch**, com:
- **Fuzzy Search** — tolerância a erros de digitação
- **Ordenação por relevância** (score)

O parâmetro `take` é opcional (controla o número de resultados retornados).

**Sincronização:** sempre que um jogo é cadastrado ou atualizado no banco principal (`POST /Cadastrar/`, `PUT /AtualizarGame/{id}`), o índice no Elasticsearch é atualizado automaticamente.

---

## 📨 Eventos

### Publicados

| Exchange | Tipo | Quando |
|---|---|---|
| `order-placed-exchange` | Fanout | Ao iniciar uma compra |

### Consumidos

| Exchange | Tipo | Ação |
|---|---|---|
| `payment-processed-exchange` | Fanout | Libera o jogo na biblioteca quando pagamento aprovado |

---

## 🔐 Autenticação

Esta API utiliza **JWT Bearer Token** emitido pela **UsersAPI**.

1. Faça login na UsersAPI via `POST /api/Authentication/login/`
2. Copie o token retornado
3. No Swagger, clique em **Authorize** 🔒 e insira `Bearer <token>`

---

## 🗃️ Banco de Dados

| Configuração | Valor |
|---|---|
| Connection String | `MS_CatalogAPI` |
| Database | `MS_CatalogAPI` |

---

## 🐳 Rodando Localmente (Docker Compose)

Este serviço faz parte da orquestração central. Para rodar o ambiente completo:

```bash
# Clone todos os repositórios na mesma pasta pai
git clone https://github.com/pablosdlima/OrchestrationApi
git clone https://github.com/marciotorquato/CatalogAPI

# Suba o ambiente
cd OrchestrationAPI
docker compose up --build
```

Swagger disponível em: `http://localhost:5002/swagger`

> ℹ️ O ambiente completo inclui SQL Server, MongoDB, RabbitMQ e os 4 microsserviços. Consulte o repositório [OrchestrationAPI](https://github.com/pablosdlima/OrchestrationApi) para mais detalhes.

---

## ☸️ Rodando com Kubernetes

### Pré-requisitos

- Docker Desktop com **Kubernetes habilitado**
- `kubectl` disponível no terminal
- Infraestrutura já aplicada via OrchestrationAPI

### Estrutura dos manifestos

```
CatalogAPI/
└── k8s/
    ├── configmap.yaml   ← variáveis não sensíveis (inclui Elasticsearch__Uri)
    ├── secret.yaml      ← variáveis sensíveis (Base64) — repositório deve conter apenas placeholders
    ├── deployment.yaml  ← gerencia os Pods
    └── service.yaml     ← expõe o serviço internamente (ClusterIP, acesso externo via Kong)
```

> 🔒 As credenciais reais (SQL Server, RabbitMQ, JWT, MongoDB) nunca devem ser commitadas no `secret.yaml`. Em produção, são gerenciadas via GitHub Secrets/Azure Key Vault e injetadas em tempo de execução.

### 1. Aplicar os manifestos

```bash
# Na raiz do repositório CatalogAPI
kubectl apply -f k8s/
```

### 2. Verificar se está rodando

```bash
kubectl get pods
kubectl get services
```

### 3. Acessar o Swagger

**Localmente (Docker Desktop):** como o `service.yaml` é `ClusterIP`, use port-forward:

```bash
kubectl port-forward service/catalog-api-service 5002:80
```

Acesse: `http://localhost:5002/swagger`

**Em produção (AKS):** o acesso externo é feito via **Kong API Gateway** (`kong-proxy`, exposto como `LoadBalancer`), que roteia as requisições até o `catalog-api-service`.

### Parar o serviço

```bash
kubectl delete -f k8s/
```

---

## 🎓 Contexto Acadêmico

Desenvolvido para o **Tech Challenge — PosTech FIAP**, Arquitetura de Software em .NET com Azure.

Este repositório evoluiu ao longo das fases do curso:
- **Fase 2:** estrutura inicial do CRUD de jogos, fluxo de compra e mensageria (RabbitMQ, com uma etapa intermediária via Kafka antes da migração definitiva)
- **Fase 3:** containerização, Kubernetes local e persistência poliglota com MongoDB (avaliações de jogos)
- **Fase 4:** busca avançada com Elasticsearch (fuzzy search + relevância), deploy gerenciado em Azure AKS via Kong, e automação de CI/CD com GitHub Actions


