#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7444

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RabbitProducer/RabbitProducer.csproj", "RabbitProducer/"]
RUN dotnet restore "RabbitProducer/RabbitProducer.csproj"
COPY . .
WORKDIR "/src/RabbitProducer"
RUN dotnet build "RabbitProducer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitProducer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitProducer.dll"]