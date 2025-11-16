import AppInterface from "./Classes/AppInterface.js";

const response = await fetch('http://localhost:5244/api/Global/VratiBrojeveStanova');
const data = await response.json();

data.unshift("odaberite stan");

const appInterface = new AppInterface();
appInterface.drawInterface(data);