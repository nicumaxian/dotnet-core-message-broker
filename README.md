# Message Broker

## Intro

This message broker is a part of laboratory works for the course "Distributed systems".

## Basics
These message broker is not a "Platform" dependent as it's protocol is a "Plain text". Any serialization can be implemented as another layer of clients library. 

## Features
* Ability to create queues
* Ability to publish into existing queues
* Ability to subscribe to a queue or using a glob pattern
* Ability to dequeue from a queue
* Messages are persisted to the storage and are automatically recovered on crash. Saving structure is strongly inspired by *Unix* principles.
* Disconnect notifications Client <> Server
* Language free. As long it uses this standart, you can use what language you want
* Extensible. We can create new protocol on top of this and add serialization of objects.

## Protocol definition

Generally, in this message broker we have 3 types of actions : 

### Message delimiter

Messages are delimited in stream by **ETB** character. 

```
<message>\ETB<message>\ETB
```

### Request command
```
<command> [args]
```

### Response
Here we can define two response types


First means **success**

```
Ok [data]
```

Second one means **fail**
```
Error <reason>
```


### Adhoc Message - message receiving
```
QueueMessage <queue> <message>
```


### Existing commands

```bash
publish <queue> <message> #publish a message into the queue
queue-list #get the list of queues
queue-create <queue> #create queue
subscribe <queue> #subscribes to the queue
dequeue <queue> #gets the message from the queue
disconnect #disconnects the client from the server
```

When subscribing or dequeueing you can use glob patterns such as
```
*.google.com
google.*
?b.com
```

## Persistance

Once publishing, MB creates a file on a storage for given message. Message is located into
``$MessageBrokerPersistancePath/<queue>/<createdTimeTicks>``

Having this structure we can easily recover them because we have folders which represents queues and mesesages which are simple file with name of *timestamp*.

### Example 
~/MessageBroker/facebook.com/636444702630906443

This file it's described by
* queue - facebook.com
* creationTime - 636444702630906443 ticks
* message - file content

## Running demo

### Server
```bash
dotnet build Broker
...
dotnet Broker.dll #start server
```

### Client
```bash
dotnet build Client
...
dotnet Client.dll 127.0.0.1 31012 #run client and connect to host and port
```

### Client Demo interaction
```
$ dotnet Client.dll 127.0.0.1 31012
Connecting to 127.0.0.1:31012
do
Server:
Error BAD_COMMAND
queue-list
Server:
Ok facebook.com
queue-create google.com
Server:
Ok
queue-list
Server:
Ok google.com
facebook.com
publish google.com Hey
Server:
Ok
dequeue google.com
Server:
TopicMessage google.com Hey
disconnect        
Server:
Disconnect
Disconnected. Press enter to exit.
```
