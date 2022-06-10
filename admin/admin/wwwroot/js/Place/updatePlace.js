const formUpdatePlace = document.getElementById('updatePlace');

formUpdatePlace.addEventListener('submit', async e => {
    e.preventDefault();

    const namePlace = document.getElementById('namePlace').value
    const address = document.getElementById('address').value
    const idPlace = document.getElementById('IdPlace').value
    var erorr_color = document.getElementById("erorr-color")
    console.log(namePlace)
    const checkNamePlace = /^\S*[AĂÂÁẮẤÀẰẦẢẲẨÃẴẪẠẶẬĐEÊÉẾÈỀẺỂẼỄẸỆIÍÌỈĨỊOÔƠÓỐỚÒỒỜỎỔỞÕỖỠỌỘỢUƯÚỨÙỪỦỬŨỮỤỰYÝỲỶỸỴAĂÂÁẮẤÀẰẦẢẲẨÃẴẪẠẶẬĐEÊÉẾÈỀẺỂẼỄẸỆIÍÌỈĨỊOÔƠÓỐỚÒỒỜỎỔỞÕỖỠỌỘỢUƯÚỨÙỪỦỬŨỮỤỰYÝỲỶỸỴAĂÂÁẮẤÀẰẦẢẲẨÃẴẪẠẶẬĐEÊÉẾÈỀẺỂẼỄẸỆIÍÌỈĨỊOÔƠÓỐỚÒỒỜỎỔỞÕỖỠỌỘỢUƯÚỨÙỪỦỬŨỮỤỰYÝỲỶỸỴAĂÂÁẮẤÀẰẦẢẲẨÃẴẪẠẶẬĐEÊÉẾÈỀẺỂẼỄẸỆIÍÌỈĨỊOÔƠÓỐỚÒỒỜỎỔỞÕỖỠỌỘỢUƯÚỨÙỪỦỬŨỮỤỰYÝỲỶỸỴAĂÂÁẮẤÀẰẦẢẲẨÃẴẪẠẶẬĐEÊÉẾÈỀẺỂẼỄẸỆIÍÌỈĨỊOÔƠÓỐỚÒỒỜỎỔỞÕỖỠỌỘỢUƯÚỨÙỪỦỬŨỮỤỰYÝỲỶỸỴAĂÂÁẮẤÀẰẦẢẲẨÃẴẪẠẶẬĐEÊÉẾÈỀẺỂẼỄẸỆIÍÌỈĨỊOÔƠÓỐỚÒỒỜỎỔỞÕỖỠỌỘỢUƯÚỨÙỪỦỬŨỮỤỰYÝỲỶỸỴA-Z]+\S*/i
    if (!checkNamePlace.test(namePlace)) {
        document.getElementById("erorrPlaceName").innerHTML = "Place names can include a-z, 0-9, spaces and must start with a letter."
        erorr_color.classList.add("ui-state-error")
    }
    else {
        erorr_color.classList.remove("ui-state-error")
        document.getElementById("erorrPlaceName").innerHTML = ""
    }

    if (checkNamePlace.test(namePlace)) {
        const url = `/Places/${idPlace}/update`;

        const res = await fetch(url, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                placeName: namePlace,
                address: address,
            })
        });

        const result = await res.json();
        if (result.msg == "successed") {
            alert("successed")
            window.location.href = result.url
        }
        else {
            erorr_color.classList.add("ui-state-error")
            document.getElementById("erorrPlaceName").innerHTML = "Location already exists"
        }
    }
});