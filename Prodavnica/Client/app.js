import Prodavnica from './classes/prodavnica.js';
import Proizvod from './classes/proizvod.js';
import UserInterface from './classes/interface.js';

const prodavnice = [];

const res = await fetch('http://localhost:5022/api/Global/VratiProdavnice');
var data = await res.json();

data.forEach(p => {
    let prodavnica = new Prodavnica(p.prodavnicaID, p.naziv, p.adresa, p.brojTelefona, []);

    p.proizvodi.forEach(pr => {
        let proizvod = new Proizvod(pr.proizvodID, pr.naziv, pr.kategorija, pr.cena, pr.kolicina);

        prodavnica.proizvodi.push(proizvod);
    });

    prodavnice.push(prodavnica);
});

const i = new UserInterface();

i.generateGUI(prodavnice);