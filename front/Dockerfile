# Build stage
FROM node:20-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .

# In Docker the frontend is served behind nginx and proxies /api to the backend,
# so the API URL is relative to the current origin.
ARG VITE_APP_API_URL=/api/
ENV VITE_APP_API_URL=${VITE_APP_API_URL}

RUN npm run build

# Serve stage
FROM nginx:alpine

COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
