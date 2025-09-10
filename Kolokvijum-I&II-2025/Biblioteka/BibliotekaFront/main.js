import Biblioteka from "./classes/biblioteka.js"
import Knjiga from "./classes/knjiga.js"
import Draw from "./classes/draw.js"

const res = await fetch("http://localhost:5294/Global/VratiBiblioteke");
const resData = await res.json();

const drawObj = new Draw();

resData.forEach(data => {
    let knjige = data.knjige.map(knjiga => new Knjiga(knjiga.knjigaID, knjiga.naslov, knjiga.imeAutora, knjiga.izdavac, 
        knjiga.godinaIzdavanja, knjiga.evidencioniBroj, knjiga.izdataKnjiga
    ));

    let biblioteka = new Biblioteka(data.bibliotekaID, data.ime, data.adresa, data.email, knjige, data.izdateKnjige);

    drawObj.DrawLibrary(document.body, biblioteka);
});