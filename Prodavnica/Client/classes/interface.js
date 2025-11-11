import Proizvod from "./proizvod.js";

export default class Interface {
    constructor() {
        this.container = null;
    }

    generateGUI(shops) {
        this.container = document.createElement('div');
        this.container.className = "container";
        document.body.appendChild(this.container);

        shops.forEach(shop => {
            const shopDiv = document.createElement('div');
            shopDiv.className = "shop";
            this.container.appendChild(shopDiv);

            const addFormDiv = document.createElement('div');
            addFormDiv.className = "add-form";
            shopDiv.appendChild(addFormDiv);

            const addFormHeader = document.createElement('h3');
            addFormHeader.innerText = "Upis proizvoda";
            addFormDiv.appendChild(addFormHeader);

            const addForm = document.createElement('div');
            addForm.className = "form";
            addFormDiv.appendChild(addForm);

            var div = document.createElement('div');
            div.className = "element";
            addForm.appendChild(div);

            const nazivLabel = document.createElement('label');
            nazivLabel.innerText = "Naziv:";
            div.appendChild(nazivLabel);

            const nazivInput = document.createElement('input');
            nazivInput.type = "text";
            div.appendChild(nazivInput);

            div = document.createElement('div');
            div.className = "element";
            addForm.appendChild(div);

            const kategorijaLabel = document.createElement('label');
            kategorijaLabel.innerText = "Kategorija:";
            div.appendChild(kategorijaLabel);

            const kategorijaInput = document.createElement('select');

            const option0 = document.createElement('option');
            option0.value = "";
            option0.innerText = "kategorija";
            kategorijaInput.appendChild(option0);

            const option1 = document.createElement('option');
            option1.value = "igracka";
            option1.innerText = "igracka";
            kategorijaInput.appendChild(option1);

            const option2 = document.createElement('option');
            option2.value = "pribor";
            option2.innerText = "pribor";
            kategorijaInput.appendChild(option2);

            const option3 = document.createElement('option');
            option3.value = "knjiga";
            option3.innerText = "knjiga";
            kategorijaInput.appendChild(option3);

            div.appendChild(kategorijaInput);

            div = document.createElement('div');
            div.className = "element";
            addForm.appendChild(div);

            const cenaLabel = document.createElement('label');
            cenaLabel.innerText = "Cena:";
            div.appendChild(cenaLabel);

            const cenaInput = document.createElement('input');
            cenaInput.type = "number";
            div.appendChild(cenaInput);

            div = document.createElement('div');
            div.className = "element";
            addForm.appendChild(div);

            const kolicinaLabel = document.createElement('label');
            kolicinaLabel.innerText = "Kolicina:";
            div.appendChild(kolicinaLabel);

            const kolicinaInput = document.createElement('input');
            kolicinaInput.type = "number";
            div.appendChild(kolicinaInput);

            const productsList = document.createElement('div');

            var proizvodiNiz = [];
            kategorijaInput.addEventListener('change', async (e) => {
                let kategorija = e.target.value;

                if (kategorija === "") {
                    productsList.innerHTML = "";
                }
                else {
                    await this.#nacrtajListu(proizvodiNiz, productsList, kategorija, shop);
                }
            });

            const addButton = document.createElement('button');
            addButton.innerText = "Dodaj proizvod";
            addFormDiv.appendChild(addButton);

            addButton.addEventListener('click', async () => {
                let polja = addForm.querySelectorAll(".element > input, .element > select");

                const response = await fetch(`http://localhost:5022/api/Global/DodajProizvod/${encodeURIComponent(shop.id)}`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        naziv: polja[0].value,
                        kategorija: polja[1].value,
                        cena: parseFloat(polja[2].value),
                        kolicina: parseInt(polja[3].value)
                    })
                });

                if (response.ok) {
                    await this.#nacrtajListu(proizvodiNiz, productsList, polja[1].value, shop);
                }

                polja.forEach(polje => polje.value = "");
            });

            const productsDiv = document.createElement('div');
            productsDiv.className = "products";
            shopDiv.appendChild(productsDiv);

            const productsHeader = document.createElement('h3');
            productsHeader.innerText = "Prodavnica: " + shop.naziv;
            productsDiv.appendChild(productsHeader);

            productsList.className = "products-list";
            productsDiv.appendChild(productsList);
        });
    }

    #productsPart(parent, products, shop) {
        parent.innerHTML = "";

        products.forEach(proizvod => {
            if (proizvod.kolicina > 0) {
                const productDiv = document.createElement('div');
                productDiv.className = "product";

                const divPartOne = document.createElement('div');
                divPartOne.className = "part-one";
                productDiv.appendChild(divPartOne);

                const opis = document.createElement("span");
                opis.innerText = proizvod.naziv + ": " + proizvod.kolicina;
                divPartOne.appendChild(opis);

                const pomDiv1 = document.createElement('div');
                pomDiv1.className = "holder";
                divPartOne.appendChild(pomDiv1);

                const progressBar = document.createElement('div');
                progressBar.className = "progress-bar";
                progressBar.style.height = "15px";
                progressBar.style.width = proizvod.kolicina > 100 ? 333 : Math.floor((proizvod.kolicina / 100) * 333) + "px";
                progressBar.style.backgroundColor = "red";
                pomDiv1.appendChild(progressBar);

                const divPartTwo = document.createElement('div');
                divPartTwo.className = "part-two";
                productDiv.appendChild(divPartTwo);

                const label = document.createElement('label');
                label.innerText = "Kolicina:";
                divPartTwo.appendChild(label);

                const input = document.createElement('input');
                input.type = "number";
                divPartTwo.appendChild(input);

                const button = document.createElement('button');
                button.innerText = "Prodaj";
                divPartTwo.appendChild(button);

                button.addEventListener('click', async () => {
                    let kolicina = parseInt(input.value);

                    const response = await (await fetch(`http://localhost:5022/api/Global/ProdajProizvod/${encodeURIComponent(shop.id)}/${encodeURIComponent(proizvod.id)}/${kolicina}`)).json();

                    if (typeof response !== "string") {
                        opis.innerText = response.naziv + ": " + response.kolicina;
                    }
                });

                parent.appendChild(productDiv);
            }
        });
    }

    async #nacrtajListu(parentArray, products, kategorija, shop) {
        const res = await fetch(`http://localhost:5022/api/Global/ProizvodiPoKategoriji/${encodeURIComponent(kategorija)}/${shop.id}`);

        if (res.ok) {
            let privremeniNiz = await res.json();

            privremeniNiz.forEach(proizvod => {
                let p = new Proizvod(proizvod.id, proizvod.naziv, proizvod.kategorija, proizvod.cena, proizvod.kolicina);

                parentArray.push(p);
            });

            this.#productsPart(products, parentArray, shop);

            parentArray.length = 0;
        }
    }
}