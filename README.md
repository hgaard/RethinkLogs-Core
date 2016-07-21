# RethinkLogs
Simple log dashboard using RethinkDB

## Query examples
Examples of the C# implementation of the ReQL query syntax for filtereing logs, can be found [here](query-samples). It written using [LinqPad](https://www.linqpad.net/)

## Dothet core

## Docker


### Linking

```bash
    docker build . -t rethinklogs:latest
```

```bash
    docker run -itd -p 5000:5000 --link rethinkdb --name=rethinklogs rethinklogs
```

### Networking

Create backend network:

```docker network create backend```

Run Rethinklogs app (and RethinkDb) --todo add volume

```docker run -itd -p 8080:8080 -p 28015:28015 -p 29015:29015 --net=backend -v ~/rethinkdb_data --name=rethinkdb rethinkdb```

```docker run -itd -p 80:5000 --net=backend --name=rethinklogs rethinklogs```



### Docker-compose



## Useful commands

stop all containers:

```docker kill $(docker ps -q)```

remove all containers

```docker rm $(docker ps -a -q)```

remove all docker images

```docker rmi $(docker images -q)```