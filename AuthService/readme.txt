Команды выполняем в терминале powershell в папке c csproj файлом, в этой же директории должен лежать Dockerfile:

//Сборка образа
docker build -t authenticationservice .

//создаем dev cертификат для https (заменить <your password> на свой пароль)
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\authenticationservice.pfx -p <your password>

dotnet dev-certs https --trust

//Создаем и запускаем контейнер из образа (<your password> - заменить на пароль указанный выше)
docker run --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_Kestrel__Certificates__Default__Password="<your password>" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/authenticationservice.pfx -v $env:USERPROFILE\.aspnet\https:/https/ authenticationservice