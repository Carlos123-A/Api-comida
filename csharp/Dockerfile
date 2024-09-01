# Usa la imagen SDK para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

RUN dotnet new webapi -o MyApiCo

COPY Meal.cs /app/MyApiCo/Models/Meal.cs
COPY MealsController.cs /app/MyApiCo/Controllers/MealsController.cs
COPY MealService.cs /app/MyApiCo/Services/MealService.cs
COPY Program.cs /app/MyApiCo/Program.cs

WORKDIR /app/MyApiCo

RUN dotnet restore
RUN dotnet build --configuration Release

RUN dotnet publish --configuration Release --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MyApiCo.dll"]
