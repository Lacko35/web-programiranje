import Automobil from "./classes/automobil.js";
import Draw from "./classes/draw.js";

const automobili = [];

const res = await fetch("http://localhost:5268/Global/VratiAutomobile");
const resData = await res.json();

resData.forEach(a => {
    let auto = new Automobil(a.automobilID, a.model, a.predjenaKilometraza, a.brojSedista, a.cenaPoDanu, a.godiste, a.jelIznajmljen);
    automobili.push(auto);
});

const drawObj = new Draw();
drawObj.DrawApp(document.body, automobili);