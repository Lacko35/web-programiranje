import Dete from "./dete.js";
import Grupa from "./grupa.js";

export default class Graphic {
  constructor() {
    this.container = null;
  }

  drawUI(grupe) {
    this.container = document.createElement("div");
    this.container.className = "app-container";
    document.body.appendChild(this.container);

    const addChildForm = document.createElement("div");
    addChildForm.className = "form";
    this.container.appendChild(addChildForm);

    const formElements = document.createElement("div");
    formElements.className = "elements";
    addChildForm.appendChild(formElements);

    let labels = ["Ime i prezime:", "JMBG:", "Ime roditelja:"];
    let inputs = ["text", "text", "text"];

    labels.forEach((label, index) => {
      let element = document.createElement("div");
      element.className = "element";
      formElements.appendChild(element);

      let l = document.createElement("label");
      l.textContent = label;
      element.appendChild(l);

      let i = document.createElement("input");
      i.type = inputs[index];
      element.appendChild(i);
    });

    const addChildBtn = document.createElement("button");
    addChildBtn.textContent = "Upisi dete";

    addChildBtn.addEventListener("click", async () => {
      let values = formElements.querySelectorAll(".element > input");

      let imePrezime = values[0].value.split(" ");

      const response = await fetch(`http://localhost:5227/App/DodajClana`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          ime: imePrezime[0],
          prezime: imePrezime[1],
          imeRoditelja: values[2].value,
          jmbg: values[1].value,
        }),
      });

      if (response.ok) {
        let podaci = await (
          await fetch(`http://localhost:5227/App/VratiGrupe`)
        ).json();

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
      }

      this.#drawKindergarden(kindergardenDiv, grupe);

      values.forEach((v) => {
        v.value = "";
      });
    });

    addChildForm.appendChild(addChildBtn);

    const kindergardenDiv = document.createElement("div");
    kindergardenDiv.className = "kindergarden";
    this.container.appendChild(kindergardenDiv);

    this.#drawKindergarden(kindergardenDiv, grupe);
  }

  #drawKindergarden(parent, groups) {
    parent.innerHTML = "";

    groups.forEach((grupa) => {
      const group = document.createElement("div");
      group.className = "group";
      group.style.backgroundColor = grupa.bojaGrupe;
      parent.appendChild(group);

      const professorDiv = document.createElement("div");
      professorDiv.className = "group-element";
      group.appendChild(professorDiv);

      let label1 = document.createElement("label");
      label1.textContent = "Vaspitac:";
      professorDiv.appendChild(label1);

      let pomdiv = document.createElement("div");

      let label2 = document.createElement("label");
      label2.textContent = `${grupa.imeVaspitaca}`;
      pomdiv.appendChild(label2);
      professorDiv.appendChild(pomdiv);

      const nameDiv = document.createElement("div");
      nameDiv.className = "group-element";
      group.appendChild(nameDiv);

      label1 = document.createElement("label");
      label1.textContent = "Ime grupe:";
      nameDiv.appendChild(label1);

      pomdiv = document.createElement("div");

      label2 = document.createElement("label");
      label2.textContent = `${grupa.naziv}`;
      pomdiv.appendChild(label2);
      nameDiv.appendChild(pomdiv);

      const childNumberDiv = document.createElement("div");
      childNumberDiv.className = "group-element";
      group.appendChild(childNumberDiv);

      label1 = document.createElement("label");
      label1.textContent = "Broj dece:";
      childNumberDiv.appendChild(label1);

      pomdiv = document.createElement("div");

      label2 = document.createElement("label");
      label2.textContent = `${grupa.trenutniBrojDece}`;

      pomdiv.appendChild(label2);

      childNumberDiv.appendChild(pomdiv);

      grupa.clanovi.forEach((clan) => {
        let member = document.createElement("div");
        member.className = "group-member";
        group.appendChild(member);

        member.textContent = `${clan.ime} ${clan.prezime}, ${clan.imeRoditelja}, ${clan.jmbg}`;
      });
    });
  }
}
