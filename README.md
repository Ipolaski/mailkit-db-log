1. В `appsettings.json` заполнить строки для:
    - подключения к бд: `PostgreSql`; `SqlServer`;
    - почта куда удет уходить письмо: `Email`;
2. В `TestApp/Infrastructure/EmailSender/EmailService.cs` заполнить данные для настройки `SmtpClient`;
3. В SQL запросах добавлять необходимые для взаимоддействия таблицы и редактировать сами запросы на выборку;
