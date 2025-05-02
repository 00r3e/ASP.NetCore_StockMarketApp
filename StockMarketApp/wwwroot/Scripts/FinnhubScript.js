
//Create a WebSocket to communicate with server

const token = document.getElementById("FinnhubToken").value;
var stockSymbol = document.getElementById("StockSymbol").value; 
const socket = new WebSocket(`wss://ws.finnhub.io?token=${token}`);


// Open the connection and subscribe 
socket.addEventListener('open', function (event) {
    socket.send(JSON.stringify({ 'type': 'subscribe', 'symbol': stockSymbol }))
});

// Set up the event listener
socket.addEventListener('message', function (event) {

    //if error message is received from server
    if (event.data.type == "error") {
        $(".price").text(event.data.msg);
        return;
    }

    //data received from server
    /*console.log('Message from server ', event.data);*/

    /*response:
    {"data":[{"p":220.89,"s":"MSFT","t":1575526691134,"v":100}],"type":"trade"}
    type: message type
    data: [ list of trades ]
    s: symbol of the company
    p: Last price
    t: UNIX milliseconds timestamp
    v: volume 
    c: trade conditions
    */

    var eventData = JSON.parse(event.data);
    if (eventData) {
        if (eventData.data) {
            //get the updated price
            var parsedData = JSON.parse(event.data);
            var updatedPrice = parsedData.data[parsedData.data.length - 1 ].p;
            //console.log(updatedPrice);

            //update the UI
            document.getElementById("curentPrice").textContent = updatedPrice.toFixed(2);
        }
    }
});

// Unsubscribe and disconect from server
var unsubscribe = function (symbol) {
    socket.send(JSON.stringify({ 'type': 'unsubscribe', 'symbol': symbol }))
}

//when the page is being closed, unsubscribe from the WebSocket
window.onunload = function () {
    unsubscribe(stockSymbol);
};
