FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7249

ENV ASPNETCORE_URLS=http://+:7249

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FinancialApplication/FinancialApplication.csproj", "FinancialApplication/"]
RUN dotnet restore "FinancialApplication\FinancialApplication.csproj"
COPY . .
WORKDIR "/src/FinancialApplication"
RUN dotnet build "FinancialApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinancialApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinancialApplication.dll"]
