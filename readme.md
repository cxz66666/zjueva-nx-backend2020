# 2020EVA纳新后台

- 短信ak出于隐私原因已经删掉，需要自己申请模板并填入相应的码
- 函数计算功能经测试已可以正常使用，直接点击详细资料里的图片即可下载靓照
- **相应的前端功能请移步**[zjueva-landing](https://github.com/cxz66666/zjueva-landing)
- 导出数据的json文件可以通过简单的python程序进行处理，当然也可以选择直接导出为excel更直观
- :construction: 提供修改选择最终面试时间的接口



这里是一个基于asp.net  3.1core实现的简单报名系统后台，同时提供了选择面试时间、自动发送短信（报名确认、选择时间、面试时间确认、结果通知）、自动排班、导出报名表、函数计算上传图片等功能。



### 使用方法：

#### docker版

- 安装docker-ce和docker-compose

  进入根目录后

  ~~~
  docker build -t eva-nx-backend .
  ~~~

  

- 创建一个docker-compse.yml文件，内容按照下图配置

  ~~~
  version: '3'
  services:
    app:
      image: eva-nx-backend
      restart: always
      environment:
        DB_HOST: db
        DB_NAME: naxin
        DB_USER: "random"
        DB_PASSWORD: "random_pd"
        ADMIN_USERNAME: "zjueva"
        ADMIN_PASSWORD: "zjueva" 
      ports:
        - 127.0.0.1:xxxx:80
  
    db:
      image: postgres:latest
      restart: always
      volumes:
        - ./data/postgres:/var/lib/postgresql/data
      ports:
        - 6669:5432
      environment:
        POSTGRES_DB: naxin
        POSTGRES_USER: random
  ~~~

- 之后创建data文件夹，

  ~~~
  docker-compse up -d
  ~~~

  即可在本机5003端口跑起来



#### 懒得用docker版

- 进入根目录下

  ~~~asp
  dotnet restore "./2020-backend.csproj"
  dotnet build "2020-backend.csproj" -c Release -o /app/build
  dotnet publish "2020-backend.csproj" -c Release -o /app/publish
  cd /app/publish
  dotnet 2020-backend.dll
  ~~~

  





### 一些注意：

- 首次使用前必须必须用管理员登录，进入数据库迁移页面，点击migrate！！！！！进行数据库迁移
- 函数计算模块由于隐私原因不予暴露，有兴趣可以移步阿里云函数计算服务以及oss静态存储服务
- 短信的模块写的还有缺陷，拉取最新回复有时会失效
- 本地开发根据自己的情况改变configuration里面设置的数据库名字和端口后，进行迁移