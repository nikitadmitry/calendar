FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["front-end/Calendar.WebApp/Calendar.WebApp.csproj", "Calendar.WebApp/"]
RUN dotnet restore "front-end/Calendar.WebApp/Calendar.WebApp.csproj"
COPY . .
WORKDIR "/src/Calendar.WebApp"
RUN dotnet build "Calendar.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Calendar.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Calendar.WebApp.dll"]
