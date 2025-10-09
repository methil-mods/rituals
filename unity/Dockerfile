FROM unityci/editor:ubuntu-6000.2.7f2-webgl-3.1.0 as builder

WORKDIR /project

COPY . .

RUN /opt/unity/Editor/Unity \
    -batchmode \
    -nographics \
    -quit \
    -projectPath /project \
    -buildTarget WebGL \
    -executeMethod BuildScript.PerformBuild \
    -logFile /dev/stdout

FROM nginx:alpine
COPY --from=builder /project/Build/WebGL /usr/share/nginx/html

COPY nginx.conf /etc/nginx/nginx.conf

RUN chmod -R 777 /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
    