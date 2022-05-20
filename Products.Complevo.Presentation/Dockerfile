#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Products.Complevo.Presentation/Products.Complevo.Presentation.csproj", "Products.Complevo.Presentation/"]
COPY ["Products.Complevo.Infrastructure/Products.Complevo.Infrastructure.Data.csproj", "Products.Complevo.Infrastructure/"]
COPY ["Products.Complevo.Application/Products.Complevo.Application.Core.csproj", "Products.Complevo.Application/"]
COPY ["Products.Complevo.Domain/Products.Complevo.Domain.csproj", "Products.Complevo.Domain/"]
COPY ["Products.Complevo.Service.Commands/Products.Complevo.Service.Commands.csproj", "Products.Complevo.Service.Commands/"]
COPY ["Products.Complevo.Infrastructure.CrossCutting/Products.Complevo.Infrastructure.CrossCutting.csproj", "Products.Complevo.Infrastructure.CrossCutting/"]
COPY ["Products.Complevo.Service.Queries/Products.Complevo.Service.Queries.csproj", "Products.Complevo.Service.Queries/"]
RUN dotnet restore "Products.Complevo.Presentation/Products.Complevo.Presentation.csproj"
COPY . .
WORKDIR "/src/Products.Complevo.Presentation"
RUN dotnet build "Products.Complevo.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Products.Complevo.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Products.Complevo.Presentation.dll"]
