# Etapa 1: build com SDK .NET 9 Preview
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /app

# Copiar arquivos do projeto e restaurar dependências
COPY pontoMais.csproj ./
RUN dotnet restore

# Copiar tudo e publicar
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: runtime com ASP.NET Core .NET 9 Preview
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "pontoMais.dll"]
