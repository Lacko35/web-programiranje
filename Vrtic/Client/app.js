import Dete from "./classes/dete.js";
import Grupa from "./classes/grupa.js";
import Graphic from "./classes/graphic.js";

const response = await fetch(`http://localhost:5227/App/VratiGrupe`);

let podaci = await response.json();

let grupe = [];

podaci.forEach((grupa) => {
  let g = new Grupa(
    grupa.grupaID,
    grupa.naziv,
    grupa.imeVaspitaca,
    grupa.bojaGrupe,
    grupa.clanovi,
    grupa.trenutniBrojDece
  );

  grupe.push(g);
});

const g = new Graphic();
g.drawUI(grupe);
