# CMSBackEnd
C# version of CMSBackEnd(Java)

项目整体架构如图所示，[CMSAPP](https://github.com/JunjieLl/CMSAPP)。

![未命名文件-2](https://p.ipic.vip/do6k4f.png)

每个节点都以docker 容器来运行，相应的Dockeerfile文件如下。

## Nginx Dockerfile（包含了Web）

### Vue + Nginx Dockerfile

```dockerfile
FROM node:17 as base
WORKDIR /app
COPY ./ ./
RUN npm install && npm run build

FROM nginx as production
RUN mkdir /app
EXPOSE 80
COPY --from=base /app/dist /app
#COPY tjsegof.club_bundle.crt tjsegof.club.key /etc/nginx/
COPY nginx.conf /etc/nginx/nginx.conf
```

### Nginx 网关配置

Nginx作为网关，还要处理其它请求，因此设置了转发。

配置文件如下：

```nginx
user  nginx;
worker_processes  1;
error_log  /var/log/nginx/error.log warn;
pid        /var/run/nginx.pid;
events {
  worker_connections  1024;
}
http {
  include       /etc/nginx/mime.types;
  default_type  application/octet-stream;
  log_format  main  '$remote_addr - $remote_user [$time_local] "$request" '
                    '$status $body_bytes_sent "$http_referer" '
                    '"$http_user_agent" "$http_x_forwarded_for"';
  access_log  /var/log/nginx/access.log  main;
  sendfile        on;
  keepalive_timeout  65;

  server {
    listen       12580;

    # 转发请求
    location /api/ {
      proxy_pass http://myAspnet;#80
      proxy_set_header Host $http_host;
    }

    location /file/{
      proxy_pass http://101.35.111.182;
      proxy_set_header Host $http_host;
    }

    location /images/{
      proxy_pass http://101.35.111.182;
      proxy_set_header Host $http_host;
    }

    location / {
      root   /app;
      index  index.html;
      try_files $uri $uri/ /index.html;
    }
    
    error_page   500 502 503 504  /50x.html;
    location = /50x.html {
      root   /usr/share/nginx/html;
    }
  }
}
```

## AspNet 后端

由于涉及到与C++/CLI和Win32Dll的互操作，通过Visual Studio的发布功能得到项目的各种dll，然后再使用Dockerfile打包。

```dock
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY ./ ./
ENTRYPOINT ["dotnet", "CMSBackEnd.dll"]
```

## 编排容器

最后结合上述打包好的容器，使用docker-compose技术来编排容器。

```dockerfile
services:
  myRedis:
   image: redis
   ports:
    - 35229:6379

  myAspnet:
   image: junjiely/netcms:latest
   #ports:
           #- 9999:80

  myFrontend:
    image: junjiely/netfrontend:u
    ports:
     - 12580:12580
     
  db:
    image: mysql
    ports:
      - 52125:3306
    command: --default-authentication-plugin=mysql_native_password
    environment:
      - MYSQL_ROOT_PASSWORD=mysqlroot123
      - MYSQL_USER=cms
      - MYSQL_PASSWORD=hello
    volumes:
      - db-data:/var/lib/mysql

volumes:
  mydata:
  db-data:
```



## Run

```
$ docker-compose -p net -f myservice.yml up -d
```

![image-20220625075354318](https://p.ipic.vip/38jify.png)

