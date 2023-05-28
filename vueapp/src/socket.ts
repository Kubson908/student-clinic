import { ref } from "vue";
import { websocket } from "./config";

const socket = ref<WebSocket>();

const connect = () => {
  socket.value = new WebSocket(websocket);
  socket.value?.addEventListener("open", (event) => {
    socket.value?.send(localStorage.getItem("token") as string);
  });

  socket.value?.addEventListener("message", (event) => {
    console.log(event.data);
  });
};

export { socket, connect };
