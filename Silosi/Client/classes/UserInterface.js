export default class UserInterface {
    constructor() {
        this.container = null;
    }

    drawUI(fabrika) {
        this.container = document.createElement('div');
        this.container.classList.add('user-interface');
        document.body.appendChild(this.container);

        const silosStateDIV = document.createElement('div');
        silosStateDIV.classList.add('silos-state');
        this.container.appendChild(silosStateDIV);

        const factoryTitle = document.createElement('h2');
        factoryTitle.textContent = `${fabrika.naziv}`;
        silosStateDIV.appendChild(factoryTitle);

        const silosHolderDIV = document.createElement('div');
        silosHolderDIV.classList.add('silos-holder');
        silosStateDIV.appendChild(silosHolderDIV);

        this.#drawSilos(silosHolderDIV, fabrika.silosi);

        const changeStateDIV = document.createElement('div');
        changeStateDIV.classList.add('change-state');
        this.container.appendChild(changeStateDIV);

        const formDIV = document.createElement('div');
        changeStateDIV.appendChild(formDIV);

        let div = document.createElement('div');
        div.className = "form-element";
        formDIV.appendChild(div);

        const selectLabel = document.createElement('label');
        selectLabel.textContent = 'Silos:';
        div.appendChild(selectLabel);

        const silosSelect = document.createElement('select');
        div.appendChild(silosSelect);

        const option = document.createElement('option');
        option.value = "";
        option.textContent = "izaberite";
        silosSelect.appendChild(option);

        fabrika.silosi.forEach(silos => {
            const option = document.createElement('option');
            option.value = silos.silosID;
            option.textContent = silos.oznaka;
            silosSelect.appendChild(option);
        });

        div = document.createElement('div');
        div.className = "form-element";
        formDIV.appendChild(div);

        const kolicinaLabel = document.createElement('label');
        kolicinaLabel.textContent = 'Kolicina:';
        div.appendChild(kolicinaLabel);

        const kolicinaInput = document.createElement('input');
        kolicinaInput.type = 'number';
        div.appendChild(kolicinaInput);

        const changeStateBtn = document.createElement('button');
        changeStateBtn.textContent = 'Sipaj u silos';

        changeStateBtn.addEventListener('click', async () => {
            const selectedSilosID = silosSelect.value;
            const kolicina = parseInt(kolicinaInput.value);

            if (selectedSilosID && !isNaN(kolicina) && kolicina > 0) {
                const response = await fetch(`http://localhost:5087/App/PromeniStanjeSilosa/${encodeURIComponent(selectedSilosID)}/${encodeURIComponent(kolicina)}`, {
                    method: 'PUT',
                });

                if (response.status === 400) {
                    let msg = await response.text();
                    alert(msg);
                    return;
                }
                else {
                    const list = await (await fetch(`http://localhost:5087/App/VratiSilose`)).json();

                    this.#drawSilos(silosHolderDIV, list);
                }
            }
            else {
                alert('Molimo izaberite silos i unesite validnu kolicinu.');
            }
        });

        changeStateDIV.appendChild(changeStateBtn);
    }

    #drawSilos(parent, silosList) {
        parent.innerHTML = '';

        silosList.forEach(silos => {
            const silosDIV = document.createElement('div');
            silosDIV.classList.add('silos');
            parent.appendChild(silosDIV);

            const labelOznaka = document.createElement('label');
            labelOznaka.textContent = `${silos.oznaka}`;
            silosDIV.appendChild(labelOznaka);

            const labelStanje = document.createElement('label');
            labelStanje.textContent = `${silos.trenutnoStanje}/${silos.kapacitet}`;
            silosDIV.appendChild(labelStanje);

            const progressBar = document.createElement('div');
            progressBar.className = 'progress-bar';

            const fillBar = document.createElement('div');
            fillBar.style.height = `${(silos.trenutnoStanje / silos.kapacitet) * 100}%`;
            fillBar.style.width = '100%';
            fillBar.style.backgroundColor = 'green';

            progressBar.appendChild(fillBar);

            silosDIV.appendChild(progressBar);
        });
    }
}