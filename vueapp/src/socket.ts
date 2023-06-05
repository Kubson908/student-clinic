import { websocket } from "./config";

const getSocket = (message_ref: any) => {
  const socket = new WebSocket(websocket);
  socket.addEventListener("open", () => {
    socket.send(localStorage.getItem("token") as string);
  });

  socket.addEventListener("message", (event) => {
    message_ref.value = event;
  });
  return socket;
};

export { getSocket };
