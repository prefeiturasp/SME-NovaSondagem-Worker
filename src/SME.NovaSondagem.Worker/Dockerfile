FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/SME.NovaSondagem.Worker/SME.NovaSondagem.Worker.csproj", "src/SME.NovaSondagem.Worker/"]
COPY ["src/SME.NovaSondagem.IoC/SME.NovaSondagem.IoC.csproj", "src/SME.NovaSondagem.IoC/"]
COPY ["src/SME.NovaSondagem.Infra/SME.NovaSondagem.Infra.csproj", "src/SME.NovaSondagem.Infra/"]
COPY ["src/SME.NovaSondagem.Dominio/SME.NovaSondagem.Dominio.csproj", "src/SME.NovaSondagem.Dominio/"]
COPY ["src/SME.NovaSondagem.Dados/SME.NovaSondagem.Dados.csproj", "src/SME.NovaSondagem.Dados/"]
COPY ["src/SME.NovaSondagem.Aplicacao/SME.NovaSondagem.Aplicacao.csproj", "src/SME.NovaSondagem.Aplicacao/"]

RUN dotnet restore "src/SME.NovaSondagem.Worker/SME.NovaSondagem.Worker.csproj"

COPY . .
WORKDIR "/src/src/SME.NovaSondagem.Worker"
RUN dotnet publish "SME.NovaSondagem.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SME.NovaSondagem.Worker.dll"]
