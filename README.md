# TestDatabaseMySql
Программа предназначена для просмотра и редактирования сущностей в базе данных (сотрудники, подразделения, заказы, товары).

Базу можно создать скриптом CreateDb.sql или она автоматически создастся, если программа не найдет ее по строке подключения (вынесена в файл appsettings.json).

Для поиска сущностей используется список из сущностей и строка поиска (находятся вверху приложения), а также кнопка "Поиск".

Если строка поиска не заполнена, будут найдены все сущности выбранной категории.

Найденные сущности отображаются списком ниже строки поиска.

При выборе сущности в этом списке ее можно удалить (кнопка "Удалить выбранный"), просмотреть данные о сущности (ниже списка), изменить данные и записать изменения в базу (кнопка "Сохранить изменения").

Также можно добавить новую запись в таблицу. Для этого требуется нажать кнопку "Добавить запись", далее заполнить поля данных новой сущности (обязательные или некорректно заполненные поля подсвечиваются красной границей) и записать данные в базу, нажав кнопку "Сохранить изменения".

Внизу приложения отображается строка состояния.

Взаимодействие с программой логируется.
