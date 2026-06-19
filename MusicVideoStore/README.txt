Инструкция по запуску

1. Откройте файл MusicVideoStoreCourseProject.sln в Visual Studio.
2. В App.config уже указано подключение к SQL Server: Data Source=localhost.
3. Запустите проект. При первом запуске программа автоматически создаст базу MusicVideoStore на сервере localhost.
4. После запуска в SSMS нажмите Refresh на папке «Базы данных». База MusicVideoStore должна появиться рядом с 11111, 123, 1234 и другими базами.
5. Для входа используйте:
   admin / admin
   manager / manager

Если база не появилась:
- убедитесь, что в SSMS подключение именно к localhost;
- проверьте, что служба SQL Server запущена;
- запустите Visual Studio от имени администратора;
- если сервер называется иначе, замените Data Source=localhost в App.config на имя сервера из SSMS.
