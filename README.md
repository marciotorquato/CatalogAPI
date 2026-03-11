# 🎮 CatalogAPI — FCG FIAP Cloud Games

Microsserviço responsável pelo **catálogo de jogos e biblioteca do usuário** da plataforma FIAP Cloud Games. Gerencia o CRUD de jogos e inicia o fluxo de compra via eventos RabbitMQ.

---

## 🧱 Tecnologias

- .NET 9
- SQL Server (dados relacionais)
- MongoDB (logs via Serilog)
- RabbitMQ (publicação e consumo de eventos)
- JWT Bearer Authentication

---

## 📡 Endpoints

### 🎮 Games — `/api/Game`

| Método | Rota | Descrição | Auth |
|---|---|---|---|
| `POST` | `/api/Game/Cadastrar/` | Cadastra novo jogo | ✅ administrador |
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
| `POST` | `/Comprar/` | Inicia compra de um jogo | ❌ Público |
| `PUT` | `/Atualizar/{id}` | Atualiza item da biblioteca | ✅ usuario |
| `DELETE` | `/Deletar/{id}` | Remove jogo da biblioteca | ✅ usuario |

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
    ├── configmap.yaml   ← variáveis não sensíveis
    ├── secret.yaml      ← variáveis sensíveis (Base64)
    ├── deployment.yaml  ← gerencia os Pods
    └── service.yaml     ← expõe o serviço na rede
```

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

```bash
# Descubra a porta externa atribuída
kubectl get services

# Acesse no browser (substitua pela porta real)
http://localhost:30002/swagger
```

> ⚠️ O Docker Desktop pode atribuir uma porta diferente da definida no manifesto. Verifique a porta real com `kubectl get services` na coluna `PORT(S)`.

### Parar o serviço

```bash
kubectl delete -f k8s/
```

---

## 🎓 Contexto Acadêmico

Desenvolvido para o **Tech Challenge Fase 2 — PosTech FIAP**
Arquitetura de Software em .NET com Azure.