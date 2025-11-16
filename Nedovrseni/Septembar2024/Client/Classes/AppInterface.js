import Racun from "./Racun.js";
import Stan from "./Stan.js";

export default class AppInterface {
    constructor() {
        this.container = null;
    }

    drawInterface(identificators) {
        this.container = document.createElement('div');
        this.container.id = 'app-interface';
        document.body.appendChild(this.container);

        const stanDataDiv = document.createElement('div');
        stanDataDiv.className = "stan-data";
        this.container.appendChild(stanDataDiv);

        const selectStanDiv = document.createElement('div');
        selectStanDiv.className = "select-stan";
        stanDataDiv.appendChild(selectStanDiv);

        const selectLabel = document.createElement('label');
        selectLabel.innerText = "Izaberite broj stana: ";
        selectStanDiv.appendChild(selectLabel);

        const selectElement = document.createElement('select');
        identificators.forEach(id => {
            const option = document.createElement('option');
            option.value = id;
            option.innerText = id;
            selectElement.appendChild(option);
        });
        selectStanDiv.appendChild(selectElement);
    
        const selectButton = document.createElement('button');
        selectButton.innerText = "Prikaz informacija";
        selectStanDiv.appendChild(selectButton);

        const infoDiv = document.createElement('div');
        infoDiv.className = "stan-info";
        stanDataDiv.appendChild(infoDiv);

        const racuniDiv = document.createElement("div");
        racuniDiv.className = "racuni";
        this.container.appendChild(racuniDiv);

        const stan = null;
        selectButton.addEventListener('click', async () => {
            const selectedStan = selectElement.value;

            if(isNaN(selectedStan)) {
                alert("Odaberite stan za prikaz informacija!");
                return;
            }

            const response = await fetch(`'http://localhost:5244/api/Global/StanInformacije/${encodeURIComponent(selectedStan)}`);

            const stanInfo = await response.json();

            const racuni = stanInfo.racuni.map(racunData => new Racun(racunData.racunID, racunData.mesecIzdavanja, racunData.jelPlacen, 
                racunData.racunZaVodu, racunData.racunZaStruju, racunData.racunZaKomunalije)
            );

            stan = new Stan(stanInfo.brojStana, stanInfo.imeVlasnika, stanInfo.kvadratura, stanInfo.brojClanova, racuni);

            this.#drawStan(infoDiv, racuniDiv, stan);
        });
    }

    #drawStan(parentInfo, parentRacuni, stan) {
        parentInfo.innerHTML = "";
        parentRacuni.innerHTML = "";

        let labels = ["Broj stana:", "Ime vlasnika:", "Povrsina (m^2):", "Broj clanova:"];
        let datas = [stan.id, stan.vlasnik, stan.kvadratura, stan.brojStanara];

        labels.forEach((text, index) => {
            let infoElement = document.createElement('div');
            infoElement.className = "info-element";
            parentInfo.appendChild(infoElement);

            let labelDesc = document.createElement('label');
            labelDesc.textContent = text;
            infoElement.appendChild(labelDesc);

            let subDiv = document.createElement('div');
            infoElement.appendChild(subDiv);

            let labelVal = document.createElement('label');
            labelVal.textContent = datas[index];
            subDiv.appendChild(labelVal);
        });

        let obracunBtn = document.createElement('button');
        obracunBtn.textContent = "Izracunaj ukupno zaduzenje";
        parentInfo.appendChild(obracunBtn);

        obracunBtn.addEventListener('click', async () => {
            const response = await fetch(`http://localhost:5244/api/Global/ObracunDugovanja/${encodeURIComponent(selectedStan)}`);

            if(response.ok) {
                let dugovanja = await response.text();

                obracunBtn.textContent = dugovanja;
                obracunBtn.disabled = true;
            }
        });
        
        stan.racuni.forEach((racun) => {
            let racunDiv = document.createElement('div');
            racunDiv.className = "racun-div";
            parentRacuni.appendChild(racunDiv);
            
            let element = document.createElement('div');
            element.className = "concrete-element";
            racunDiv.appendChild(element);

            let labelDesc = document.createElement('label');
            labelDesc.textContent = "Mesec:";
            element.appendChild(labelDesc);

            let subDiv = document.createElement('div');
            element.appendChild(subDiv);

            let labelVal = document.createElement('label');
            labelVal.textContent = racun.mesecIzdavanja;
            subDiv.appendChild(labelVal);

            element = document.createElement('div');
            element.className = "concrete-element";
            racunDiv.appendChild(element);
            
            labelDesc = document.createElement('label');
            labelDesc.textContent = "Voda:";
            element.appendChild(labelDesc);

            subDiv = document.createElement('div');
            element.appendChild(subDiv);

            labelVal = document.createElement('label');
            labelVal.textContent = racun.racunZaVodu;
            subDiv.appendChild(labelVal);
            
            element = document.createElement('div');
            element.className = "concrete-element";
            racunDiv.appendChild(element);
            
            labelDesc = document.createElement('label');
            labelDesc.textContent = "Struja:";
            element.appendChild(labelDesc);

            subDiv = document.createElement('div');
            element.appendChild(subDiv);

            labelVal = document.createElement('label');
            labelVal.textContent = racun.racunZaStruju;
            subDiv.appendChild(labelVal); 
            
            element = document.createElement('div');
            element.className = "concrete-element";
            racunDiv.appendChild(element);
            
            labelDesc = document.createElement('label');
            labelDesc.textContent = "Komunalne usluge:";
            element.appendChild(labelDesc);

            subDiv = document.createElement('div');
            element.appendChild(subDiv);

            labelVal = document.createElement('label');
            labelVal.textContent = racun.racunZaKomunalije;
            subDiv.appendChild(labelVal);

            element = document.createElement('div');
            element.className = "concrete-element";
            racunDiv.appendChild(element);
            
            labelDesc = document.createElement('label');
            labelDesc.textContent = "Placen:";
            element.appendChild(labelDesc);

            subDiv = document.createElement('div');
            element.appendChild(subDiv);

            labelVal = document.createElement('label');
            labelVal.textContent = '';
            subDiv.appendChild(labelVal);

            if(racun.jelPlacen === true) {
                labelVal.textContent = "Da";
                racunDiv.style.backgroundColor = "green";
            }
            else {
                labelVal.textContent = "Ne";
                racunDiv.style.backgroundColor = "red";                
            }
        });
    }
}