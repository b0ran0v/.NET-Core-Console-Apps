This project is about how to make console chat in C#

In this case there is used TCPLister for receiving and sending messages.

Server can have many clients, so it receives message from all clients and can also send it to them.

Client can receive and send message to other clients, but first of all it is received by server, then server sends it to others.

Additional operations are:
    1. Clients must have their own UNIQUE name.
    2. Clients choose their console chat color (Client's color can be seen in Server and other Clients' console). It doesn't have to be unique for all. 
    3. Server can see alive connections.
 

