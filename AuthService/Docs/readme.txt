Команды выполняем в терминале powershell в папке c csproj файлом, в этой же директории должен лежать Dockerfile:

//Сборка образа
docker build -t authservice .

//создаем dev cертификат для https
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\homework.pfx -p 123

dotnet dev-certs https --trust

//Создаем и запускаем контейнер из образа (<your password> - заменить на пароль указанный выше)
docker run --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_Kestrel__Certificates__Default__Password="123" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/authservice.pfx -v $env:USERPROFILE\.aspnet\https:/https/ authservice