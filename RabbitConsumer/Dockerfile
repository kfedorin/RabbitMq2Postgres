#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7555

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RabbitConsumer/RabbitConsumer.csproj", "RabbitConsumer/"]
RUN dotnet restore "RabbitConsumer/RabbitConsumer.csproj"
COPY . .
WORKDIR "/src/RabbitConsumer"
RUN dotnet build "RabbitConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitConsumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitConsumer.dll"]