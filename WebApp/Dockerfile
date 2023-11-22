# Utilisez l'image SDK pour construire l'app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copiez csproj et effectuez un restore (cette étape est séparée pour tirer parti de la mise en cache de Docker)
COPY ["BlazorBazaar.csproj", "./"]
RUN dotnet restore

# Copiez tout le reste et construisez l'app
COPY . ./
RUN dotnet publish -c Release -o out

# Générez l'image finale
FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=build-env /app/out/wwwroot /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf

