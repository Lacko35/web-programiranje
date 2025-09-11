import Knjiga from "./knjiga.js";

export default class Draw {
    constructor() {
        this.container = null;
    }

    DrawLibrary(host, library) {
        this.container = document.createElement("div");
        this.container.className = "main-container";
        host.appendChild(this.container);

        const libraryHeader = document.createElement("h1");
        libraryHeader.textContent = library.ime;
        this.container.appendChild(libraryHeader);

        const funcContainer = document.createElement("div");
        funcContainer.className = "functional-container";
        this.container.appendChild(funcContainer);

        const addBookContainer = document.createElement("div");
        addBookContainer.className = "add-book";
        funcContainer.appendChild(addBookContainer);

        const addBookHeader = document.createElement("h2");
        addBookHeader.textContent = "Knjige";
        addBookContainer.appendChild(addBookHeader);

        const inputsContainer = document.createElement("div");
        inputsContainer.className = "inputs-container";
        addBookContainer.appendChild(inputsContainer);

        let labels = ["Naslov:", "Autor:", "Godina:", "Izdavac:", "Broj:"];
        let inputTypes = ["text", "text", "number", "text", "text"];

        labels.forEach((label, index) => {
            let holder = document.createElement("div");
            holder.className = "add-book-element";
            inputsContainer.appendChild(holder);

            let l = document.createElement("label");
            l.textContent = label;
            holder.appendChild(l);

            let i = document.createElement("input");
            i.type = inputTypes[index];
            holder.appendChild(i);
        });

        const addBookBtn = document.createElement("button");
        addBookBtn.textContent = "Dodaj";
        inputsContainer.appendChild(addBookBtn);

        addBookBtn.addEventListener("click", async () => {
            const bookDetails = inputsContainer.querySelectorAll("div > input");

            bookDetails.forEach(bookDetail => {
                if(!bookDetail.value) {
                    alert("Sva polja moraju biti popunjena!");
                }
            });

            console.log(bookDetails);

            const res = await fetch(`http://localhost:5294/Global/DodajKnjigu/${parseInt(library.bibliotekaID)}`, {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    "naslov": bookDetails[0].value,
                    "imeAutora": bookDetails[1].value,
                    "izdavac": bookDetails[3].value,
                    "godinaIzdavanja": parseInt(bookDetails[2].value),
                    "evidencioniBroj": bookDetails[4].value,
                    "izdataKnjiga": false
                })
            });

            if(res.status === 200) {
                alert("Uspesno dodata knjiga u bazu i biblioteku!");

                const res2 = await fetch(`http://localhost:5294/Global/VratiKnjige/${parseInt(library.bibliotekaID)}`);
                
                if(res2.status === 200) {
                    const res2Data = await res2.json();

                    const knjige = res2Data.map(knjiga => new Knjiga(knjiga.identifikator, knjiga.naslovKnjige, knjiga.autor, knjiga.izdavacKnjige,
                        knjiga.godina, knjiga.brojEvidencije, knjiga.izdata));
                    
                    this.popuniSelect(bookSelector, knjige);
                }
            }
            else {
                alert("Doslo je do greske");
            }
        });

        const libraryFunctionalitiesContainer = document.createElement("div");
        libraryFunctionalitiesContainer.className = "library-functionalities";
        funcContainer.appendChild(libraryFunctionalitiesContainer);

        const libraryFunctionalitiesHeader = document.createElement("h2");
        libraryFunctionalitiesHeader.textContent = "Izdavanje/vracanje";
        libraryFunctionalitiesContainer.appendChild(libraryFunctionalitiesHeader);

        const subDiv = document.createElement("div");
        subDiv.className = "functionalities-sub-div";
        libraryFunctionalitiesContainer.appendChild(subDiv);

        const elementsDiv = document.createElement("div");
        subDiv.appendChild(elementsDiv);

        const bookInput = document.createElement("input");
        bookInput.type = "text";
        elementsDiv.appendChild(bookInput);

        const findBtn = document.createElement("button");
        findBtn.textContent = "Nadji";
        elementsDiv.appendChild(findBtn);

        findBtn.addEventListener("click", async () => {
            const par = elementsDiv.querySelector("input").value;

            if(!par) {
                alert("Unesite naziv pisca ili knjige!");
                return;
            }

            const response = await fetch(`http://localhost:5294/Global/PretraziKnjigeUBiblioteci/${parseInt(library.bibliotekaID)}/${par}`);
        
            if(response.status === 200) {
                    const response2 = await response.json();

                    const knjige = response2.map(knjiga => new Knjiga(knjiga.knjigaID, knjiga.naslov, knjiga.imeAutora, knjiga.izdavac,
                        knjiga.godinaIzdavanja, knjiga.evidencioniBroj, knjiga.izdataKnjiga));
                    
                    this.popuniSelect(bookSelector, knjige);                
            }
        });

        const bookSelector = document.createElement("select");

        this.popuniSelect(bookSelector, library.knjige);

        bookSelector.addEventListener("change", (event) => {
            let vrednost = event.target.value;

            if(vrednost === "true") {
                mainBtn.textContent = "Vrati";
            }
            else {
                mainBtn.textContent = "Izdaj";
            }
        });

        subDiv.appendChild(bookSelector);

        const mainBtn = document.createElement("button");
        mainBtn.textContent = "Vrati";
        subDiv.appendChild(mainBtn);
        mainBtn.addEventListener("click", async (event) => {
            let knjiga = bookSelector.options[bookSelector.selectedIndex].id;

            if(event.target.textContent === "Vrati") {
                const response = await fetch(`http://localhost:5294/Global/VratiKnjigu/${parseInt(knjiga)}/${parseInt(library.bibliotekaID)}`, {
                    method: "POST"
                });

                if(response.status === 200) {
                    alert(response.text());
                }
            }
            else {
                const response = await fetch(`http://localhost:5294/Global/IzdajKnjigu/${parseInt(knjiga)}/${parseInt(library.bibliotekaID)}`, {
                    method: "POST"
                });

                if(response.status === 200) {
                    alert(response.text());
                }
            }

            obj.najcitanijaKnjiga(displayBookContent);
        });

        const dispayBookContainer = document.createElement("div");
        dispayBookContainer.className = "display-book";
        funcContainer.appendChild(dispayBookContainer);

        const displayBookHeader = document.createElement("h2");
        displayBookHeader.textContent = "Najcitanija knjiga";
        dispayBookContainer.appendChild(displayBookHeader);

        const displayBookContent = document.createElement("h2");
        displayBookContent.style.color = "red";
        dispayBookContainer.appendChild(displayBookContent);

        this.najcitanijaKnjiga(displayBookContent);
    }

    async najcitanijaKnjiga(host) {
        let res = await fetch(`http://localhost:5294/Global/NajcitanijaKnjiga`);
        
        if(res.status == 200) {
            let resData = await res.json();

            host.textContent = resData.naslov + "-" + resData.autor;
        }
    }

    popuniSelect(host, list) {
        host.innerHTML = "";

        list.forEach(knjiga => {
            let opcija = document.createElement("option");
            opcija.id = knjiga.knjigaID;
            opcija.value = knjiga.izdataKnjiga;
            opcija.textContent = knjiga.naslov + "-" + knjiga.imeAutora;

            host.appendChild(opcija);
        });
    }
}
