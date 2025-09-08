export default class Draw {
    constructor() {
        this.container = null;
    }

    DrawApp(host, automobili) {
        this.container = document.createElement("div");
        this.container.className = "main-container";
        host.appendChild(this.container);

        const divFilterIznajmi = document.createElement("div");
        divFilterIznajmi.className = "filter-iznajmi-container";
        this.container.appendChild(divFilterIznajmi);

        const divIznajmljivanje = document.createElement("div");
        divIznajmljivanje.className = "iznajmljivanje-container";
        divFilterIznajmi.appendChild(divIznajmljivanje);

        const labele = ["Ime i prezime:", "JMBG:", "Broj vozacke dozvole:", "Broj dana:"];
        const inputi = ["text", "text", "text", "number"];

        labele.forEach((labela, index) => {
            let holder = document.createElement("div");
            holder.className = "iznajmljivanje-container-element";
            divIznajmljivanje.appendChild(holder)

            let l = document.createElement("label");
            l.textContent = labela;
            holder.appendChild(l);

            let i = document.createElement("input");
            i.type = inputi[index];
            holder.appendChild(i);
        });

        const divPrikaz = document.createElement("div");
        divPrikaz.className = "prikaz-container";
        this.container.appendChild(divPrikaz);

        automobili.forEach(auto => {
            this.nacrtajAuto(divPrikaz, auto);
        });

        const divFiltriranje = document.createElement("div");
        divFiltriranje.className = "filtriranje-container";
        divFilterIznajmi.appendChild(divFiltriranje);

        const labele2 = ["Predjena kilometraza:", "Broj sedista:", "Godiste:", "Model:"];
        const inputi2 = ["number", "number", "number", "select"];

        labele2.forEach((labela, index) => {
            let holder = document.createElement("div");
            holder.className = "filtriranje-container-element";
            divFiltriranje.appendChild(holder);

            let l = document.createElement("label");
            l.textContent = labela;
            holder.appendChild(l);

            if(inputi2[index] === "select") {
                let select = document.createElement("select");
                holder.appendChild(select);

                this.popuniSelect().then(rez => {
                    if(rez.length === 0) {
                        alert("Greska!");
                    }
                    else {
                        let opcija = document.createElement("option");
                        opcija.textContent = "";
                        select.appendChild(opcija);

                        rez.forEach(model => {
                            let o = document.createElement("option");
                            o.value = model.modelAuta;
                            o.textContent = model.modelAuta;

                            select.appendChild(o);
                        });
                    }
                });
            }
            else {
                let i = document.createElement("input");
                i.type = inputi2[index];
                holder.appendChild(i);
            }
        });

        const filtrirajBtn = document.createElement("button");
        filtrirajBtn.textContent = "Filtriraj prikaz";
        divFiltriranje.appendChild(filtrirajBtn);

        filtrirajBtn.addEventListener("click", async () => {
            let polja = document.body.querySelectorAll(".filtriranje-container > .filtriranje-container-element > input");
            let modelA = document.body.querySelector(".filtriranje-container > .filtriranje-container-element > select").value;

            const res = await fetch("http://localhost:5268/Global/FiltrirajPrikaz", {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    model: modelA,
                    brojSedista: parseInt(polja[1].value),
                    godiste: parseInt(polja[2].value),
                    kilometri: parseInt(polja[0].value),
                })                
            });

            if(res.status === 200) {
                let resData = await res.json();

                automobili = resData;

                document.body.querySelector(".prikaz-container").innerHTML = "";

                automobili.forEach(auto => {
                    this.nacrtajAuto(document.body.querySelector(".prikaz-container"), auto);
                });
            }

            polja.forEach(polje => {
                polje.value = "";
            });
            
            modelA = "";
        });
    }

    async popuniSelect() {
        let res = await fetch("http://localhost:5268/Global/VratiModele");

        if(res.status == 200) {
            let resData = await res.json();

            return resData;
        }
        else {
            return [];
        }
    }

    nacrtajAuto(host, auto) {
        let automobilDiv = document.createElement("div");
        automobilDiv.className = "automobil-container";
        host.appendChild(automobilDiv);

        let labele = ["Model:", "Kilometraza:", "Godiste:", "Broj sedista:", "Cena po danu:", "Iznajmljen:"];
        let svojstva = ["model", "predjenaKilometraza", "godiste", "brojSedista", "cenaPoDanu", "jelIznajmljen"];

        labele.forEach((labela, index) => {
            let holder = document.createElement("div");
            holder.className = "automobil-container-element";
            automobilDiv.appendChild(holder);

            let i1 = document.createElement("label");
            i1.textContent = labela;
            holder.appendChild(i1);

            let pomDiv = document.createElement("div");
            holder.appendChild(pomDiv);

            let i2 = document.createElement("label");
            i2.textContent = auto[svojstva[index]];
            pomDiv.appendChild(i2);   
        });

        const btnIznajmi = document.createElement("button");
        btnIznajmi.textContent = "Iznajmi";
        automobilDiv.appendChild(btnIznajmi);

        btnIznajmi.addEventListener("click", async () => {
            let polja = document.body.querySelectorAll(".iznajmljivanje-container > .iznajmljivanje-container-element > input");

            if(!polja[0].value || !polja[1].value || !polja[2].value || !polja[3].value) {
                alert("Popunite sva polja potrebna za iznajmljivanje!");
                return;
            }

            const res = await fetch("http://localhost:5268/Global/IznajmiAutomobil", {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    PunoIme: polja[0].value,
                    JMBG: polja[1].value,
                    BrojVozacke: polja[2].value,
                    BrojDana: parseInt(polja[3].value),
                    Automobil: auto.id
                })
            });

            if(res.status === 200) {
                automobilDiv.style.backgroundColor = "red";
                btnIznajmi.disabled = true;
                automobilDiv.querySelectorAll("div")[5].querySelectorAll("label")[1].textContent = "true";        
            }

            polja.forEach(polje => {
                polje.value = "";
            });
        });

        if(auto.jelIznajmljen === false) {
            automobilDiv.style.backgroundColor = "green";
            btnIznajmi.disabled = false;
        }
        else {
            automobilDiv.style.backgroundColor = "red";
            btnIznajmi.disabled = true;
        }
    }
}