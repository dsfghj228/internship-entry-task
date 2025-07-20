# 🎮 Решение тестового задания: Web API для игры в Крестики Нолики


## 📌 Архитектурные решения

- **Чистая архитектура (Layered Architecture)** -   для разделения проекта на несколько логических уровней для выполнения своих ролей.
- **Переменные окружения** - размер поля, строки подключения.
- **Идемпотентность для POST /moves запросов на совершение хода** - Для этого используется ETag.
- **Тестирование** - юнит тесты для проверки работы моделей, а также интеграционные тесты для проверки системы в целом.
- **Запуск тестов в CI** - настроен файл GitHub Actions для проверки сборки и запуска тестов.

##Структура проекта

- **Presentation Layer (UI Layer)**

GameController  - контроллер эндпоинтов самой игры.
HealthController - контроллер возвращающий 200 OK

- **Application Layer (Business Logic Layer, BLL)**
 ETagService - сервис для генерации ETag  3. Data Access Layer (DAL)

- **ApplicationDbContext** - контекст базы данных GameRepository - репозитория для игры
- **Domain Layer**
Game - сущность игры

##Запуск проект


 ##Описание API

- **Создание новой игры:** 
   - Endpoint: POST api/Game/create
   - Описание: Создает новую игру с параметрами (размер поля, условие победы), взятыми из конфигурации на сервере.
   - Тело запроса: Guid playerOneId Guid plyaerTwoId
   - Успешный ответ: 200 OK
 
```json
 {
   "id": "588ebe63-e13d-44be-9a49-e2c3d52ee67c",
   "board": [
     [
       0,
       0,
       0
      ],
     [
       0,
       0,
       0
     ],
     [
       0,
       0,
       0
     ]
   ],
   "size": 3,
   "playerOneId": "1aa3d8ef-a562-4011-8e48-a11447742202",
   "playerTwoId": "e6bedffe-4987-45a1-b3b9-af6a4f7aba60",
   "currentPlayerId": "1aa3d8ef-a562-4011-8e48-a11447742202",
   "status": 0,
   "stepCount": 0
  }
```

- **Получение игры по Id:** 
   - Endpoint: GET api/Game/{gameId}
   - Описание: Возвращает игру с данным gameId
   - Тело запроса: Guid gameId
   - Успешный ответ: 200 OK
 
```json
 {
   "id": "588ebe63-e13d-44be-9a49-e2c3d52ee67c",
   "board": [
     [
       0,
       0,
       0
      ],
     [
       0,
       0,
       0
     ],
     [
       0,
       0,
       0
     ]
   ],
   "size": 3,
   "playerOneId": "1aa3d8ef-a562-4011-8e48-a11447742202",
   "playerTwoId": "e6bedffe-4987-45a1-b3b9-af6a4f7aba60",
   "currentPlayerId": "1aa3d8ef-a562-4011-8e48-a11447742202",
   "status": 0,
   "stepCount": 0
  }
```
 - Ошибки: 404 Not Found: Игра не найдена

- **Удаление игры по Id:** 
   - Endpoint: DELETE api/Game/{gameId}
   - Описание: Удаляет игру с данным gameId
   - Тело запроса: Guid gameId
   - Успешный ответ: 200 OK
   - Ошибки: 404 Not Found: Игра не найдена

- **Сделать ход:** 
   - Endpoint: POST api/Game/move/{gameId}
   - Описание: Делает ход
   - Тело запроса: Guid gameId, int row, int col, string If-Match(последний ETag)
   - Успешный ответ: 200 OK
```json
{
  "id": "588ebe63-e13d-44be-9a49-e2c3d52ee67c",
  "board": [
    [
      1,
      0,
      0
    ],
    [
      0,
      0,
      0
    ],
    [
      0,
      0,
      0
    ]
  ],
  "size": 3,
  "playerOneId": "1aa3d8ef-a562-4011-8e48-a11447742202",
  "playerTwoId": "e6bedffe-4987-45a1-b3b9-af6a4f7aba60",
  "currentPlayerId": "e6bedffe-4987-45a1-b3b9-af6a4f7aba60",
  "status": 1,
  "stepCount": 1
}
```
   - Ошибки:
     404 Not Found: Игра не найдена,
     404 Not Found: Ячейки с таким индексом не существует,
     409 Conflict: Игра уже закончилась,
     409 Conflict: Сейчас очередь другого игрока

- **Проверка работоспособности:** 
   - Endpoint: GET api/Health
   - Описание: Проверяет работоспособность
   - Успешный ответ: 200 OK
     
