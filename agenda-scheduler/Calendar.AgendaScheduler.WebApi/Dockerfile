FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "agenda-scheduler/Calendar.AgendaScheduler.WebApi/Calendar.AgendaScheduler.WebApi.csproj"
COPY . .
WORKDIR "/src/agenda-scheduler/Calendar.AgendaScheduler.WebApi"
RUN dotnet build "Calendar.AgendaScheduler.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Calendar.AgendaScheduler.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Calendar.AgendaScheduler.WebApi.dll"]