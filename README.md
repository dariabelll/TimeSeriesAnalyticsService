# TimeSeriesAnalyticsService

Web API приложение для обработки и хранения time-series данных из CSV-файлов.

## Описание

Приложение позволяет:

- Загружать CSV-файлы с временными данными
- Валидировать содержимое файла
- Сохранять исходные значения в таблицу `Values`
- Вычислять агрегированные показатели
- Сохранять агрегаты в таблицу `Results`
- Получать агрегированные данные с фильтрацией
- Получать последние N значений по имени файла

Все операции импорта выполняются в транзакции. Если файл невалидный, изменения в базе данных не сохраняются.

---

## Методы API

### 1. Импорт CSV

POST /api/import/{fileName}

- Принимает CSV-файл (multipart/form-data)
- Выполняет валидацию данных
- Перезаписывает данные, если файл с таким именем уже существует
- Возвращает загруженные значения и агрегированные результаты

---

### 2. Получение агрегированных результатов

GET /api/results

Поддерживаемые параметры фильтрации:

- fileName
- firstStartFrom
- firstStartTo
- avgValueFrom
- avgValueTo
- avgExecFrom
- avgExecTo
- skip
- take

---

### 3. Получение последних значений

GET /api/files/{fileName}/values/latest?count=10

Возвращает последние записи, отсортированные по Date по убыванию.

---

## Формат CSV

Файл должен иметь следующий формат:

Date;ExecutionTime;Value  
YYYY-MM-DDTHH:mm:ss.ffffZ;ExecutionTimeInSeconds;FloatingPointValue  

Пример:

Date;ExecutionTime;Value  
2026-03-01T10:00:00.0000Z;1.5;10.0  
2026-03-01T10:00:10.0000Z;2.0;20.0  
2026-03-01T10:00:20.0000Z;0.0;30.0  

---

## Правила валидации

Файл считается невалидным если:

- Дата раньше 01.01.2000
- Дата позже текущего времени (UTC)
- ExecutionTime < 0
- Value < 0
- Количество строк < 1 или > 10 000
- Отсутствует одно из значений
- Некорректный формат данных

При ошибке:
- Возвращается 400 BadRequest
- Транзакция откатывается
- Данные в БД не изменяются

---

## Используемые технологии

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- PostgreSQL
- FluentValidation
- Swagger 

---

## Требования

- .NET 8 SDK
- PostgreSQL 14+

---

## Запуск проекта

1. Клонировать репозиторий

2. Создать базу данных в PostgreSQL:

    Создать БД с именем:

    timeseries

3. Настроить строку подключения:

    Скопировать файл appsettings.Development.example.json в appsettings.Development.json

    Открыть appsettings.Development.json и указать пароль PostgreSQL:

    ```
    {
    "ConnectionStrings": {
        "Default": "Host=localhost;Port=5432;Database=timeseries;Username=postgres;Password=YOUR_PASSWORD"
        }
    }
    ```

    Заменить YOUR_PASSWORD на реальный пароль.

4. Запустить проект:
    ```
    dotnet run
    ```
5. Открыть Swagger:

    После запуска в консоли будет указан адрес приложения. Swagger доступен по адресу:
    ```
    http://localhost:<port>/swagger
    ```
    Порт можно посмотреть в консоли после выполнения dotnet run.

---

## Примеры запросов

Импорт CSV:
```
curl -F "file=@test.csv" http://localhost:{port}/api/import/test1
```
Получение последних значений:
```
curl http://localhost:{port}/api/files/test1/values/latest?count=10
```
Получение агрегированных результатов:
```
curl http://localhost:{port}/api/results?fileName=test1
```
