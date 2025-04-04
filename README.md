# 🧠 Rabotiaga (Сервис поздравлений)

📦 **GitHub основной реп**: [https://github.com/Gandoler/OTSC_DBProxy](https://github.com/Gandoler/OTSC_DBProxy)

---

## 1. Назначение сервиса
**Rabotiaga** — это фоновый сервис, отвечающий за автоматическую отправку поздравлений пользователям Telegram, чьи дни рождения совпадают с текущей датой. Работает в связке с другими микросервисами, получая данные о пользователях через API и отправляя поздравления через Telegram-бота.

**Основные функции:**
- Отправка поздравлений в Telegram.
- Периодический запуск по таймеру (по умолчанию — каждые 5 минут).
- Получение информации о пользователях из внешних микросервисов через HTTP.

---

## 2. Архитектура и зависимости

**Технологии и фреймворки:**
- `ASP.NET Core` — базовый фреймворк.
- `Serilog` — логирование в консоль и в файл.
- `Telegram.Bot` — работа с Telegram Bot API.
- `HttpClientFactory` — интеграция с микросервисами `DBProxy` и `TGSUBS`.

**Взаимодействие с другими микросервисами:**
- `DBProxy` — получение данных о днях рождения.
- `TGSUBS` — взаимодействие с Telegram-подписчиками.

**Основные сервисы и интерфейсы:**
- `TelegramBotService` — обёртка над логикой Telegram-бота.
- `CongratulationService` — бизнес-логика поздравлений.
- `ITgSubProxy`, `IGetCongr`, `IGetTodayBDayFriends` — интерфейсы доступа к данным.

---

## 3. Способы запуска сервиса

**Запуск с Docker (если настроен docker-compose):**

обновить ссылку нп репозиторий на текущую в докер файле
поставить свои переменные окружения 

```bash
DOCKER_BUILDKIT=1 docker build --no-cache -t  telegramcongrservice_image -f  telegramCongrservice_dockerfile .
docker run --network glebintrenet  -d -p 8085:8080 --name telegramcongrcont telegramcongrservice_image .
```

или

```bash
docker-compose up --build
```

**Запуск без Docker:**
```bash
dotnet run
```

**Переменные окружения (.env или системные):**
```env
ApiKey=your_telegram_bot_token
DbProxy=http://your_db_proxy_address
TGSUBS=http://your_tgsubs_address
```

---

## 4. Поведение и логика

Сервис запускает `TelegramBotService` в фоне и по расписанию вызывает метод `SendCongratulate`, который:
- Получает список пользователей с днем рождения.
- Формирует поздравления.
- Отправляет сообщения через Telegram.
- Получает подписку пользователя и через сервис тгподписки связывает в бд tgid пользователя с внутренним appid

**Периодичность запуска:** каждые 5 минут (можно изменить в `Worker.cs`).

---

## 6. Swagger и REST-интерфейс

У данного сервиса нет открытого HTTP API — он работает как фоновая служба (BackgroundService) и использует `HttpClient` только как клиент к другим API.

---

## 7. Контакты и поддержка

- GitHub Issues: [https://github.com/Gandoler/OTSC_DBProxy/issues](https://github.com/Gandoler/OTSC_DBProxy/issues)
