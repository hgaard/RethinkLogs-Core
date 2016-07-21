# RethinkLogs
Simple log dashboard using RethinkDB

## Query examples
Examples of the C# implementation of the ReQL query syntax for filtereing logs, can be found [here](query-samples). It written using [LinqPad](https://www.linqpad.net/)

## Dothet core

## Docker

```shell
    docker build . -t rethinklogs:latest
```

```shell
    docker run -itd -p 5000:5000 --link rethinkdb --name=rethinklogs rethinklogs
```