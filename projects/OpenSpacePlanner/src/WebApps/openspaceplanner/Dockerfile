ARG NODE_VERSION=20
ARG NGINX_VERSION=1.25

FROM node:${NODE_VERSION}-alpine AS build
WORKDIR /src

COPY . .
RUN npm ci
RUN npm run build -- --output-path=dist

FROM nginx:${NGINX_VERSION}-alpine AS base
WORKDIR /usr/share/nginx/html

RUN rm -rf ./*

COPY --from=build /src/dist ./

EXPOSE 80

ENTRYPOINT ["nginx", "-g", "daemon off;"]
