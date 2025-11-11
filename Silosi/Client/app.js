import Fabrika from './classes/Fabrika.js';
import Silos from './classes/Silos.js';
import UserInterface from './classes/UserInterface.js';

const podaci = await (await fetch('http://localhost:5087/App/VratiFabrike')).json();

let silosArray = [];

podaci[0].silosi.forEach(silos => {
    silosArray.push(new Silos(silos.silosID, silos.oznaka, silos.kapacitet, silos.trenutnoStanje));
});

let fabrika = new Fabrika(podaci[0].fabrikaID, podaci[0].naziv, silosArray);

let ui = new UserInterface();
ui.drawUI(fabrika);