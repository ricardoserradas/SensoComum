FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 10685
EXPOSE 44348

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY SensoComum.APIs.Queries.csproj SensoComum.APIs.Queries/
COPY Nuget.Config ./
RUN dotnet restore --configfile Nuget.Config SensoComum.APIs.Queries/SensoComum.APIs.Queries.csproj
COPY . ./
RUN dotnet build SensoComum.APIs.Queries.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SensoComum.APIs.Queries.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SensoComum.APIs.Queries.dll"]
