FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 17726
EXPOSE 44375

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY SensoComum.APIs.Commands/SensoComum.APIs.Commands.csproj SensoComum.APIs.Commands/
RUN dotnet restore SensoComum.APIs.Commands/SensoComum.APIs.Commands.csproj
COPY . .
WORKDIR /src/SensoComum.APIs.Commands
RUN dotnet build SensoComum.APIs.Commands.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SensoComum.APIs.Commands.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SensoComum.APIs.Commands.dll"]
