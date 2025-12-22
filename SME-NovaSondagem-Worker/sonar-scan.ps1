# Configurações do SonarQube
$SONAR_HOST_URL = "http://seu-servidor-sonarqube:9000"
$SONAR_TOKEN = "seu-token-aqui"
$PROJECT_KEY = "SME-NovaSondagem-Worker"

# Iniciar análise
dotnet sonarscanner begin `
  /k:"$PROJECT_KEY" `
  /d:sonar.host.url="$SONAR_HOST_URL" `
  /d:sonar.token="$SONAR_TOKEN" `
  /d:sonar.cs.opencover.reportsPaths="**/coverage.opencover.xml"

# Build do projeto
dotnet build SME-NovaSondagem-Worker.sln --configuration Release

# Executar testes com cobertura (se houver testes)
dotnet test SME-NovaSondagem-Worker.sln `
  --configuration Release `
  --no-build `
  --collect:"XPlat Code Coverage" `
  --results-directory ./TestResults `
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover

# Finalizar análise
dotnet sonarscanner end /d:sonar.token="$SONAR_TOKEN"