window.addEventListener("swagger-ready", async () => {
    let currentUserId = null;
    let token = null;

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/hubs/support-chat", {
            accessTokenFactory: () => {

                const authorized = window.ui.authSelectors.authorized();
                token = authorized.getIn(["Bearer", "value"]);

                if (!token) {
                    return null;
                }

                const payload = JSON.parse(atob(token.split(".")[1]))

                currentUserId = payload.uid;

                return token;
            }
        })
        .build();


    function displayMessage(message) {

        const messageElement = document.createElement("div");
        messageElement.classList.add("message");

        console.log("Message: ", message);
        console.log("CustomerId: ", message.ticket.customerId);
        console.log("SenderId: ", message.senderId);
        console.log("CurrentUserId: ", currentUserId);

        if (message.senderId === message.ticket.customerId) {
            if (currentUserId === message.ticket.customerId) {
                console.log("First If Condition of Id : ", currentUserId === message.ticket.customerId);
                messageElement.classList.add("message-sent");
            }

            else {
                console.log("First else Condition of If : ", currentUserId === message.ticket.customerId);
                messageElement.classList.add("message-received");
            }
        }


        else {
            if (currentUserId === message.ticket.customerId) {
                console.log("First if Condition of else : ", currentUserId === message.ticket.customerId);
                messageElement.classList.add("message-received");
            }

            else {
                console.log("First else Condition of else : ", currentUserId === message.ticket.customerId);
                messageElement.classList.add("message-sent");
            }
        }



        if (message.content) {
            const contentElement = document.createElement("div");
            contentElement.textContent = message.content;
            messageElement.appendChild(contentElement);
        }


        if (message.attachments && message.attachments.length > 0) {

            message.attachments.forEach(attachment => {

                const attachmentElement = document.createElement("a");

                attachmentElement.href = `/${attachment.filePath}`;
                attachmentElement.textContent = attachment.fileName;
                attachmentElement.target = "_blank";

                messageElement.appendChild(attachmentElement);
            });
        }

        document.getElementById("messages").appendChild(messageElement);
    }



    async function loadMessages(ticketId) {

        document.getElementById("messages").innerHTML = "";

        const response = await fetch(`/api/ticket/${ticketId}/messages`, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });


        if (!response.ok) {
            console.error("Failed to load messages.");
            return;
        }

        const messages = await response.json();

        messages.forEach(message => {
            displayMessage(message);
        });
    }

 


    connection.on("ReceiveMessage", message => {

        displayMessage(message);
 
    });


    connection.on("MessagesDeleted", messageIds => {
        messageIds.forEach(messageId => {

            const messageElement = document.querySelector(`[data-message-id = "${messageId}"]`);

            if (messageElement) {
                messageElement.remove();
            }
        });
    });



    connection.on("ReceiveNotification", notification => {
        alert("Notification: " + notification.message)
    });


    document.getElementById("connect-signalr")
        .addEventListener("click", async () => {
            try
            {
                if (connection.state !== signalR.HubConnectionState.Disconnected) {
                    await connection.stop();
                }

                document.getElementById("messages").innerHTML = "";

                await connection.start();
 
            }
            catch (error)
            {
                console.error("SignalR connection failed:", error);
            }
        })


    document.getElementById("messages-button")
        .addEventListener("click", async () => {
            const ticketId = document.getElementById("ticket-id").value;

            if (!ticketId) {
                console.error("Please enter a ticket ID.");
                return;
            }
            await loadMessages(ticketId);
        })    
    
});








