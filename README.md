# RabbitMq2Postgres

После загрузки в Docker появятся образы:
rabbitmq
postgres
rabbitproducer
rabbitconsumer

Контейнеры должны автоматически запуститься 
Перед запуском Docker Compose, откройте docker-compose.yml и убедитесь, что порты и имена контейнеров не будут конфликтовать с существующими.
В appsettings я вывел только параметры подключения к postgres, к rabbitmq использую стандартную конфигурацию. 

rabbitproducer
http://localhost:7444/swagger/index.html 

Имеет пост запрос, который отправляет объект User в шину данных (при вводе используется валидация)

rabbitconsumer
http://localhost:7555/swagger/index.html 

Принимает объект User с шины данных и складывает его в соответствующую таблицу в БД 
Так же имеет операции: 

get: api/Organization/GetAll - получить все организации
get: api/Organization/ByIdOrganization - получить организацию по ID
post: api/Organization - добавить организацию
put: api/Organization - обновить организацию по ID
post: api/Organization/SeedPost - добавить случайную организацию
delete: api/Organization/DeleteByIdOrganization - удалить организацию

get: api/User/GetAll - получить всех пользователей
get: api/User/ByIdUser - получить пользователя по ID
get: api/User/GetByIdOrganization - получить пользователей по IdOrganization с пагинацией 
post: api/User - добавить пользователя (при вводе используется валидация)
put: api/User - связывает пользователя с организацией (при вводе используется валидация)
post: api/User/SeedPost - добавить случайного пользователя
delete: api/User/DeleteByIdUser - удалить пользователя
