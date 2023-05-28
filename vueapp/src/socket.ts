// import io from "socket.io-client";
import { ref } from "vue";
// import axios from "axios";
import { websocket } from "./config";

const socket = ref<WebSocket>();

const connect = () => {
  socket.value = new WebSocket(websocket);
};
// io(websocket, {
//   reconnectionDelayMax: 10000,
//   autoConnect: false,
//   transportOptions: {
//     polling: {
//       extraHeaders: {
//         token: localStorage.getItem("token"),
//       },
//     },
//   },
// });

export { socket, connect };
